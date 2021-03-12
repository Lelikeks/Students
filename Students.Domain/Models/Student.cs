using Students.Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Domain.Models
{
	public class Student
	{
		public Guid Id { get; set; }

		[Required, EnumDataType(typeof(Gender))]
		public Gender Gender { get; set; }

		[Required, StringLength(40)]
		public string FirstName { get; set; }

		[Required, StringLength(40)]
		public string LastName { get; set; }

		[StringLength(60)]
		public string MiddleName { get; set; }

		[StringLength(16, MinimumLength = 6)]
		public string UniqueCode { get; set; }
	}
}
