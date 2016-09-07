using System;
using System.Collections.Generic;
using System.Drawing;

namespace Casts.Providers
{
    class ColorProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            if (obj is Color)
            {
                var v = (Color)obj;

                var ret = new List<byte>();
                ret.Add(v.A);
                ret.Add(v.R);
                ret.Add(v.G);
                ret.Add(v.B);

                return ret.ToArray();
            }
            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == typeof(Color).Name)
            {
                if (raw.Length == 4)
                {
                    return Color.FromArgb(BitConverter.ToInt32(raw, 0));
                }
            }
            return null;
        }
    }
}
