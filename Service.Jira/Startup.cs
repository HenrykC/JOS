using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Global.Handler;
using Service.Jira.Logic;
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

            var connectionProfile = Configuration
                .GetSection(nameof(ConnectionProfile))
                .Get<ConnectionProfile>();

            var dashboardProfile = Configuration
                .GetSection(nameof(DashboardProfile))
                .Get<DashboardProfile>();

            services.AddSingleton<IConnectionProfile>(connectionProfile);
            services.AddSingleton<IDashboardProfile>(dashboardProfile);

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
    }
}
