using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiProj.Helper;
using apiProj.Infrastructure;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace apiProj
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services
                .AddMvc(options =>
                {

                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddControllersAsServices()
                .AddNewtonsoftJson(); // https://stackoverflow.com/questions/59198417/deserialization-of-reference-types-without-parameterless-constructor-is-not-supp/61124257#61124257
                                      // Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 3.1.6


            //.. Setup Redis Memory ..//
            services.Configure<BasketSettings>(Configuration);

            //By connecting here we are making sure that our service
            //cannot start until redis is ready. This might slow down startup,
            //but given that there is a delay on resolving the ip address
            //and then creating the connection it seems reasonable to move
            //that cost to startup instead of having the first request pay the
            //penalty.
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;
                var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });


            //.. Dependency Injection ..//
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IBasketRedisRepository, BasketRedisRepository>();
            services.AddOptions();


            //.. Swagger ..//
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            });




        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                setupAction.RoutePrefix = "";
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseFileServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
