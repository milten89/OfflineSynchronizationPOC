using System.Data;
using Dotmim.Sync;
using Dotmim.Sync.MariaDB;
using Dotmim.Sync.Web.Server;
using OfflineSynchronizationPOC.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MariaDbDatabaseContext>();

builder.Services.AddHostedService<StartupService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var connectionString = MariaDbDatabaseContext.ConnectionString;
var syncOptions = new SyncOptions();
var webServerOptions = new WebServerOptions();

var setup = new SyncSetup("Facilities", "Users", "UserFacility", "Files");

var facilitiesFilter = new SetupFilter("Facilities");
facilitiesFilter.AddParameter("FacilityId", DbType.Guid);
facilitiesFilter.AddWhere("Uuid", "Facilities", "FacilityId");
setup.Filters.Add(facilitiesFilter);

var userFacilityFilter = new SetupFilter("UserFacility");
userFacilityFilter.AddParameter("FacilityId", DbType.Guid);
userFacilityFilter.AddJoin(Join.Left, "Facilities")
                  .On("Facilities", "Id", "UserFacility", "FacilityId");
userFacilityFilter.AddWhere("Uuid", "Facilities", "FacilityId");
setup.Filters.Add(userFacilityFilter);

var usersFilter = new SetupFilter("Users");
usersFilter.AddParameter("FacilityId", DbType.Guid);
usersFilter.AddJoin(Join.Left, "UserFacility")
           .On("UserFacility", "UserId", "Users", "Id");
usersFilter.AddJoin(Join.Left, "Facilities")
           .On("UserFacility", "FacilityId", "Facilities", "Id");
usersFilter.AddWhere("Uuid", "Facilities", "FacilityId");
setup.Filters.Add(usersFilter);

var fileFilter = new SetupFilter("Files");
fileFilter.AddParameter("FacilityId", DbType.Guid);
//fileFilter.AddJoin(Join.Left, "Users")
//          .On("Users", "AvatarId", "Files", "Id");
//fileFilter.AddJoin(Join.Left, "UserFacility")
//          .On("UserFacility", "UserId", "Users", "Id");
//fileFilter.AddJoin(Join.Left, "Facilities")
//          .On("Facilities", "Id", "UserFacility", "FacilityId")
//          .On("Facilities", "LogoId", "Files", "Id");
//fileFilter.AddWhere("Uuid", "Facilities", "FacilityId");
fileFilter.AddCustomWhere("""
    base.Id IN (SELECT Facilities.LogoId 
                FROM Facilities 
                WHERE Facilities.Uuid = @FacilityId)
    OR
    base.Id In (SELECT Users.AvatarId
                FROM Users
                JOIN UserFacility ON Users.Id=UserFacility.UserId
                JOIN Facilities ON UserFacility.FacilityId=Facilities.Id
                WHERE Facilities.Uuid = @FacilityId)
    OR
    side.sync_row_is_tombstone = 1
    """);
setup.Filters.Add(fileFilter);

builder.Services.AddSyncServer<MariaDBSyncProvider>(connectionString, setup, syncOptions, webServerOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
