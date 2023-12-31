using Microsoft.OpenApi.Models;
using RestaurantProject.API.Middleware;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Datalayer.Repositories;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Managers;

namespace RestaurantProject.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Add services to the container.
            RestaurantDbContext context = new();

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IRestaurantRepository>(r => new RestaurantRepository(context));
            builder.Services.AddSingleton<IRestaurantService, RestaurantManager>();

            builder.Services.AddSingleton<IReservationRepository>(r => new ReservationRepository(context));
            builder.Services.AddSingleton<IReservationService, ReservationManager>();

            builder.Services.AddSingleton<IUserRepository>(u => new UserRepository(context));
            builder.Services.AddSingleton<IUserService,UserManager>();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();
            ILogger logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseLoggingMiddleware();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}