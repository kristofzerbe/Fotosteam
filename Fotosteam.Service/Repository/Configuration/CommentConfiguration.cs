using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        internal CommentConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Comment");
            HasKey(x => x.CommentId);

            Property(x => x.CommentId)
                .HasColumnName("CommentId")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ParentCommentId).HasColumnName("ParentCommentId").IsOptional();
            Property(x => x.PhotoId).HasColumnName("PhotoId").IsRequired();
            Property(x => x.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(256);
            Property(x => x.UserAlias).HasColumnName("UserAlias").HasMaxLength(256);
            Property(x => x.Text).HasColumnName("Text").IsRequired();
            Property(x => x.UserAvatarLink).HasColumnName("UserAvatarLink").IsOptional().HasMaxLength(2083);
            Property(x => x.TotalCount).HasColumnName("TotalCount").IsRequired();
            Property(x => x.Date).HasColumnName("Date");

            HasOptional(x => x.Parent).WithMany().HasForeignKey(x => x.ParentCommentId);
        }
    }
}