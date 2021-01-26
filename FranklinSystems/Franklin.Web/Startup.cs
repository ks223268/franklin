using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Franklin.Core;
using Franklin.Data;

namespace Franklin.Web {
    public class Startup {

        public Startup(IConfiguration configuration) {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "Franklin Systems APIs v1",
                    Description = "Franklin Systems APIs for our trading system.",
                    TermsOfService = new Uri("https://policies.google.com/terms"),
                    Contact = new OpenApiContact {
                        Name = "SK",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/openapispec"),
                    },
                    License = new OpenApiLicense {
                        Name = "Open License",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            services.AddDbContext<FranklinDbContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:FranklinDbContext"]));

            // DI
            services.AddTransient(typeof(ISecurityService), typeof(SecurityService));
            services.AddTransient(typeof(IRepository), typeof(Repository));
            services.AddTransient(typeof(IOrderManagementService), typeof(OrderManagementService));




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {


            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Franklin Systems APIs v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

        }
    }
}
