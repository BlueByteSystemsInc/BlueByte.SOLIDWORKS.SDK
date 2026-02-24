using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BlueByte.TestAddIn
{
    [ComVisible(true)]
    [Guid("BF1C1567-53D8-4E2B-B588-0518A1EBFA55")]

    [Icon("icon.ico")]

    [Name("Addin")]
    [Description("This is the description")]
    [StartUp(true)]
    [MenuItem("SDK\\Click Me", DocumentTypes_e.swDocNONE | DocumentTypes_e.swDocPART, nameof(OnMenuClick), "ToolbarSmall.bmp")]
  
   
    public class AddIn : AddInBase
    {
        #region fields 


        #endregion


      

        protected override void OnConnectToSOLIDWORKS(SldWorks swApp)
        {

            Debug.Print(Identity.Name);

            
            base.OnConnectToSOLIDWORKS(swApp);

            this.DocumentManager.ActiveDocumentChanged += DocumentManager_ActiveDocumentChanged;
          


        }

        

      

        

        private void DocumentManager_ActiveDocumentChanged(object sender, IDocument e)
        {
            if (e != null)
            Debug.Print($"This document {e.FileName} has been activated.");
            else 
                Debug.Print($"There is no active document.");

        }

        protected override void OnDisconnectFromSOLIDWORKS()
        {
            base.OnDisconnectFromSOLIDWORKS();

        }


        #region menu event handlers 

        public void OnMenuClick()
        {

            var doc = this.DocumentManager.ActiveDocument;

            var stBuilder = new StringBuilder();

            var docs = DocumentManager.GetDocuments();

            stBuilder.AppendLine(docs.Length.ToString());

            docs.ToList().ForEach(x => stBuilder.AppendLine(x.FileName));

            this.Application.SendInformationMessage(stBuilder.ToString());
        
        }

        private void DocumentManager_DocumentGotClosed(object sender, System.Tuple<IDocument, swDestroyNotifyType_e> e)
        {

            Debug.Print($"This document {e.Item1.FileName} was closed [{e.Item2.ToString()}].");
        }

        private void DocumentManager_DocumentGotCreated(object sender, IDocument e)
        {
            Debug.Print($"A new document has been created {e.FileName}.");

        }

        private void DocumentManager_DocumentGotOpened(object sender, IDocument e)
        {
            Debug.Print($"A document has been opened {e.FileName}.");

        }

        public void ExportIFC()
        {
            throw new NotImplementedException();
        }

        public int ExportIFCEnable()
        {

            return 1;
        }

        #endregion

    }
}
