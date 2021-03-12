using System;

namespace Students.Domain.Models
{
	public class StudentInfo
	{
		public Guid Id { get; set; }

		public string FIO { get; set; }

		public string UniqueCode { get; set; }

		public string Groups { get; set; }
	}
}
