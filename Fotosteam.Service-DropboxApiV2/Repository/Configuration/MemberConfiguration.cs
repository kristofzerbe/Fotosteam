using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class MemberConfiguration : EntityTypeConfiguration<Member>
    {
        internal MemberConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Member");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Alias).HasColumnName("Alias").IsRequired().HasMaxLength(256);
            Property(x => x.PlainName).HasColumnName("PlainName").IsOptional().HasMaxLength(256);
            Property(x => x.Avatar100Url).HasColumnName("Avatar100Url").IsOptional().HasMaxLength(1026);
            Property(x => x.Avatar200Url).HasColumnName("Avatar200Url").IsOptional().HasMaxLength(1026);
            Property(x => x.AvatarColor).HasColumnName("AvatarColor").IsOptional().HasMaxLength(15);

            Property(x => x.Header640Url).HasColumnName("Header640Url").IsOptional().HasMaxLength(1026);
            Property(x => x.Header1024Url).HasColumnName("Header1024Url").IsOptional().HasMaxLength(1026);
            Property(x => x.Header1440Url).HasColumnName("Header1440Url").IsOptional().HasMaxLength(1026);
            Property(x => x.Header1920Url).HasColumnName("Header1920Url").IsOptional().HasMaxLength(1026);
            Property(x => x.HeaderColor).HasColumnName("HeaderColor").IsOptional().HasMaxLength(15);

            Property(x => x.AspNetUserId).HasColumnName("AspNetUserId").IsRequired().HasMaxLength(128);
            Property(x => x.Email).HasColumnName("Email").IsRequired().HasMaxLength(256);
            Property(x => x.Motto).HasColumnName("Motto").HasMaxLength(250);
            Property(x => x.Description).HasColumnName("Description").HasMaxLength(2000);
            Property(x => x.StorageAccessType).HasColumnName("StorageAccessType").IsOptional();
            Property(x => x.HomeLocation_Id).HasColumnName("HomeLocation_Id").IsOptional();

            HasMany(x => x.StorageAccesses).WithRequired().HasForeignKey(s => s.MemberId).WillCascadeOnDelete(true);
            HasMany(x => x.SocialMedias).WithOptional().HasForeignKey(s => s.MemberId).WillCascadeOnDelete(true);
            HasMany(m => m.Buddies).WithRequired().HasForeignKey(l => l.MemberId).WillCascadeOnDelete(true);
            HasMany(m => m.Stories).WithRequired().HasForeignKey(s => s.MemberId).WillCascadeOnDelete(true);
            HasMany(m => m.Locations).WithRequired().HasForeignKey(l => l.MemberId).WillCascadeOnDelete(true);
            HasMany(m => m.Events).WithRequired().HasForeignKey(e => e.MemberId).WillCascadeOnDelete(true);
            HasMany(m => m.Photos).WithRequired().HasForeignKey(e => e.MemberId).WillCascadeOnDelete(true);

            HasOptional(x => x.HomeLocation).WithMany().HasForeignKey(m => m.HomeLocation_Id);
            HasOptional(x => x.Options).WithRequired();
            Ignore(x => x.ProviderKey);
            Ignore(x => x.ProviderName);
            Ignore(x => x.ProviderUserName);
            Ignore(x => x.ProviderUserMail);
        }
    }
}