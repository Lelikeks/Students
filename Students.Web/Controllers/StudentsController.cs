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
	public class StudentsController : ControllerBase
	{
		private readonly IEducationService _educationService;

		public StudentsController(IEducationService educationService)
		{
			this._educationService = educationService;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Student>> GetStudent(Guid id)
		{
			var student = await _educationService.GetStudent(id);

			if (student == null)
			{
				return NotFound();
			}

			return student;
		}

		[HttpGet]
		public async Task<ActionResult<StudentInfo[]>> GetStudents([FromQuery] StudentsSearchRequest request)
		{
			return await _educationService.GetStudents(request);
		}

		[HttpPost]
		public async Task<ActionResult<Student>> CreateStudent(Student student)
		{
			if (await _educationService.CodeExists(student.UniqueCode))
			{
				ModelState.AddModelError("UniqueCode", $"Unique code '{student.UniqueCode}' already in use");
				return Conflict(ModelState);
			}

			var created = await _educationService.CreateStudent(student);
			return CreatedAtAction(nameof(GetStudent), new { created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateStudent(Guid id, Student student)
		{
			if (await _educationService.CodeExists(student.UniqueCode, id))
			{
				ModelState.AddModelError("UniqueCode", $"Unique code '{student.UniqueCode}' already in use");
				return Conflict(ModelState);
			}

			if (await _educationService.UpdateStudent(id, student))
			{
				return NoContent();
			}

			return NotFound();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStudent(Guid id)
		{
			if (await _educationService.DeleteStudent(id))
			{
				return NoContent();
			}

			return NotFound();
		}
	}
}
