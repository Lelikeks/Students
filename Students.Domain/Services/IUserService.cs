using Students.Domain.Models;
using System.Threading.Tasks;

namespace Students.Domain.Services
{
	public interface IUserService
	{
		Task<User> GetUser(string login, string password);
	}
}
