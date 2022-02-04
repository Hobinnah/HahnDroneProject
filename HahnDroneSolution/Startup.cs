using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using HahnDroneAPI.Configurations;
using HahnDroneAPI.Db;
using HahnDroneAPI.HangFire.HangFireJobScheduler;
using HahnDroneAPI.Helpers;
using HahnDroneAPI.Middlewares;
using HahnDroneAPI.Services.Implementations;
using HahnDroneAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using HahnDroneAPI.Db.Repositories.Interfaces;
using HahnDroneAPI.Db.Repositories.Implementations;

namespace HahnDroneAPI
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
            services.AddAutoMapper(typeof(Startup));

            // ========== Connection String ============
            services.AddDbContext<HahnDroneDBContext>(options => options.UseInMemoryDatabase(databaseName: "HahnDroneDB"));
            services.AddHangfire(options => options.UseMemoryStorage());
            services.AddHangfireServer();

            #region ===== Add Cookies =====
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            #endregion

            #region ===== DI =====

            //=========================REPOs================================
            services.AddScoped<IAuditEventLogRepository, AuditEventLogRepository>();
            services.AddScoped<IDroneRepository, DroneRepository>();
            services.AddScoped<IDroneMedicationMasterRepository, DroneMedicationMasterRepository>();
            services.AddScoped<IDroneMedicationDetailRepository, DroneMedicationDetailRepository>();
            services.AddScoped<IMedicationRepository, MedicationRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();


            //==========================Services=============================
            services.AddScoped<IDroneService, DroneService>();
            services.AddSingleton<ICustomConfiguration, CustomConfiguration>();
            services.AddScoped<IMedicationService, MedicationService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IAuditEventLogService, AuditEventLogService>();
            services.AddScoped<IDroneMedicationService, DroneMedicationService>();

            #endregion

            #region ===== CORS configuration =====
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.SetIsOriginAllowed((host) => true).AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", this.Configuration["Domain:ClientUrl"]).AllowCredentials().Build();
                });

            });
            #endregion

            #region ========== API Versioning============
            services.AddApiVersioning(options => {

                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });
            #endregion

            #region ===== Swagger generator =====

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseCors("EnableCORS");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder => {

                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault occurred. Please try again later.");
                    });

                });
            }

            app.UseGlobalExceptionMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region ========== HangFire ==========
            app.UseHangfireDashboard("/hangfire", new DashboardOptions {   });
            
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            HangFireJobScheduler.ScheduleRecurringJobs();

            #endregion
        }
    }
}
