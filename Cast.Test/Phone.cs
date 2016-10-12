using System;
using System.Collections.Generic;

namespace Cast.Test
{
    public struct Phone
    {
        public int Vorwahl { get; set; }
        public int Number { get; set; }
        public short Durchwahl { get; set; }

        public static explicit operator byte[](Phone p)
        {
            var raw = new List<byte>();

            raw.AddRange(BitConverter.GetBytes(p.Vorwahl));
            raw.AddRange(BitConverter.GetBytes(p.Number));
            raw.AddRange(BitConverter.GetBytes(p.Durchwahl));

            return raw.ToArray();
        }

        public static explicit operator Phone(byte[] r)
        {
            var p = new Phone();

            p.Vorwahl = BitConverter.ToInt32(r, 0);
            p.Number = BitConverter.ToInt32(r, 4);
            p.Durchwahl = BitConverter.ToInt16(r, 8);

            return p;
        }
    }
}