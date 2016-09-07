using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;

namespace Casts.Providers
{
    class SizeProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            if (obj is Size)
            {
                var v = (Size)obj;

                var ret = new List<byte>();
                ret.AddRange(BitConverter.GetBytes(v.Width));
                ret.AddRange(BitConverter.GetBytes(v.Height));

                return ret.ToArray();
            }
            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == typeof(Size).Name)
            {
                var ret = new Size();
                ret.Width = BitConverter.ToInt32(raw, 0);
                ret.Height = BitConverter.ToInt32(raw, 4);

                return ret;
            }
            return null;
        }
    }
}