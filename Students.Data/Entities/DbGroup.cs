using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Data.Entities
{
	public class DbGroup
	{
		public Guid Id { get; set; }

		[Required, MaxLength(25)]
		public string Name { get; set; }
	}
}
