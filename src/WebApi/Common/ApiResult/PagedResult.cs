﻿namespace WebApi.Common.ApiResult;

/// <summary>
/// Kết quả phân trang chuẩn hóa.
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu trong danh sách.</typeparam>
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
