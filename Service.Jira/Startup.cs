using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Global.Handler;
using Newtonsoft.Json;
using Service.Jira.Logic;
using Service.Jira.Models;
using Service.Jira.Models.Mapping;
using Service.Jira.Models.Profiles;
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
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service.Jira", Version = "v1" });

            });

            services.AddScoped<IBoardLogic, BoardLogic>();
            services.AddScoped<IBoardRepository, BoardRepository>();

            services.AddScoped<ISprintLogic, SprintLogic>();
            services.AddScoped<ISprintRepository, SprintRepository>();

            services.AddScoped<IIssueLogic, IssueLogic>();
            services.AddScoped<IIssueRepository, IssueRepository>();

            services.AddScoped<IReportLogic, ReportLogic>();

            services.AddScoped<IDashboardRepository, DashboardRepository>();

            services.AddScoped<IGadgetProfile, GadgetProfile>();

            services.AddSingleton<IConnectionProfile>(GetConnectionProfile());
            services.AddSingleton<IDashboardProfile>(GetDashboardProfile());

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mapperConfig.CreateMapper());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service.Jira v1"));

            app.UseExceptionHandler(ex => ex.Run(ExceptionHandler.Handle));

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
            var fileName = "D:\\JOS\\Jira\\user.pwd";

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(new ConnectionProfile()
                {
                    UserName = string.Empty,
                    Password = string.Empty,
                    Url = string.Empty,
                    Domain = string.Empty,
                    Email = string.Empty
                },
                Formatting.Indented));

                throw new Exception();
            }

            var connectionProfile = JsonConvert.DeserializeObject<ConnectionProfile>(File.ReadAllText(fileName));

            //ToDo: Fehlerhandling

            return connectionProfile;
        }

        private IDashboardProfile GetDashboardProfile()
        {
            var fileName = "D:\\JOS\\Jira\\dashboard.profile";

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(new DashboardProfile()
                {
                    JiraServer = "https://<ServerName>/",
                    BoardId = 0,
                    DashBoardId = 0,
                    GadgetProfiles = new List<GadgetProfile>()
                        {
                            new GadgetProfile()
                            {
                                GadgetId = 0,
                                GadgetName = string.Empty,
                                SprintNameFilter = string.Empty
                            }
                        }
                },
                    Formatting.Indented));

                throw new Exception();
            }
            var dashboardProfile = JsonConvert.DeserializeObject<DashboardProfile>(File.ReadAllText(fileName));

            //ToDo: Fehlerhandling

            return dashboardProfile;
        }
    }
}
