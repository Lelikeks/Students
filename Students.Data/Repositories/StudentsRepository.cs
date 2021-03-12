using Microsoft.EntityFrameworkCore;
using Students.Data.Entities;
using Students.Domain.Models;
using Students.Domain.Models.Enums;
using Students.Domain.Models.SearchRequests;
using Students.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Students.Data.Repositories
{
	public class StudentsRepository : IStudentsRepository
	{
		private readonly DataContext _context;

		public StudentsRepository(DataContext context)
		{
			this._context = context;
		}

		public async Task<bool> IdExists(Guid id)
		{
			return await _context.Students.AnyAsync(x => x.Id == id);
		}

		public async Task<bool> CodeExists(string uniqueCode, Guid? excludeId)
		{
			var query = _context.Students.Where(x => x.UniqueCode == uniqueCode);
			if (excludeId != null)
			{
				query = query.Where(x => x.Id != excludeId);
			}

			return await query.AnyAsync();
		}

		public async Task<Student> GetOne(Guid id)
		{
			var student = await _context.Students.FindAsync(id);
			if (student == null)
			{
				return null;
			}

			return new Student
			{
				Id = student.Id,
				FirstName = student.FirstName,
				MiddleName = student.MiddleName,
				LastName = student.LastName,
				UniqueCode = student.UniqueCode,
				Gender = Enum.Parse<Gender>(student.Gender)
			};
		}

		public async Task<StudentInfo[]> Search(StudentsSearchRequest request)
		{
			IQueryable<DbStudent> query = GetFilteredQuery(request);

			var withGroups = from q in query
							 join sg in _context.StudentGroups on q.Id equals sg.StudentId into sgs
							 from sg in sgs.DefaultIfEmpty()
							 join g in _context.Groups on sg.GroupId equals g.Id into gs
							 from g in gs.DefaultIfEmpty()
							 select new { Student = q, Group = g.Name };

			var listGroups = await withGroups.ToListAsync();
			var grouped = from s in listGroups
						  group s by s.Student into g
						  select new { Student = g.Key, Groups = g.Select(x => x.Group) };

			return grouped.Select(x => new StudentInfo
			{
				Id = x.Student.Id,
				FIO = $"{x.Student.LastName} {x.Student.FirstName} {x.Student.MiddleName}",
				UniqueCode = x.Student.UniqueCode,
				Groups = string.Join(", ", x.Groups)
			}).ToArray();
		}

		public async Task<Student> Create(Student student)
		{
			student.Id = Guid.NewGuid();

			_context.Add(new DbStudent
			{
				Id = student.Id,
				FirstName = student.FirstName,
				LastName = student.LastName,
				MiddleName = student.MiddleName,
				UniqueCode = student.UniqueCode,
				Gender = student.Gender.ToString()
			});
			await _context.SaveChangesAsync();

			return student;
		}

		public async Task<bool> Update(Guid id, Student student)
		{
			var entity = await _context.Students.FindAsync(id);
			if (entity == null)
			{
				return false;
			}

			entity.FirstName = student.FirstName;
			entity.LastName = student.LastName;
			entity.MiddleName = student.MiddleName;
			entity.UniqueCode = student.UniqueCode;
			entity.Gender = student.Gender.ToString();

			_context.Update(entity);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> Delete(Guid id)
		{
			var entity = await _context.Students.FindAsync(id);
			if (entity == null)
			{
				return false;
			}

			_context.Remove(entity);
			await _context.SaveChangesAsync();

			return true;
		}

		private IQueryable<DbStudent> GetFilteredQuery(StudentsSearchRequest request)
		{
			var query = _context.Students.AsQueryable();

			if (!string.IsNullOrEmpty(request.FIO))
			{
				query = query.Where(x => x.FirstName.Contains(request.FIO) || x.LastName.Contains(request.FIO) || x.MiddleName.Contains(request.FIO));
			}

			if (!string.IsNullOrEmpty(request.UniqueCode))
			{
				query = query.Where(x => x.UniqueCode == request.UniqueCode);
			}

			if (!string.IsNullOrEmpty(request.Gender))
			{
				query = query.Where(x => x.Gender == request.Gender);
			}

			if (!string.IsNullOrEmpty(request.GroupName))
			{
				var studentsInGroup = from sg in _context.StudentGroups
									  join g in _context.Groups on sg.GroupId equals g.Id
									  where g.Name == request.GroupName
									  select sg.StudentId;

				query = query.Where(x => studentsInGroup.Contains(x.Id));
			}

			return query.OrderBy(x => x.Id).Skip(request.PageSize * (request.PageNumber - 1)).Take(request.PageSize);
		}
	}
}
