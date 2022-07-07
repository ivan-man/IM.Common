using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using IM.Common.Extensions;
using IM.Common.Extensions.Helpers;
using IM.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace IM.Common.EF.Repositories.Implementations;

public class BaseRepositoryAdvanced<T, TId> : BaseRepositoryAdvanced<T>, IBaseRepositoryAdvanced<T, TId>
    where T : BaseEntity<TId>, new()
{
    protected BaseRepositoryAdvanced(DbContext context) : base(context)
    {
    }

    public virtual Task<T?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
        => GetItemAsync(q => q.Id!.Equals(id), cancellationToken, include);
}

public class BaseRepositoryAdvanced<T> : BaseRepository<T>, IBaseRepositoryAdvanced<T> where T : BaseEntity, new()
{
    protected BaseRepositoryAdvanced(DbContext context) : base(context)
    {
    }

    protected override async Task<Common.Models.PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        string? sortBy = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        if (string.IsNullOrWhiteSpace(sortBy) && orderByProperty == null)
            orderByProperty ??= x => x.Created;

        var itemsSet = await base.Get(predicate, orderByProperty, sortBy, orderByDescending, take, skip, include)
            .ToListAsync(cancellationToken);
        
        var total = await base.CountAsync(predicate, cancellationToken);
        
