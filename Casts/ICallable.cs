using System;

namespace Casts
{
    public interface ICallable
    {
        void Call(params object[] args);
    }

    public class DynamicCallable : ICallable
    {
        internal Delegate  _d;
        public DynamicCallable(Delegate d)
        {
            _d = d;
        }

        public void Call(params object[] args)
        {
            _d.DynamicInvoke(args);
        }
    }
}