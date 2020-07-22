using System;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Definiert ein Ereignis, z.B. Reise durch Sri Lanka
    /// </summary>
    public class Event : PocoBase
    {
        /// <summary>
        ///     Die Id des Benutzers, der das Ereignis definiert
        /// </summary>
        [Required]
        public int MemberId { get; set; }

        /// <summary>
        ///     Name des Ereignis, z.B. Reise nach Brügge
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Datum wann das Ereignis statt fand, bzw. der erste Tag
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///     Das Enddatum des Ereignis, wenn es sich um ein mehrtägiges Ereignis handelt
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///     Eine kurze Beschreibung für das Ereignis.
        ///     Es sind maxiumal 4000 Zeichen zuläßig
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Id für den verbundenen Ort
        ///     Dies ist nur notwendig, da man auf Eigenschaftsebene aktualisieren möchte
        /// </summary>
        public int? LocationId { get; set; }

        /// <summary>
        ///     Ein definierte Ort zu dem das Ereignis gehört
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        ///     Anzahl der Fotos zu dem Ereignis
        /// </summary>
        /// <remarks> Private Fotos werden nur für den Besitzer mitgezählt</remarks>
        public int PhotoCount { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "MemberId: {0}, Name: {1}, Date: {2}, DateTo: {3}, Description: {4}, Location: {5}, PhotoCount: {6}",
                    MemberId, Name, Date, DateTo, Description, Location, PhotoCount);
        }
    }
}