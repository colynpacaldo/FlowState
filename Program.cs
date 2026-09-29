using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FlowState;
using FlowState.Services;
var b = WebAssemblyHostBuilder.CreateDefault(args);
b.RootComponents.Add<App>("#app");
b.RootComponents.Add<HeadOutlet>("head::after");
b.Services.AddSingleton<GameState>();
await b.Build().RunAsync();
