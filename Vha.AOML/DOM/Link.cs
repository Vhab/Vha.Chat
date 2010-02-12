using System;
using System.Collections.Generic;
using System.Text;

namespace Vha.AOML.DOM
{
    public enum LinkType
    {
        Command,
        Window,
        Item
    }

    public abstract class Link
    {
        /// <summary>
        /// Returns the type of this link
        /// </summary>
        public LinkType Type { get { return this._type; } }

        public abstract Link Clone();

        #region internal
        private LinkType _type;
        
        internal Link(LinkType type)
        {
            this._type = type;
        }
        #endregion
    }
}
