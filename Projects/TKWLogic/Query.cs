using Microsoft.EntityFrameworkCore;
using TKWData;

// public class QueryProcessor : IQueryProcessor
// {
//     // private ILifetimeScope container;

//     // public QueryProcessor(ILifetimeScope containerFactory)
//     // {
//     //     this.container = containerFactory;
//     // }
//     public TResult ProcessQuery<TResult, TEntity>(IBaseQuery<TResult, TEntity> query, TKWContext context, TKWDataContext dbContext)
//     {
//         // var handlerInterface = typeof(IQueryHandler<,,>);
//         // var handlerType = handlerInterface.MakeGenericType(query.GetType(), typeof(TResult), typeof(TEntity));

//         // using (var scope = container.BeginLifetimeScope(builder =>
//         // {
//         //     builder.RegisterInstance(context);
//         // }))
//         // {
//         //     dynamic handler = scope.Resolve(handlerType, new TypedParameter(typeof(DbContext), dbContext));

//         //     var value = handler.Handle((dynamic)query);

//         //     return value;
//         // }
//         return null;
//     }
// }

// public interface IQueryProcessor
// {
//     TResult ProcessQuery<TResult, TEntity>(IBaseQuery<TResult, TEntity> query, TKWContext Context, TKWDataContext dbContext);
// }




public abstract class IBaseQuery<TResult, TEntity>
{
    // public Func<IQueryable<U>, IQueryable<U>> Include { get; set; }
}

public abstract class IQueryEnumerable<TEntity> : IBaseQuery<IEnumerable<TEntity>, TEntity>, IQueryInclude<TEntity>
{
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> Include { get; set; } = null!;
}
public abstract class IQueryEnumerable<T, TEntity> : IBaseQuery<IEnumerable<T>, TEntity> { }
public abstract class IQueryQueryable<TEntity> : IBaseQuery<IQueryable<TEntity>, TEntity>, IQueryInclude<TEntity>
{
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> Include { get; set; } = null!;
}
public abstract class IQueryQueryable<T, TEntity> : IBaseQuery<IQueryable<T>, TEntity> { }
public abstract class IQuery<TEntity> : IBaseQuery<TEntity, TEntity>, IQueryInclude<TEntity>
{
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> Include { get; set; } = null!;
}
public abstract class IQuery<T, TEntity> : IBaseQuery<T, TEntity> { }

public interface IQueryInclude<TEntity>
{
    Func<IQueryable<TEntity>, IQueryable<TEntity>> Include { get; set; }
}