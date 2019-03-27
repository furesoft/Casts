using System;

namespace Casts.Providers
{
    class DateTimeProvider : ICastProvider
    {
        public override byte[] ToBinary(object obj)
        {
            var v = obj as DateTime?;

            if (v != null)
            {
                long utcNowAsLong = v.Value.ToBinary();
                byte[] utcNowBytes = BitConverter.GetBytes(utcNowAsLong);

                return utcNowBytes;
            }

            return null;
        }

        public override object FromBinary(byte[] raw, Type to)
        {
            if (to.Name == nameof(DateTime))
            {
                long utcNowLongBack = BitConverter.ToInt64(raw, 0);

                return DateTime.FromBinary(utcNowLongBack);
            }
            return null;
        }
    }
}