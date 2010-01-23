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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Vha.Net.Packets;

namespace Vha.Net
{
	public class PacketQueue
	{
		public enum Priority
		{
			Urgent = 1,
			Standard,
			Low
		}
        private Queue<Packet> _queueLow = new Queue<Packet>();
        private Queue<Packet> _queueNormal = new Queue<Packet>();
        private Queue<Packet> _queueHigh = new Queue<Packet>();
        private DateTime lastAction;
        public double delay = 0;

        public PacketQueue()
        {
            lastAction = DateTime.Now;
        }

		public Packet Dequeue()
		{
            lastAction = DateTime.Now;

			lock(this._queueHigh)
			{
				if (this._queueHigh.Count > 0)
				{
                    return this._queueHigh.Dequeue();
				}
			}
            lock (this._queueNormal)
            {
                if (this._queueNormal.Count > 0)
                {
                    return this._queueNormal.Dequeue();
                }
            }
            lock (this._queueLow)
            {
                if (this._queueLow.Count > 0)
                {
                    return this._queueLow.Dequeue();
                }
            }
			return null;
		}
        public void Enqueue(Priority order, Packet item)
		{
            if (item == null)
				return;

			if (! Enum.IsDefined(typeof(Priority), order))
				order = Priority.Standard;

            switch (order)
            {
                case Priority.Low:
                    lock (this._queueLow)
                    {
                        this._queueLow.Enqueue(item);
                    }
                    break;
                case Priority.Urgent:
                    lock (this._queueHigh)
                    {
                        this._queueHigh.Enqueue(item);
                    }
                    break;
                default:
                    lock (this._queueNormal)
                    {
                        this._queueNormal.Enqueue(item);
                    }
                    break;
            }
		}
		public Int32 Count
		{
            get
            {
                Int32 count = this._queueHigh.Count;
                count += this._queueNormal.Count;
                count += this._queueLow.Count;
                return count;
            }
		}

        public bool Available
        {
            get
            {
                TimeSpan ts = DateTime.Now - lastAction;
                if (ts.TotalMilliseconds > delay && this.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
	}
}
