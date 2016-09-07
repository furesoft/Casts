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

        private Type[] GetParamterTypes(Type T)
        {
            if (T.Name.StartsWith("Action")) return T.GenericTypeArguments;
            if (T.Name.StartsWith("Func"))
            {
                var args = T.GenericTypeArguments.Reverse().ToList();
                args.RemoveAt(0);
                args.Reverse();
                return args.ToArray();
            }

            return null;
        }
        private Type GetReturnType(Type T)
        {
            if (T.Name.StartsWith("Action")) return typeof (void);
            if (T.Name.StartsWith("Func")) return T.GenericTypeArguments.Last();

            return typeof (void);
        }
    }
}