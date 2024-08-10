namespace LayeredAPI.Domain.Models.Request;

public class BasePaginationRequest
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
}