using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Das Kernstück der Anwendung. Die Repräsentation eines Fotos
    /// </summary>
    public class Photo : PocoBase
    {
        internal const int Orignal640UrlSize = 641;
        private List<Category> _categories = new List<Category>();
        private CategoryType _category;

        /// <summary>
        ///     Initialisiert ein neues Objekt mit Standardwerten
        /// </summary>
        public Photo()
        {
            CommentCount = 0;
            RatingSum = 0;
            IsPrivate = true;
            IsNew = true;
            ShowInOverview = true;
        }

        /// <summary>
        ///     Die Id des zugehörigen Benutzers
        /// </summary>
        [Required]
        public int MemberId { get; set; }

        /// <summary>
        ///     Das zugehörige Member-Objekt
        /// </summary>
        public Member Member { get; set; }

        /// <summary>
        ///     Der Name, ist gleich dem Ordner beim Provider
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Der Name, i.d.R. der Dateiname
        /// </summary>
        [Required]
        public string OriginalName { get; set; }

        /// <summary>
        ///     Liefert den Link zu dem unbearbeiten Orginalbild zurück.
        ///     Das Bild ist auf 640px reduziert
        /// </summary>
        public string Original640Url
        {
            get
            {
                if (DirectLinks == null || !DirectLinks.Any())
                    return null;

                var link = DirectLinks.FirstOrDefault(l => l.Size == Orignal640UrlSize);

                return link == null ? null : link.Url;
            }
        }

        /// <summary>
        ///     Der Ordner, in dem das Foto abgelegt wurde
        /// </summary>
        [Required]
        public string Folder { get; set; }

        /// <summary>
        ///     Titel
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        /// <summary>
        ///     Id eines zugehörigen Ereignisses
        /// </summary>
        public int? EventId { get; set; }

        /// <summary>
        ///     Id des Orts der Aufnahme
        /// </summary>
        public int? LocationId { get; set; }

        /// <summary>
        ///     Breite des Originalfotos
        /// </summary>
        [Required]
        public int Width { get; set; }

        /// <summary>
        ///     Höhe des Originalfotos
        /// </summary>
        [Required]
        public int Height { get; set; }

        /// <summary>
        ///     Seitenverhältnis
        /// </summary>
        [Required]
        public double AspectRation { get; set; }

        /// <summary>
        ///     Ausrichtung, landscape oder portrait
        /// </summary>
        [Required]
        public string Orientation { get; set; }

        /// <summary>
        ///     Datum der Aufnahme
        /// </summary>
        public DateTime CaptureDate { get; set; }

        /// <summary>
        ///     Datum der Veröffentlichung
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        ///     Liefert die Anzahl der Kommentare zu dem Foto
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        ///     Liefert die druchschnittliche Bewertung des Fotos
        /// </summary>
        /// <summary>
        ///     Liefert die Summe aller Bewertungungen des Fotos
        /// </summary>
        public int RatingSum { get; set; }

        /// <summary>
        ///     Gibt an ob das Foto nicht für alle sichtbar ist
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///     Art der Lizenz. Ein Foto kann mehrere Lizenzen unterstützen
        /// </summary>
        public LicenseType License { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto in original heruntergeladen werden darf
        /// </summary>
        public bool AllowFullSizeDownload { get; set; }

        /// <summary>
        ///     Liste aller Kommentare
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        ///     Liste aller Bewertungen
        /// </summary>
        public ICollection<Rating> Ratings { get; set; }

        /// <summary>
        ///     Extended image format
        /// </summary>
        public ExifData Exif { get; set; }

        /// <summary>
        ///     Liste aller Links fürs direkte Herunterladen
        /// </summary>
        public ICollection<DirectLink> DirectLinks { get; set; }

        /// <summary>
        ///     Die Kategorien des Fotos als Enum
        /// </summary>
        public CategoryType Category
        {
            get { return _category; }
            set
            {
                _category = value;
                _categories = new List<Category>();
            }
        }

        /// <summary>
        ///     Liste der Kategorien des Fotos
        /// </summary>
        public List<Category> Categories
        {
            get
            {
                _categories.Clear();

                foreach (var type in _category.ToString().Split(','))
                {
                    var value = (int) Enum.Parse(typeof (CategoryType), type, true);
                    _categories.Add(new Category {Type = type.Trim(), PhotoCount = 1, TypeValue = value});
                }
                if (_categories.Count == 0)
                    _categories.Add(new Category {Type = CategoryType.NotSet.ToString().Trim(), PhotoCount = 1});

                return _categories;
            }
            internal set { _categories = value; }
        }

        /// <summary>
        ///     Liste aller Themeen
        /// </summary>
        public ICollection<Topic> Topics { get; set; }

        /// <summary>
        ///     Liste aller Themeen
        /// </summary>
        public ICollection<Story> Stories { get; set; }

        /// <summary>
        ///     Das Ereignis, zu dem das Foto gehört
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        ///     Ort, an dem das Foto aufgenommen wurde
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        ///     Definiert den Standardzugriff für den Cloud-Speicher, z.B. Dropbox oder Google
        /// </summary>
        public StorageProviderType StorageAccessType { get; set; }

        /// <summary>
        ///     Definiert, ob das Foto nur hochgeladen, aber noch nicht mit zusätzlichen Informationen versehen wurde
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        ///     Definiert, ob das Foto nur innherhalb einer Geschichte angezeigt werden soll
        /// </summary>
        public bool IsForStoryOnly { get; set; }

        /// <summary>
        ///     Gibt an, ob das Bild in der Übersicht angezeigt werden soll
        /// </summary>
        public bool ShowInOverview { get; set; }

        /// <summary>
        ///     Dominante Farbe des Bildes
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///     Gib an, ob ein Bild beworben werden darf
        /// </summary>
        public bool AllowPromoting { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto kommentiert werden darf.
        ///     Dies will man eventuell auschalten, wenn zuviele Kommentare eingehen
        /// </summary>
        public bool AllowCommenting { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto getteilt werden darf.
        /// </summary>
        public bool AllowSharing { get; set; }

        /// <summary>
        ///     Gibt an ob das Foto bewertet werden darf
        /// </summary>
        public bool AllowRating { get; set; }

        /// <summary>Beschreibung</summary>
        public string Description { get; set; }

        /// <summary>
        ///     Der prozentuale Abstand vom linken Rand
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        ///     Der prozentuale Abstand vom oberen Rand
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "Categories: {0}, Category: {1}, MemberId: {2}, Member: {3}, Name: {4}, OriginalName: {5}, OriginalUrl: {6}, Folder: {7}, Title: {8}, EventId: {9}, LocationId: {10}, Width: {11}, Height: {12}, AspectRation: {13}, Orientation: {14}, CaptureDate: {15}, PublishDate: {16}, CommentCount: {17}, RatingSum: {18}, IsPrivate: {19}, License: {20}, AllowFullSizeDownload: {21}, Comments: {22}, Ratings: {23}, Exif: {24}, DirectLinks: {25}, Category: {26}, Categories: {27}, Topics: {28}, Stories: {29}, Event: {30}, Location: {31}, StorageAccessType: {32}, IsNew: {33}, IsForStoryOnly: {34}, ShowInOverview: {35}, Color: {36}, AllowPromoting: {37}, AllowCommenting: {38}, AllowSharing: {39}, AllowRating: {40}, Description: {41}, Left: {42}, Top: {43}",
                    _categories, _category, MemberId, Member, Name, OriginalName, Original640Url, Folder, Title, EventId,
                    LocationId, Width, Height, AspectRation, Orientation, CaptureDate, PublishDate, CommentCount,
                    RatingSum, IsPrivate, License, AllowFullSizeDownload, Comments, Ratings, Exif, DirectLinks, Category,
                    Categories, Topics, Stories, Event, Location, StorageAccessType, IsNew, IsForStoryOnly,
                    ShowInOverview, Color, AllowPromoting, AllowCommenting, AllowSharing, AllowRating, Description, Left,
                    Top);
        }
    }
}