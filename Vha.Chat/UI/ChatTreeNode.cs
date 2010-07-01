/*
* Vha.Chat
* Copyright (C) 2009-2010 Remco van Oosterhout
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
using Vha.Chat;

namespace Vha.Chat.UI
{
    public class ChatTreeNode : TreeNode
    {
        protected readonly MessageType _type;

        public ChatTreeNode(MessageType type, string text)
            : base(text)
        {
            this._type = type;
        }

        public bool ContainsNode(string text)
        {
            foreach (TreeNode node in this.Nodes)
            {
                if (node.Text == text)
                    return true;
            }
            return false;
        }

        public void RemoveNode(string text)
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

        public void AddNode(string text, string icon)
        {
            TreeNode node = new TreeNode(text);
            if (string.IsNullOrEmpty(icon) == false)
                node.ImageKey = node.SelectedImageKey = icon;
            int index = 0;
            foreach (TreeNode n in this.Nodes)
            {
                if (n.Text.CompareTo(text) > 0)
                {
                    this.Nodes.Insert(index, node);
                    return;
                }
                index++;
            }
            this.Nodes.Add(node);
        }

        public TreeNode GetNode(string text)
        {
            foreach (TreeNode node in this.Nodes)
            {
                if (node.Text == text)
                    return node;
            }
            return null;
        }

        public MessageType Type { get { return this._type; } }
    }
}
