using BlueByte.SOLIDWORKS.Extensions;
using BlueByte.SOLIDWORKS.Extensions.Helpers;
using BlueByte.SOLIDWORKS.SDK.Core.BillOfMaterials;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

           


            for (int i = 0; i < 10; i++)
            {
                var solidworksManager = new BlueByte.SOLIDWORKS.Extensions.SOLIDWORKSInstanceManager();


                var swApp = solidworksManager.GetNewInstance();


                swApp.Visible = true;


                swApp.MaximizeWindow();

                string[] warnings;

                string[] errors;

                swApp.OpenDocument(@"C:\SOLIDWORKSPDM\Bluebyte\api\sandbox\24919\24919TRL - ASSEMBLY.SLDASM", out errors, out warnings);


                var addin = new BlueByte.TestAddIn.AddIn();

                addin.ConnectToSW(swApp, 1510);


                var docs = addin.DocumentManager.GetDocuments();

                var doc = docs.FirstOrDefault(x => x.FileName.EndsWith("4919TRL - ASSEMBLY.SLDASM", StringComparison.OrdinalIgnoreCase));

                var assembly = doc as IAssembly;

                var retf = assembly.Initialize(assembly.ActiveConfigurationName);

               


                var bomSettings = new BOMSettings();

                bomSettings.IgnoreBOMExcludedComponents = true;
                bomSettings.IgnoreVirtualComponents = true;
                bomSettings.IgnoreEnvelopeComponents = true;

                var ret = assembly.GetQuantitiedReferences(bomSettings);

                foreach (var item in ret)
                {
                    Console.WriteLine($"{item.Item1.PadRight(60)} {item.Item2}");
                }

                System.Threading.Thread.Sleep(1000);


                swApp.CloseAllDocuments(false);

                swApp.ExitApp();

                solidworksManager.ReleaseInstance(swApp);

                System.Diagnostics.Debugger.Break();

                Console.Clear();
            }
            


            

        }
    }
}
