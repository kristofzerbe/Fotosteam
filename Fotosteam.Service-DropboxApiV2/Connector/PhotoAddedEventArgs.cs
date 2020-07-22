using System;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Connector
{

    /// <summary>
    /// Definiert die Informationen, die beim Ausl�sen des Ereignisses <see cref="IConnector.PhotoAdded"/> �bergeben werden sollen
    /// </summary>
    public class PhotoAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Das aktuelle verarbeitete Fotos
        /// </summary>
        public Photo Photo { get; set; }
        /// <summary>
        /// Der aktuelle Index innerhalb der Schleife der zu verarbeitenden Fotos
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Die Gesamtanzahl aller zu verarbeiten Fotos
        /// </summary>
        public int TotalFileCount { get; set; }
    }
}