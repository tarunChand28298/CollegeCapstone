using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerApplication.HardwareInterface;
using ServerApplication.PredictionUnit;
using ServerApplication.Storage;

namespace ServerApplication
{
    public class Startup
    {
        //add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHardwareInterface, HardwareInterfaceUnit>();
            services.AddSingleton<IStorageUnit, StorageUnit>();
            services.AddSingleton<IPredictionUnit, PredictionUnit.PredictionUnit>();
            services.AddControllers();
        }

        //configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
