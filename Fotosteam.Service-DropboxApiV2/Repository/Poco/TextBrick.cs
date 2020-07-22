using System.ComponentModel.DataAnnotations;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    public class TextBrick : Brick
    {
        /// <summary>
        ///     Initialisiert den Typ
        /// </summary>
        /// <remarks>Der Typ ist für die JSON-(DE)Serialisierung wichtig</remarks>
        public TextBrick()
        {
            Type = BrickType.Text.ToString().ToLower();
        }

        [Required]
        public string Text { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}, Text: {1}", base.ToString(), Text);
        }
    }
}