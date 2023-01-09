using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using PermissionsWebApi.Configuration;
using PermissionsWebApi.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PermissionsWebApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PermissionsWebApiContext") ?? throw new InvalidOperationException("Connection string 'PermissionsWebApiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var elasticUri = builder.Configuration.GetValue<string>("ELKConfiguration:Uri");
var elasticIndex = builder.Configuration.GetValue<string>("ELKConfiguration:index");

var elasticPool = new SingleNodeConnectionPool(new Uri(elasticUri));

var settings = new ConnectionSettings(elasticPool)
    .DefaultIndex(elasticIndex);

var elasticClient = new ElasticClient(settings);
builder.Services.AddSingleton(elasticClient);

var app = builder.Build();

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
