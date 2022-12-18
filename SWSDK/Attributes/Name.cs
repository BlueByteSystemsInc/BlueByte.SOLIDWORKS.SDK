using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Attributes
{
    public class NameAttribute : Attribute
    {

        public string AddInName { get; set; }

        public NameAttribute(string name)
        {
            this.AddInName = name;
        }
    }
}
