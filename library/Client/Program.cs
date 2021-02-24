using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using grpcCrud;
using library.Client.Services;
using library.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace library.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddOptions();
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<AuthProvider>();
			builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthProvider>());
			builder.Services.AddSingleton(service =>
			{
				string serverLocation = service.GetRequiredService<NavigationManager>().BaseUri;
				var client = new HttpClient
				(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
				var chanel = GrpcChannel.ForAddress(serverLocation, new GrpcChannelOptions { HttpClient = client });
				return new GrpcServ.GrpcServClient(chanel);
			});
			builder.Services.AddTransient<Converter>();
			await builder.Build().RunAsync();
		}
    }
}
