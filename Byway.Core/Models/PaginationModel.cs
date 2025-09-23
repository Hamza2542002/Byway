using Byway.Core.Entities;

namespace Byway.Core.Models;

public class PaginationModel<T> : ServiceResultModel<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}
