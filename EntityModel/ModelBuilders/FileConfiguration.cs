using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = OfflineSynchronizationPOC.EntityModel.Entities.File;

namespace OfflineSynchronizationPOC.EntityModel.ModelBuilders;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.Property(x => x.Uuid)
               .IsRequired();

        builder.Property(x => x.Path)
               .IsRequired();

        builder.Property(x => x.Mime)
               .IsRequired();
    }
}