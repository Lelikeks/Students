using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Domain.Models
{
	public class Group
	{
		public Guid Id { get; set; }

		[Required, StringLength(25)]
		public string Name { get; set; }
	}
}
