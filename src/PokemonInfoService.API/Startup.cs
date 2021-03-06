using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PokemonInfoService.Services.PokemonServices;
using PokemonInfoService.Services.Mappers;
using System;
using PokemonInfoService.Services.TranslationServices;

namespace PokemonInfoService.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PokemonInfoService.API", Version = "v1" });
            });

            services.AddHttpClient("PokemonApi", c =>
            {
                c.Timeout = TimeSpan.FromSeconds(double.Parse(Configuration.GetSection("PokemonApi")["Timeout"]));
                c.BaseAddress = new Uri(Configuration.GetSection("PokemonApi")["BaseUrl"]);
            });
            services.AddHttpClient("TranslationApi", c =>
            {
                c.Timeout = TimeSpan.FromSeconds(double.Parse(Configuration.GetSection("TranslationApi")["Timeout"]));
                c.BaseAddress = new Uri(Configuration.GetSection("TranslationApi")["BaseUrl"]);
            });

            services.AddScoped<IPokemonInformationService, PokemonInformationService>();
            services.AddScoped<IPokemonApiClient, PokemonApiClient>();
            services.AddScoped<IPokemonApiModelMapper, PokemonApiModelMapper>();
            services.AddScoped<ITranslationApiClient, TranslationApiClient>();
            services.AddScoped<ITranslationService, TranslationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokemonInfoService.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
