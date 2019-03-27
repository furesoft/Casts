using System;
using System.Numerics;

namespace Casts.Providers
{
    class BigIntegerProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            var bi = obj as BigInteger?;
            return bi?.ToByteArray();
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == nameof(BigInteger))
            {
              return new BigInteger(raw);  
            }
            return null;
        }
    }
}
