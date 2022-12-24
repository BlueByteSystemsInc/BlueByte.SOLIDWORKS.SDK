using System.ComponentModel;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    public interface ISOLIDWORKSObject : INotifyPropertyChanged

    {
        object UnSafeObject { get; }

        T As<T>();


    }
}