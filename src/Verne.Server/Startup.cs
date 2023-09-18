using I2C;
using I2C.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace server
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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "server", Version = "v1" });
            });
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy( o => {
                    o.AllowAnyHeader();
                    o.AllowAnyOrigin();
                    o.AllowAnyMethod();
                });
            });
            
            services.AddSingleton<IChannel, Channel>(); 
            services.AddSingleton<ILedStrip, LedStrip>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseStaticFiles();


     
                app.UseDeveloperExceptionPage();
                object value = app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "server v1"));
          

            app.UseRouting();
                    app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
