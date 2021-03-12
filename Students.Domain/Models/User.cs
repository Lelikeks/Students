using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Domain.Models
{
	public class User
	{
		public Guid Id { get; set; }

		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
