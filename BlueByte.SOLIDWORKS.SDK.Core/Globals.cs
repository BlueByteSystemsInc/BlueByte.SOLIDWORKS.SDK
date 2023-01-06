using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
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
