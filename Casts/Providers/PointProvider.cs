using System;
using System.Collections.Generic;
using System.Drawing;

namespace Casts.Providers
{
    class PointProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            if (obj is Point)
            {
                var v = (Point)obj;

                var ret = new List<byte>();
                ret.AddRange(BitConverter.GetBytes(v.X));
                ret.AddRange(BitConverter.GetBytes(v.Y));

                return ret.ToArray();
            }
            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == typeof(Point).Name)
            {
                var ret = new Point();
                ret.X = BitConverter.ToInt32(raw, 0);
                ret.Y = BitConverter.ToInt32(raw, 4);

                return ret;
            }
            return null;
        }
    }
}
