using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class DirectLinkConfiguration : EntityTypeConfiguration<DirectLink>
    {
        internal DirectLinkConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".DirectLink");
            HasKey(x => new {x.PhotoId, x.Size, x.Url});

            Property(x => x.PhotoId).HasColumnName("PhotoId").IsRequired();
            Property(x => x.Size).HasColumnName("Size").IsRequired();
            Property(x => x.Url).HasColumnName("Url").IsRequired().HasMaxLength(2083);
        }
    }
}