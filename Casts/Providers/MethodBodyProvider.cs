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
        public override byte[] ToBinary(object obj)
        {
            if (obj is Delegate)
            {
                var del = (Delegate)obj;
                var methodBody = del.Method?.GetMethodBody();
                if (methodBody != null) return methodBody.GetILAsByteArray();
            }
            return null;
        }
    }
}