using Application.CommandHandler;
using Application.Commands;
using Application.QueryHandler;
using Domain;
using Domain.Configuration;
using Domain.Data;
using Domain.DTOs;
using Domain.Models;
using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using PermissionsWebApi.Kafka;


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
builder.Services.AddScoped<ICommandHandler<RemoveByIdCommand>, RemovePermissionCommandHandler>();
builder.Services.AddScoped<IQueryHandler<Permission, QueryByIdCommand>, PermissionQueryHandler>();


var topic = builder.Configuration.GetValue<string>("KafkaConfiguration:Topic");
var groupId = builder.Configuration.GetValue<string>("KafkaConfiguration:GroupId");
var bootstrapServers = builder.Configuration.GetValue<string>("KafkaConfiguration:BootstrapServers");
var saslUsername = builder.Configuration.GetValue<string>("KafkaConfiguration:SaslUsername"); 
var saslPassword = builder.Configuration.GetValue<string>("KafkaConfiguration:SaslPassword");

builder.Services
    .AddSingleton<IHostedService, KafkaConsumerHandler>
    (kafkaCounsumerHandler => new KafkaConsumerHandler(topic, groupId, bootstrapServers, saslUsername, saslPassword));

builder.Services
    .AddSingleton<IKafkaProducerHandler, KafkaProducerHandler>
    (kafkaCounsumerHandler => new KafkaProducerHandler(topic, groupId, bootstrapServers, saslUsername, saslPassword));


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
