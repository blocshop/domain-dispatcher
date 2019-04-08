using System;
using System.Collections.Generic;

namespace DomainDispatcher.Helper
{
    internal static class Comparer
    {
        public static IEqualityComparer<TIn> On<TOut, TIn>(Func<TIn, TOut> func)
        {
            return new DelegateComparer<TOut, TIn>(func);
        }

        private class DelegateComparer<TOut, TIn> : IEqualityComparer<TIn>
        {
            private readonly Func<TIn, TOut> _selector;

            public DelegateComparer(Func<TIn, TOut> selector)
            {
                _selector = selector;
            }

            public bool Equals(TIn x, TIn y)
            {
                var xConverted = _selector(x);
                var yConverted = _selector(y);
                if (xConverted == null || yConverted == null)
                {
                    return xConverted == null && yConverted == null;
                }

                return xConverted.Equals(yConverted);
            }

            public int GetHashCode(TIn obj)
            {
                return _selector(obj).GetHashCode();
            }
        }
    }
}