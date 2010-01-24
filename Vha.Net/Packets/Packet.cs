/*
* Vha.Net
* Copyright (C) 2005-2010 Remco van Oosterhout
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
using System.Net;
using System.Collections;
using System.Text;

namespace Vha.Net.Packets
{
	/// <summary>
	/// The base packet for all aoChat packets
	/// </summary>
	/// <remarks>
	/// All packets are derived from this base class.
	///
	///		Each packet should override the BytesToData function to 
	///		translate incomming socket data to a readable format.
	///		
	///		A constructor is provided in this base class to initialize
	///		the read-only type and direction members.
	/// 
	/// </remarks>
	public class Packet
	{
		#region Packet Type Enumeration
        public enum Type : short
        {
            // INCOMING PACKETS
            NULL = -1,
            LOGIN_SEED = 0,
            LOGIN_OK = 5,
            LOGIN_ERROR = 6,
            LOGIN_CHARACTERLIST = 7,
            CLIENT_UNKNOWN = 10,
            CLIENT_NAME = 20,
            VICINITY_MESSAGE = 34,
            ANON_MESSAGE = 35,
            SYSTEM_MESSAGE = 36,
            MESSAGE_SYSTEM = 37,
            FRIEND_STATUS = 40,
            FRIEND_REMOVED = 41,
            PRIVATE_CHANNEL_JOIN = 52,
            PRIVATE_CHANNEL_PART = 53,
            PRIVATE_CHANNEL_KICKALL = 54,
            PRIVATE_CHANNEL_CLIENTJOIN = 55,
            PRIVATE_CHANNEL_CLIENTPART = 56,
            CHANNEL_JOIN = 60,
            CHANNEL_PART = 61,
            PONG = 100,
            FORWARD = 110,
            AMD_MUX_INFO = 1100,

            // OUTGOING PACKETS
            LOGIN_RESPONSE = 2,
            LOGIN_SELCHAR = 3,
            FRIEND_ADD = 40,
            FRIEND_REMOVE = 41,
            CHANNEL_UPDATE = 64,
            CHANNEL_CLIMODE = 66,
            PRIVATE_CHANNEL_INVITE = 50,
            PRIVATE_CHANNEL_KICK = 51,
            CLIENTMODE_GET = 70,
            CLIENTMODE_SET = 71,
            PING = 100,
            CHAT_COMMAND = 120,

            // BIDIRECTIONAL PACKETS
            NAME_LOOKUP = 21,
            PRIVGRP_MESSAGE = 57,
            CHANNEL_MESSAGE = 65,
            PRIVATE_MESSAGE = 30,
        }
		#endregion // Packet Types

		/// <summary>
		/// the maximum amount of data that can be sent in a single packet.
		/// </summary>
		public const Int32 MaxPacketSize = 8000;

		/// <summary>
		/// the Packet.Type of the message
		/// </summary>
		protected readonly Packet.Type _type;

		/// <summary>
		/// the byte array containing packet data
		/// </summary>
		private readonly byte[] _bytes = null;

		/// <summary>
		/// the array of packet data in readable format
		/// </summary>
		private ArrayList _msg;

		protected PacketQueue.Priority _priority = PacketQueue.Priority.Standard;
		public PacketQueue.Priority Priority
		{
			get { return this._priority; }
			set { this._priority = value; }
		}

		/// <summary>
		/// the default constructor
		/// </summary>
		internal Packet()
		{
			this._msg = new ArrayList();
		}

		/// <summary>
		/// Constructor for setting packet type and direction
		/// </summary>
		/// <param name="type">the Packet.Type of the message</param>
		/// <param name="dir">the direction of the packet, in or out</param>
		internal Packet(Packet.Type type) { this._type = type; }
		
		/// <summary>
		/// the constructor for incomming packets
		/// </summary>
		/// <param name="type">the Packet.Type of the message</param>
		/// <param name="data">the byte array containing socket data</param>
		internal Packet(Packet.Type type, byte[] data)
		{
			BytesToData(data);
			this._type = type;
		}

		/// <summary>
		/// The read-only type of the message
		/// </summary>
		/// <value>returns the Packet.Type</value>
		internal Packet.Type PacketType
		{
			get	{ return this._type; }
		}
		/// <summary>
		/// the read-only data in the packet
		/// </summary>
		/// <value>returns a byte array containing packet information</value>
		internal byte[] PacketData
		{
			get { return this._bytes; }
		}

		/// <summary>
		/// Method for adding data to be sent in a specific location in the array.
		/// </summary>
		/// <param name="index">place to insert the data</param>
		/// <param name="o">object to insert into the array</param>
		internal void AddData(int index, object o)
		{
			if (this._msg == null)
			{
				this._msg = new ArrayList();
			}
			this._msg.Insert(index, o);
		}

		/// <summary>
		/// Adds an object to the end of the data array
		/// </summary>
		/// <param name="o">the object to add</param>
		internal void AddData(object o)
		{
			if (this._msg == null)
			{
				this._msg = new ArrayList();
			}
			this._msg.Add(o);
		}

		/// <summary>
		/// The data in readable format
		/// </summary>
		/// <value>returns the arraylist containing packet data
		/// </value>
		internal ArrayList Data
		{
			get	{ return this._msg;	}
		}
		
		/// <summary>
		/// Method for pulling a string off the byte array.
		/// </summary>
		/// <param name="data">the byte array</param>
		/// <param name="offset">index to start pulling from the array</param>
		/// <returns>returns an AoString object</returns>
		internal static NetString popString(ref byte[] data, ref Int32 offset)
		{
			if (data == null || data.Length - offset < 3)
				return new NetString(String.Empty);

			short len = popShort(ref data, ref offset);
            NetString ret;
            if (data.Length >= len && len > 0)
            {
                ret = new NetString(data, offset, len);
            }
            else
            {
                ret = new NetString("");
            }
			offset += len;
			return ret;
		}

		/// <summary>
		/// Method for pulling a byte array off the byte array.
		/// </summary>
		/// <param name="data">the byte array</param>
		/// <param name="offset">index to start pulling from the array</param>
		/// <returns>returns a byte array</returns>
		internal static byte[] popData(ref byte[] data, ref Int32 offset)
		{
			if (data == null || data.Length - offset < 3)
				return null;

			short len = popShort(ref data, ref offset);
            if (len < 0)
                return new byte[0];
			byte[] ret = new byte[len];
			Array.Copy(data, offset, ret, 0, len);
			offset += len;
			return ret;
		}

		/// <summary>
		/// Method for pulling an integer from the byte array.
		/// </summary>
		/// <param name="data">the byte array</param>
		/// <param name="offset">index where to start pulling from the array</param>
		/// <returns>returns a 4-byte integer</returns>
		internal static Int32 popInteger(ref byte[] data, ref Int32 offset)
		{
			if (data.Length - offset < 4)
				return 0;

			Int32 ret = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, offset));
			offset += 4;
			return ret;
		}

        /// <summary>
        /// Method for pulling an unsigned integer from the byte array.
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <param name="offset">index where to start pulling from the array</param>
        /// <returns>returns a 4-byte unsigned integer</returns>
        internal static UInt32 popUnsignedInteger(ref byte[] data, ref Int32 offset)
        {
            if (data.Length - offset < 4)
                return 0;

            UInt32 ret = NetConvert.NetworkToHostOrder(BitConverter.ToUInt32(data, offset));
            offset += 4;
            return ret;
        }

		/// <summary>
		/// Method for pulling a 5-byte channel id off the array
		/// </summary>
		/// <param name="data">byte array containing the data</param>
		/// <param name="offset">the index where to begin pulling off the array</param>
		/// <returns>5-byte channel id</returns>
		internal static BigInteger popChannelID(ref byte[] data, ref Int32 offset)
		{
			if (data.Length - offset < 5)
				return 0;
			
			byte[] bi = new byte[5];
			bi[0] = data[offset++];
			bi[1] = data[offset++];
			bi[2] = data[offset++];
			bi[3] = data[offset++];
			bi[4] = data[offset++];
			BigInteger ret = new BigInteger(bi);
			return ret;
		}

		/// <summary>
		/// Method for pulling a short (2-byte) integer off the array. 
		/// </summary>
		/// <param name="data">the byte array</param>
		/// <param name="offset">index where to begin pulling off the array</param>
		/// <returns>a short (2-byte) integer</returns>
		internal static Int16 popShort(ref byte[] data, ref Int32 offset)
		{
			if (data.Length - offset < 2)
				return 0;

			Int16 ret = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data, offset));
			offset += 2;
			return ret;
		}

        /// <summary>
        /// Method for pulling an unsigned short (2-byte) integer off the array. 
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <param name="offset">index where to begin pulling off the array</param>
        /// <returns>an unsigned short (2-byte) integer</returns>
        internal static UInt16 popUnsignedShort(ref byte[] data, ref Int32 offset)
        {
            if (data.Length - offset < 2)
                return 0;

            UInt16 ret = NetConvert.NetworkToHostOrder(BitConverter.ToUInt16(data, offset));
            offset += 2;
            return ret;
        }

        /// <summary>
        /// Method for pulling a byte off the array. 
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <param name="offset">index where to begin pulling off the array</param>
        /// <returns>a byte</returns>
        internal static Byte popByte(ref byte[] data, ref Int32 offset)
        {
            if (data.Length - offset < 1)
                return 0;

            offset += 1;
            return data[offset - 1];
        }

        /// <summary>
        /// Method for pulling a char (1-byte) integer off the array. 
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <param name="offset">index where to begin pulling off the array</param>
        /// <returns>a char (1-byte) integer</returns>
        internal static Char popChar(ref byte[] data, ref Int32 offset)
        {
            if (data.Length - offset < 1)
                return (Char)0;

            offset += 1;
            return (Char)data[offset - 1];
        }

        /// <summary>
        /// Method for pulling a boolean (1-byte) off the array. 
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <param name="offset">index where to begin pulling off the array</param>
        /// <returns>a boolean (1-byte)</returns>
        internal static Boolean popBoolean(ref byte[] data, ref Int32 offset)
        {
            if (data.Length - offset < 1)
                return false;

            offset += 1;
            return (data[offset - 1] != 0);
        }

		/// <summary>
		/// Method for converting the array data into bytes for transmission over the socket.
		/// </summary>
		/// <returns>a byte array</returns>
		protected virtual byte[] DataToBytes()
		{
			ArrayList a = new ArrayList();
			int len = 0;

			if (this.Data == null)
				return new byte[0];

			foreach (Object o in this.Data)
			{
				byte[] b = null;
				String name = "";
				if (o != null)
					name = o.GetType().FullName;
				else
					System.Diagnostics.Trace.WriteLine("Trying to send a null?");

				switch (name)
				{
					case "":
						b = new byte[] { 0 };
						break;
					case "System.Boolean":
						b = BitConverter.GetBytes((Boolean)o);
						break;
					case "System.Char":
						b = BitConverter.GetBytes((Char)o);
						break;
					case "System.Double":
						b = BitConverter.GetBytes((Double)o);
						break;
					case "System.Int16":
						b = BitConverter.GetBytes((Int16)o);
						break;
					case "System.Int32":
						b = BitConverter.GetBytes((Int32)o);
						break;
					case "System.Int64":
						b = BitConverter.GetBytes((Int64)o);
						break;
					case "System.Single":
						b = BitConverter.GetBytes((Single)o);
						break;
					case "System.UInt16":
						b = BitConverter.GetBytes((UInt16)o);
						break;
					case "System.UInt32":
						b = BitConverter.GetBytes((UInt32)o);
						break;
					case "System.UInt64":
						b = BitConverter.GetBytes((UInt64)o);
						break;
					case "System.Byte[]":
						b = (byte[])o;
						break;
					case "System.Byte":
						b = new byte[] { (byte)o };
						break;
					case "System.String":
						b = new NetString(o.ToString()).GetBytes();
						break;
					default:
						if (o.GetType() == typeof(BigInteger) || o.GetType().IsSubclassOf(typeof(BigInteger)))
							b = ((BigInteger)o).getBytes();
                        else if (o.GetType() == typeof(NetString) || o.GetType().IsSubclassOf(typeof(NetString)))
							b = ((NetString)o).GetBytes();
						else
							System.Diagnostics.Trace.WriteLine("Hmm... no idea how to process type: " + o.ToString());
						break;
				}

				if (b != null)
				{
					len += b.Length;
					a.Add(b);
				}
			} // end foreach

			if (a.Count == 0)
				return null;
			else 
			{
				byte[] ret = new byte[len];
				int i = 0;
				foreach (byte[] d in a)
				{
					d.CopyTo(ret, i);
					i += d.Length;
				}

				return ret;
			}
		}

		/// <summary>
		/// Method for getting the packet data as a byte array
		/// </summary>
		/// <returns>a byte array</returns>
		virtual internal byte[] GetBytes()
		{
			return this.DataToBytes();
		}

		/// <summary>
		/// Abstract method for converting byte array into a readable format.
		/// </summary>
		/// <param name="data">a byte array</param>
		/// <remarks>All derived packets should contain an implementation for this method.</remarks>
		virtual protected void BytesToData(byte[] data) {}

	}
}
