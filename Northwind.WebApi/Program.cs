using Microsoft.AspNetCore.Mvc.Formatters; // To use IOutputFormatter.
using Microsoft.AspNetCore.HttpLogging; // To use HttpLoggingFields
using Northwind.EntityModels; // To use AddNorthwindContext method.
using Northwind.WebApi.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMemoryCache>(
    new MemoryCache(new MemoryCacheOptions()));
builder.Services.AddNorthwindContext();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddControllers(options =>
    {
        WriteLine("Default output formatters:");
        foreach (IOutputFormatter formatter in options.OutputFormatters)
        {
            OutputFormatter? mediaFormatter = formatter as OutputFormatter;
            if (mediaFormatter is null)
            {
                WriteLine($"  {formatter.GetType().Name}");
            }
            else // OutputFormatter class has SupportedMediaTypes.
            {
                WriteLine("  {0}, Media types: {1}",
                    arg0: mediaFormatter.GetType().Name,
                    arg1: string.Join(", ",
                    mediaFormatter.SupportedMediaTypes));
            }

        }
    })
    .AddXmlDataContractSerializerFormatters()
    .AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.All;
        options.RequestBodyLogLimit = 4096;
        options.ResponseBodyLogLimit = 4096;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpLogging();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Northwind Service API Version 1");
            c.SupportedSubmitMethods(new[] { 
            SubmitMethod.Get, SubmitMethod.Post,
            SubmitMethod.Put, SubmitMethod.Delete });
        });   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
