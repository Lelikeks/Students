using Students.Domain.Models;
using Students.Domain.Repositories;
using System.Threading.Tasks;

namespace Students.Domain.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			this._userRepository = userRepository;
		}

		public async Task<User> GetUser(string login, string password)
		{
			return await _userRepository.GetUser(login, password);
		}
	}
}
