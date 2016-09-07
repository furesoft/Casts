using System;
using System.Collections.Generic;
using System.Drawing;

namespace Casts.Providers
{
    class RectangleProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            if (obj is Rectangle)
            {
                var v = (Rectangle)obj;

                var ret = new List<byte>();
                ret.AddRange(BitConverter.GetBytes(v.X));
                ret.AddRange(BitConverter.GetBytes(v.Y));
                ret.AddRange(BitConverter.GetBytes(v.Width));
                ret.AddRange(BitConverter.GetBytes(v.Height));

                return ret.ToArray();
            }
            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == typeof(Rectangle).Name)
            {
                var ret = new Rectangle();
                ret.X = BitConverter.ToInt32(raw, 0);
                ret.Y = BitConverter.ToInt32(raw, 4);
                ret.Width = BitConverter.ToInt32(raw, 8);
                ret.Height = BitConverter.ToInt32(raw, 12);

                return ret;
            }
            return null;
        }
    }
}
