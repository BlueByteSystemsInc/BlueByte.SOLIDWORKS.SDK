using SolidWorks.Interop.swconst;

namespace BlueByte.SOLIDWORKS.SDK.Core.Documents.Components
{
    public interface IComponent
    {
        IComponent[] Children { get; set; }

        void AddChild(IComponent child);

        void RemoveChild(IComponent child);

        bool IsVirtual { get; set; }

        swComponentSuppressionState_e SuppressionState { get; set; }

        IDocument Document { get; set; }

        string ReferencedConfiguration { get; set; }

        bool IsPatternInstance { get; set; }

        bool IsSpeedPak { get; set; }

        bool IsSmartComponent { get; set; }

        dynamic UnsafeObject { get; set; }


        string GetNameRelativeTo(IComponent rootComponent);

        string GetSelectionRelativeTo(IComponent rootComponent);

        void Initialize();
    }
}

 