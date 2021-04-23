using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Newtonsoft.Json;
using Service.Jira.Logic;
using Service.Jira.Models;
using Service.Jira.Repository;

namespace Service.Jira
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service.Jira", Version = "v1" });
            });

            services.AddScoped<IBoardLogic, BoardLogic>();
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddSingleton<IConnectionProfile>(GetConnectionProfile());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service.Jira v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IConnectionProfile GetConnectionProfile()
        {
            var fileName = "user.pwd";

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(new ConnectionProfile()
                {
                    UserName = "",
                    Password = "",
                    Url = "",
                    Domain = "",
                    Email = ""
                },
                Formatting.Indented));

                throw new Exception();
            }

            var connectionProfile = JsonConvert.DeserializeObject<ConnectionProfile>(File.ReadAllText(fileName));

            //ToDo: Fehlerhandling

            return connectionProfile;
        }
    }
}
