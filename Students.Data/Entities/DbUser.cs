using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Data.Entities
{
	public class DbUser
	{
		public Guid Id { get; set; }

		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
