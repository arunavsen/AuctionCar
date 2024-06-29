using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;
using SearchService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

    //                                                                                                     await DB.InitAsync("SearchDB", MongoClientSettings
    //.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));

try
{
    await DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
