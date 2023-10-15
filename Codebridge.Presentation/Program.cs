using Autofac.Extensions.DependencyInjection;
using Autofac;
using Codebridge.Application.Extensions;
using Codebridge.Persistant.Data.Contexts;
using Codebridge.Persistant.Extention;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Codebridge.Persistant.ModuleContainer;
using AspNetCoreRateLimit;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using Codebridge.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration, builder.Host);
builder.Services.AddDbContext<CodebridgeContext>();
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly,typeof(Program).Assembly);
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddRateLimiter(options=> 
{
    options.AddFixedWindowLimiter("fixedWindow",opt =>
    {
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueLimit = 10;
        opt.PermitLimit = 10;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;

    }).RejectionStatusCode = 429;

});



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Codebridge API",
        Version = "v1"
    });
});




var app = builder.Build();


if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","Codebridge API V1");
    });
}
app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
