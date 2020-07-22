using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class EventConfiguration : EntityTypeConfiguration<Event>
    {
        internal EventConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Event");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            Property(x => x.Date).HasColumnName("Date").IsOptional();
            Property(x => x.DateTo).HasColumnName("DateTo").IsOptional();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(4000);
            Ignore(x => x.PhotoCount);
            
            //HasOptional(x => x.Location).WithMany().Map(m => { m.MapKey("LocationId"); });
            Property(x => x.LocationId).HasColumnName("LocationId").IsOptional();
        }
    }
}