using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        internal LocationConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Location");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(1000);
            Property(x => x.CountryIsoCode).HasColumnName("CountryIsoCode").IsRequired().HasMaxLength(3);
            Property(x => x.Longitude).HasColumnName("Longitude").IsOptional();
            Property(x => x.Latitude).HasColumnName("Latitude").IsOptional();
            Property(x => x.City).HasColumnName("City").HasMaxLength(256);
            Property(x => x.Country).HasColumnName("Country").HasMaxLength(256);
            Property(x => x.County).HasColumnName("County").HasMaxLength(256);
            Property(x => x.Street).HasColumnName("Street").HasMaxLength(256);
            Property(x => x.PhotoCount).HasColumnName("PhotoCount");
        }
    }
}