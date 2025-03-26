using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TicketToCode.Client.Services;

namespace TicketToCode.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddScoped<UserState>();
            builder.RootComponents.Add<App>("#app");

            // Peka mot ditt API
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7206/")
            });

            await builder.Build().RunAsync();
        }
    }
}
