using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Runtime.InteropServices;

namespace SWSDK
{
    [ComVisible(true)]
    public abstract class AddInBase : ISwAddin
    {
        #region properties 

        public SldWorks Application { get; private set; }

        public int Cookie { get; private set; }

        public Identity Identity { get; private set; }
        
        #endregion 

        public AddInBase()
        {
            try
            {

                this.Identity = Identity.Get(this);
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public virtual void ConnectToSOLIDWORKS(SldWorks swApp)
        {

        }


        #region com registration

        [ComRegisterFunction]
        private static void RegisterAssembly(Type t)
        {
            try
            {
                var instance = Activator.CreateInstance(t);
                var identity = Identity.Get(instance);

                const string AddInFormat = @"SOFTWARE\SolidWorks\AddIns\{0:b}";
                string keyPath = string.Format(AddInFormat, t.GUID);
                RegistryKey rk = Registry.LocalMachine.CreateSubKey(keyPath);
                rk.SetValue("Title", identity.Name);
                rk.SetValue("Description", identity.Description);
                rk.SetValue("StartUp", identity.Enabled ? 1 : 0);
            }
            catch (Exception ex)
            {
     
            }
        }

        [ComUnregisterFunction]
        private static void UnregisterAssembly(Type t)
        {
            try
            {
                bool doesExist = false;
                const string AddInFormat = @"SOFTWARE\SolidWorks\AddIns\{0:b}";
                string keyPath = string.Format(AddInFormat, t.GUID);
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    doesExist = key == null ? false : true;
                }
                if (doesExist) Registry.LocalMachine.DeleteSubKey(keyPath);
            }
            catch (Exception ex)
            {
             
            }
        }

        #endregion


        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            try
            {
                this.Application = ThisSW as SldWorks;

                this.Cookie = Cookie;


                // enable menu callbacks
                this.Application.SetAddinCallbackInfo2(0, this, Cookie);

                
            }
            catch (Exception)
            {

                throw;
            }




            return true;
        }
        public bool DisconnectFromSW()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
    
    
    
    }
}
