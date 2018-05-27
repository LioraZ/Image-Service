using System;

/// <summary>
/// The Event namespace.
/// </summary>
namespace Infrastructure.Event
{
    /// <summary>
    /// Class SettingsEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class SettingsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        public string OutputDir { get; set; }
        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>The name of the source.</value>
        public string SourceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the log.
        /// </summary>
        /// <value>The name of the log.</value>
        public string LogName { get; set; }
        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>The size of the thumbnail.</value>
        public int ThumbnailSize { get; set; }
        /// <summary>
        /// Gets or sets the handlers.
        /// </summary>
        /// <value>The handlers.</value>
        public string[] Handlers { get; set; }
    }
}
