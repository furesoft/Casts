using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Casts.Providers
{
    class MethodBodyProvider : ICastProvider
    {
        public override object Cast(object val, Type T)
        {
            if (T.Name == typeof (byte[]).Name && val is Delegate)
            {
                var del = (Delegate) val;
                var methodBody = del.Method?.GetMethodBody();
                if (methodBody != null) return methodBody.GetILAsByteArray();
            }

            return null;
        }
    }
}