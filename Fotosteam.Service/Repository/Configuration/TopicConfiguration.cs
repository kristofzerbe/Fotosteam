using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class TopicConfiguration : EntityTypeConfiguration<Topic>
    {
        internal TopicConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Topic");
            HasKey(x => new {x.Id});

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(2000);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Ignore(x => x.PhotoCount);
            Ignore(x => x.Member);
        }
    }
}