using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using SentimentAnalysisAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Registra serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra o HttpClient nomeado "sentiment" (para chamar o serviço Python)
var sentimentUrl = builder.Configuration["SentimentApi:BaseUrl"]
                   ?? throw new InvalidOperationException("SentimentApi:BaseUrl não configurada.");
builder.Services.AddHttpClient("sentiment", client =>
{
    client.BaseAddress = new Uri(sentimentUrl);
});

// Registra memória cache (para cache de análises)
builder.Services.AddMemoryCache();

// Registra o filtro de API key
builder.Services.AddScoped<ApiKeyFilter>();

var app = builder.Build();

// Configura middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.Run();
