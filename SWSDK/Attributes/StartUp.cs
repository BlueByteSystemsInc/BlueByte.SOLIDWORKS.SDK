using System;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    public class StartUp : Attribute
    {

        public bool Enabled { get; set; }

        public StartUp(bool enabled)
        {
            this.Enabled = enabled;
        }
    }
}
