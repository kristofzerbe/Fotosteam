using System;

namespace Fotosteam.Imaging.Dto
{
    public class ExifData
    {
        public string Make { get; set; } // Hersteller
        public string Model { get; set; } // Modell
        public DateTime CaptureDate { get; set; } // Aufnahmedatum
        public string Description { get; set; } // Beschreibung
        public string Software { get; set; } // Software
        public string Artist { get; set; } // Autor
        public string Copyright { get; set; } // Copyright
        public double FocalLength { get; set; } // Brennweite (ohne Nachkommastellen + 'mm')
        public double FNumber { get; set; } // Blendenzahl
        public double ApertureValue { get; set; } // ???
        public double MaxApertureValue { get; set; } // Maximale Blendenzahl
        public double ExposureTime { get; set; } // // Belichtungszeit
        public double ExposureBiasValue { get; set; } // Belichtungskorrektur ( + 'EV' )
        public int ExposureProgram { get; set; } // Belichtungsprogramm
        public int ExposureMode { get; set; } // Belichtungsmodus
        public int ISOSpeedRatings { get; set; } // ISO-Lichtempfindlichkeit
        public int MeteringMode { get; set; } // Messmethode        

        public double XResolution { get; set; } // Horizontale Auflösung
        public double YResolution { get; set; } // Vertikale Auflösung
        public int ResolutionUnit { get; set; } // Auflösungseinheit

        public int Width { get; set; } // Breite
        public int Height { get; set; } // Höhe        
        public double AspectRatio { get; set; } // Bildseitenverhältnis
        public string Orientation { get; set; } // Bildformat

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // noch interessant:
        // - Flash

        // http://www.awaresystems.be/imaging/tiff/tifftags/privateifd/exif.html

        // ResolutionUnit
        // --------------
        // 1 = No absolute unit of measurement
        // 2 = Inch
        // 3 = Centimeter

        // ExposureMode
        // ------------
        // 0 = Auto exposure
        // 1 = Manual exposure
        // 2 = Auto bracket

        // ExposureProgram
        // ---------------
        // 0 = Not Defined ; Unbekannt
        // 1 = Manual ; Manuell
        // 2 = Program AE : Programmautomatik
        // 3 = Aperture-priority AE : Blendenautomatik
        // 4 = Shutter speed priority AE : Zeitautomatik
        // 5 = Creative ; Kreativprogramm
        // 6 = Action ; Action-Programm
        // 7 = Portrait ; Porträtprogramm
        // 8 = Landscape ; Landschaftsprogramm
        // 9 = Bulb ; Langzeitbelichtung

        // MeteringMode
        // ------------
        // 0 = Unknown
        // 1 = Average
        // 2 = CenterWeightedAverage
        // 3 = Spot
        // 4 = MultiSpot
        // 5 = Pattern
        // 6 = Partial
        // 255 = other

    }
}