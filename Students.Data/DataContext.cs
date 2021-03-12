using Microsoft.EntityFrameworkCore;
using Students.Data.Entities;
using System;

namespace Students.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		public DbSet<DbStudent> Students { get; set; }

		public DbSet<DbGroup> Groups { get; set; }

		public DbSet<DbStudentGroup> StudentGroups { get; set; }

		public DbSet<DbUser> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DbStudentGroup>(b =>
			{
				b.HasKey(x => new { x.StudentId, x.GroupId });
				b.HasOne<DbStudent>().WithMany().HasForeignKey(x => x.StudentId);
				b.HasOne<DbGroup>().WithMany().HasForeignKey(x => x.GroupId);
			});

			// store clear password for simplicity, in production code there should be a password hash
			modelBuilder.Entity<DbUser>().HasData(new DbUser { Id = Guid.NewGuid(), Login = "admin", Password = "qwe123" });
		}
	}
}
