namespace Students.Domain.Models.SearchRequests
{
	public class PagingRequest
	{
		public int PageNumber { get; set; }

		public int PageSize { get; set; } = int.MaxValue;
	}
}
