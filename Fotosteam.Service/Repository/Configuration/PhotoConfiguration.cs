using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class PhotoConfiguration : EntityTypeConfiguration<Photo>
    {
        internal PhotoConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Photo");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            Property(x => x.OriginalName).HasColumnName("OriginalName").IsRequired().HasMaxLength(200);            
            Property(x => x.Folder).HasColumnName("Folder").IsRequired().HasMaxLength(200);
            Property(x => x.Category).HasColumnName("Category").IsRequired();
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.EventId).HasColumnName("EventId").IsOptional();
            Property(x => x.LocationId).HasColumnName("LocationId").IsOptional();
            Property(x => x.Width).HasColumnName("Width").IsRequired();
            Property(x => x.Height).HasColumnName("Height").IsRequired();
            Property(x => x.AspectRation).HasColumnName("AspectRation").IsRequired();
            Property(x => x.Orientation).HasColumnName("Orientation").IsRequired().HasMaxLength(15);
            Property(x => x.CaptureDate).HasColumnName("CaptureDate").IsRequired();
            Property(x => x.PublishDate).HasColumnName("PublishDate").IsOptional();
            Property(x => x.CommentCount).HasColumnName("CommentCount").IsRequired();
            Property(x => x.RatingSum).HasColumnName("RatingSum").IsRequired();
            Property(x => x.IsPrivate).HasColumnName("IsPrivate").IsRequired();
            Property(x => x.License).HasColumnName("License").IsRequired();
            Property(x => x.AllowFullSizeDownload).HasColumnName("AllowFullSizeDownload").IsRequired();            
            Property(x => x.StorageAccessType).HasColumnName("StorageAccessType");
            Property(x => x.IsNew).HasColumnName("IsNew");
            Property(x => x.IsForStoryOnly).HasColumnName("IsForStoryOnly");
            Property(x => x.Color).HasColumnName("Color").IsOptional().HasMaxLength(15);
            Property(x => x.AllowPromoting).HasColumnName("AllowPromoting");
            Property(x => x.AllowCommenting).HasColumnName("AllowCommenting");
            Property(x => x.AllowSharing).HasColumnName("AllowSharing");
            Property(x => x.AllowRating).HasColumnName("AllowRating");
            Property(x => x.ShowInOverview).HasColumnName("ShowInOverview");
            Property(x => x.Description).HasColumnName("Description");
            Property(x => x.Top).HasColumnName("Top");
            Property(x => x.Left).HasColumnName("Left");

            //Relationships
            HasMany(p => p.Topics).WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("PhotoId");
                    m.MapRightKey("TopicId");
                    m.ToTable("PhotoTopic");
                });

            HasMany(p => p.DirectLinks).WithOptional().HasForeignKey(d => d.PhotoId).WillCascadeOnDelete(true);
            HasOptional(p => p.Exif).WithRequired().WillCascadeOnDelete(true);
            HasOptional(p => p.Location);
            HasOptional(p => p.Event);

            Ignore(p => p.Categories);
            Ignore(p => p.Original640Url);
        }
    }
}