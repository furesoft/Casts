using System;
using System.Linq;
using System.Net;

namespace Casts.Providers
{
    class IPAddressProvider : ICastProvider
    {
        public override object Cast(object val, Type T)
        {
            if (T.Name == nameof(IPAddress) && val is string)
            {
                return IPAddress.Parse(val.ToString());
            }
            return null;
        }

        public override byte[] ToBinary(object obj)
        {
            var c = (IPAddress) obj;
            return c.GetAddressBytes();
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