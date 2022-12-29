using SolidWorks.Interop.sldworks;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents
{
    internal class Drawing : Document, IDrawing
    {
        #region events 

       
        #endregion


        #region fields 


        #endregion 

        /// <summary>
        /// Initializes a new instance of the <see cref="Assembly"/> class.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="fullFileName">Full name of the file.</param>
        /// <param name="isRoot">if set to <c>true</c> [is root].</param>
        public Drawing(ModelDoc2 doc, string fullFileName) : base(doc, fullFileName, false)
        {

        }

        public void Initialize(string referencedConfiguration)
        {

            var configuration = (UnSafeObject as ModelDoc2).GetConfigurationByName(referencedConfiguration) as Configuration;

            
        }

        public override void AttachEventHandlers()
        {
            base.AttachEventHandlers();

            // attach event handlers 

            if (drawingDoc != null)
            {
                 

            }


        }

         public override void DettachEventHandlers()
        {

            // attach event handlers 

            if (drawingDoc != null)
            { 


            }

            base.AttachEventHandlers();

        }


      

    }
}
