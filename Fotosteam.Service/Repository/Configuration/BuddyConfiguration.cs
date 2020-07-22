using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class BuddyConfiguration : EntityTypeConfiguration<Buddy>
    {
        internal BuddyConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".MemberBuddy");
            HasKey(x => new {x.MemberId, x.BuddyMemberId});

            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.BuddyMemberId).HasColumnName("BuddyMemberId").IsRequired();
            Property(x => x.IsMutual).HasColumnName("IsMutual");
        }
    }
}