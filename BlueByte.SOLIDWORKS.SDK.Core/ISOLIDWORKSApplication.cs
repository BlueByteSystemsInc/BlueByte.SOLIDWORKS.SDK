using System;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// Wrapper over the <see cref="SolidWorks.Interop.sldworks.SldWorks"/> application.
    /// </summary>
    public interface ISOLIDWORKSApplication : IDisposable
    {
        /// <summary>
        /// Gets or sets the unsafe object.
        /// </summary>
        /// <value>
        /// The un safe object.
        /// </value>
        dynamic UnSafeObject { get; }

        /// <summary>
        /// Casts the unsafe object as the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T As<T>();




        /// <summary>
        /// Sends the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SendWarningMessage(string message);

        /// <summary>
        /// Sends the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SendErrorMessage(string message);

        /// <summary>
        /// Sends the information message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SendInformationMessage(string message);


        /// <summary>
        /// Gets the last save error message.
        /// </summary>
        /// <returns></returns>
        string GetLastSaveErrorMessage();


        /// <summary>
        /// Gets the last 20 error message.
        /// </summary>
        /// <returns></returns>
        string[] GetRecentErrors();

    }


}
