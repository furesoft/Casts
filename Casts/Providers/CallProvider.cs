using System;

namespace Casts.Providers
{
    class CallProvider : ICastProvider
    {
        public override object Cast(object val, Type T)
        {
            var dlg = val as Delegate;
            var callb = val as ICallable;

            if (dlg != null && T.Name == nameof(ICallable))
            {
                return (ICallable)new DynamicCallable(dlg);
            }
            if (callb != null && callb is DynamicCallable)
            {
                return ((DynamicCallable)callb)._d;
            }
            return null;
        }
    }
}
