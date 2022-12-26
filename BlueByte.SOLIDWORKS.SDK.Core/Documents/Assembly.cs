using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{

    internal class Assembly : Document, IAssembly
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Assembly"/> class.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="fullFileName">Full name of the file.</param>
        /// <param name="isRoot">if set to <c>true</c> [is root].</param>
        public Assembly(ModelDoc2 doc, string fullFileName, bool isRoot) : base(doc, fullFileName, isRoot)
        {

        }
    }
}