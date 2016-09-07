using System;
using System.IO;

namespace Casts.Providers
{
    class StreamProvider : ICastProvider
    {
        public override object Cast(object val, Type type)
        {
            if (type.Name == nameof(MemoryStream))
            {
                var ms = new MemoryStream();
                var strm = (Stream)val;

                while (strm.Position < strm.Length)
                {
                    byte[] buffer = new byte[16];
                    strm.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, buffer.Length);
                }

                return ms;
            }
            return null;
        }
    }
}