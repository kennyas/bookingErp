using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Tornado.Shared.Test.Fakes;

namespace Tornado.Shared.Test.Extensions
{
    public static class DbSetExtensions
    {
        public static DbSet<T> MockFromSql<T>(this DbSet<T> dbSet, SpAsyncEnumerableQueryable<T> spItems) where T : class
        {
            var queryProviderMock = new Mock<IQueryProvider>();
            queryProviderMock.Setup(p => p.CreateQuery<T>(It.IsAny<MethodCallExpression>()))
                .Returns<MethodCallExpression>(x =>
                {
                    return spItems;
                });

            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>()
                .SetupGet(q => q.Provider)
                .Returns(() =>
                {
                    return queryProviderMock.Object;
                });

            dbSetMock.As<IQueryable<T>>()
                .Setup(q => q.Expression)
                .Returns(Expression.Constant(dbSetMock.Object));
            return dbSetMock.Object;
        }
    }
}
