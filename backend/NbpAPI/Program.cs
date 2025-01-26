using NbpAPI.Data;
using NbpAPI.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NbpAPI.Services.MapperProfile;
using NbpAPI.Data.RepositoryRegistration;
using NbpAPI.Services.ServiceRegistration;
using NbpAPI.Services.Middleware;

namespace NbpAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("master"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin() // Zezwól na ¿¹dania z dowolnego Ÿród³a
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register HttpClient for external API calls

            builder.Services.AddHttpClient<CurrencyService>(client =>
            {
                client.BaseAddress = new Uri("http://api.nbp.pl");
                client.Timeout = TimeSpan.FromSeconds(60); // Ustaw timeout na 60 sekund
            });

            builder.Services.AddRepository();
            builder.Services.AddServices();



            
            builder.Services.AddAutoMapper(typeof(MapperProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Urls.Add("http://0.0.0.0:8080");

            app.Run();
        }
    }
}
