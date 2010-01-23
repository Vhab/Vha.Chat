/*
* Vha.Chat
* Copyright (C) 2009 Remco van Oosterhout
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
using System.Windows.Forms;

namespace Vha.Chat
{
    public class ChatTreeNode : TreeNode
    {
        protected readonly ChatInputType _type;

        public ChatTreeNode(ChatInputType type, string text) : base(text)
        {
            this._type = type;
        }

        public bool ContainsText(string text)
        {
            foreach (TreeNode node in this.Nodes)
            {
                if (node.Text == text)
                    return true;
            }
            return false;
        }

        public void RemoveText(string text)
        {
            foreach (TreeNode node in this.Nodes)
            {
                if (node.Text == text)
                {
                    this.Nodes.Remove(node);
                    return;
                }
            }
        }

        public ChatInputType Type { get { return this._type; } }
    }
}
