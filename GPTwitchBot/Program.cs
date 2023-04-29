using GPTwitchBot;
using GPTwitchBot.GPT;
using GPTwitchBot.Twitch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

services.GetRequiredService<GPTwitchClient>().Run();

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<GPTClient>();
            services.AddSingleton<TwitchBot>();
            services.AddSingleton<Users>();
            services.AddSingleton<GPTwitchClient>();
        });
}                                