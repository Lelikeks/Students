using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Students.Domain.Models;
using Students.Domain.Models.SearchRequests;
using Students.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Students.Web.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class GroupsController : ControllerBase
	{
		private readonly IEducationService _educationService;

		public GroupsController(IEducationService educationService)
		{
			this._educationService = educationService;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Group>> GetGroup(Guid id)
		{
			var group = await _educationService.GetGroup(id);

			if (group == null)
			{
				return NotFound();
			}

			return group;
		}

		[HttpGet]
		public async Task<ActionResult<GroupInfo[]>> GetGroups([FromQuery] GroupsSearchRequest request)
		{
			return await _educationService.GetGroups(request);
		}

		[HttpPost]
		public async Task<ActionResult<Group>> CreateGroup(Group group)
		{
			var created = await _educationService.CreateGroup(group);
			return CreatedAtAction(nameof(GetGroup), new { created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateGroup(Guid id, Group group)
		{
			if (await _educationService.UpdateGroup(id, group))
			{
				return NoContent();
			}

			return NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGroup(Guid id)
		{
			if (await _educationService.DeleteGroup(id))
			{
				return NoContent();
			}

			return NotFound();
		}

		[HttpPost]
		[Route("{groupId}/students/{studentId}")]
		public async Task<IActionResult> AddStudentToGroup(Guid groupId, Guid studentId)
		{
			if (!await _educationService.StudentAndGroupExists(groupId, studentId))
			{
				return NotFound();
			}

			await _educationService.AddStudentToGroup(groupId, studentId);
			return NoContent();
		}

		[HttpDelete]
		[Route("{groupId}/students/{studentId}")]
		public async Task<IActionResult> DeleteStudentFromGroup(Guid groupId, Guid studentId)
		{
			if (await _educationService.DeleteStudentFromGroup(groupId, studentId))
			{
				return NoContent();
			}

			return NotFound();
		}
	}
}
