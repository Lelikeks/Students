using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Students.Data;
using System;

namespace Students.Web.Extensions
{
	static class EnsureMigrationsExtensions
	{
		public static void EnsureMigrations(this IApplicationBuilder app)
		{
			using (var scope = ((IServiceProvider)app.ApplicationServices.GetService(typeof(IServiceScopeFactory))).CreateScope())
			{
				var context = scope.ServiceProvider.GetService<DataContext>();
				context.Database.Migrate();
			}
		}
	}
}
