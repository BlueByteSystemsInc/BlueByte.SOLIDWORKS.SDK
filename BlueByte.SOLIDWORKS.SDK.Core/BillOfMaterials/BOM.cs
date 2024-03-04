using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.BillOfMaterials
{
    public class BOM
    {

        
    }


    public class Column
    {

    }

    public class Property
    {
        public Object Value { get; set; }

        public string DisplayText { get; set; }

        public string ColumnName { get; set; }
    }
    public class BOMRow
    {
        public string MyProperty { get; set; }

        public IDocument[] Documents { get; set; }

        public int Quantity { get; set; }
    }
}
