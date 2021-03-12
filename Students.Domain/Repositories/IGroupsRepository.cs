using Students.Domain.Models;
using Students.Domain.Models.SearchRequests;
using System;
using System.Threading.Tasks;

namespace Students.Domain.Repositories
{
	public interface IGroupsRepository
	{
		Task<bool> IdExists(Guid id);

		Task<Group> GetOne(Guid id);

		Task<GroupInfo[]> Search(GroupsSearchRequest request);

		Task<Group> Create(Group group);

		Task<bool> Update(Guid id, Group group);

		Task<bool> Delete(Guid id);

		Task AddStudent(Guid groupId, Guid studentId);

		Task<bool> DeleteStudent(Guid groupId, Guid studentId);
	}
}
