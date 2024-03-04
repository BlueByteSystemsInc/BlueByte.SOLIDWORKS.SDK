using BlueByte.SOLIDWORKS.Extensions;
using BlueByte.SOLIDWORKS.Extensions.Helpers;
using BlueByte.SOLIDWORKS.SDK.Core.BillOfMaterials;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
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

            var solidworksManager = new BlueByte.SOLIDWORKS.Extensions.SOLIDWORKSInstanceManager();


            var swApp = solidworksManager.GetNewInstance();


            swApp.Visible = true;


            swApp.MaximizeWindow();

            string[] warnings;

            string[] errors;
            
            swApp.OpenDocument(@"C:\SOLIDWORKSPDM\Bluebyte\API\mrl\23908 ASSEM\MRL\PRODUCTS\23908 - ASSEMBLY-MRL.SLDASM", out errors, out warnings);


            var addin = new BlueByte.TestAddIn.AddIn();

            addin.ConnectToSW(swApp, 1510);


            var docs = addin.DocumentManager.GetDocuments();

            var doc = docs.FirstOrDefault(x => x.FileName.EndsWith("23908 - ASSEMBLY-MRL.SLDASM", StringComparison.OrdinalIgnoreCase));

            var assembly = doc as IAssembly;

            assembly.Initialize(assembly.ActiveConfigurationName);


            var bomSettings = new BOMSettings();

            var ret = assembly.GetQuantitiedReferences(bomSettings);

            foreach (var item in ret)
            {
                Console.WriteLine($"{item.Item1.PadRight(60)} {item.Item2}");
            }

            System.Threading.Thread.Sleep(1000);

            swApp.CloseAllDocuments(false);

            swApp.ExitApp();

            solidworksManager.ReleaseInstance(swApp);


            

        }
    }
}
