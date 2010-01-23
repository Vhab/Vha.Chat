/*
* Vha.Net
* Copyright (C) 2005-2009 Remco van Oosterhout
* See Credits.txt for all aknowledgements.
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; version 2 of the License only.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
* USA
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Vha.Net.Events
{
    public class ForwardEventArgs : EventArgs
    {
        private readonly Int32 _id1 = 0;
        private readonly Int32 _id2 = 0;
        public ForwardEventArgs(Int32 ID1, Int32 ID2)
        {
            this._id1 = ID1;
            this._id2 = ID2;
        }
        public Int32 ID1 { get { return this._id1; } }
        public Int32 ID2 { get { return this._id2; } }
    }
}
