using System.Reflection;
using FluentValidation.AspNetCore;
using MongoDB.Driver;
using SchoolNetAssociation.Api.Middlewares;
using SchoolNetAssociation.Application.Mappings;
using SchoolNetAssociation.Application.Services;
using SchoolNetAssociation.Domain.Repositories;
using SchoolNetAssociation.Infrastructure.MongoData;
using SchoolNetAssociation.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Mongo Db config
var mongoDbSettingsSection = builder.Configuration.GetSection("MongoDbSettings");
builder.Services.Configure<MongoDbSettings>(mongoDbSettingsSection);

var mongoDbSettings = mongoDbSettingsSection.Get<MongoDbSettings>();
builder.Services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoDbSettings?.ConnectionString));

builder.Services.AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbSettings.DatabaseName));
builder.Services.AddScoped<ISchoolDistrictService, SchoolDistrictService>();
builder.Services.AddTransient<ISchoolDistrictRepository, SchoolDistrictRepository>();
builder.Services.AddAutoMapper(typeof(SchoolNetAssociationProfile));
builder.Services.AddControllers().AddFluentValidation(C => C.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }