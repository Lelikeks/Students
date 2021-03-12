using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Students.Data;
using Students.Data.Repositories;
using Students.Domain.Repositories;
using Students.Domain.Services;
using Students.Web.Configuration;
using Students.Web.Extensions;
using System.Text;
using System.Text.Json.Serialization;

namespace Students.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));

			services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

			var connectionString = Configuration.GetConnectionString("MainDatabase");
			services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

			services.AddTransient<IStudentsRepository, StudentsRepository>();
			services.AddTransient<IGroupsRepository, GroupsRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IEducationService, EducationService>();
			services.AddTransient<IUserService, UserService>();

			var jwtOptions = Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = jwtOptions.Audience,
					ValidIssuer = jwtOptions.Issuer,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
				};
			});

			services.AddSwaggerGen();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.EnsureMigrations();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
