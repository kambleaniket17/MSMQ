namespace WebApplication11
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic;
    using BusinessLogic.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;

    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
           
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddTransient<IGetCustomer, EmployeeDatabase>();
            services.AddCors(o => o.AddPolicy("MyPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
            services.Configure<MyData>(Configuration.GetSection("MyData"));
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
         
            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
