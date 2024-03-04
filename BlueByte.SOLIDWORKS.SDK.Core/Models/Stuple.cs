using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueByte.SOLIDWORKS.SDK.Core.Models
{
    /// <summary>
    /// Tuple with settable properties. I know it's not cool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class Stuple<T,V>
    {
        /// <summary>
        /// Gets or sets the item1.
        /// </summary>
        /// <value>
        /// The item1.
        /// </value>
        public T Item1 { get; set; }
        /// <summary>
        /// Gets or sets the item2.
        /// </summary>
        /// <value>
        /// The item2.
        /// </value>
        public V Item2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stuple{T, V}"/> class.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        public Stuple(T item1, V item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }


        public override string ToString()
        {

            return $"{this.Item1.ToString().PadRight(20)} {this.Item2}";
        }
    }
}
