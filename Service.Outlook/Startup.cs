using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Global.Handler;
using Global.Models.Profiles;
using Newtonsoft.Json;
using Service.Outlook.Logic;
using Service.Outlook.Models.Mapping;
using Service.Outlook.Models.Profiles;
using Service.Outlook.Repository;

namespace Service.Outlook
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

            services.AddScoped<IAppointmentLogic, AppointmentLogic>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddSingleton<IOutlookConnectionProfile>(GetConnectionProfile());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service.Outlook", Version = "v1" });
            });

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service.Outlook v1"));
            }

          //  app.UseExceptionHandler(ex => ex.Run(ExceptionHandler.Handle));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private IOutlookConnectionProfile GetConnectionProfile()
        {
            var fileName = "D:\\JOS\\Outlook\\user.pwd";

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(new OutlookConnectionProfile()
                    {
                        UserName = string.Empty,
                        Password = string.Empty,
                        Url = string.Empty,
                        Domain = string.Empty,
                        Email = string.Empty,
                        MailServerUrl = string.Empty
                    },
                    Formatting.Indented));

                throw new Exception();
            }

            var connectionProfile = JsonConvert.DeserializeObject<OutlookConnectionProfile>(File.ReadAllText(fileName));

            if (string.IsNullOrEmpty(connectionProfile.UserName))
            {
                throw new Exception($"File not found: {Directory.GetCurrentDirectory()}");
            }

            //ToDo: Fehlerhandling

            return connectionProfile;
        }
    }
}
