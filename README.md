## SDK Infrastructure: Base Classes, Attributes, and Core Concepts


### VS Templates 

>[!Note]
>Please make sure to update the nuget packages. The templates are always behind the nuget packages.
```bash
https://marketplace.visualstudio.com/items?itemName=BlueByteSystemsInc.SWSDK32
```

## ▶️ YouTube AddIn Example

[![Example Tutorial](https://img.youtube.com/vi/D0HywJrycBQ/maxresdefault.jpg)](https://www.youtube.com/watch?v=D0HywJrycBQ)




### AddInBase (Base Class of Your Addin)
All SOLIDWORKS add-ins using this SDK inherit from `AddInBase`, which provides:

- **Key Properties:**
  - `Identity`: Holds your add-in's Name, Description, and Startup flag (see below).
  - `DocumentManager`: Lets you track and interact with SOLIDWORKS documents and events.
  - `Application`: Access to the SOLIDWORKS application object.
  - `CustomPropertyManager`: Interface for custom properties in a document.
  - `Logger`, `LoggerType`, `Container`: Diagnostic and dependency injection support.
  - `Cookie`: Internal SOLIDWORKS connection identifier.

- **Initialization:**  
  The `AddInBase` constructor reads your class attributes (Name, Description, StartUp) and populates `Identity`.

- **Connect/Disconnect Methods:**
  - `OnConnectToSOLIDWORKS(SldWorks app)`: Entry point called when SOLIDWORKS loads your add-in. You override this to set up menus, events, and any state.
  - `OnDisconnectFromSOLIDWORKS()`: Called when your add-in is unloaded. Use this to release resources or unsubscribe from events.

### Attributes: Name, Description, Startup, Icon, and MenuItem

- **NameAttribute**  
  Decorates your add-in class to specify the display name.

  ```csharp
  [Name("Addin")]
  ```

- **DescriptionAttribute**  
  Provides a user-friendly description visible in SOLIDWORKS Add-ins manager.

  ```csharp
  [Description("This is the description")]
  ```

- **StartUpAttribute**  
  Controls whether your add-in loads by default upon SOLIDWORKS startup.

  ```csharp
  [StartUp(true)]
  ```

- **IconAttribute**  
  Associates a custom icon (16x16) for your add-in, shown in UI.

  ```csharp
  [Icon("icon.ico")]
  ```

- **MenuItemAttribute**  
  Used to declaratively add menus and menu items to SOLIDWORKS, specifying document type, item label, click handler, and icon.

  ```csharp
  [MenuItem("SDK", swDocumentTypes_e.swDocNONE, true)]
  [MenuItem("Click Me...@SDK", swDocumentTypes_e.swDocNONE, false, nameof(OnMenuClick), "ToolbarSmall.bmp")]
  ```

  - **Parameters:**
    - Text/Name: The menu or item label.
    - Document type: Parts, Assemblies, Drawings, or "none" for global.
    - IsMenu: True for parent menu, false for menu item.
    - Handler method name: For menu item click actions.
    - Icon file (optional): To show alongside item.

### Menu Concepts
- **Menus in AddInBase:**  
  Use `[MenuItem]` attributes to build hierarchical menus.  
  Handler methods, like `OnMenuClick`, implement response logic.

### The Identity, Name, and Description System

- **Identity struct**  
  Built from your class attributes, ensures every add-in has a unique and displayable name and description.
  - **Name**: Provided via `[Name]` attribute.
  - **Description**: Provided via `[Description]` attribute.

### Display Concepts
- **Icons and DisplayText**
  - IconAttribute and DisplayText (in properties, menu items, or custom UI elements) make your add-in visually user-friendly.

### DocumentManager

- **Purpose:**  
  Central access point for all open SOLIDWORKS documents, document events (open/close/save), and document-specific operations.

- **Capabilities:**  
  - Subscribe to document lifecycle events like `ActiveDocumentChanged`, `DocumentGotOpened`, `DocumentGotClosed`.
  - Get info and collections with `GetDocuments()`.
  - Access `ActiveDocument` for the currently focused file.

  **Example usage:**  
  ```csharp
  this.DocumentManager.ActiveDocumentChanged += DocumentManager_ActiveDocumentChanged;
  var docs = DocumentManager.GetDocuments();
  ```

  Acts as the “hub” for files and events in your add-in.

### Connect/Disconnect Methods

- **OnConnectToSOLIDWORKS(SldWorks app)**
  - Called when SOLIDWORKS loads your add-in.
  - Setup document/event hooks, build menus, initialize your environment.

- **OnDisconnectFromSOLIDWORKS()**
  - Called before unloading.
  - Free resources, unsubscribe listeners, cleanup.

### Other Concepts: Property Display, DI Container, Logger

- **DisplayText**:  
  - Used in properties and menu items for UI presentation.

- **Container (SimpleInjector):**
  - Integrates dependency injection for advanced scenarios.

- **Logger and LoggerType:**
  - For logging diagnostics and errors.

---
## Complete Explanation of TestAddIn Example

### What is TestAddIn?

TestAddIn is a sample SOLIDWORKS add-in included in the SDK repository, showcasing best practices for building add-ins with Blue Byte Systems’ SDK. It serves as both a template and a functional example.

**Key Features Demonstrated:**
- How to declare add-in metadata (name, description, startup, icon)
- How to add custom menus and menu items using attributes
- How to interact with SOLIDWORKS documents and respond to their events
- How to handle menu clicks and show information to the user
- Connecting/disconnecting lifecycle hooks

### Project Structure

- `SWSDK.TestAddIn/AddIn.cs`: Main add-in class. Decorated with attributes and inherits from `AddInBase`.
- `SWSDK.TestAddIn/SWSDK.TestAddIn.csproj`: Project file, references SDK, interops, and dependencies.
- `SWSDK.TestAddIn/packages.config`: NuGet dependencies for Interops and SimpleInjector.
- `icon.ico`, `ToolbarSmall.bmp`: Icon files for menu display.

### How TestAddIn Works

- Uses SDK attributes to set its name, icon, description, and menus.  
- The `[MenuItem]` attributes build an SDK menu for each document type (global, part, assembly).
- Upon connection (`OnConnectToSOLIDWORKS`), subscribes to document events (like activation).
- When the menu item "Click Me..." is clicked, the add-in finds all open documents, builds a list, and sends it to the SOLIDWORKS information bar.

**Sample Handler:**
```csharp
public void OnMenuClick()
{
    var docs = DocumentManager.GetDocuments();
    var info = string.Join("\n", docs.Select(x => x.FileName));
    Application.SendInformationMessage(info);
}
```

### Using TestAddIn as Your Starting Point

1. **Clone the repository**
   
   ```sh
   git clone https://github.com/BlueByteSystemsInc/BlueByte.SOLIDWORKS.SDK.git
   ```

2. **Open in Visual Studio and build SWSDK.TestAddIn.**
   - Restore NuGet packages if needed.
   - Build for Debug or Release.

3. **Register the DLL with SOLIDWORKS**
   - Release build registers it for COM automatically (`RegisterForComInterop`).
   - Or, use `regasm` to register manually:

     ```sh
     regasm /codebase SWSDK.TestAddIn.dll
     ```

4. **Launch SOLIDWORKS and enable the add-in**
   - Find "Addin" in Tools > Add-ins, enable it.

5. **Try the custom menu**
   - Open or create parts/assemblies.
   - Use the SDK menu and "Click Me..." item to see document info.

### Customizing for Your Own Add-In

- Copy TestAddIn project as your template.
- Change `[Name]`, `[Description]`, `[Icon]` attributes.
- Adjust menu structure using `[MenuItem]` attributes.
- Implement your own menu handlers and event logic, using DocumentManager and Application features.
- Expand functionality by leveraging SDK base classes and core features.

### Why Use TestAddIn?

TestAddIn illustrates all the foundational SDK features.
- Minimal boilerplate—focus on functionality, not setup.
- Demonstrates robust SOLIDWORKS event handling.
- Shows simple menu/UI extension.

**Best Practice:**  
Start with TestAddIn, build and register, then iteratively add your add-in features, event handlers, and custom menu commands.


---
TestAddIn is your gateway example for SOLIDWORKS add-ins with Blue Byte Systems SDK.


