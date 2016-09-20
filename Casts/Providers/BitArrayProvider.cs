using System;
using System.Collections;

namespace Casts.Providers
{
    class BitArrayProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            var c = obj as BitArray;
            if (c != null)
            {
                byte[] raw = new byte[c.Length];
                c?.CopyTo(raw, 0);

                return raw;
            }
            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == nameof(BitArray))
            {
                return new BitArray(raw);
            }

            return null;
        }
    }
}