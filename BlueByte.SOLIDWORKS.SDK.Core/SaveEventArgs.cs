using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// Save As EventArgs
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class SaveEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SaveEventArgs"/> is handled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if handled; otherwise, <c>false</c>.
        /// </value>
        public bool Handled { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public IDocument Document { get; set; }
        /// <summary>
        /// Prevents a default instance of the <see cref="SaveEventArgs"/> class from being created.
        /// </summary>
        private SaveEventArgs()
        {

        }

        /// <summary>
        /// News the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static SaveEventArgs New(IDocument document, string fileName = "")
        {
            var instance = new SaveEventArgs();
            instance.Document = document;
            instance.FileName = fileName;


            return instance;
        }
    }
}
