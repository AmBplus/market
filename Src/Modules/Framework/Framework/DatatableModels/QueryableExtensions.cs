using Framework.DatatableModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyDataTableOrdering<T>(this IQueryable<T> source, DatatableFullRequest request)
    {
        if (request.order == null || request.order.Length == 0 || request.columns == null)
            return source;

        var orderCol = request.order[0];
        if (orderCol.column < 0 || orderCol.column >= request.columns.Length)
            return source;

        var colName = request.columns[orderCol.column].data;
        if (string.IsNullOrWhiteSpace(colName))
            return source;

        var property = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var lambda = Expression.Lambda(propertyAccess, parameter);

        var methodName = orderCol.dir == "desc" ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), property.PropertyType },
            source.Expression,
            Expression.Quote(lambda)
        );

        return source.Provider.CreateQuery<T>(resultExpression);
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
