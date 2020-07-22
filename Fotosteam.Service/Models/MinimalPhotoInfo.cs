using Fotosteam.Service.Repository.Poco;
namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Kapselt Informationen zu einem Foto, die für das Versenden von Nachrichten notwendig sind
    /// </summary>
    public class MinimalPhotoInfo
    {
        /// <summary>
        /// Id des Fotos
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Das Mitglied, dem das Foto gehört
        /// </summary>
        internal Member Member { get; set; }
        /// <summary>
        /// Der Name des Fotos
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Der Titel des Fotos
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 640-Link des Fotos
        /// </summary>
        public string Url640 { get; set; }
    }
}
