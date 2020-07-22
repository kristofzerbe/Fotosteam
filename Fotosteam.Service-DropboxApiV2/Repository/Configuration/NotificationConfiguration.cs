using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        internal NotificationConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Notification");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.Date).HasColumnName("Date").IsRequired();
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.IsRead).HasColumnName("IsRead").IsRequired();
            Property(x => x.UserAlias).HasColumnName("UserAlias").IsRequired().HasMaxLength(256);
            Property(x => x.UserAvatarLink).HasColumnName("UserAvatarLink").HasMaxLength(2083);
            Property(x => x.IsUserAmember).HasColumnName("IsUserAmember").IsRequired();
            Property(x => x.PhotoName).HasColumnName("PhotoName").HasMaxLength(256);

            Ignore(x => x.Data);
            Ignore(x => x.PhotoUrl);
            Ignore(x => x.PhotoTitle);

        }
    }
}