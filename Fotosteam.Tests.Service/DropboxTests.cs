using System;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Fotosteam.Service.Connector.Dropbox;
using Fotosteam.Service.Imaging;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Properties;
using ImageMagick;
using log4net;
using log4net.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fotosteam.Tests.Service
{
    /// <summary>
    /// Diese Test-Klasse besteht hauptsächlich aus manuellen Tests,
    /// die nur unter Anpassung von Werten, z.B. Secret/Token funktioieren.
    /// Außerdem hat es keine Anpassung in Bezug der Datenbank gegeben
    /// </summary>
    [TestClass]
    public class DropboxTests
    {
        private DropboxConnector _target = new DropboxConnector(RequestHelper.CurrentDataRepository);
        private const string Secret = "j9884tvio5kofxy";
        private const string Token = "f19p00ll9j1gm3ls";

        private void SetUpConnection()
        {
            _target = new DropboxConnector(RequestHelper.CurrentDataRepository);

        }

        [TestMethod]
        public void GetUserInformations()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void GetAvatar()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void GetPhoto()
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void GetMetaData()
        {

            var imageStream = new MemoryStream(File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"\TestData\full.jpg"));
            var result = Processing.ReadExifData(imageStream);

            var expected = new ExifData
            {
                ApertureValue = 4,
                CaptureDate = new DateTime(2014, 9, 18, 19, 24, 07),
                ExposureBiasValue = 0.33333333333333331,
                ExposureProgram = 3,
                ExposureTime = 0.2,
                FNumber = 4,
                FocalLength = 45.0,
                Make = "Canon",
                Model = "Canon EOS 6D",
                Title = "Der Kristof als Title"
            };

            Assert.AreEqual(expected.ApertureValue, result.ApertureValue);
            Assert.AreEqual(expected.CaptureDate, result.CaptureDate);
            Assert.AreEqual(expected.ExposureBiasValue, result.ExposureBiasValue);
            Assert.AreEqual(expected.ExposureProgram, result.ExposureProgram);
            Assert.AreEqual(expected.ExposureTime, result.ExposureTime);
            Assert.AreEqual(expected.FNumber, result.FNumber);
            Assert.AreEqual(expected.FocalLength, result.FocalLength);
            Assert.AreEqual(expected.Make, result.Make);
            Assert.AreEqual(expected.Model, result.Model);
            Assert.AreEqual(expected.Title, result.Title);
        }
        [TestMethod]
        public void CreateThumbNails()
        {
            const string startDir = @"C:\Users\Robert\Dropbox\Apps\fotosteamDebug\Internal\Fotos";
            var dir = System.IO.Directory.GetDirectories(startDir);
            foreach (var d in dir)
            {
                var source = new Bitmap(d + "\\full.jpg");
                var stream = new MemoryStream();
                source.Save(stream, ImageFormat.Jpeg);

                var processing = new Processing();
                new Bitmap(processing.ResizeImage(stream, 100, true)).Save(d + "\\100.jpg");
                new Bitmap(processing.ResizeImage(stream, 200, true)).Save(d + "\\200.jpg");
                new Bitmap(processing.ResizeImage(stream, 400, true)).Save(d + "\\400.jpg");
            }

        }

        [TestMethod]
        public void Uploading_a_picture_will_rotated_it_correctly()
        {
            var image = Resources.rotated;
            Stream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);

            var exif = Processing.ReadExifData(stream);
            var result = Processing.CorrectImageRotation(ref stream, exif);
            image = new Bitmap(stream);
            image.Save(@"C:\temp\corrected.jgp", ImageFormat.Jpeg);

            Assert.Equals(result, RotateFlipType.Rotate270FlipNone);
        }

        [TestMethod]
        public void Categories_and_topics_will_be_recognized()
        {
            var image = Resources.keyWords;
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            stream.Seek(0, SeekOrigin.Begin);
            using (MagickImage nImage = new MagickImage(stream))
            {
                //var profile = nImage.GetExifProfile();
                var iptcProfle = nImage.GetIptcProfile();
                //var xmpProfile = nImage.GetXmpProfile();
                var keywords = iptcProfle.Values.Where(v => v.Tag == IptcTag.Keyword);
                if (keywords.Any())
                {
                    var list = keywords.Select(v => v.Value.ToString()).ToList();
                    Assert.IsTrue(list.Any(), "Es wurden keine Schlüsselwörter gefunden");
                    CategoryType result = CategoryType.NotSet;

                    foreach (var keyword in list)
                    {
                        CategoryType category;

                        if (Enum.TryParse(keyword, true, out category))
                        {
                            result = result | category;
                        }
                    }
                }
                else
                {
                    Assert.Fail("Es wurden keine Schlüsselwörter gefunden");
                }
            }

        }

        [TestMethod]
        public void CreateThumbnail_from_a_certain_area()
        {

            var image = Resources.chess;
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            var processing = new Processing();
            new Bitmap(processing.ResizeImage(stream, 400, true, 0.5f, 0f)).Save("400.jpg");
            stream.Dispose();
        }
        [TestMethod]
        public void Optimizing_an_image_will_reduce_the_size()
        {
            var image = Resources.full;
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            var size = stream.Length;
            var mImage = new MagickImage(stream);

            var newImage = new Bitmap(Processing.OptimizeImage(mImage));

            newImage.Save(stream, ImageFormat.Jpeg);
            newImage.Save("jpgtan.jpg");
            var newSize = stream.Length;

            var stream1 = new MemoryStream();
            image = Resources.full;
            image.Save(stream1, ImageFormat.Jpeg);
            var outputStream = new MemoryStream();
            stream1.Seek(0, SeekOrigin.Begin);
            using (MagickImage nImage = new MagickImage(stream1))
            {
                nImage.Strip();
                nImage.Resize(1920, 0);
                nImage.Interlace = Interlace.Plane;
                nImage.Quality = 80;
                nImage.FilterType = FilterType.Lanczos2;
                nImage.GaussianBlur(0.05, 10);
                nImage.Write(outputStream, MagickFormat.Jpg);
                nImage.AddProfile(ColorProfile.SRGB);
                nImage.Write("MagickNet.jpg");

                nImage.Resize(100, 100);
                var colors = nImage.Histogram().OrderByDescending(h => h.Value);
                nImage.Depth = 8;
                var opt = new JpegOptimizer();
                opt.Progressive = true;
                opt.LosslessCompress("MagickNet.jpg");
                nImage.Quantize(new QuantizeSettings() { });
                var color = nImage.Histogram().FirstOrDefault().Key.ToColor();



            }
            var newSize1 = outputStream.Length;
            Assert.IsTrue(newSize < size, "Die Bildgröße hat sich nicht geändert");
            Assert.IsTrue(newSize1 < newSize, "Die Bildgröße von Magick war nicht kleiner sich nicht geändert");
        }

        [TestMethod]
        public void Getting_title_from_image_will_return_title()
        {
            var image = Resources.full;
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            string title = null;
            stream.Seek(0, SeekOrigin.Begin);
            using (MagickImage nImage = new MagickImage(stream))
            {
                var profile = nImage.GetExifProfile();
                var iptcProfle = nImage.GetIptcProfile();
                var xmpProfile = nImage.GetXmpProfile();

                var doc = xmpProfile.ToXDocument();
                var pa = xmpProfile.ToIXPathNavigable();

                var exifTitle = profile.GetValue(ExifTag.DocumentName);
                var comment = profile.GetValue(ExifTag.ImageDescription).Value.ToString();
                var tags = iptcProfle.GetValue(IptcTag.SupplementalCategories).Value.ToString();
                if (exifTitle == null)
                {
                    var title1 = nImage.GetIptcProfile().GetValue(IptcTag.Title);
                    if (title1.Value != null)
                    {
                        title = title1.Value;
                    }
                }
                else
                {
                    title = exifTitle.Value.ToString();
                }
            }
            Assert.IsTrue(title != null, "Kein Titel gefudnen");
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(DropboxTests));

        //Das ist keine echte Testmethode, aber sinnvoll um eventuell Informationen aus den Exif-Daten zu aktualisieren
        //[TestMethod]
        public void UpdateThumbs()
        {
            var repository = new DataRepository();
            var connector = new DropboxConnector(repository);
            var member = new Member();
            var photos = repository.Queryable<Photo>().Include(p => p.DirectLinks)
                .Include(p => p.Exif).Where(p => p.CaptureDate == DateTime.MinValue).ToList();

            foreach (var photo in photos)
            {
                if (photo.DirectLinks == null)
                    continue;

                Log.Info(string.Format("Bearbeite Foto {0} von {3}, mit Namen {1} und Ordner {2}", photo.Id, photo.Name,
                    photo.Folder, photo.MemberId));

                var link = photo.DirectLinks.FirstOrDefault(d => d.Size == 0);
                if (link != null)
                {
                    var url = link.Url;
                    var image = GetImageFromURL(url);
                    if (image == null)
                    {
                        Log.Info(string.Format("Die Url {0} ist nicht gültig", url));
                        continue;
                    }

                    var stream = new MemoryStream();
                    image.Write(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var exif = Processing.ReadExifData(stream);
                    if (exif != null)
                    {
                        member.Id = photo.MemberId;
                        connector.CurrentMember = member;

                        exif.PhotoId = photo.Id;
                        if ((string.IsNullOrEmpty(photo.Title) || photo.Title.Equals("Untitled"))
                            && string.IsNullOrEmpty(exif.Title) && !exif.Title.Equals("Untitled"))
                            photo.Title = exif.Title;

                        photo.Exif = exif;
                        connector.ExtractCategoriesAndTopics(photo);
                        photo.CaptureDate = exif.CaptureDate;
                        repository.Update(photo);

                    }

                    stream.Close();
                    stream.Dispose();
                    image.Dispose();
                }
            }
        }

        [TestMethod]
        public void UpdateDateFields()
        {
            var repository = new DataRepository();
            var connector = new DropboxConnector(repository);
            var member = new Member();
            var photos = repository.Queryable<Photo>().Include(p => p.DirectLinks)
                .Include(p => p.Exif).Where(p => p.MemberId ==33).ToList();

            foreach (var photo in photos)
            {
                if (photo.DirectLinks == null)
                    continue;

                Log.Info(string.Format("Bearbeite Foto {0} von {3}, mit Namen {1} und Ordner {2}", photo.Id, photo.Name,
                    photo.Folder, photo.MemberId));

                var link = photo.DirectLinks.FirstOrDefault(d => d.Size == 0);
                if (link != null)
                {
                    var url = link.Url;
                    var image = GetImageFromURL(url);
                    if (image == null)
                    {
                        Log.Info(string.Format("Die Url {0} ist nicht gültig", url));
                        continue;
                    }

                    var stream = new MemoryStream();
                    image.Write(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var processing = new Processing();
                    var sizes = new[] {400, 200, 100};
                    foreach (var size in sizes)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        
                        processing.ResizeImage(stream, size, true, (float)photo.Left, (float)photo.Top);
                        var exif = Processing.ReadExifData(stream);
                        if (exif != null)
                        {
                            member.Id = photo.MemberId;
                            connector.CurrentMember = member;

                            exif.PhotoId = photo.Id;
                            if ((string.IsNullOrEmpty(photo.Title) || photo.Title.Equals("Untitled"))
                                && string.IsNullOrEmpty(exif.Title) && !exif.Title.Equals("Untitled"))
                                photo.Title = exif.Title;
                            
                            connector.ExtractCategoriesAndTopics(photo);
                            photo.CaptureDate = exif.CaptureDate;
                            repository.Update(photo);

                        }
                    }
                    stream.Close();
                    stream.Dispose();
                    image.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the image from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private static MagickImage GetImageFromURL(string url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var stream = httpWebReponse.GetResponseStream();
                var image = new MagickImage(stream);
                
                if (stream != null)
                    stream.Close();
                return image;

            }
            catch { }
            return null;

        }

        [TestMethod]
        public void Category_Flags_will_return_correct_int_value()
        {
            var mulitCategory = CategoryType.Nature | CategoryType.Animals;
            Assert.IsTrue((int) CategoryType.Architecture == 2, "Der Wert stimmt nicht überein");
            Assert.IsTrue((int)CategoryType.Landscape == 8, "Der Wert stimmt nicht überein");
            Assert.IsTrue((int) CategoryType.Technics == 4096, "Der Wert stimmt nicht überein");
            Assert.IsTrue((int)mulitCategory == 528, "Der Wert stimmt nicht überein");
        }
    }
}
