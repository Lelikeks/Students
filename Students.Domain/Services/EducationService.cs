using Students.Domain.Models;
using Students.Domain.Models.SearchRequests;
using Students.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Students.Domain.Services
{
	public class EducationService : IEducationService
	{
		private readonly IStudentsRepository _studentsRepository;
		private readonly IGroupsRepository _groupsRepository;

		public EducationService(IStudentsRepository studentsRepository, IGroupsRepository groupsRepository)
		{
			this._studentsRepository = studentsRepository;
			this._groupsRepository = groupsRepository;
		}

		public async Task<bool> CodeExists(string uniqueCode, Guid? excludeId)
		{
			if (uniqueCode == null)
			{
				return false;
			}

			return await _studentsRepository.CodeExists(uniqueCode, excludeId);
		}

		public async Task<Student> GetStudent(Guid id)
		{
			return await _studentsRepository.GetOne(id);
		}

		public async Task<StudentInfo[]> GetStudents(StudentsSearchRequest request)
		{
			return await _studentsRepository.Search(request);
		}

		public async Task<Student> CreateStudent(Student student)
		{
			return await _studentsRepository.Create(student);
		}

		public async Task<bool> UpdateStudent(Guid id, Student student)
		{
			return await _studentsRepository.Update(id, student);
		}

		public async Task<bool> DeleteStudent(Guid id)
		{
			return await _studentsRepository.Delete(id);
		}

		public async Task<Group> GetGroup(Guid id)
		{
			return await _groupsRepository.GetOne(id);
		}

		public async Task<GroupInfo[]> GetGroups(GroupsSearchRequest request)
		{
			return await _groupsRepository.Search(request);
		}

		public async Task<Group> CreateGroup(Group group)
		{
			return await _groupsRepository.Create(group);
		}

		public async Task<bool> UpdateGroup(Guid id, Group group)
		{
			return await _groupsRepository.Update(id, group);
		}

		public async Task<bool> DeleteGroup(Guid id)
		{
			return await _groupsRepository.Delete(id);
		}

		public async Task<bool> StudentAndGroupExists(Guid groupId, Guid studentId)
		{
			var result = await Task.WhenAll(_groupsRepository.IdExists(groupId), _studentsRepository.IdExists(studentId));
			return result.All(x => x);
		}

		public async Task AddStudentToGroup(Guid groupId, Guid studentId)
		{
			await _groupsRepository.AddStudent(groupId, studentId);
		}

		public async Task<bool> DeleteStudentFromGroup(Guid groupId, Guid studentId)
		{
			return await _groupsRepository.DeleteStudent(groupId, studentId);
		}
	}
}
