using BlueByte.SOLIDWORKS.SDK.Core.Documents;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// Globals 
    /// </summary>
    public static class Globals
    {

        /// <summary>
        /// Gets or sets the document manager.
        /// </summary>
        /// <value>
        /// The document manager.
        /// </value>
        public static IDocumentManager DocumentManager { get; set; }
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public static ISOLIDWORKSApplication Application { get; set; }
    }
}
