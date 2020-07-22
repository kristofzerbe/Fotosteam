using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class MemberSocialMediaConfiguration : EntityTypeConfiguration<SocialMedia>
    {
        internal MemberSocialMediaConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".MemberSocialMedia");
            HasKey(x => x.Id);

            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.Type).HasColumnName("Type").IsRequired();
            Property(x => x.Url).HasColumnName("Url").IsRequired().HasMaxLength(1026);
        }
    }
}