        return Common.Models.PagedResult<T>.Ok(
            itemsSet, 
            count: take ?? 0,
            page: PageCalculateHelper.CalculatePageNumber(take, skip), 
            total);
    }

    protected override IQueryable<T> Get(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        string? sortBy = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        params Expression<Func<T, object>>[]? include)
    {
        if (string.IsNullOrWhiteSpace(sortBy) && orderByProperty == null)
            orderByProperty ??= x => x.Created;

        return base.Get(predicate, orderByProperty, sortBy, orderByDescending, take, skip, include);
    }
}

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    protected DbContext Context { get; }

    protected BaseRepository(DbContext context)
    {
        Context = context;
    }

    public IQueryable<T> AsQueryable()
    {
        return Context.Set<T>();
    }

    /// <inheritdoc />
    public async Task<Common.Models.PagedResult<TSelector>> GetPagedAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        string? sortBy = null,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        var itemsSet = await Get(predicate, orderByProperty: null, sortBy, orderByDescending: null, take, skip, include)
            .Select(selector)
            .ToListAsync(cancellationToken);
        
        var total = await CountAsync(predicate, cancellationToken);
        
        return Common.Models.PagedResult<TSelector>.Ok(
            itemsSet, 
            count: take ?? 0,
            page: PageCalculateHelper.CalculatePageNumber(take, skip), 
            total);
    }

    /// <inheritdoc />
    public async Task<Common.Models.PagedResult<TSelector>> GetPagedAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        Expression<Func<T, object>>? orderByProperty = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        var itemsSet = await Get(predicate, orderByProperty, sortBy: null, orderByDescending, take, skip, include)
            .Select(selector)
            .ToListAsync(cancellationToken);
        
        var total = await CountAsync(predicate, cancellationToken);
        
        return Common.Models.PagedResult<TSelector>.Ok(
            itemsSet, 
            count: take ?? 0,
            page: PageCalculateHelper.CalculatePageNumber(take, skip), 
            total);
    }

    /// <inheritdoc />
    public async Task<Common.Models.PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>> predicate,
        string? sortBy = null,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        return await GetPagedAsync(
            predicate, 
            orderByProperty: null, 
            sortBy,
            orderByDescending: null, 
            take, 
            skip,
            cancellationToken, 
            include);
    }

    /// <inheritdoc />
    public async Task<Common.Models.PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        return await GetPagedAsync(
            predicate, 
            orderByProperty, 
            sortBy: null,
            orderByDescending, 
            take, 
            skip,
            cancellationToken, 
            include);
    }

    protected virtual async Task<Common.Models.PagedResult<T>> GetPagedAsync(Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        string? sortBy = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        var itemsSet = await Get(predicate, orderByProperty, sortBy, orderByDescending, take, skip, include)
            .ToListAsync(cancellationToken);
        
        var total = await CountAsync(predicate, cancellationToken);
        
        return Common.Models.PagedResult<T>.Ok(
            itemsSet, 
            count: take ?? 0,
            page: PageCalculateHelper.CalculatePageNumber(take, skip), 
            total);
    }
    
    /// <inheritdoc />
    public virtual IQueryable<T> GetQuery(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        bool orderByDescending = true,
        params Expression<Func<T, object>>[]? include)
    {
        return Get(
            predicate,
            orderByProperty,
            sortBy: null,
            orderByDescending,
            take: null,
            skip: null,
            include: include);
    }

    /// <inheritdoc />
    public virtual IQueryable<T> GetQuery(
        Expression<Func<T, bool>> predicate,
        string? sortBy = null,
        params Expression<Func<T, object>>[]? include)
    {
        return Get(
            predicate,
            orderByProperty: null,
            sortBy: sortBy,
            orderByDescending: null,
            take: null,
            skip: null,
            include: include);
    }

    /// <inheritdoc />
    public virtual IQueryable<TSelector?> GetQuery<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        string? sortBy = null,
        params Expression<Func<T, object>>[]? include)
    {
        return Get(predicate, orderByProperty: null, sortBy, orderByDescending: null, take: null, skip: null, include)
            .Select(selector);
    }

    /// <inheritdoc />
    public virtual IQueryable<TSelector?> GetQuery<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        Expression<Func<T, object>>? orderByProperty = null,
        bool orderByDescending = true,
        params Expression<Func<T, object>>[]? include)
    {
        return GetQuery(
                predicate, 
                orderByProperty, 
                orderByDescending, 
                include: include)
            .Select(selector);
    }

    protected virtual IQueryable<T> Get(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        string? sortBy = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        params Expression<Func<T, object>>[]? include)
    {
        IQueryable<T> query = Context.Set<T>();
        
        if (include != null && include.Any())
            query = include.Aggregate(query, (current, inc) => current.Include(inc));

        if (!string.IsNullOrWhiteSpace(sortBy) && orderByProperty != null)
            throw new ArgumentException("Must be only one ordering");

        query = query.Where(predicate);

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var orderString = string.IsNullOrWhiteSpace(sortBy) ? string.Empty : sortBy?.GetSorting<T>();
            query = string.IsNullOrWhiteSpace(orderString) ? query : query.OrderBy(orderString);
        }

        if (orderByProperty != null)
            query = orderByDescending.HasValue && orderByDescending.Value ? query.OrderByDescending(orderByProperty) : query.OrderBy(orderByProperty);

        query = skip.HasValue ? query.Skip(skip.Value) : query;
        query = take.HasValue ? query.Take(take.Value) : query;

        return query;
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetItemAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        bool orderByDescending = true,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        var query = GetQuery(
            predicate,
            orderByProperty,
            orderByDescending,
            include: include);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetItemAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        var query = Get(
            predicate: predicate,
            orderByProperty: null,
            sortBy: null,
            orderByDescending: true,
            include: include);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<TSelector?> GetItemAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        var query = GetQuery(
            predicate,
            selector,
            orderByProperty: null,
            orderByDescending: true,
            include: include);

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<TSelector?> GetItemAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        string? sortBy = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include)
    {
        return Get(
                predicate,
                orderByProperty: null,
                sortBy: sortBy,
                include: include)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual void Add(T entity) => Context.Set<T>().Add(entity);

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual Task AddRange(IEnumerable<T> entity, CancellationToken cancellationToken)
        => Context.Set<T>().AddRangeAsync(entity, cancellationToken);
    
    public virtual void Remove(T entity) => Context.Set<T>().Remove(entity);
    public virtual void RemoveRange(IEnumerable<T> entity, CancellationToken cancellationToken) => Context.Set<T>().RemoveRange(entity);
    public virtual Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => Context.Set<T>().CountAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => Context.Set<T>().AnyAsync(predicate, cancellationToken);
}
