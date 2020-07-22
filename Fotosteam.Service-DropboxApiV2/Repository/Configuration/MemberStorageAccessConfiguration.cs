using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class MemberStorageAccessConfiguration : EntityTypeConfiguration<MemberStorageAccess>
    {
        internal MemberStorageAccessConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".MemberStorageAccess");
            HasKey(x => new {x.MemberId, x.Type});
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.Type).HasColumnName("StorageProviderId").IsRequired();
            Property(x => x.Secret).HasColumnName("Secret").IsRequired().HasMaxLength(256);
            Property(x => x.Token).HasColumnName("Token").IsRequired().HasMaxLength(1026);
            Property(x => x.UserId).HasColumnName("UserId");
        }
    }
}