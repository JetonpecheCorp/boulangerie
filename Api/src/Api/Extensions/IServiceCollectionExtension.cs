using Api.Services.Categories;
using Api.Services.Groupes;
using Api.Services.Ingredients;
using Api.Services.Produits;
using Api.Services.Recettes;
using Api.Services.Tvas;
using Api.Services.Utilisateurs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.DeuxFa;
using Services.Jwts;
using Services.Mdp;
using Services.QrCodes;
using System.Reflection;
using System.Security.Cryptography;

namespace Api.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AjouterService(this IServiceCollection _service, RSA _rsa)
    {
        _service.AddScoped<IGroupeService, GroupeService>()
            .AddScoped<IUtilisateurService, UtilisateurService>()
            .AddScoped<IIngredientService, IngredientService>()
            .AddScoped<IRecetteService, RecetteService>()
            .AddScoped<ITvaService, TvaService>()
            .AddScoped<ICategorieService, CategorieService>()
            .AddScoped<IProduitService, ProduitService>();

        _service.AddSingleton<IJwtService>(new JwtService(_rsa, ""))
            .AddSingleton<IMdpService, MdpService>();

        _service.AddTransient<IDeuxFaService, DeuxFaService>()
            .AddTransient<IQrCodeService, QrCodeService>();

        // donne acces a httpContext dans les validators
        _service.AddHttpContextAccessor();

        return _service;
    }

    public static IServiceCollection AjouterSecuriteJwt(this IServiceCollection _service, RSA _rsa)
    {
        _service.AddAuthorizationBuilder();
        _service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    // se qu'on veut valider ou non
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // permet de valider le chiffrement du JWT en definissant la clé utilisée
                option.Configuration = new OpenIdConnectConfiguration
                {
                    SigningKeys = { new RsaSecurityKey(_rsa) }
                };

                // pour avoir les cl� valeur normal comme dans les claims
                // par defaut ajouter des Uri pour certain truc comme le "sub"
                option.MapInboundClaims = false;
            });

        return _service;
    }

    public static IServiceCollection AjouterSwagger(this IServiceCollection _service)
    {
        _service.AddSwaggerGen(swagger =>
        {
            // genere un XML et permet de voir la doc dans swagger pour chaque Routes API
            string xmlNomFichier = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlNomFichier));

            // ajout d'une option pour mettre le token en mode Bearer
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                // ou le trouver
                In = ParameterLocation.Header,

                // description
                Description = "Token",

                // nom dans le header
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",

                // JWT de type Bearer
                Scheme = "Bearer"
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[]{}
                }
            });
        });

        return _service;
    }
}
