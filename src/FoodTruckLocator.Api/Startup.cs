using System.IO;
using System.IO.Abstractions;
using FoodTruckLocator.Configuration;
using FoodTruckLocator.HostedServices;
using FoodTruckLocator.Providers;
using FoodTruckLocator.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FoodTruckLocator.Api
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
            services.Configure<AppOptions>(Configuration.GetSection(AppOptions.Section));
            services.Configure<CachingOptions>(Configuration.GetSection(CachingOptions.Section));

            services.AddMemoryCache();
            services.AddHostedService<Downloader>();
            services.AddSingleton(_ => new FileSystem().File);
            services.AddHttpClient<IBlobProvider, BlobProvider>();
            services.AddTransient<IStorageProvider, StorageProvider>();
            services.AddTransient<ISearchService, SearchService>();

            services.AddControllers();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Food Truck Locator API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Food Truck Locator V1");
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
