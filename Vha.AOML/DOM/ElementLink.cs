using System;
using System.Collections.Generic;
using System.Text;

namespace Vha.AOML.DOM
{
    /// <summary>
    /// A link to an element tree to be shown.
    /// Commonly known as text://
    /// </summary>
    public class ElementLink : Link
    {
        /// <summary>
        /// Returns the element tree which should be shown
        /// </summary>
        public readonly Element Element;

        /// <summary>
        /// Initializes a new instance of ElementLink
        /// </summary>
        /// <param name="command">The element tree which should be shown</param>
        public ElementLink(Element element)
            : base(LinkType.Element)
        {
            if (element == null)
                throw new ArgumentNullException();
            this.Element = element;
        }

        /// <summary>
        /// Creates a clone of this ElementLink
        /// </summary>
        /// <returns>A new ElementLink</returns>
        public override Link Clone()
        {
            return new ElementLink(this.Element);
        }
    }
}
