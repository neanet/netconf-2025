using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BudgetApp;
using BudgetApp.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Register application services
builder.Services.AddScoped<BudgetApp.Services.ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<AutoSaveService>();

await builder.Build().RunAsync();
