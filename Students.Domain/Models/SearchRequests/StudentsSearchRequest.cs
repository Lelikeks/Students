namespace Students.Domain.Models.SearchRequests
{
	public class StudentsSearchRequest : PagingRequest
	{
		public string FIO { get; set; }

		public string Gender { get; set; }

		public string GroupName { get; set; }

		public string UniqueCode { get; set; }
	}
}
