using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using TvMaze.BLL;
using TvMaze.BLL.AutoMapper;
using TvMaze.DAL;

namespace TvMaze.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextFactory<TvMazeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TvMazeContext")));
            services.AddHttpClient<ITvMazeService, TvMazeService>(client =>
            {
                
                client.BaseAddress = new Uri(Configuration["TvMazeBaseUrl"]);
            })  
                .AddPolicyHandler(GetRetryPolicy());
            services.AddControllers();
            services.AddMvcCore();

            services.AddSwaggerGen();
            services.AddAutoMapper(GetAssembliesForAutoMapper());

            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IShowService, ShowService>();

        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
        private IEnumerable<Assembly> GetAssembliesForAutoMapper()
        {
            yield return typeof(EntityToDtoProfile).GetTypeInfo().Assembly;
            yield return typeof(DtoToEntityProfile).GetTypeInfo().Assembly;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShowApi");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
