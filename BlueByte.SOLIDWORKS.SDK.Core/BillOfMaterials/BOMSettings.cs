using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Core.BillOfMaterials
{
    /// <summary>
    /// BOM Settings
    /// </summary>
    public struct BOMSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether [ignore virtual components].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore virtual components]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreVirtualComponents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore bom excluded components].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore bom excluded components]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreBOMExcludedComponents { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore envelope components].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore envelope components]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreEnvelopeComponents { get; set; }

    }
}
