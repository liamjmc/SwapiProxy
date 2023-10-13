using AspNetCoreRateLimit;
using Proxy.Domain;
using SwapiProxy;
using SwapiProxy.API.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.AddSwagger();

builder.AddApiVersioning();

builder.Services.AddTransient<IProxyRequester, ProxyRequester>();
builder.Services.AddTransient<IAggregateProxyRequester, AggregateProxyRequester>();
builder.Services.AddSingleton<IRateLimiter, RateLimiter>();

builder.AddHttpClient();

builder.AddAuth();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.AddRateLimiter();

var app = builder.Build();

var ipPolicyStore = app.Services.GetRequiredService<IIpPolicyStore>(); 
ipPolicyStore.SeedAsync().GetAwaiter().GetResult();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Swapi Proxy API";
        options.SwaggerEndpoint($"/swagger/V1/swagger.json", "V1");
        options.SwaggerEndpoint($"/swagger/V2/swagger.json", "V2");
    });
}

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();