using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Fotosteam.Service.Connector.Dropbox;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;
using ImageMagick;
using log4net;
using RestSharp;

// ReSharper disable CompareOfFloatsByEqualityOperator : Nachkommastellen sind für uns nicht relevant

namespace Fotosteam.Service.Imaging
{
    /// <summary>
    ///     Die Klasse kaspelt Methoden zum Bearbeiten von Bildern,
    ///     wie z.B. die Erzeugung von Thumbnails, Auslesen der Exif-Daten
    /// </summary>
    public class Processing
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Processing));
        private static readonly Dictionary<string, string> Countries = new Dictionary<string, string>();
        private static string _currentPath;

        private static string CurrentPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentPath))
                    return _currentPath;

                _currentPath = AppDomain.CurrentDomain.BaseDirectory;
                if (!_currentPath.EndsWith("\\")) _currentPath = _currentPath + "\\";

                return _currentPath;
            }
        }

        /// <summary>
        ///     Ändert die Größe eines Bildes
        /// </summary>
        /// <param name="stream">Der Memorystream des Ausgangsbild</param>
        /// <param name="sizeValue">gibt das größte Maß vor</param>
        /// <param name="crop">Wenn ja, dann wir ein Quadratisches Bild erzeugt</param>
        /// <param name="xOffset">Abstand von links</param>
        /// <param name="yOffset">Abstand von oben</param>
        /// <param name="forceOffset">Gibt an ob der Abstand überschrieben werden soll</param>
        /// <returns>Das geänderte Bild</returns>
        public Stream ResizeImage(Stream stream, int sizeValue, bool crop = false, 
            float xOffset = 0, float yOffset = 0, bool forceOffset = false)
        {
            var watch = Stopwatch.StartNew();
            stream.Seek(0, SeekOrigin.Begin);
            using (var newImage = new MagickImage(stream))
            {

                Rectangle sourceBounds;
                var width = newImage.Width;
                var height = newImage.Height;
                var sourceRatio = width / height;
                var edgeX = (int)((xOffset) * width);
                var edgeY = (int)((yOffset) * height);

                if (sourceRatio >= 1.0f)
                {
                    //landscape
                    if (crop)
                    {
                        if (xOffset == 0 && yOffset == 0 && !forceOffset )
                        {
                            sourceBounds = new Rectangle((width - height) / 2, 0, height, height);
                        }
                        else
                        {
                            sourceBounds = new Rectangle(edgeX, edgeY, height, height);
                        }
                        newImage.Crop(new MagickGeometry(sourceBounds));
                    }
                    newImage.Resize(sizeValue, 0);
                }
                else
                {
                    //portrait
                    if (crop)
                    {
                        if (xOffset == 0 && yOffset == 0 && !forceOffset)
                        {
                            sourceBounds = new Rectangle(0, (height - width) / 2, width, width);
                        }
                        else
                        {
                            sourceBounds = new Rectangle(edgeX, edgeY, width, width);
                        }
                        newImage.Crop(new MagickGeometry(sourceBounds));
                    }
                    newImage.Resize(0, sizeValue);
                }

                var fileName = GetRandomFilenameAndCreateDirectory();

                newImage.Strip();
                newImage.Quality = (sizeValue < 640) ?  55 : 80;
                
                newImage.Write(fileName);

                Debug.Print("Resize {0}:{1}", sizeValue, watch.Elapsed);
                var newStream= OptimizeImage(fileName);
                File.Delete(fileName);
                return newStream;
            }

        }

        public static RotateFlipType CorrectImageRotation(ref Stream stream, ExifData exif)
        {

            if (exif.OriginalOrientation == 0)
                return RotateFlipType.RotateNoneFlipNone;

            var flip = OrientationToFlipType(exif.OriginalOrientation);

            if (flip == RotateFlipType.RotateNoneFlipNone) return RotateFlipType.RotateNoneFlipNone;

            stream.Seek(0, SeekOrigin.Begin);
            var image = new Bitmap(stream);
            stream.Close();

            image.RotateFlip(flip);
            stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            stream.Seek(0, SeekOrigin.Begin);

            exif.Width = image.Width;
            exif.Height = image.Height;
            exif.AspectRatio = image.Width / (double)image.Height;
            exif.Orientation = image.Width == image.Height
                ? "square"
                : (image.Width > image.Height ? "landscape" : "portrait");

            return flip;
        }

        private static RotateFlipType OrientationToFlipType(int orientation)
        {
            switch (orientation)
            {
                case 1:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }

        internal static Stream OptimizeImage(MagickImage sourceImage)
        {
            var fileName = GetRandomFilenameAndCreateDirectory();
            sourceImage.Write(fileName);
            return OptimizeImage(fileName);
        }

        private static string GetRandomFilenameAndCreateDirectory()
        {

            var path = string.Format("{0}\\temp", CurrentPath);
            var fileName = string.Format("{0}\\{1}", path, Path.GetRandomFileName());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return fileName;
        }

        private static Stream OptimizeImage(string fileName)
        {
            var watch = Stopwatch.StartNew();
            var imageOptimizer = new Process();
            imageOptimizer.StartInfo.Arguments =
                string.Format(" -optimize -progressive -copy none -outfile \"{0}\" \"{1}\"", fileName, fileName);
            imageOptimizer.StartInfo.FileName = string.Format("{0}lib\\jpegtran.exe", CurrentPath);
            imageOptimizer.StartInfo.CreateNoWindow = true;
            imageOptimizer.StartInfo.UseShellExecute = false;

            imageOptimizer.Start();
            imageOptimizer.WaitForExit();

            var stream = new MemoryStream(File.ReadAllBytes(fileName));
            stream.Seek(0, SeekOrigin.Begin);
            
            Debug.Print("OptimizeImage {0}:{1}", fileName, watch.Elapsed);
            return stream;
        }

        /// <summary>
        ///     Liest die Exifdaten eines Bilder aus
        /// </summary>
        /// <param name="sourceStream">Das Bild als Memorysteam</param>
        /// <returns>Ein <see cref="ExifData"/>zurück </returns>
        public static ExifData ReadExifData(Stream sourceStream)
        {
            //Den Stream kopieren, damit der Reader ihn nicht verwirft
            var readerStream = new MemoryStream();
            sourceStream.Seek(0, SeekOrigin.Begin);
            sourceStream.CopyTo(readerStream);
            readerStream.Seek(0, SeekOrigin.Begin);
            var magickImage = new MagickImage(readerStream);
            try
            {
                var exif = new ExifData();
                var profile = magickImage.GetExifProfile();
                var itpcProfile = magickImage.GetIptcProfile();

                exif.Title = GetExifDataFromTag(itpcProfile, IptcTag.Title);
                exif.Make = GetExifDataFromTag<string>(profile, ExifTag.Make);
                exif.Model = GetExifDataFromTag<string>(profile, ExifTag.Model);
                exif.CaptureDate = GetExifDataFromTag<DateTime>(profile, ExifTag.DateTimeOriginal);
                exif.Description = GetExifDataFromTag<string>(profile, ExifTag.ImageDescription);
                exif.Artist = GetExifDataFromTag<string>(profile, ExifTag.Artist);
                exif.Copyright = GetExifDataFromTag<string>(profile, ExifTag.Copyright);
                exif.Software = GetExifDataFromTag<string>(profile, ExifTag.Software);
                exif.FocalLength = GetExifDataFromTag<double>(profile, ExifTag.FocalLength);
                exif.FNumber = GetExifDataFromTag<double>(profile, ExifTag.FNumber);
                exif.ApertureValue = GetExifDataFromTag<double>(profile, ExifTag.ApertureValue);
                exif.MaxApertureValue = GetExifDataFromTag<double>(profile, ExifTag.MaxApertureValue);
                exif.ExposureTime = GetExifDataFromTag<double>(profile, ExifTag.ExposureTime);
                exif.ExposureBiasValue = GetExifDataFromTag<double>(profile, ExifTag.ExposureBiasValue);
                exif.ExposureProgram = GetExifDataFromTag<ushort>(profile, ExifTag.ExposureProgram);
                exif.ExposureMode = GetExifDataFromTag<ushort>(profile, ExifTag.ExposureMode);
                exif.ISOSpeedRatings = GetExifDataFromTag<ushort>(profile, ExifTag.ISOSpeedRatings);
                exif.MeteringMode = GetExifDataFromTag<ushort>(profile, ExifTag.MeteringMode);
                exif.OriginalOrientation = GetExifDataFromTag<ushort>(profile, ExifTag.Orientation);

                if (itpcProfile != null)
                {
                    var keyWords = itpcProfile.Values.Where(v => v.Tag == IptcTag.Keyword).ToList();
                    if (keyWords.Any())
                    {
                        exif.Keywords =
                            itpcProfile.Values.Where(v => v.Tag == IptcTag.Keyword)
                                .Select(v => v.Value.ToString())
                                .ToList();
                    }
                }

                var x = GetExifDataFromTag<double>(profile, ExifTag.XResolution);
                var y = GetExifDataFromTag<double>(profile, ExifTag.YResolution);
                exif.XResolution = x;
                exif.YResolution = y;
                exif.ResolutionUnit = GetExifDataFromTag<ushort>(profile, ExifTag.ResolutionUnit);

                GetRequiredExifData(magickImage, exif);
                SetLocationData(profile, exif);
                return exif;

            }
            catch
            {
                var data = new ExifData();
                GetRequiredExifData(magickImage, data);
                return data;
            }
            finally
            {
                magickImage.Dispose();
            }

        }

        private static void GetRequiredExifData(MagickImage image, ExifData exif)
        {
            exif.Width = image.Width;
            exif.Height = image.Height;
            exif.AspectRatio = image.Width / (double)image.Height;
            exif.Orientation = image.Width == image.Height
                ? "square"
                : (image.Width > image.Height ? "landscape" : "portrait");

            if (string.IsNullOrEmpty(exif.Title)) exif.Title = "Untitled";
        }

        private static void SetLocationData(ExifProfile profile, ExifData exif)
        {
            var lat = GetExifDataFromTag<double[]>(profile, ExifTag.GPSLatitude);
            var lng = GetExifDataFromTag<double[]>(profile, ExifTag.GPSLongitude);
            var latRef = GetExifDataFromTag<string>(profile, ExifTag.GPSLatitudeRef);
            var lngRef = GetExifDataFromTag<string>(profile, ExifTag.GPSLongitudeRef);

            if (lat != null)
            {
                var latitude = lat[0] + lat[1] / 60.0 + lat[2] / 3600.0;
                if (latRef == "S")
                {
                    latitude = -latitude;
                }
                var longitude = lng[0] + lng[1] / 60.0 + lng[2] / 3600.0;

                if (lngRef == "W")
                {
                    longitude = -longitude;
                }

                exif.Latitude = latitude;
                exif.Longitude = longitude;
                exif.Location = SetLocationDetails(latitude, longitude);
            }
        }

        internal static Location SetLocationDetails(double latitude, double longitude)
        {
            // > http://maps.googleapis.com/maps/api/geocode/json?latlng=LATITUDE,LONGITUDE&sensor=true
            
            var request = DropboxConnector.CreateGeoLocationRequest(longitude, latitude);
            try
            {
                var restClient = GetClient("http://maps.googleapis.com");

                var response = restClient.Execute<GoogleGeoCodeResponse>(request);
                if (response.StatusCode == HttpStatusCode.OK && response.Data.results.Any())
                {
                    var components = response.Data.results[0].address_components;
                    var location = new Location
                    {
                        Latitude = latitude,
                        Longitude = longitude,
                        Country =
                            components.Where(x => x.types.Contains("country"))
                                .DefaultIfEmpty(new address_component())
                                .First()
                                .long_name,
                        CountryIsoCode =
                            components.Where(x => x.types.Contains("country"))
                                .DefaultIfEmpty(new address_component())
                                .First()
                                .short_name,
                        State =
                            components.Where(x => x.types.Contains("administrative_area_level_1"))
                                .DefaultIfEmpty(new address_component())
                                .First()
                                .long_name,
                        County =
                            components.Where(x => x.types.Contains("administrative_area_level_2"))
                                .DefaultIfEmpty(new address_component())
                                .First()
                                .long_name,
                        City =
                            components.Where(x => x.types.Contains("locality"))
                                .DefaultIfEmpty(new address_component())
                                .First()
                                .long_name,
                        Street =
                            components.Where(x => x.types.Contains("route"))
                                .DefaultIfEmpty(new address_component())
                                .First()
                                .long_name,
                        PhotoCount = 1
                    };
                    var arrayToConcat = new[] { location.City ?? location.County ?? location.State, location.Country };
                    location.Name = string.Join(", ", arrayToConcat
                        .Where(x => !string.IsNullOrEmpty(x)));

                    if (!string.IsNullOrEmpty(location.CountryIsoCode) && location.CountryIsoCode.Length == 2)
                    {
                        location.CountryIsoCode = GetIsoCode3(location.CountryIsoCode);
                    }

                    if (string.IsNullOrEmpty(location.CountryIsoCode))
                    {
                        location.CountryIsoCode = "???";
                    }
                    return location;
                }
            }
            catch (Exception ex)
            {
                //Wir ignorieren das, da das Auslesen auch noch manuell implementiert wird
                Log.Debug("Error setting location", ex);
            }
            return null;
        }

        private static RestClient GetClient(string baseUrl)
        {
            var restClient = new RestClient(baseUrl);
            restClient.ClearHandlers();
            restClient.AddHandler("*", new RestSharp.Deserializers.JsonDeserializer());
            return restClient;
        }

        private static T GetExifDataFromTag<T>(ExifProfile profile, ExifTag tag)
        {
            var outValue = default(T);
            if (profile == null)
                return outValue;

            try
            {
                outValue = (T)profile.GetValue(tag).Value;
            }
            catch (InvalidCastException)
            {
                if (tag == ExifTag.DateTimeOriginal || tag == ExifTag.DateTime || tag == ExifTag.DateTimeDigitized)
                {
                    var formats = new[] { "yyyy:MM:dd HH:mm:ss", "M-d-yyyy", "dd-MM-yyyy", "MM-dd-yyyy", "M.d.yyyy", "dd.MM.yyyy", "MM.dd.yyyy" };

                    object value = DateTime.ParseExact(profile.GetValue(tag).Value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                    return (T)value;
                }
            }
            catch
            {// Wert ist nicht vorhanden / konnte nicht konvertiert werden
            }

            return outValue;
        }
        private static string GetExifDataFromTag(IptcProfile profile, IptcTag tag)
        {
            if (profile == null)
                return null;

            var outValue = string.Empty;
            try
            {
                outValue = profile.GetValue(tag).Value;
            }
            catch
            {// Wert ist nicht vorhanden / konnte nicht konvertiert werden
            }

            return outValue;
        }


        private static string GetIsoCode3(string isoCode2)
        {
            if (Countries.Count == 0)
            {
                PrepareIsoCodes();
            }
            string code;
            Countries.TryGetValue(isoCode2, out code);
            return code;
        }

        private static void PrepareIsoCodes()
        {
            Countries.Add("AD", "AND");
            Countries.Add("AE", "ARE");
            Countries.Add("AF", "AFG");
            Countries.Add("AG", "ATG");
            Countries.Add("AI", "AIA");
            Countries.Add("AL", "ALB");
            Countries.Add("AM", "ARM");
            Countries.Add("AN", "ANT");
            Countries.Add("AO", "AGO");
            Countries.Add("AQ", "ATA");
            Countries.Add("AR", "ARG");
            Countries.Add("AS", "ASM");
            Countries.Add("AT", "AUT");
            Countries.Add("AU", "AUS");
            Countries.Add("AW", "ABW");
            Countries.Add("AX", "ALA");
            Countries.Add("AZ", "AZE");
            Countries.Add("BA", "BIH");
            Countries.Add("BB", "BRB");
            Countries.Add("BD", "BGD");
            Countries.Add("BE", "BEL");
            Countries.Add("BF", "BFA");
            Countries.Add("BG", "BGR");
            Countries.Add("BH", "BHR");
            Countries.Add("BI", "BDI");
            Countries.Add("BJ", "BEN");
            Countries.Add("BL", "BLM");
            Countries.Add("BM", "BMU");
            Countries.Add("BN", "BRN");
            Countries.Add("BO", "BOL");
            Countries.Add("BR", "BRA");
            Countries.Add("BS", "BHS");
            Countries.Add("BT", "BTN");
            Countries.Add("BV", "BVT");
            Countries.Add("BW", "BWA");
            Countries.Add("BY", "BLR");
            Countries.Add("BZ", "BLZ");
            Countries.Add("CA", "CAN");
            Countries.Add("CC", "CCK");
            Countries.Add("CD", "COD");
            Countries.Add("CF", "CAF");
            Countries.Add("CG", "COG");
            Countries.Add("CH", "CHE");
            Countries.Add("CI", "CIV");
            Countries.Add("CK", "COK");
            Countries.Add("CL", "CHL");
            Countries.Add("CM", "CMR");
            Countries.Add("CN", "CHN");
            Countries.Add("CO", "COL");
            Countries.Add("CR", "CRI");
            Countries.Add("CU", "CUB");
            Countries.Add("CV", "CPV");
            Countries.Add("CX", "CXR");
            Countries.Add("CY", "CYP");
            Countries.Add("CZ", "CZE");
            Countries.Add("DE", "DEU");
            Countries.Add("DJ", "DJI");
            Countries.Add("DK", "DNK");
            Countries.Add("DM", "DMA");
            Countries.Add("DO", "DOM");
            Countries.Add("DZ", "DZA");
            Countries.Add("EC", "ECU");
            Countries.Add("EE", "EST");
            Countries.Add("EG", "EGY");
            Countries.Add("EH", "ESH");
            Countries.Add("ER", "ERI");
            Countries.Add("ES", "ESP");
            Countries.Add("ET", "ETH");
            Countries.Add("FI", "FIN");
            Countries.Add("FJ", "FJI");
            Countries.Add("FK", "FLK");
            Countries.Add("FM", "FSM");
            Countries.Add("FO", "FRO");
            Countries.Add("FR", "FRA");
            Countries.Add("GA", "GAB");
            Countries.Add("GB", "GBR");
            Countries.Add("GD", "GRD");
            Countries.Add("GE", "GEO");
            Countries.Add("GF", "GUF");
            Countries.Add("GG", "GGY");
            Countries.Add("GH", "GHA");
            Countries.Add("GI", "GIB");
            Countries.Add("GL", "GRL");
            Countries.Add("GM", "GMB");
            Countries.Add("GN", "GIN");
            Countries.Add("GP", "GLP");
            Countries.Add("GQ", "GNQ");
            Countries.Add("GR", "GRC");
            Countries.Add("GS", "SGS");
            Countries.Add("GT", "GTM");
            Countries.Add("GU", "GUM");
            Countries.Add("GW", "GNB");
            Countries.Add("GY", "GUY");
            Countries.Add("HK", "HKG");
            Countries.Add("HM", "HMD");
            Countries.Add("HN", "HND");
            Countries.Add("HR", "HRV");
            Countries.Add("HT", "HTI");
            Countries.Add("HU", "HUN");
            Countries.Add("ID", "IDN");
            Countries.Add("IE", "IRL");
            Countries.Add("IL", "ISR");
            Countries.Add("IM", "IMN");
            Countries.Add("IN", "IND");
            Countries.Add("IO", "IOT");
            Countries.Add("IQ", "IRQ");
            Countries.Add("IR", "IRN");
            Countries.Add("IS", "ISL");
            Countries.Add("IT", "ITA");
            Countries.Add("JE", "JEY");
            Countries.Add("JM", "JAM");
            Countries.Add("JO", "JOR");
            Countries.Add("JP", "JPN");
            Countries.Add("KE", "KEN");
            Countries.Add("KG", "KGZ");
            Countries.Add("KH", "KHM");
            Countries.Add("KI", "KIR");
            Countries.Add("KM", "COM");
            Countries.Add("KN", "KNA");
            Countries.Add("KP", "PRK");
            Countries.Add("KR", "KOR");
            Countries.Add("KW", "KWT");
            Countries.Add("KY", "CYM");
            Countries.Add("KZ", "KAZ");
            Countries.Add("LA", "LAO");
            Countries.Add("LB", "LBN");
            Countries.Add("LC", "LCA");
            Countries.Add("LI", "LIE");
            Countries.Add("LK", "LKA");
            Countries.Add("LR", "LBR");
            Countries.Add("LS", "LSO");
            Countries.Add("LT", "LTU");
            Countries.Add("LU", "LUX");
            Countries.Add("LV", "LVA");
            Countries.Add("LY", "LBY");
            Countries.Add("MA", "MAR");
            Countries.Add("MC", "MCO");
            Countries.Add("MD", "MDA");
            Countries.Add("ME", "MNE");
            Countries.Add("MF", "MAF");
            Countries.Add("MG", "MDG");
            Countries.Add("MH", "MHL");
            Countries.Add("MK", "MKD");
            Countries.Add("ML", "MLI");
            Countries.Add("MM", "MMR");
            Countries.Add("MN", "MNG");
            Countries.Add("MO", "MAC");
            Countries.Add("MP", "MNP");
            Countries.Add("MQ", "MTQ");
            Countries.Add("MR", "MRT");
            Countries.Add("MS", "MSR");
            Countries.Add("MT", "MLT");
            Countries.Add("MU", "MUS");
            Countries.Add("MV", "MDV");
            Countries.Add("MW", "MWI");
            Countries.Add("MX", "MEX");
            Countries.Add("MY", "MYS");
            Countries.Add("MZ", "MOZ");
            Countries.Add("NA", "NAM");
            Countries.Add("NC", "NCL");
            Countries.Add("NE", "NER");
            Countries.Add("NF", "NFK");
            Countries.Add("NG", "NGA");
            Countries.Add("NI", "NIC");
            Countries.Add("NL", "NLD");
            Countries.Add("NO", "NOR");
            Countries.Add("NP", "NPL");
            Countries.Add("NR", "NRU");
            Countries.Add("NU", "NIU");
            Countries.Add("NZ", "NZL");
            Countries.Add("OM", "OMN");
            Countries.Add("PA", "PAN");
            Countries.Add("PE", "PER");
            Countries.Add("PF", "PYF");
            Countries.Add("PG", "PNG");
            Countries.Add("PH", "PHL");
            Countries.Add("PK", "PAK");
            Countries.Add("PL", "POL");
            Countries.Add("PM", "SPM");
            Countries.Add("PN", "PCN");
            Countries.Add("PR", "PRI");
            Countries.Add("PS", "PSE");
            Countries.Add("PT", "PRT");
            Countries.Add("PW", "PLW");
            Countries.Add("PY", "PRY");
            Countries.Add("QA", "QAT");
            Countries.Add("RE", "REU");
            Countries.Add("RO", "ROU");
            Countries.Add("RS", "SRB");
            Countries.Add("RU", "RUS");
            Countries.Add("RW", "RWA");
            Countries.Add("SA", "SAU");
            Countries.Add("SB", "SLB");
            Countries.Add("SC", "SYC");
            Countries.Add("SD", "SDN");
            Countries.Add("SE", "SWE");
            Countries.Add("SG", "SGP");
            Countries.Add("SH", "SHN");
            Countries.Add("SI", "SVN");
            Countries.Add("SJ", "SJM");
            Countries.Add("SK", "SVK");
            Countries.Add("SL", "SLE");
            Countries.Add("SM", "SMR");
            Countries.Add("SN", "SEN");
            Countries.Add("SO", "SOM");
            Countries.Add("SR", "SUR");
            Countries.Add("SS", "SSD");
            Countries.Add("ST", "STP");
            Countries.Add("SV", "SLV");
            Countries.Add("SY", "SYR");
            Countries.Add("SZ", "SWZ");
            Countries.Add("TC", "TCA");
            Countries.Add("TD", "TCD");
            Countries.Add("TF", "ATF");
            Countries.Add("TG", "TGO");
            Countries.Add("TH", "THA");
            Countries.Add("TJ", "TJK");
            Countries.Add("TK", "TKL");
            Countries.Add("TL", "TLS");
            Countries.Add("TM", "TKM");
            Countries.Add("TN", "TUN");
            Countries.Add("TO", "TON");
            Countries.Add("TR", "TUR");
            Countries.Add("TT", "TTO");
            Countries.Add("TV", "TUV");
            Countries.Add("TW", "TWN");
            Countries.Add("TZ", "TZA");
            Countries.Add("UA", "UKR");
            Countries.Add("UG", "UGA");
            Countries.Add("UM", "UMI");
            Countries.Add("US", "USA");
            Countries.Add("UY", "URY");
            Countries.Add("UZ", "UZB");
            Countries.Add("VA", "VAT");
            Countries.Add("VC", "VCT");
            Countries.Add("VE", "VEN");
            Countries.Add("VG", "VGB");
            Countries.Add("VI", "VIR");
            Countries.Add("VN", "VNM");
            Countries.Add("VU", "VUT");
            Countries.Add("WF", "WLF");
            Countries.Add("WS", "WSM");
            Countries.Add("YE", "YEM");
            Countries.Add("YT", "MYT");
            Countries.Add("ZA", "ZAF");
            Countries.Add("ZM", "ZMB");
            Countries.Add("ZW", "ZWE");
        }

        /// <summary>
        /// Ermittelt die dominante Farbe eines Bildes
        /// http://www.imagemagick.org/Usage/quantize/#extract
        /// </summary>
        /// <param name="stream">Bitmap</param>
        /// <returns></returns>
        public static string GetDominantColor(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var newImage = new MagickImage(stream, new MagickReadSettings() { Format = MagickFormat.Jpg }))
            {
                try
                {
                    newImage.Resize(1, 1);
                    var color = newImage.Histogram().FirstOrDefault().Key.ToColor();
                    return string.Format("{0:N0},{1:N0},{2:N0}", color.R, color.G, color.B);
                }
                catch
                {
                    return "0,0,0";
                }
            }
        }
    }
}