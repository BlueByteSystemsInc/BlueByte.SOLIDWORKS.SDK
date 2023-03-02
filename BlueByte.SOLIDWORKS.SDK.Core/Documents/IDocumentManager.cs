using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    /// <summary>
    /// Document Management class.
    /// </summary>
    public interface IDocumentManager
    {


        /// <summary>
        /// Gets the active document. This property implements <see cref="INotifyPropertyChanged"/>
        /// </summary>
        IDocument ActiveDocument { get; }


        /// <summary>
        /// Gets an array the open loaded and visible documents.
        /// </summary>
        /// <returns></returns>
        IDocument[] GetDocuments();

        /// <summary>
        /// Occurs when [document got closed]. 
        /// </summary>
        event EventHandler<Tuple<IDocument, swDestroyNotifyType_e>> DocumentGotClosed;


        /// <summary>
        /// Occurs when <see cref="ActiveDocument"/> changes. The argument e might be null if there is no active document.
        /// </summary>
        event EventHandler<IDocument> ActiveDocumentChanged;


        /// <summary>
        /// Occurs when [document got created].
        /// </summary>
        event EventHandler<IDocument> DocumentGotCreated;

        /// <summary>
        /// Occurs when [document got opened].
        /// </summary>
        event EventHandler<IDocument> DocumentGotOpened;


        /// <summary>
        /// Occurs when [document about to be saved as].
        /// </summary>
        event EventHandler<SaveEventArgs> DocumentAboutToBeSavedAs;

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
        void DettachEventHandlers();

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
        void InitializeWithPreloadedDocuments();



        void Refresh(SldWorks unsafeObject);


    }
}