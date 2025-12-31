using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace WorldCities.Server.Data;

public class ApiResult<T>
{
    /// <summary>
    /// The data result.
    /// </summary>
    public List<T> Data { get; private set; }

    /// <summary>
    /// Filter column name (or null if none set).
    /// </summary>
    public string? FilterColumn { get; private set; }

    /// <summary>
    /// Filter query to be used within the given FilterColumn.
    /// </summary>
    public string? FilterQuery { get; private set; }

    /// <summary>
    /// Zero-based index of current page.
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// Number of data items contained in each page.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Sorting column name (or null if none set).
    /// </summary>
    public string? SortColumn { get; private set; }

    /// <summary>
    /// Sorting order ("ASC", "DESC" or null if none set).
    /// </summary>
    public string? SortOrder { get; private set; }

    /// <summary>
    /// Total number of data items.
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// TRUE if the current page has a previous page, otherwise FALSE.
    /// </summary>
    public bool HasPreviousPage => (PageIndex > 0);

    /// <summary>
    /// TRUE if the current page has a next page, otherwise FALSE.
    /// </summary>
    public bool HasNextPage => ((PageIndex + 1) < TotalPages);

    /// <summary>
    /// Private constructor called by the CreateASync method.
    /// </summary>
    /// <param name="data">The data result</param>
    /// <param name="count">Total number of data items</param>
    /// <param name="pageIndex">Zero-based index of current page</param>
    /// <param name="pageSize">Number of data items contained in each page</param>
    /// <param name="sortColumn">The sorting column name</param>
    /// <param name="sortOrder">The sorting order ("ASC" or "DESC")</param>
    private ApiResult(
        List<T> data, int count, int pageIndex, int pageSize, string? sortColumn, string? sortOrder, 
        string? filterColumn, string? filterQuery)
    {
        Data = data;
        FilterColumn = filterColumn;
        FilterQuery = filterQuery;
        PageIndex = pageIndex;
        PageSize = pageSize;
        SortColumn = sortColumn;
        SortOrder = sortOrder;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count  / (double)PageSize);
    }

    /// <summary>
    /// Pages, sorts and/or filters an IQueryable source.
    /// </summary>
    /// <param name="source">An IQueryable source of generic type</param>
    /// <param name="pageIndex">Zero-based current page index (0 = first page)</param>
    /// <param name="pageSize">The actual size of each page</param>
    /// <param name="sortColumn">The sorting column name</param>
    /// <param name="sortOrder">The sorting order ("ASC" or "DESC")</param>
    /// <param name="filterColumn">The filtering column name</param>
    /// <param name="filterQuery">The filtering query (value to lookup)</param>
    /// <returns>
    /// An object containing the paged/sorted result and all the relevant 
    /// paging/sorting/filtering navigation info.
    /// </returns>
    public static async Task<ApiResult<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize, 
        string? sortColumn = null, string? sortOrder = null, 
        string? filterColumn = null, string? filterQuery = null)
    {
        if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(filterQuery) && IsValidProperty(filterColumn)) 
        {
            source = source.Where($"{filterColumn}.StartsWith(@0)", filterQuery);
        }

        int count = await source.CountAsync();

        if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn)) 
        {
            sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
            source = source.OrderBy($"{sortColumn} {sortOrder}");
        }
        
        source = source.Skip(pageIndex * pageSize).Take(pageSize);
        var data = await source.ToListAsync();

        return new ApiResult<T>(data, count, pageIndex, pageSize, sortColumn, sortOrder, filterColumn, filterQuery);
    }

    /// <summary>
    /// Checks if the given property name exists to protect against SQL injection attacks.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="throwExceptionIfNotFound"></param>
    /// <returns></returns>
    public static bool IsValidProperty(string propertyName, bool throwExceptionIfNotFound = true) 
    {
        var prop = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (prop is null && throwExceptionIfNotFound)
            throw new NotSupportedException($"ERROR: Property '{propertyName}' does not exist.");

        return prop != null;
    }
}
