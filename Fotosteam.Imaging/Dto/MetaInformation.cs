namespace Fotosteam.Imaging.Dto
{
    public class MetaInformation
    {
        public ExifData Exif { get; set; }
        // > EXIF: public string Beschreibung { get; set; }
        // > EXIF: public string GoogleMapsLocation { get; set; }

        public string Location { get; set; }
        
    }
}
