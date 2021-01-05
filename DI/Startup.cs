using AutoMapper;
using DI.DataLayer;
using DI.Handler;
using DI.Helper;
using DI.LogicNs;
using DI.Model;
using DI.Outlook.Logic;
using DI.Outlook.Models;
using DI.Outlook.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DI
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

            services.AddScoped<ILogic, Logic>();
            services.AddScoped<IOutlookLogic, OutlookLogic>();

            services.AddScoped<IEFRepository, EFRepository>();
            services.AddScoped<IOutlookRepository, OutlookRepository>();

            services.AddDbContext<JiraContext>(options => options.UseSqlite("Data Source=jira.db"));
            services.AddDbContext<OutlookDbContext>(options => options.UseSqlite("Data Source=outlook.db"));

            //services.AddDbContext<JiraContext>(opt => opt.UseInMemoryDatabase("JiraDb"));

            services.Configure<OutlookConnectionProfile>(
                Configuration.GetSection(nameof(OutlookConnectionProfile)));
            services.AddSingleton<IOutlookConnectionProfile>(sp =>
                    sp.GetRequiredService<IOptions<OutlookConnectionProfile>>().Value);

            //services.AddDbContext<JiraContext>(opt => opt.UseInMemoryDatabase("JiraDb"));
            //Access for document db Mongo
            //services.Configure<JiraDbSettings>(
            //    Configuration.GetSection(nameof(JiraDbSettings)));
            //services.AddSingleton<IJiraDbSettings>(sp =>
            //        sp.GetRequiredService<IOptions<JiraDbSettings>>().Value);


            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            //Swagger
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler<ExceptionHandlerExtensions>;
            //}

            app.UseExceptionHandler(ex => ex.Run(ExceptionHandler.Handle));

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My JIRA API V1");
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
