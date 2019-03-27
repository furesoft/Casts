using System;
using System.Linq;
using System.Net;

namespace Casts.Providers
{
    class IPAddressProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            var c = obj as IPAddress;
            return c?.GetAddressBytes();
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == nameof(IPAddress))
            {
                return new IPAddress(raw.Take(4).ToArray());
            }
            return null;
        }
    }
}