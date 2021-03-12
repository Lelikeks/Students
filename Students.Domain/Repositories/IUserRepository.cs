using Students.Domain.Models;
using System.Threading.Tasks;

namespace Students.Domain.Repositories
{
	public interface IUserRepository
	{
		Task<User> GetUser(string login, string password);
	}
}
