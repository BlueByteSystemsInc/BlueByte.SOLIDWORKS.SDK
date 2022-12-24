using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    /// <summary>
    /// Document Management class.
    /// </summary>
    public interface IDocumentManager
    {
        /// <summary>
        /// Gets or sets the documents. Do not set, add, replace and remove items from this list.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        ObservableCollection<IDocument> Documents { get; set; }

        /// <summary>
        /// Occurs when [document got closed].
        /// </summary>
        event EventHandler<Tuple<IDocument, swDestroyNotifyType_e>> DocumentGotClosed;

        /// <summary>
        /// Occurs when [document got created].
        /// </summary>
        event EventHandler<IDocument> DocumentGotCreated;

        /// <summary>
        /// Occurs when [document got opened].
        /// </summary>
        event EventHandler<IDocument> DocumentGotOpened;

        /// <summary>
        /// Adds an unloaded document. i.e. document is a suppressed reference of another document. For example: suppressed component in an assembly.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        IDocument AddUnloadedDocument(string fileName);

        /// <summary>
        /// Attaches the event handlers.
        /// </summary>
        void AttachEventHandlers();

        /// <summary>
        /// Dettaches the event handlers.
        /// </summary>
        void DeattachEventHandlers();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets the document from unsafe object.
        /// </summary>
        /// <param name="unsafeObject">The unsafe object.</param>
        /// <returns></returns>
        Tuple<IDocument, DocumentAddOperationRet_e> GetDocumentFromUnsafeObject(object unsafeObject);

        /// <summary>
        /// Loads the existing documents. Calls this after creating an instance of this class to load existing documents.
        /// </summary>
        /// <returns></returns>
        Dictionary<IDocument, DocumentAddOperationRet_e> LoadExistingDocuments();
    }
}