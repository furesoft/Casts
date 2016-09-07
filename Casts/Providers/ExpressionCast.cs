using System;
using System.Linq.Expressions;

namespace Casts.Providers
{
    class ExpressionCast : ICastProvider
    {
        public override object Cast(object val, Type T)
        {
            if (T.Name == nameof(Expression))
            {
                return Expression.Constant(val);
            }

            return null;
        }
    }
}