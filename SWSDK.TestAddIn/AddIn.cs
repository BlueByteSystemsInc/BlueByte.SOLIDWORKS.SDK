using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BlueByte.TestAddIn
{
    [ComVisible(true)]
    [Guid("BF1C1567-53D8-4E2B-B588-0518A1EBFA55")]

    [Name("Addin")]
    [Description("This is the description")]
    [StartUp(true)]
    
    [MenuItem("SDK", swDocumentTypes_e.swDocNONE, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocNONE, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
    [MenuItem("SDK", swDocumentTypes_e.swDocPART, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocPART, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
    [MenuItem("SDK", swDocumentTypes_e.swDocPART, true)]
    [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocASSEMBLY, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
    public class AddIn : AddInBase
    {
        #region fields 


        #endregion
         

        protected override void OnConnectToSOLIDWORKS(SldWorks swApp)
        {
            base.OnConnectToSOLIDWORKS(swApp);
            
            AttachDebugger();

            this.DocumentManager.DocumentGotOpened += DocumentManager_DocumentGotOpened;
            this.DocumentManager.DocumentGotCreated += DocumentManager_DocumentGotCreated;
            this.DocumentManager.DocumentGotClosed += DocumentManager_DocumentGotClosed;
            this.CustomPropertyManager.CustomPropertyAdded += CustomPropertyManager_CustomPropertyAdded;
            this.CustomPropertyManager.CustomPropertyChanged += CustomPropertyManager_CustomPropertyChanged;
            this.CustomPropertyManager.CustomPropertyDeleted += CustomPropertyManager_CustomPropertyDeleted;

        }

        private void CustomPropertyManager_CustomPropertyDeleted(object sender, SOLIDWORKS.SDK.CustomProperties.CustomPropertyChangedEventArgs e)
        {

            e.Handled = true;
            this.Application.SendErrorMessage("This action is not permitted.");
        }

        private void CustomPropertyManager_CustomPropertyChanged(object sender, SOLIDWORKS.SDK.CustomProperties.CustomPropertyChangedEventArgs e)
        {

            e.Handled = true;
            this.Application.SendErrorMessage("This action is not permitted.");
        }

        private void CustomPropertyManager_CustomPropertyAdded(object sender, SOLIDWORKS.SDK.CustomProperties.CustomPropertyChangedEventArgs e)
        {
            e.Handled = true;
            this.Application.SendErrorMessage("This action is not permitted.");
        }

        protected override void OnDisconnectFromSOLIDWORKS()
        {
            base.OnDisconnectFromSOLIDWORKS();

        }


        #region menu event handlers 

        public void OnMenuClick()
        {


            Debug.Print($"There are {this.DocumentManager.GetDocuments().Length} open documents.");


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

        #endregion

    }
}
