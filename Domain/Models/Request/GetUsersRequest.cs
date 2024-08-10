namespace LayeredAPI.Domain.Models.Request;

public class GetUsersRequest : BasePaginationRequest
{
    public string SearchQuery { get; set; } = string.Empty;
}