namespace BlueByte.SOLIDWORKS.SDK.Core
{
    /// <summary>
    /// Wrapper over the <see cref="SolidWorks.Interop.sldworks.SldWorks"/> application.
    /// </summary>
    public interface ISOLIDWORKSApplication
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
         
    }


}
