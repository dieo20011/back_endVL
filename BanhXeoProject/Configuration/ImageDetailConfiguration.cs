using BanhXeoProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanhXeoProject.Configuration
{
    public class ImageDetailConfiguration : IEntityTypeConfiguration<ImageDetail>
    {
        public void Configure(EntityTypeBuilder<ImageDetail> builder)
        {
            builder.ToTable(nameof(ImageDetail));
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.GroupImage).WithMany(x => x.ImageDetail).HasForeignKey(y => y.GroupId);
        }
    }
}
