using Microsoft.EntityFrameworkCore;

namespace WorldCities.Server.Data;

public class ApiResult<T>
{
    /// <summary>
    /// The data result.
    /// </summary>
    public List<T> Data { get; private set; }

    /// <summary>
    /// Zero-based index of current page.
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// Number of data items contained in each page.
    /// </summary>
    public int PageSize { get; private set; }

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
    private ApiResult(List<T> data, int count, int pageIndex, int pageSize)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(count  / (double)PageSize);
    }

    /// <summary>
    /// Pages an IQueryable source.
    /// </summary>
    /// <param name="source">An IQueryable source of generic type</param>
    /// <param name="pageIndex">Zero-based current page index (0 = first page)</param>
    /// <param name="pageSize">The actual size of each page</param>
    /// <returns>An object containing the paged result and all the relevant paging navigation info.</returns>
    public static async Task<ApiResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        int count = await source.CountAsync();
        source = source.Skip(pageIndex * pageSize).Take(pageSize);
        var data = await source.ToListAsync();

        return new ApiResult<T>(data, count, pageIndex, pageSize);
    }
}
