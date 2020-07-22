using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class StoryConfiguration : EntityTypeConfiguration<Story>
    {
        internal StoryConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Story");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
            Property(x => x.ChapterCount).HasColumnName("ChapterCount");
            Property(x => x.PhotoCount).HasColumnName("PhotoCount");
            Property(x => x.HeaderPhotoId).IsOptional().HasColumnName("HeaderPhotoId");
            Property(x => x.IsPrivate).HasColumnName("IsPrivate").IsRequired();
            HasOptional(x => x.HeaderPhoto);
            Ignore(x => x.Key);

            HasMany(p => p.Chapters).WithRequired().HasForeignKey(c => c.StoryId).WillCascadeOnDelete(true);
        }
    }

    internal class StoryForPhotoConfiguration : EntityTypeConfiguration<StoryForPhoto>
    {
        internal StoryForPhotoConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".PhotosInStories");

            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
            Property(x => x.ChapterCount).HasColumnName("ChapterCount");
            Property(x => x.PhotoCount).HasColumnName("PhotoCount");
            Property(x => x.HeaderPhotoId).IsOptional().HasColumnName("HeaderPhotoId");
            Property(x => x.IsPrivate).HasColumnName("IsPrivate").IsRequired();
            HasOptional(x => x.HeaderPhoto);
            Ignore(x => x.Key);
            HasMany(p => p.Chapters).WithRequired().HasForeignKey(c => c.StoryId).WillCascadeOnDelete(true);

            Property(x => x.PhotoId).HasColumnName("PhotoId");
            
        }
    }

    internal class ChapterConfiguration : EntityTypeConfiguration<Chapter>
    {
        internal ChapterConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Chapter");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
            Property(x => x.Order).HasColumnName("Order");
            Property(x => x.StoryId).HasColumnName("StoryId");
            Property(x => x.IsPrivate).HasColumnName("IsPrivate").IsRequired();

            HasMany(p => p.Ledges).WithRequired().HasForeignKey(l => l.ChapterId).WillCascadeOnDelete(true);
        }
    }

    internal class LedgeConfiguration : EntityTypeConfiguration<Ledge>
    {
        internal LedgeConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Ledge");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Template).HasColumnName("Template").IsRequired().HasMaxLength(10);
            Property(x => x.Order).HasColumnName("Order");
            Property(x => x.ChapterId).HasColumnName("ChapterId");

            HasMany(p => p.Bricks).WithRequired().HasForeignKey(l => l.LedgeId).WillCascadeOnDelete(true);
        }
    }

    internal class BrickConfiguration : EntityTypeConfiguration<Brick>
    {
        internal BrickConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Brick");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Order).HasColumnName("Order");
            Property(x => x.Type).HasColumnName("Type").HasMaxLength(10);
            Property(x => x.LedgeId).HasColumnName("LedgeId");
            Property(x => x.Type).HasColumnName("Type");
        }
    }

    internal class TextBrickConfiguration : EntityTypeConfiguration<TextBrick>
    {
        internal TextBrickConfiguration(string schema = "dbo")
        {
            Property(x => x.Text).HasColumnName("Text");
        }
    }

    internal class MapBrickConfiguration : EntityTypeConfiguration<MapBrick>
    {
        internal MapBrickConfiguration(string schema = "dbo")
        {
            Property(x => x.Latitude).HasColumnName("Latitude");
            Property(x => x.Longitude).HasColumnName("Longitude");
            Property(x => x.Zoom).HasColumnName("Zoom");
        }
    }

    internal class PhotoBrickConfiguration : EntityTypeConfiguration<PhotoBrick>
    {
        internal PhotoBrickConfiguration(string schema = "dbo")
        {
            Property(x => x.Caption).HasColumnName("Caption");
            Property(x => x.PhotoId).IsOptional().HasColumnName("PhotoId");
            HasOptional(x => x.Photo);
        }
    }
}