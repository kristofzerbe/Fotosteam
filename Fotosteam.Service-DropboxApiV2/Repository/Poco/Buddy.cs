namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt die Information, zu der Verknüpfung von Mitgliedern
    /// </summary>
    /// <remarks>
    ///     Die Serializierung von Buddies als Member-Objekte führt zu zierkulären Referenzen,
    ///     deshalb wird hier der nicht objektorientierte Ansatz gewählt
    /// </remarks>
    public class Buddy
    {
        /// <summary>
        ///     Die linke Seite der Verbindung zwischen zwei Mitgliedern
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        ///     Die rechte Seite der Verbindung zwischen zwei Mitgliedern
        /// </summary>
        public int BuddyMemberId { get; set; }

        /// <summary>
        ///     Gibt an, ob die Verbindung einvernehmlich ist
        /// </summary>
        public bool IsMutual { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("MemberId: {0}, BuddyMemberId: {1}, IsMutual: {2}", MemberId, BuddyMemberId, IsMutual);
        }
    }
}