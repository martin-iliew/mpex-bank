using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.OpenApi.Models;
using MpexTestApi.Extensions;
using MpexTestApi.Infrastructure.Data.Models;
using System.Net;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddApplicationServices();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

    options.AddPolicy("AllowFrontEnd",
        policy => policy
            .WithOrigins("https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Enter your token in the text input below. Example: `abc123`"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions
{
    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
    ClientCertificateMode = ClientCertificateMode.AllowCertificate,
    ServerCertificate = new X509Certificate2("../../../certificate/certificate.pfx", "123456")

};
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(listenOptions =>
        listenOptions.UseHttps(httpsConnectionAdapterOptions));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontEnd");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
