using Microsoft.VisualStudio.TestTools.UnitTesting;
using Students.Domain.Services;
using Students.Domain.Models;
using Students.Domain.Models.Enums;
using System.Threading.Tasks;
using Students.Web.Controllers;
using Students.Data;
using Microsoft.EntityFrameworkCore;
using Students.Data.Repositories;

namespace Students.Test
{
	[TestClass]
	public class UnitTest1
	{
		DataContext _context;

		[TestInitialize]
		public void Initialize()
		{
			var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("database").Options;
			_context = new DataContext(options);
		}

		[TestCleanup]
		public void Cleanup()
		{
			_context.Dispose();
		}

		[TestMethod]
		public async Task StudentCreated()
		{
			var studentsRepository = new StudentsRepository(_context);
			var groupsRepository = new GroupsRepository(_context);
			var educationService = new EducationService(studentsRepository, groupsRepository);
			var controller = new StudentsController(educationService);

			var expected = new Student { FirstName = "test first name", LastName = "test last name", UniqueCode = "test code", Gender = Gender.Female };
			var result = await controller.CreateStudent(expected);
		}
	}
}
