using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FactWeb.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Include<TEntity, TFrom>(this IQueryable<TEntity> source,
              int levelIndex, Expression<Func<TEntity, TFrom>> expression)
        {
            if (levelIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(levelIndex));
            var member = (MemberExpression)expression.Body;
            var property = member.Member.Name;
            var sb = new StringBuilder();
            for (int i = 0; i < levelIndex; i++)
            {
                if (i > 0)
                    sb.Append(Type.Delimiter);
                sb.Append(property);
            }
            return source.Include(sb.ToString());
        }
    }
}
