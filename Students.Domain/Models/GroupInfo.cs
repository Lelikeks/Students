using System;

namespace Students.Domain.Models
{
	public class GroupInfo
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int StudentsAmount { get; set; }
	}
}
