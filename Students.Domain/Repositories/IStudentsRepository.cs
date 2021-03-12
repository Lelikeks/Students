using Students.Domain.Models;
using Students.Domain.Models.SearchRequests;
using System;
using System.Threading.Tasks;

namespace Students.Domain.Repositories
{
	public interface IStudentsRepository
	{
		Task<bool> IdExists(Guid id);

		Task<bool> CodeExists(string uniqueCode, Guid? excludeId);

		Task<Student> GetOne(Guid id);

		Task<StudentInfo[]> Search(StudentsSearchRequest request);

		Task<Student> Create(Student student);

		Task<bool> Update(Guid id, Student student);

		Task<bool> Delete(Guid id);
	}
}
