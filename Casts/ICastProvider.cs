using System;

namespace Casts
{
    public abstract class ICastProvider
    {
        public virtual object Cast(object val, Type T)
        {
            return null;
        }

        public virtual byte[] ToBinary(object obj)
        {
            return null;
        }

        public virtual object FromBinary(byte[] raw, Type to)
        {
            return null;
        }
    }
}