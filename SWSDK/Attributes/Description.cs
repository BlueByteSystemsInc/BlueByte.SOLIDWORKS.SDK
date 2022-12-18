using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    public class Description : Attribute
    {

        public string AddInDescription { get; set; }

        public Description(string Description)
        {
            this.AddInDescription = Description;
        }
    }
}
