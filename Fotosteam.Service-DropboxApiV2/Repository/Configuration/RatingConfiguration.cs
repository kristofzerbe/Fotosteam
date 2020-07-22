using System.Data.Entity.ModelConfiguration;
using System.Security.Cryptography.X509Certificates;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class RatingConfiguration : EntityTypeConfiguration<Rating>
    {
        internal RatingConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Rating");
            HasKey(x => new {x.PhotoId, x.UserAlias, x.Value});

            Property(x => x.PhotoId).HasColumnName("PhotoId").IsRequired();
            Property(x => x.UserAlias).HasColumnName("UserName").IsRequired().HasMaxLength(256);
            Property(x => x.UserAlias).HasColumnName("UserAlias").HasMaxLength(256);
            Property(x => x.UserAvatarLink).HasColumnName("UserAvatarLink").IsOptional().HasMaxLength(2083);
            Property(x => x.Value).HasColumnName("Value").IsRequired();
            Property(x => x.Date).HasColumnName("Date");
            Property(x => x.MemberId).HasColumnName("MemberId");

            //Kristof: obsolet, da nun die Bewertungen summiert werden
            //Property(x => x.AverageRating).HasColumnName("AverageRating").IsRequired();

            Ignore(x => x.RatingSum);
        }
    }
}