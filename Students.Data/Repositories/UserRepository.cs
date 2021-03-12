using Microsoft.EntityFrameworkCore;
using Students.Domain.Models;
using Students.Domain.Repositories;
using System.Threading.Tasks;

namespace Students.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			this._context = context;
		}

		public async Task<User> GetUser(string login, string password)
		{
			var user = await _context.Users.SingleOrDefaultAsync(x => x.Login == login && x.Password == password);
			if (user == null)
			{
				return null;
			}

			return new User
			{
				Id = user.Id,
				Login = user.Login,
				Password = user.Password
			};
		}
	}
}
