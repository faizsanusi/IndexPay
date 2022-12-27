using indexPay.Repositories;
using indexPay.Services;
using indexPay.Services.IServices;
using indexPay.Utilities;
using indexPay.Utilities.IUtilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.Json.Serialization;

namespace indexPay
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        private readonly IWebHostEnvironment _env;

        public IConfiguration Configuration { get; }
        public static class ConfigurationHelper
        {
            public static IConfiguration config;
            public static void Initialize(IConfiguration Configuration)
            {
                config = Configuration;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //register services

            services.AddScoped<IMemCache, MemCache>();
            services.AddScoped<ICoreBankingServices, CoreBankingService>();
            services.AddScoped<IFlutterWaveAPI, FlutterWaveAPI>();
            services.AddScoped<IFlutterwaveClient, FlutterwaveClient>();
            services.AddScoped<IPaystackAPI, PaystackAPI>();
            services.AddScoped<IPaystackClient, PaystackClient>();
            services.AddScoped<IProvidersStrategy, Flutterwave>();
            services.AddScoped<IProvidersStrategy, Paystack>();
            services.AddScoped<ITransferRepository, TransferRepository>();

            services.AddMemoryCache();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "indexPay", Version = "v1" });
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DBConnection"),
                b =>
                {
                    // b.CommandTimeout(300);
                    b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                //opt.LogTo(Console.WriteLine, LogLevel.Trace);
            });
            services.AddOptions();

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "indexPay v1"));
            }


            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
