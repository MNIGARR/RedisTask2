using Microsoft.AspNetCore.SignalR;
using RedisTask2.Hubs;
using RedisTask2.Services;
using RedisTask2.Hubs;
using RedisTask2.Services;
using StackExchange.Redis;

namespace RedisTask2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis-15044.c282.east-us-mz.azure.redns.redis-cloud.com:15044,password=hLAvfxxMwndLkLP3uKHYb6si0TMhvQgu"));
            builder.Services.AddScoped<IMessageListener, MessageListener>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }
}