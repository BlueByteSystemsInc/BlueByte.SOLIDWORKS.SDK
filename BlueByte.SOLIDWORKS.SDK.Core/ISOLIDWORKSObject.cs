using System.ComponentModel;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    public interface ISOLIDWORKSObject : INotifyPropertyChanged

    {
        dynamic UnSafeObject { get; }

        T As<T>();


    }
}