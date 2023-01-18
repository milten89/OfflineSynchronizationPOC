using OfflineSynchronizationPOC.EntityModel.Entities;
using File = OfflineSynchronizationPOC.EntityModel.Entities.File;

namespace OfflineSynchronizationPOC.Server
{
    public class StartupService : IHostedService
    {
        private readonly IServiceProvider _services;

        public StartupService(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _services.CreateAsyncScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<MariaDbDatabaseContext>();

            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);

            //return;

            const int facilitiesCount = 3;
            const int usersCount = 10;
            
            var files = new List<File>();
            var users = new List<User>();
            var facilities = new List<Facility>();

            for (var i = 0; i < facilitiesCount + usersCount; i++)
            {
                var file = new File
                {
                    Uuid = Guid.NewGuid(),
                    Path = $"https://storage.google.com/file/{i}",
                    Mime = "image/png",
                };
                files.Add(file);
            }

            for (var i = 0; i < facilitiesCount; i++)
            {
                var facility = new Facility
                {
                    Uuid = i == 0 ? Guid.Parse("2a0706e7-9201-4a57-80f2-dc8fc230b169") : Guid.NewGuid(),
                    Name = $"Facility #{i + 1}",
                    Logo = files[i],
                };
                facilities.Add(facility);
            }

            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    Uuid = Guid.NewGuid(),
                    FirstName = "User",
                    LastName = $"#{i + 1}",
                    Avatar = files[facilitiesCount + i],
                };
                users.Add(user);
            }

            facilities[0].Users = new List<User>
            {
                users[0],
                users[1],
                users[2],
                users[3],
            };
            facilities[1].Users = new List<User>
            {
                users[0],
                users[4],
                users[5],
                users[6],
            };
            facilities[2].Users = new List<User>
            {
                users[7],
                users[8],
                users[9],
            };

            await dbContext.Files.AddRangeAsync(files, cancellationToken);
            await dbContext.Facilities.AddRangeAsync(facilities, cancellationToken);
            await dbContext.Users.AddRangeAsync(users, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            var it = 0;
            var roles = new[] { FacilityRole.Client, FacilityRole.Specialist, FacilityRole.Owner };

            foreach (var userFacility in facilities.SelectMany(facility => facility.UserFacilities))
                userFacility.Role = roles[it++ % roles.Length];

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
