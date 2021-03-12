using Students.Domain.Models;
using Students.Domain.Models.SearchRequests;
using System;
using System.Threading.Tasks;

namespace Students.Domain.Services
{
	public interface IEducationService
	{
		Task<Student> GetStudent(Guid id);

		Task<StudentInfo[]> GetStudents(StudentsSearchRequest request);

		Task<bool> CodeExists(string uniqueCode, Guid? excludeId = null);

		Task<Student> CreateStudent(Student student);

		Task<bool> UpdateStudent(Guid id, Student student);

		Task<bool> DeleteStudent(Guid id);

		Task<Group> GetGroup(Guid id);

		Task<GroupInfo[]> GetGroups(GroupsSearchRequest request);

		Task<Group> CreateGroup(Group Group);

		Task<bool> UpdateGroup(Guid id, Group Group);

		Task<bool> DeleteGroup(Guid id);

		Task<bool> StudentAndGroupExists(Guid groupId, Guid studentId);

		Task AddStudentToGroup(Guid groupId, Guid studentId);

		Task<bool> DeleteStudentFromGroup(Guid groupId, Guid studentId);
	}
}
