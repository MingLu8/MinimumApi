using RepoDb.Interfaces;
using RepoDb;
using System.Data;
using System.Linq.Expressions;

namespace MinimumApi.Extensions
{
    public static class DbExtensions
    {
        public static TEntity QuerySingle<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> where, IEnumerable<Field> fields = null, IEnumerable<OrderField> orderBy = null, int? top = 0, string hints = null, string cacheKey = null, int? cacheItemExpiration = 180, int? commandTimeout = null, string traceKey = "Query", IDbTransaction transaction = null, ICache cache = null, ITrace trace = null, IStatementBuilder statementBuilder = null) where TEntity : class
            => connection.Query<TEntity>(
                ClassMappedNameCache.Get<TEntity>(),
                where,
                fields,
                orderBy,
                top,
                hints,
                cacheKey,
                cacheItemExpiration,
                commandTimeout,
                traceKey,
                transaction,
                cache,
                trace,
                statementBuilder).FirstOrDefault();

        public static async Task<TEntity> QuerySingleAsync<TEntity>(this IDbConnection connection, Expression<Func<TEntity, bool>> where, IEnumerable<Field> fields = null, IEnumerable<OrderField> orderBy = null, int? top = 0, string hints = null, string cacheKey = null, int? cacheItemExpiration = 180, int? commandTimeout = null, string traceKey = "Query", IDbTransaction transaction = null, ICache cache = null, ITrace trace = null, IStatementBuilder statementBuilder = null, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class
        {
            var result = await connection.QueryAsync<TEntity>(ClassMappedNameCache.Get<TEntity>(), where, fields, orderBy, top, hints, cacheKey, cacheItemExpiration, commandTimeout, traceKey, transaction, cache, trace, statementBuilder, cancellationToken);
            return result.FirstOrDefault();
        }
    }
}
