using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class StorageProviderConfiguration : EntityTypeConfiguration<StorageProvider>
    {
        internal StorageProviderConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".StorageProvider");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsOptional().HasMaxLength(20);
        }
    }
}