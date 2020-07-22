using System;
using System.IO;
using System.Drawing;
using ExifLib;
using Fotosteam.Imaging.Dto;

namespace Fotosteam.Imaging {
    public static class Processing {

        public static Image ResizeImage(Image source, RectangleF destinationBounds) {
            var sourceBounds = new RectangleF(0.0f, 0.0f, source.Width, source.Height);

            float resizeRatio;
            var sourceRatio = source.Width / source.Height;
            var scaleHeight = destinationBounds.Height;
            var scaleWidth = destinationBounds.Width;

            if (sourceRatio >= 1.0f) {
                //landscape
                resizeRatio = sourceBounds.Height / sourceBounds.Width;
                scaleHeight = destinationBounds.Height * resizeRatio;
            } else {
                //portrait
                resizeRatio = sourceBounds.Width / sourceBounds.Height;
                scaleWidth = destinationBounds.Width * resizeRatio;

            }
            Image destinationImage = new Bitmap((int)scaleWidth, (int)scaleHeight);
            Graphics graph = Graphics.FromImage(destinationImage);
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Fill with background color
            graph.FillRectangle(new SolidBrush(Color.Transparent), destinationBounds);

            graph.DrawImage(source, 0, 0, scaleWidth, scaleHeight);
            return destinationImage;

        }

        public static MetaInformation ReadExifData(MemoryStream sourceStream) {            

            sourceStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new ExifReader(sourceStream)) {
                var meta = new MetaInformation();
                var exif = new ExifData();

                exif.Make = SetExifData<string>(reader, ExifTags.Make);
                exif.Model = SetExifData<string>(reader, ExifTags.Model);
                exif.CaptureDate = SetExifData<DateTime>(reader, ExifTags.DateTimeOriginal);
                exif.Description = SetExifData<string>(reader, ExifTags.ImageDescription);
                exif.Artist = SetExifData<string>(reader, ExifTags.Artist);
                exif.Copyright = SetExifData<string>(reader, ExifTags.Copyright);
                exif.Software = SetExifData<string>(reader, ExifTags.Software);
                exif.FocalLength = SetExifData<double>(reader, ExifTags.FocalLength);
                exif.FNumber = SetExifData<double>(reader, ExifTags.FNumber);
                exif.ApertureValue = SetExifData<double>(reader, ExifTags.ApertureValue);
                exif.MaxApertureValue = SetExifData<double>(reader, ExifTags.MaxApertureValue);
                exif.ExposureTime = SetExifData<double>(reader, ExifTags.ExposureTime);
                exif.ExposureBiasValue = SetExifData<double>(reader, ExifTags.ExposureBiasValue);
                exif.ExposureProgram = SetExifData<ushort>(reader, ExifTags.ExposureProgram);
                exif.ExposureMode = SetExifData<ushort>(reader, ExifTags.ExposureMode);
                exif.ISOSpeedRatings = SetExifData<ushort>(reader, ExifTags.ISOSpeedRatings);
                exif.MeteringMode = SetExifData<ushort>(reader, ExifTags.MeteringMode);

                double x = SetExifData<double>(reader, ExifTags.XResolution);
                double y = SetExifData<double>(reader, ExifTags.YResolution);
                exif.XResolution = x;
                exif.YResolution = y;
                exif.ResolutionUnit = SetExifData<ushort>(reader, ExifTags.ResolutionUnit);

                var image = new Bitmap(sourceStream);
                exif.Width = image.Width;
                exif.Height = image.Height;
                exif.AspectRatio = (double)image.Width / (double)image.Height;
                exif.Orientation = image.Width == image.Height ? "square" : (image.Width > image.Height ? "landscape" : "portrait");

                double[] lat = SetExifData<double[]>(reader, ExifTags.GPSLatitude);
                double[] lng = SetExifData<double[]>(reader, ExifTags.GPSLongitude);
                string latRef = SetExifData<string>(reader, ExifTags.GPSLatitudeRef);
                string lngRef = SetExifData<string>(reader, ExifTags.GPSLongitudeRef);

                if (lat != null){
                    double latitude = 0;
                    double longitude = 0;
                    latitude = lat[0] +
                        lat[1] / 60.0 +
                        lat[2] / 3600.0;
                    if (latRef == "S") latitude = -latitude;
                    longitude = lng[0] +
                        lng[1] / 60.0 +
                        lng[2] / 3600.0;
                    if (lngRef == "W") longitude = -longitude;

                    exif.Latitude = latitude;
                    exif.Longitude = longitude;                    
                }

                meta.Exif = exif;
                return meta;
            }
        }

        private static T SetExifData<T>(ExifReader reader, ExifTags tag) {
            T outValue;
            reader.GetTagValue(tag, out outValue);
            return outValue;
        }


    }
}
