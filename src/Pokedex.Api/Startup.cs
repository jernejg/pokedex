using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokedex.Services.PokeApi;
using System;
using System.Reflection;

namespace Pokedex.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		private readonly IWebHostEnvironment _env;

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddProblemDetails(setup =>
			{
				setup.IncludeExceptionDetails = (ctx, exception) => _env.IsDevelopment();
			});
			services.AddMediatR(Assembly.GetExecutingAssembly());
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpClient<IPokeApiService, PokeApiRestService>(client =>
			{
				client.BaseAddress = new Uri(Configuration["ExternalServices:PokeApi:Uri"]);
			});
			services.AddProblemDetails(setup =>
			{
				setup.IncludeExceptionDetails = (ctx, exception) => _env.IsDevelopment();
				setup.Map<SpecieNotFoundException>((exception) =>
					new ProblemDetails()
					{
						Status = 404,
						Title = exception.Message
					}
				);
				setup.Map<PokemonServiceException>((exception) =>
					new ProblemDetails()
					{
						Status = 502,
						Title = exception.Message
					}
				);
			});
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseProblemDetails();
			app.UsePathBase(new PathString("/api"));
			app.UseRouting();
			app.UseStatusCodePages();
			app.UseEndpoints(endpoints =>
			{
				app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});
			});
		}
	}
}
