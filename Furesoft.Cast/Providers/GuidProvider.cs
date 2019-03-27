using System;

namespace Casts.Providers
{
    class GuidProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            if (obj is Guid)
            {
                var v = (Guid)obj;

                return v.ToByteArray();
            }

            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == typeof(Guid).Name)
            {
                return new Guid(raw);
            }
            return null;
        }
    }
}