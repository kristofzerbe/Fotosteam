using System.ComponentModel.DataAnnotations;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    public class PhotoBrick : Brick
    {
        /// <summary>
        ///     Initialisiert den Typ
        /// </summary>
        /// <remarks>Der Typ ist für die JSON-(DE)Serialisierung wichtig</remarks>
        public PhotoBrick()
        {
            Type = BrickType.Photo.ToString().ToLower();
        }

        [MinLength(20)]
        public string Caption { get; set; }

        /// <summary>
        ///     Id zu dem zugehörigen Fotos, wird nur gebraucht, um Aktualisierung auf Eigenschaftsebene zuzulassen
        /// </summary>
        public int? PhotoId { get; set; }

        /// <summary>
        ///     Das verknüpfte Foto
        /// </summary>
        public Photo Photo { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}, Caption: {1}, PhotoId: {2}, Photo: {3}", base.ToString(), Caption, PhotoId, Photo);
        }
    }
}