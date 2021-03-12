using Microsoft.EntityFrameworkCore;
using Students.Data.Entities;
using Students.Domain.Models;
using Students.Domain.Models.SearchRequests;
using Students.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Students.Data.Repositories
{
	public class GroupsRepository : IGroupsRepository
	{
		private readonly DataContext _context;

		public GroupsRepository(DataContext context)
		{
			this._context = context;
		}

		public async Task<bool> IdExists(Guid id)
		{
			return await _context.Groups.AnyAsync(x => x.Id == id);
		}

		public async Task<Group> GetOne(Guid id)
		{
			var group = await _context.Groups.FindAsync(id);
			if (group == null)
			{
				return null;
			}

			return new Group
			{
				Id = group.Id,
				Name = group.Name
			};
		}

		public async Task<GroupInfo[]> Search(GroupsSearchRequest request)
		{
			var query = _context.Groups.AsQueryable();

			if (!string.IsNullOrEmpty(request.Name))
			{
				query = query.Where(x => x.Name.Contains(request.Name));
			}

			query = query.OrderBy(x => x.Id).Skip(request.PageSize * (request.PageNumber - 1)).Take(request.PageSize);

			return await query.Select(x => new GroupInfo
			{
				Id = x.Id,
				Name = x.Name,
				StudentsAmount = _context.StudentGroups.Count(s => s.GroupId == x.Id)
			}).ToArrayAsync();
		}

		public async Task<Group> Create(Group group)
		{
			group.Id = Guid.NewGuid();

			_context.Add(new DbGroup
			{
				Id = group.Id,
				Name = group.Name
			});
			await _context.SaveChangesAsync();

			return group;
		}

		public async Task<bool> Update(Guid id, Group group)
		{
			var entity = await _context.Groups.FindAsync(id);
			if (entity == null)
			{
				return false;
			}

			entity.Name = group.Name;

			_context.Update(entity);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> Delete(Guid id)
		{
			var entity = await _context.Groups.FindAsync(id);
			if (entity == null)
			{
				return false;
			}

			_context.Remove(entity);
			return await _context.SaveChangesAsync() == 1;
		}

		public async Task AddStudent(Guid groupId, Guid studentId)
		{
			if (await _context.StudentGroups.AnyAsync(x => x.GroupId == groupId && x.StudentId == studentId))
			{
				return;
			}

			_context.Add(new DbStudentGroup { GroupId = groupId, StudentId = studentId });
			await _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteStudent(Guid groupId, Guid studentId)
		{
			var entity = await _context.StudentGroups.SingleOrDefaultAsync(x => x.GroupId == groupId && x.StudentId == studentId);
			if (entity == null)
			{
				return false;
			}

			_context.Remove(entity);
			await _context.SaveChangesAsync();

			return true;
		}
	}
}
