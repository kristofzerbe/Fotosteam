using System.Data.Entity.ModelConfiguration;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository.Configuration
{
    internal class ExifDataConfiguration : EntityTypeConfiguration<ExifData>
    {
        internal ExifDataConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ExifData");
            HasKey(x => x.PhotoId);
            Property(x => x.PhotoId).HasColumnName("PhotoId").IsRequired();
            Property(x => x.Make).HasColumnName("Make").HasMaxLength(200).IsOptional();
            Property(x => x.Model).HasColumnName("Model").HasMaxLength(200).IsOptional();
            Property(x => x.CaptureDate).HasColumnName("CaptureDate").IsOptional();
            Property(x => x.Description).HasColumnName("Description").HasMaxLength(2000).IsOptional();
            Property(x => x.Software).HasColumnName("Software").HasMaxLength(200).IsOptional();
            Property(x => x.Artist).HasColumnName("Artist").HasMaxLength(200).IsOptional();
            Property(x => x.Copyright).HasColumnName("Copyright").HasMaxLength(200).IsOptional();
            Property(x => x.FocalLength).HasColumnName("FocalLength").IsOptional();
            Property(x => x.FNumber).HasColumnName("FNumber").IsOptional();
            Property(x => x.ApertureValue).HasColumnName("ApertureValue").IsOptional();
            Property(x => x.MaxApertureValue).HasColumnName("MaxApertureValue").IsOptional();
            Property(x => x.ExposureTime).HasColumnName("ExposureTime").IsOptional();
            Property(x => x.ExposureBiasValue).HasColumnName("ExposureBiasValue").IsOptional();
            Property(x => x.ExposureProgram).HasColumnName("ExposureProgram").IsOptional();
            Property(x => x.ExposureMode).HasColumnName("ExposureMode").IsOptional();
            Property(x => x.ISOSpeedRatings).HasColumnName("ISOSpeedRatings").IsOptional();
            Property(x => x.MeteringMode).HasColumnName("MeteringMode").IsOptional();
            Property(x => x.XResolution).HasColumnName("XResolution").IsOptional();
            Property(x => x.YResolution).HasColumnName("YResolution").IsOptional();
            Property(x => x.ResolutionUnit).HasColumnName("ResolutionUnit").IsOptional();
            Property(x => x.Width).HasColumnName("Width").IsOptional();
            Property(x => x.Height).HasColumnName("Height").IsOptional();
            Property(x => x.Orientation).HasColumnName("Orientation").HasMaxLength(20).IsOptional();
            Property(x => x.Latitude).HasColumnName("Latitude").IsOptional();
            Property(x => x.Longitude).HasColumnName("Longitude").IsOptional();
            Ignore(x => x.Location);
            Ignore(x => x.Title);
            Ignore(x => x.OriginalOrientation);
            Ignore(x => x.Keywords);
        }
    }
}