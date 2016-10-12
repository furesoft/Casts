using System;
using System.Diagnostics;

namespace Cast.Test
{
    struct Mini
    {
        readonly int val;
        public static Mini Max = new Mini(0xFF);

        public Mini(int v)
        {
            val = (int) (v & 0xFF);
        }

        public override string ToString()
        {
            return val.ToString();
        }

        public static explicit operator Mini(byte[] raw)
        {
            return new Mini(BitConverter.ToInt32(raw, 0));
        }

        public static explicit operator byte[](Mini m)
        {
            return BitConverter.GetBytes(m.val);
        }

        public static explicit operator Delegate(Mini raw)
        {
            return new Action(() => Debug.WriteLine(raw.val));
        }
    }
}