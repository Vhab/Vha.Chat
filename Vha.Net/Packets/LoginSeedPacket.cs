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
using System.Diagnostics;
using System.Net;

namespace Vha.Net.Packets
{
    internal class LoginSeedPacket : Packet
    {
        private const String seed1 = "eca2e8c85d863dcdc26a429a71a9815ad052f6139669dd659f98ae159d313d13c6bf2838e10a69b6478b64a24bd054ba8248e8fa778703b418408249440b2c1edd28853e240d8a7e49540b76d120d3b1ad2878b1b99490eb4a2a5e84caa8a91cecbdb1aa7c816e8be343246f80c637abc653b893fd91686cf8d32d6cfe5f2a6f";
        private const String seed2 = "9c32cc23d559ca90fc31be72df817d0e124769e809f936bc14360ff4bed758f260a0d596584eacbbc2b88bdd410416163e11dbf62173393fbc0c6fefb2d855f1a03dec8e9f105bbad91b3437d8eb73fe2f44159597aa4053cf788d2f9d7012fb8d7c4ce3876f7d6cd5d0c31754f4cd96166708641958de54a6def5657b9f2e92";
        private const String seed3 = "5";

        internal LoginSeedPacket() : base() { }

        internal LoginSeedPacket(String username, String password, String seed)
            : base(Packet.Type.LOGIN_RESPONSE)
        {
            this.Priority = PacketQueue.Priority.Urgent;
            this.AddData((int)0);
            this.AddData(username.Trim());
            this.AddData(this.makeResponse(this.makeRandomSeed().ToLower(), username, seed, password));
        }

        internal LoginSeedPacket(Packet.Type type, byte[] data) : base(type, data) { }

        override protected void BytesToData(byte[] data)
        {
            if (data == null || data.Length < 3) { return; }

            int offset = 0;
            this.AddData(popString(ref data, ref offset));
        }

        internal String Seed
        {
            get
            {
                if (this.Data == null || this.Data.Count < 1)
                {
                    return null;
                }
                else
                {
                    Object o = this.Data[0];
                    return ((NetString)o).Value;
                }
            }
        }
        private String makeRandomSeed()
        {
            Random rand = new Random();

            int j = 128 / 8;
            StringBuilder stringbuffer = new StringBuilder(j * 2);
            for (int k = 0; k < j; k++)
            {
                int l = rand.Next(255);
                stringbuffer.Append(l.ToString("x"));
            }
            return stringbuffer.ToString();
        }
        private String makeResponse(String randomSeed, String username, String serverSeed, String password)
        {
            BigInteger biginteger = new BigInteger(seed1, 16);
            BigInteger biginteger1 = new BigInteger(seed2, 16);
            BigInteger biginteger2 = new BigInteger(seed3, 16);
            BigInteger biginteger3 = new BigInteger(randomSeed, 16);
            BigInteger biginteger4 = biginteger2.modPow(biginteger3, biginteger);
            BigInteger biginteger5 = biginteger1.modPow(biginteger3, biginteger);
            String s4 = username + "|" + serverSeed + "|" + password;
            String s5 = biginteger5.ToString(16).ToLower();
            String s6 = fill(32, s5, "0").Substring(0, 32).ToLower();
            String s7 = this.encrypt(s6, s4);
            String s8 = biginteger4.ToString(16).ToLower() + "-" + s7.ToLower();
            return s8;
        }
        private String fill(int i, String s, String s1)
        {
            if (s.Length < i)
            {
                int j = i - s.Length;
                for (int k = 0; k < j; k++)
                    s = s1 + s;

                return s;
            }
            else
            {
                return s;
            }
        }
        private String encrypt(String seed, String content)
        {
            byte byte0 = 4;
            if (seed.Length < 32)
            {
                Debug.WriteLine("Input key was too short.", "Error");
                return null;
            }

            UInt32[] ai = new UInt32[seed.Length / 2];
            this.strint(seed, ref ai);
            String s2 = intstr(0);
            String s3 = "";
            String s4 = "";

            for (int i = 0; i < 8; i++)
                s3 = s3 + ((char)(int)(new Random().Next() * 255D)).ToString();

            int j = s3.Length + s2.Length + content.Length;
            int k = 8 - j % 8;
            if (k != 8)
            {
                j += k;
                for (int l = 0; l < k; l++)
                    s4 = s4 + ' ';
            }

            s2 = intstr((UInt32)content.Length);
            String s5 = s3 + s2 + content + s4;
            UInt32[] ai1 = new UInt32[s5.Length / byte0];

            strToInt(s5, ref ai1);
            UInt32[] ai2 = { 0, 0 };

            UInt32[] ai3 = new UInt32[2];
            String s6 = "";
            ai3[0] = ai1[0];
            ai3[1] = ai1[1];

            for (int i1 = 0; i1 < ai1.Length; i1 += 2)
            {
                if (i1 != 0)
                {
                    ai3[0] = ai1[i1];
                    ai3[1] = ai1[i1 + 1];
                    ai3[0] ^= ai2[0];
                    ai3[1] ^= ai2[1];
                }
                this.transf(ref ai3, ref ai);
                ai2[0] = ai3[0];
                ai2[1] = ai3[1];
                s6 = s6 + intToHexStr(ai3);
            }
            return s6;

        } // End Encrypt
        private void transf(ref UInt32[] ai, ref UInt32[] ai1)
        {
            UInt32 i = ai[0];
            UInt32 j = ai[1];
            UInt32 k = 0;
            UInt32 l = 0x9e3779b9;
            for (UInt32 i1 = 32; i1-- > 0; )
            {
                k += l;
                i += (j << 4 & 0xfffffff0) + ai1[0] ^ j + k ^ (j >> 5 & 0x7ffffff) + ai1[1];
                j += (i << 4 & 0xfffffff0) + ai1[2] ^ i + k ^ (i >> 5 & 0x7ffffff) + ai1[3];
            }

            ai[0] = i;
            ai[1] = j;
        }

        private void strToInt(String s, ref UInt32[] ai)
        {
            int i = 0;
            int j = 0;
            for (int k = 0; k < s.Length; k++)
            {
                ai[j] |= ((UInt32)s[k] & 0xff) << i;
                if ((i += 8) == 32)
                {
                    i = 0;
                    if (++j == ai.Length)
                        return;
                }
            }

        }

        private void strint(String s, ref UInt32[] ai)
        {
            Int32 i = 0;
            Int32 j = 0;
            for (int k = 0; k < s.Length; k += 2)
            {
                String s1 = s.Substring(k, 2);
                ai[j] |= (UInt32)(Int32.Parse(s1, System.Globalization.NumberStyles.HexNumber) << i);
                if ((i += 8) == 32)
                {
                    i = 0;
                    if (++j == ai.Length)
                        return;
                }
            }
        }
        public static String intstr(UInt32 i)
        {
            char[] ac = new char[4];
            ac[0] = (char)(i >> 24 & 0xff);
            ac[1] = (char)(i >> 16 & 0xff);
            ac[2] = (char)(i >> 8 & 0xff);
            ac[3] = (char)(i >> 0 & 0xff);
            return new String(ac);
        }
        private static String intToHexStr(UInt32[] ai)
        {
            String s = "";
            for (int i = 0; i < ai.Length; i++)
            {
                for (int j = 0; j < 32; j += 8)
                {
                    UInt32 k = ai[i] >> j & 0xff;
                    if (k < 16)
                        s = s + "0" + k.ToString("x");
                    else
                        s = s + k.ToString("x");
                }
            }

            return s;
        }
    }
}