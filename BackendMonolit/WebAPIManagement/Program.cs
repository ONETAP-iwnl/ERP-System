
using Microsoft.EntityFrameworkCore;
using WebAPIManagement.Interface.Repository;
using WebAPIManagement.Interface.Service;
using WebAPIManagement.Models;
using WebAPIManagement.Repository;
using WebAPIManagement.Services;

namespace WebAPIManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ErpSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();

            // репозитории
            builder.Services.AddScoped<IUnitRepository, UnitRepository>();
            builder.Services.AddScoped<IResourcesRepository, ResourcesRepository>();
            builder.Services.AddScoped<IReceiptDocumentRepository, ReceiptDocumentRepository>();
            builder.Services.AddScoped<IReceiptResourceRepository, ReceiptResourceRepository>();

            // сервисы
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddScoped<IResourceService, ResourceService>();
            builder.Services.AddScoped<IReceipDocumentService, ReceipDocumentService>();
            builder.Services.AddScoped<IReceiptResourceService, ReceiptResourceService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
