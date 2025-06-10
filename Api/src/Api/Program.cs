using Api;
using Api.Extensions;
using Api.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Mail;
using System.Security.Cryptography;

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

string cheminCleRsa = builder.Configuration.GetValue<string>("cheminCleRsa")!;

RSA rsa = RSA.Create();

if(!Directory.Exists("Rsa"))
    Directory.CreateDirectory("Rsa");

// creer la clé une seule fois
if (!File.Exists(cheminCleRsa))
{
    // cree un fichier bin pour signer le JWT
    var clePriver = rsa.ExportRSAPrivateKey();
    File.WriteAllBytes(cheminCleRsa, clePriver);
}

// recupere la clé
rsa.ImportRSAPrivateKey(File.ReadAllBytes(cheminCleRsa), out _);

builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy(NomPolicyJwt.DefautClient, x => x.RequireRole("client").RequireClaim("idUtilisateur").RequireClaim("idGroupe"))
    .AddPolicy(NomPolicyJwt.DefautAdmin, x => x.RequireRole("admin").RequireClaim("idUtilisateur"))
    .AddPolicy(NomPolicyJwt.ResetMdp, x => x.RequireClaim("mdp-oublie").RequireClaim("idUtilisateur").RequireClaim("idGroupe"));

builder.Services.AjouterSecuriteJwt(rsa);
builder.Services.AddDbContext<BoulangerieContext>(x =>
{
    try
    {
        string connexion = builder.Configuration.GetConnectionString("defaut")!;
        x.UseMySql(connexion, ServerVersion.AutoDetect(connexion));
    }
    catch
    {
        Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AjouterSwagger();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCors(x => x.AddDefaultPolicy(y => y.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Content-Disposition")));
builder.Services.AjouterService(rsa);
builder.Services.AjouterOutputCache();

builder.Services.AddSingleton<IMailService>(new MailService(new MailOptions
{
    Expediteur = "nicolas.np63@gmail.com",
    Mdp = "cewy qbhb crqd mvsi",
    NomSmtp = "smtp.gmail.com",
    NumeroPortSmtp = 587
}));

var app = builder.Build();

app.UseCors();
// l'ordre est important
app.UseAuthentication();
app.UseAuthorization();


    app.UseSwagger();

    // cacher la liste des models import / export dans swagger
    app.UseSwaggerUI(x => x.DefaultModelsExpandDepth(-1));


app.UseOutputCache();
app.AjouterRouteAPI();

app.Run();

// Scaffold-DbContext "server=mysql;database=Boulangerie;User=root;Pwd=root;GuidFormat=Char36" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -Force