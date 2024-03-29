﻿using System.Linq.Expressions;
using IM.Common.Models;
using IM.Common.Models.Domain;
using IM.Common.Models.PagedRequest;

namespace IM.Common.EF.Repositories;

public interface IBaseRepositoryAdvanced<T, TId> : IBaseRepositoryAdvanced<T> where T : IBaseEntity<TId>
{
    Task<T?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);
}

public interface IBaseRepositoryAdvanced<T> : IBaseRepository<T> where T : IBaseEntity
{
}

public interface IBaseRepository<T>
{
    /// <summary>
    /// Full query
    /// </summary>
    IQueryable<T> AsQueryable();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="sortingDescriptors">Set of <see cref="SortDescriptor"/></param>
    /// <param name="pageSize">Number of items on page</param>
    /// <param name="pageIndex">Number of page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    Task<PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>> predicate,
        IEnumerable<SortDescriptor>? sortingDescriptors = null,
        int? pageSize = null,
        int? pageIndex = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get paged result.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="sortingDescriptors">Set of <see cref="SortDescriptor"/></param>
    /// <param name="pageSize">Number of items on page</param>
    /// <param name="pageIndex">Number of page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    Task<PagedResult<TSelector>> GetPagedAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        IEnumerable<SortDescriptor>? sortingDescriptors = null,
        int? pageSize = null,
        int? pageIndex = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);
    
    /// <summary>
    /// Get paged result.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="orderByProperty">Ordering expression</param>
    /// <param name="orderByDescending">Ordering desc flag</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    Task<PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get paged result query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="sortBy">Sorting string from filter. Example: "-Id, Created" = ORDER BY Id DESC, Created</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    Task<PagedResult<T>> GetPagedAsync(
        Expression<Func<T, bool>> predicate,
        string? sortBy = null,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get paged result query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="orderByProperty">Ordering expression</param>
    /// <param name="orderByDescending">Ordering direction</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    Task<PagedResult<TSelector>> GetPagedAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        Expression<Func<T, object>>? orderByProperty = null,
        bool? orderByDescending = true,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get paged result query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="sortBy">Sorting string from filter. Example: "-Id, Created" = ORDER BY Id DESC, Created</param>
    /// <param name="take">Number of items to take</param>
    /// <param name="skip">Number of items to skip</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    Task<PagedResult<TSelector>> GetPagedAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        string? sortBy = null,
        int? take = null,
        int? skip = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get customized query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="orderByProperty">Ordering expression</param>
    /// <param name="orderByDescending">Ordering desc flag</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Query</returns>
    IQueryable<T> GetQuery(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        bool orderByDescending = true,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get customized query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="sortBy">Ordering string from filter. Example: "-Id, Created" = ORDER BY Id DESC, Created</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Query</returns>
    IQueryable<T> GetQuery(
        Expression<Func<T, bool>> predicate,
        string? sortBy = null,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get customized query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="sortBy">Sorting string from filter. Example: "-Id, Created" = ORDER BY Id DESC, Created</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Query</returns>
    IQueryable<TSelector?> GetQuery<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        string? sortBy = null,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get customized query.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="orderByProperty">Ordering expression</param>
    /// <param name="orderByDescending">Ordering desc flag</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Query</returns>
    IQueryable<TSelector?> GetQuery<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        Expression<Func<T, object>>? orderByProperty = null,
        bool orderByDescending = true,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get item.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="orderByProperty">Ordering expression</param>
    /// <param name="orderByDescending">Ordering desc flag</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Entity</returns>
    Task<T?> GetItemAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>? orderByProperty = null,
        bool orderByDescending = true,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get item.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Entity</returns>
    Task<T?> GetItemAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get item.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Entity</returns>
    Task<TSelector?> GetItemAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Get item.
    /// </summary>
    /// <param name="predicate">Predicate for data set</param>
    /// <param name="selector">Properties to get expression</param>
    /// <param name="sortBy">Sorting string from filter. Example: "-Id, Created" = ORDER BY Id DESC, Created</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="include">List of properties to include</param>
    /// <returns>Entity</returns>
    Task<TSelector?> GetItemAsync<TSelector>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TSelector>> selector,
        string? sortBy = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? include);

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    void Add(T entity);

    /// <summary>
    /// Add entity and save it. 
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="cancellationToken"></param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add list of entities
    /// </summary>
    Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove entity
    /// </summary>
    void Remove(T entity);

    /// <summary>
    /// Remove entity
    /// </summary>
    void RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get number of items by predicate
    /// </summary>
    /// <param name="predicate">Predicate expression</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Number of items</returns>
    Task<int> CountAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Check of existing any entity by predicate
    /// </summary>
    /// <returns></returns>
    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}

