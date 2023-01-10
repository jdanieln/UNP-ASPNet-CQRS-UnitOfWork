using Application.CommandHandler;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using PermissionsWebApi.Application;
using PermissionsWebApi.Application.CommandHandler;
using PermissionsWebApi.Application.QueryHandler;
using PermissionsWebApi.Configuration;
using PermissionsWebApi.Data;
using PermissionsWebApi.DTOs;
using PermissionsWebApi.Kafka;
using PermissionsWebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PermissionsWebApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PermissionsWebApiContext") ?? throw new InvalidOperationException("Connection string 'PermissionsWebApiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICommandHandler<PermissionDTO>, AddPermissionCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RemoveCommand>, RemovePermissionCommandHandler>();
builder.Services.AddScoped<IQueryHandler<Permission, QueryCommand>, PermissionQueryHandler>();


var topic = builder.Configuration.GetValue<string>("KafkaConfiguration:Topic");
var groupId = builder.Configuration.GetValue<string>("KafkaConfiguration:GroupId");
var bootstrapServers = builder.Configuration.GetValue<string>("KafkaConfiguration:BootstrapServers");

//builder.Services
    //.AddSingleton<IHostedService, KafkaConsumerHandler>
    //(kafkaCounsumerHandler => new KafkaConsumerHandler(topic, groupId, bootstrapServers));

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
