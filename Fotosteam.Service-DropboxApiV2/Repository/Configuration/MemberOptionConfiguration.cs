using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class MemberOptionConfiguration : EntityTypeConfiguration<MemberOption>
    {
        internal MemberOptionConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".MemberOption");
            HasKey(x => x.MemberId);
            Property(x => x.MemberId).HasColumnName("MemberId").IsRequired();
            Property(x => x.AllowComments).HasColumnName("AllowComments");
            Property(x => x.AllowRating).HasColumnName("AllowRating");
            Property(x => x.AllowSharing).HasColumnName("AllowSharing");
            Property(x => x.DisplayEmailAddress).HasColumnName("DisplayEmailAddress");
            Property(x => x.DisplayNotifications).HasColumnName("DisplayNotifications");
            Property(x => x.IsAvailableForProjects).HasColumnName("IsAvailableForProjects");
            Property(x => x.IsProfessional).HasColumnName("IsProfessional");
            Property(x => x.NotifyByEmailOnBuddyAddedPhoto).HasColumnName("NotifyByEmailOnBuddyAddedPhoto");
            Property(x => x.NotifyByEmailOnBuddyAdd).HasColumnName("NotifyByEmailOnBuddyAdd");
            Property(x => x.NotifyByEmailOnBuddyConfirmation).HasColumnName("NotifyByEmailOnBuddyConfirmation");
            Property(x => x.NotifyByEmailOnComment).HasColumnName("NotifyByEmailOnComment");
            Property(x => x.NotifyByEmailOnRating).HasColumnName("NotifyByEmailOnRating");
            Property(x => x.Language).HasColumnName("Language");
            Property(x => x.NotifyByEmailOnNews).HasColumnName("NotifyByEmailOnNews");
            Property(x => x.UseDropboxWebhook).HasColumnName("UseDropboxWebhook");
            Property(x => x.DefaultIsPrivate).HasColumnName("DefaultIsPrivate");
            Property(x => x.DefaultLicense).HasColumnName("DefaultLicense");
            Property(x => x.DefaultAllowFullSizeDownload).HasColumnName("DefaultAllowFullSizeDownload");
            Property(x => x.DefaultAllowPromoting).HasColumnName("DefaultAllowPromoting");
            Property(x => x.DefaultAllowRating).HasColumnName("DefaultAllowRating");
            Property(x => x.DefaultAllowCommenting).HasColumnName("DefaultAllowCommenting");
            Property(x => x.OverwriteExistingPhoto).HasColumnName("OverwriteExistingPhoto");

        }
    }
}