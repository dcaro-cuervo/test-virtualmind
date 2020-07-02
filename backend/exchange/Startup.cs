using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using exchange.Extensions;
using exchange.Models;
using exchange.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;

namespace exchange
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.Configure<PurchaseCurrencyDatabaseSettings>(
                Configuration.GetSection(nameof(PurchaseCurrencyDatabaseSettings)));

            services.AddSingleton<IPurchaseCurrencyDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PurchaseCurrencyDatabaseSettings>>().Value);

            services.AddSingleton<PurchaseService>();

            services.AddMvc().AddControllersAsServices();

            services.ConfigureLoggerService();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            // Handles exceptions and generates a custom response body
            app.UseExceptionHandler("/errors/500");

            // Handles non-success status codes with empty body
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseMvc();
        }
    }
}
