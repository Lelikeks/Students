using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Data.Entities
{
	[Index(nameof(UniqueCode), IsUnique = true)]
	public class DbStudent
	{
		public Guid Id { get; set; }

		[Required]
		[MaxLength(40)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(40)]
		public string LastName { get; set; }

		[MaxLength(60)]
		public string MiddleName { get; set; }

		[MaxLength(16)]
		public string UniqueCode { get; set; }

		[Required]
		public string Gender { get; set; }
	}
}
