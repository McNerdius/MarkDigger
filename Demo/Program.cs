using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#if RELEASE
builder.Services.AddSingleton<IMarkdownContentProvider, EmbeddedHtmlProvider>();
#else
builder.Services.AddScoped<IMarkdownContentProvider, LocalMarkdownProvider>();
#endif

await builder.Build().RunAsync();
