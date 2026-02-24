using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using BlueByte.SOLIDWORKS.SDK.Diagnostics;
using BlueByte.SOLIDWORKS.SDK.UI;
using Microsoft.Win32;
using SimpleInjector;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// Base Add-in class. Make sure you decorate your class with <see cref="GuidAttribute"/> and make it COM-Visible with <see cref="ComVisibleAttribute"/>
    /// </summary>
    /// <seealso cref="SolidWorks.Interop.swpublished.ISwAddin" />
    [ComVisible(true)]
    public abstract class AddInBase : ISwAddin
    {
        /// <summary>
        /// Attaches the debugger.
        /// </summary>
        public void AttachDebugger()
        {
            Process process = Process.GetCurrentProcess();
            if (!Debugger.IsAttached)
            {
                var information = new StringBuilder();
                information.AppendLine();
                information.AppendLine($"Add-in = {this.Identity.Name}");
                information.AppendLine($"Process name = { process.ProcessName}");
                information.AppendLine($"Process Id   = { process.Id}");

 
                if (WindowsHelper.ShowMessageBox($"Attach Debugger? {information.ToString()}",true, $"{Identity.Name}", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    Debugger.Launch();
            }
        }

        /// <summary>
        /// Attaches the debugger.
        /// </summary>
        public static void StaticDebugger()
        {
            Process process = Process.GetCurrentProcess();
            if (!Debugger.IsAttached)
            {
                var information = new StringBuilder();
                information.AppendLine();
                information.AppendLine($"Process name = { process.ProcessName}");
                information.AppendLine($"Process Id   = { process.Id}");


                if (WindowsHelper.ShowMessageBox($"Attach Debugger? {information.ToString()}", true, string.Empty, MessageBoxButtons.OKCancel) == DialogResult.OK)
                    Debugger.Launch();
            }
        }






        #region fields 

        BitmapHandler handler = new BitmapHandler();

        #endregion

        #region properties 


        /// <summary>
        /// Gets the document manager.
        /// </summary>
        /// <value>
        /// The document manager.
        /// </value>
        public IDocumentManager DocumentManager { get; private set; }

        /// <summary>
        /// Gets the custom property manager.
        /// </summary>
        /// <value>
        /// The custom property manager.
        /// </value>
        public BlueByte.SOLIDWORKS.SDK.Core.CustomProperties.ICustomPropertyManager CustomPropertyManager { get; private set; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public ISOLIDWORKSApplication Application { get; private set; }

        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <value>
        /// The cookie.
        /// </value>
        public int Cookie { get; private set; }

        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <value>
        /// The identity.
        /// </value>
        public Identity Identity { get; private set; }

        /// <summary>
        /// Gets the DI container. SimpleInjector.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public Container Container { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger  Logger { get; private set; }

        /// <summary>
        /// Sets the type of the logger to be used.
        /// </summary>
        public LoggerType_e LoggerType { get; set; }

        #endregion        
        /// <summary>
        /// Initializes a new instance of the <see cref="AddInBase"/> class.
        /// </summary>
        public AddInBase()
        {
            try
            {

                this.Identity = Identity.Get(this);

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }


        }

        void Init()
        {
            var thisAssembly = new FileInfo(this.GetType().Assembly.Location);

            OnLoadAdditionalAssemblies(thisAssembly.Directory);

            OnLoggerTypeChosen(LoggerType_e.File);

            Initialize();



            if (IsInitialized == false)
                RegisterTypes(Container);


            Logger = Container.GetInstance<ILogger>();

            OnLoggerOutputSat(string.Empty);

            IsInitialized = true;
        }

        /// <summary>
        /// Sets the output folder of the logger.
        /// </summary>
        /// <param name="defaultDirectory"></param>
        protected virtual void OnLoggerOutputSat(string defaultDirectory)
        {
            if (Container != null)
            {
                if (LoggerType == LoggerType_e.File)
                    if (Logger != null)
                        Logger.OutputLocation = defaultDirectory;
            }
        }


        /// <summary>
        /// Registers additional types.
        /// </summary>
        /// <param name="container"></param>
        protected virtual void RegisterTypes(Container container)
        {
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Initializes task (Registers types and creates logger).
        /// </summary>
        public virtual void Initialize()
        {
            if (IsInitialized == false)
            {
                RegisterTypes();
            }
        }
        /// <summary>
        /// Sets the type of the logger.
        /// </summary>
        /// <param name="defaultType"></param>
        protected virtual void OnLoggerTypeChosen(LoggerType_e defaultType)
        {
            LoggerType = defaultType;
        }
        /// <summary>
        /// Load assemblies that failed loading.
        /// </summary>
        /// <param name="addinDirectory">Directory of the add-in.</param>
        protected virtual void OnLoadAdditionalAssemblies(DirectoryInfo addinDirectory)
        {
        }

        /// <summary>
        /// Connects to SOLIDWORKS.
        /// </summary>
        /// <param name="swApp">The sw application.</param>
        protected virtual void OnConnectToSOLIDWORKS(SldWorks swApp)
        {

        }

        Tuple<int, int, string, string>[] menuIds = default(Tuple<int, int, string,string>[]);
        private void BuildMenu()
        {
            var swApp = this.Application.As<SldWorks>();

            var menuIdsList =
                new List<Tuple<int, int, string, string>>();

            var createdMenus =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            var menuItems =
                AttributeHelper.GetAttributes<MenuItemAttribute>(this);

            if (menuItems == null)
                return;

            foreach (var menu in menuItems)
            {
                #region Image

                var imagePath = string.Empty;

                if (!string.IsNullOrWhiteSpace(menu.BmpFileNameInResources))
                {
                    imagePath =
                        handler.CreateFileFromResourceBitmap(
                            menu.BmpFileNameInResources,
                            this.GetType().Assembly);
                }

                #endregion

                var value =
                    (DocumentTypes_e)menu.DocumentType;

                foreach (DocumentTypes_e docType
                    in Enum.GetValues(typeof(DocumentTypes_e)))
                {

                    if (!value.HasFlag(docType))
                        continue;

                    var parts =
                        menu.Text
                        .Trim('\\')
                        .Split('\\');

                    string parentPath = null;

                    for (int i = 0; i < parts.Length; i++)
                    {
                        var current = parts[i];

                        var fullPath =
                            parentPath == null
                            ? current
                            : current + "@" + parentPath;

                        if (!createdMenus.ContainsKey(fullPath))
                        {
                            int id;

                            bool isLast =
                                i == parts.Length - 1;

                            if (isLast)
                            {
                                id =
                                    swApp.AddMenuItem4(
                                        (int)docType,
                                        Cookie,
                                        fullPath,
                                        menu.Position,
                                        menu.Callback,
                                        menu.MenuEnableState,
                                        menu.Hint,
                                        imagePath);
                            }
                            else
                            {
                                id =
                                    swApp.AddMenu(
                                        (int)docType,
                                        fullPath,
                                        menu.Position);
                            }

                            createdMenus.Add(fullPath, id);

                            menuIdsList.Add(
                                new Tuple<int, int, string, string>(
                                    id,
                                    (int)docType,
                                    fullPath,
                                    parentPath ?? string.Empty));
                        }

                        parentPath = fullPath;
                    }
                }
            }

            menuIds = menuIdsList.ToArray();
        }
        Tuple<int, int, int, string, string, string>[] popMenuIds = default(Tuple<int, int, int, string, string, string>[]);
        private void BuildPopMenu()
        {
            var menuIdsList = new List<Tuple<int, int, int, string, string, string>>();
            var menuItems = AttributeHelper.GetAttributes<PopMenuItemAttribute>(this);
            if (menuItems != null)
            {



                foreach (var menu in menuItems)
                {
                    var value =
                  (DocumentTypes_e)menu.DocumentType;

                    foreach (DocumentTypes_e docType
                        in Enum.GetValues(typeof(DocumentTypes_e)))
                    {

                        if (!value.HasFlag(docType))
                            continue;

                        menuIdsList.Add(new Tuple<int, int, int, string, string, string>(this.Application.As<SldWorks>().AddMenuPopupItem3((int)value, Cookie, (int)menu.SelectionType, menu.Text, menu.Callback, menu.MenuEnableState, menu.Text, menu.CustomNames), (int)menu.DocumentType, (int)menu.SelectionType, menu.Text, menu.Callback, null));

                    }

                }
            }


            popMenuIds = menuIdsList.ToArray();
        }


        /// <summary>
        /// Fires when the application is initialized. Register types of calling assembly.
        /// </summary>
        private void RegisterTypes()
        {
            if (Container == null)
            {
                Container = new Container();

                Container.Options.EnableAutoVerification = false;
               
                var assembly = Assembly.GetCallingAssembly();

                switch (LoggerType)
                {
                    case LoggerType_e.Console:
                        Container.RegisterSingleton<ILogger, ConsoleLogger>();
                        break;

                    case LoggerType_e.File:
                        Container.RegisterSingleton<ILogger, FileLogger>();
                        break;

                    case LoggerType_e.SQL:
                        Container.RegisterSingleton<ILogger, SQLLogger>();
                        break;
                        throw new Exceptions.SOLIDWORKSSDKException("Logger type was not specified.", null);
                    default:
                        break;
                }

                Container.RegisterInstance<ISOLIDWORKSApplication>(this.Application);

                RegisterDefaultTypes();

            }
             
        }


        /// <summary>
        /// Registers the default types. Register as singletons both <see cref="SldWorks"/> and <see cref="IDocumentManager"/>
        /// </summary>
        protected virtual void RegisterDefaultTypes()
        {

          
            Container.RegisterSingleton<IDocumentManager, DocumentManager>();
            Container.RegisterSingleton<CustomProperties.ICustomPropertyManager, CustomProperties.CustomPropertyManager>();
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


                var icons = AttributeHelper.GetAttributes<IconAttribute>(instance);

                if (icons != null)
                {

                    var icon = icons.FirstOrDefault();

                    if (icon == null)
                        return;

                    var addinFolder = GetAddInDirectory(t);
                    var imagePath = addinFolder.GetFiles().FirstOrDefault(x=> x.Name.ToLower().Equals(icon.IconFileName.ToLower()));
               
                    if (imagePath != null)
                    rk.SetValue("Icon Path", imagePath.FullName);
                }
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

                this.Application = new SOLIDWORKSApplication(ThisSW as SldWorks);

                Globals.Application = this.Application;

                var app = this.Application.As<SldWorks>();


                app.DestroyNotify += App_DestroyNotify;

                Init();

                this.Cookie = Cookie;


                var ignoreUIHelper = AttributeHelper.GetFirstAttribute<IgnoreUI>(this);


                if (ignoreUIHelper == null)
                    this.Application.As<SldWorks>().SetAddinCallbackInfo2(0, this, Cookie);


                BuildMenu();
                BuildPopMenu();

                CreateDefaultTypesInstances();

                OnConnectToSOLIDWORKS(this.Application.As<SldWorks>());
            }
            catch (Exception e)
            {

                var error = $"{e.Message} {e.StackTrace}";
                var ret = MessageBox.Show(error);

                switch (ret)
                {
                    case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        Clipboard.SetText(error);
                        break;
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        break;
                }
               

               
            }




            return true;
        }

        /// <summary>
        /// Creates the default types instances namely <see cref="IDocumentManager"/> and <see cref="ICustomPropertyManager"/>
        /// </summary>
        public virtual void CreateDefaultTypesInstances()
        {
            // init document manager 
            DocumentManager = Container.GetInstance<IDocumentManager>();

            DocumentManager.InitializeWithPreloadedDocuments();

            DocumentManager.AttachEventHandlers();

            CustomPropertyManager = Container.GetInstance<CustomProperties.ICustomPropertyManager>();

            CustomPropertyManager.Initialize();
        }

        private int App_DestroyNotify()
        {
            OnAddInPreClose();


            return 0;
        }


        /// <summary>
        /// Gets the add-in directory from the assembly of the add-in. 
        /// </summary>
        /// <returns></returns>
        public static DirectoryInfo GetAddInDirectory(Type t)
        {
            return (new FileInfo(t.Assembly.Location)).Directory;
        }


        /// <summary>
        /// Gets the add-in directory.
        /// </summary>
        /// <returns></returns>
        public DirectoryInfo GetAddInDirectory()
        {
            return (new FileInfo(this.GetType().Assembly.Location)).Directory;
        }

        /// <summary>
        /// Called before add-in is closed.
        /// </summary>
        public virtual void OnAddInPreClose()
        {
       
        }

        /// <summary>
        /// Disconnects from SOLIDWORKS. Do not call this method. Override <see cref="OnDisconnectFromSOLIDWORKS"/> and <see cref="OnAddInPreClose"/>
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            try
            {



                var app = this.Application.As<SldWorks>();
                app.DestroyNotify -= App_DestroyNotify;
                
                DestroyMenus();

                handler.CleanFiles();


                OnDisconnectFromSOLIDWORKS();

                if (CustomPropertyManager != null)
                    CustomPropertyManager.Dispose();

                if (DocumentManager != null)
                    DocumentManager.Dispose();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.Application.UnSafeObject);



                this.Application.Dispose();


                Container = null;

                System.GC.Collect();
                System.GC.Collect();

                System.GC.WaitForPendingFinalizers();

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        protected virtual void OnDisconnectFromSOLIDWORKS()
        {

        }

        private void DestroyMenus()
        {
            if (menuIds != null)
            {
                var t = menuIds.ToList();
                t.Reverse();
                
                foreach (var ts in t)
                    this.Application.As<SldWorks>().RemoveMenu(ts.Item2, ts.Item3, ts.Item4);
                
            }

            if (popMenuIds != null)
            {
                var t = popMenuIds.ToList();
                t.Reverse();
                
                

                foreach (var ts in t)
                    this.Application.As<SldWorks>().RemoveMenuPopupItem(ts.Item2, ts.Item3,ts.Item4, ts.Item5, ts.Item6,0);

            }
        }
    }


   
}



