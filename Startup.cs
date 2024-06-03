using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySnippetService.Services;

namespace MySnippetService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ISnippetService>(provider =>
                //Stationær
                new SnippetService(@"Data Source=C:\Users\Jonathan\OneDrive\Skrivebord\DataBaseItArkitekturPrincipper\SearchDBSmall.db"));
                //Bærebar
                //new SnippetService(@"Data Source=C:\Users\jonat\Desktop\ItArkitekturPrincipper\TestData\searchDBSmall.db"));

            // //Bærebar pc --> services.AddSingleton<ISnippetService>(provider => new SnippetService(@"/Users/jonat/Desktop/ItArkitekturPrincipper/TestData/searchDBSmall.db"));
            //http://localhost:5000/api/snippets/thyme
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
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
