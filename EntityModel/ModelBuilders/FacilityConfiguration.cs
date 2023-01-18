using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfflineSynchronizationPOC.EntityModel.Entities;

namespace OfflineSynchronizationPOC.EntityModel.ModelBuilders
{
    public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.Property(x => x.Uuid)
                   .IsRequired();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);
            
            builder.HasMany(x => x.Users)
                   .WithMany(x => x.Facilities)
                   .UsingEntity<UserFacility>(j => j.HasOne(x => x.User)
                                                    .WithMany(x => x.UserFacilities)
                                                    .HasForeignKey(x => x.UserId),
                                              j => j.HasOne(x => x.Facility)
                                                    .WithMany(x => x.UserFacilities)
                                                    .HasForeignKey(x => x.FacilityId),
                                              j =>
                                              {
                                                  j.HasKey(x => new { x.UserId, x.FacilityId });
                                                  j.Property(x => x.UserId)
                                                   .IsRequired();
                                                  j.Property(x => x.FacilityId)
                                                   .IsRequired();
                                                  j.Property(x => x.Role)
                                                   .IsRequired();
                                              });
        }
    }
}
