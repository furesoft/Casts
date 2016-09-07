using System;
using System.Linq;

namespace Casts.Providers
{
    class DynamicCallProvider : ICastProvider
    {
        public override object Cast(object val, Type T)
        {
            if (T?.Name == nameof(Delegate) || T.BaseType?.Name == nameof(MulticastDelegate))
            {
                var m = val.GetType().GetMethods();
                var s =
                    m.Where(
                        _ =>
                            _.ReturnType.Name == nameof(Delegate) &&
                            _.GetParameters().FirstOrDefault()?.ParameterType.Name == val.GetType().Name);

                if (s.Any())
                {
                    var call = (Delegate)s.FirstOrDefault().Invoke(null, new object[] { val });
                    return call;
                }
            }

            return null;
        }
    }
}
