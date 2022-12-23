using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Exceptions;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    public struct Identity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Identity"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Gets the specified addin.
        /// </summary>
        /// <param name="addin">The addin.</param>
        /// <returns></returns>
        /// <exception cref="SWSDK.IdentityInfoException">
        /// Addin class is not decorated with NameAttribute. - null
        /// or
        /// Addin add-in has an empty name. - null
        /// or
        /// Addin class is not decorated with DescriptionAttribute. - null
        /// or
        /// Addin add-in has an empty description. - null
        /// </exception>
        public static Identity Get(object addin)
        {
            var i = default(Identity);

            i = new Identity();

            var addInNameAtt = AttributeHelper.GetFirstAttribute<NameAttribute>(addin);
            if (addInNameAtt == null)
                throw new IdentityInfoException("Addin class is not decorated with NameAttribute.", null);

            if (string.IsNullOrWhiteSpace(addInNameAtt.AddInName))
                throw new IdentityInfoException("Addin add-in has an empty name.", null);

            i.Name = addInNameAtt.AddInName;

            var addInDescriptionAtt = AttributeHelper.GetFirstAttribute<Description>(addin);
            if (addInDescriptionAtt == null)
                throw new IdentityInfoException("Addin class is not decorated with DescriptionAttribute.", null);


            i.Description = addInDescriptionAtt.AddInDescription;

            if (string.IsNullOrWhiteSpace(addInDescriptionAtt.AddInDescription))
                throw new IdentityInfoException("Addin add-in has an empty description.", null);


            var startUp = AttributeHelper.GetFirstAttribute<StartUp>(addin);
            if (startUp != null)
                i.Enabled = startUp.Enabled;
            return i;
        }
    }
}
