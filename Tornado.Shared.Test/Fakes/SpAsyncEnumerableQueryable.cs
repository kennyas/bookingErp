using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Tornado.Shared.Test.Fakes
{
    public class SpAsyncEnumerableQueryable<T> : IAsyncEnumerable<T>, IQueryable<T>
    {
        private IAsyncEnumerable<T> _spItems;
        public Expression Expression => throw new NotImplementedException();
        public Type ElementType => throw new NotImplementedException();
        public IQueryProvider Provider => throw new NotImplementedException();

        public SpAsyncEnumerableQueryable(params T[] spItems)
        {
            _spItems = AsyncEnumerable.ToAsyncEnumerable(spItems);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _spItems.ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        //{
        //    return _spItems.GetEnumerator();
        //}

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return _spItems.GetAsyncEnumerator();
        }
    }
}
