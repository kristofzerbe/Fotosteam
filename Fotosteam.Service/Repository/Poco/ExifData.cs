using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt ausgesuchte Informationen aus Exchangeable image file format eines Fotos
    /// </summary>
    public class ExifData
    {
        /// <summary>
        ///     Standardkonstruktor, der den Titel auf "Untitled" festlegt
        /// </summary>
        public ExifData()
        {
            Title = "Untitled";
        }

        /// <summary>
        ///     Die Id des Fotos
        /// </summary>
        [Required]
        public int PhotoId { get; set; }

        /// <summary>Hersteller</summary>
        public string Make { get; set; }

        /// <summary>Modell</summary>
        public string Model { get; set; }

        /// <summary>Aufnahmedatum</summary>
        public DateTime CaptureDate { get; set; }

        /// <summary>Beschreibung</summary>
        public string Description { get; set; }

        /// <summary>Software für die Bearbeitung</summary>
        public string Software { get; set; }

        /// <summary>Autor</summary>
        public string Artist { get; set; }

        /// <summary>Copyright</summary>
        public string Copyright { get; set; }

        /// <summary>Brennweite (ohne Nachkommastellen + 'mm')</summary>
        public double FocalLength { get; set; }

        /// <summary>Blendenzahl</summary>
        public double FNumber { get; set; }

        /// <summary>APEX-Blendenwert</summary>
        public double ApertureValue { get; set; }

        /// <summary>Maximale Blendenzahl</summary>
        public double MaxApertureValue { get; set; }

        /// <summary>Belichtungszeit</summary>
        public double ExposureTime { get; set; }

        /// <summary>Belichtungskorrektur ( + 'EV' )</summary>
        public double ExposureBiasValue { get; set; }

        /// <summary>Belichtungsprogramm</summary>
        public int ExposureProgram { get; set; }

        /// <summary>Belichtungsmodus</summary>
        public int ExposureMode { get; set; }

        /// <summary>ISO-Lichtempfindlichkeit</summary>
        public int ISOSpeedRatings { get; set; }

        /// <summary>Messmethode        </summary>
        public int MeteringMode { get; set; }

        /// <summary>Horizontale Auflösung</summary>
        public double XResolution { get; set; }

        /// <summary> Vertikale Auflösung</summary>
        public double YResolution { get; set; }

        /// <summary>Auflösungseinheit</summary>
        public int ResolutionUnit { get; set; }

        /// <summary>Breite</summary>
        public int Width { get; set; }

        /// <summary>Höhe        </summary>
        public int Height { get; set; }

        /// <summary>Bildseitenverhältnis</summary>
        public double AspectRatio { get; set; }

        /// <summary>Bildformat als landscape oder portrait </summary>
        public string Orientation { get; set; }

        /// <summary>Bildformat als numerischer Wert. Wird heangezogen, wenn das Bild rotiert werden muss</summary>
        public int OriginalOrientation { get; set; }

        /// <summary>Breitengrad</summary>
        public double Latitude { get; set; }

        /// <summary>Längengrad</summary>
        public double Longitude { get; set; }

        /// <summary>Das zu <see cref="Latitude" /> und <see cref="Longitude" /> gehörige <see cref="Location" />-Objkt </summary>
        internal Location Location { get; set; }

        /// <summary>Titel</summary>
        internal string Title { get; set; }

        internal List<string> Keywords { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "PhotoId: {0}, Make: {1}, Model: {2}, CaptureDate: {3}, Description: {4}, Software: {5}, Artist: {6}, Copyright: {7}, FocalLength: {8}, FNumber: {9}, ApertureValue: {10}, MaxApertureValue: {11}, ExposureTime: {12}, ExposureBiasValue: {13}, ExposureProgram: {14}, ExposureMode: {15}, ISOSpeedRatings: {16}, MeteringMode: {17}, XResolution: {18}, YResolution: {19}, ResolutionUnit: {20}, Width: {21}, Height: {22}, AspectRatio: {23}, Orientation: {24}, Latitude: {25}, Longitude: {26}, Location: {27}, Title: {28}",
                    PhotoId, Make, Model, CaptureDate, Description, Software, Artist, Copyright, FocalLength, FNumber,
                    ApertureValue, MaxApertureValue, ExposureTime, ExposureBiasValue, ExposureProgram, ExposureMode,
                    ISOSpeedRatings, MeteringMode, XResolution, YResolution, ResolutionUnit, Width, Height, AspectRatio,
                    Orientation, Latitude, Longitude, Location, Title);
        }
    }
}