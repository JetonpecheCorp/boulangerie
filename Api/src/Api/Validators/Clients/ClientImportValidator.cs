using Api.Extensions;
using Api.ModelsImports;
using Api.Services.Clients;
using FluentValidation;

namespace Api.Validators.Clients;

public class ClientImportValidator: AbstractValidator<ClientImport>
{
    public ClientImportValidator(IClientService _clientServ, IHttpContextAccessor _contextAccessor)
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Adresse).NotEmpty().MaximumLength(800);
        RuleFor(x => x.AdresseFacturation).MaximumLength(800);
        RuleFor(x => x.Mail).EmailAddress().When(x => x.Mail is not null).MaximumLength(250);
        RuleFor(x => x.Telephone).MaximumLength(20);
        RuleFor(x => x.InfoComplementaire).MaximumLength(1_000);

        RuleFor(x => x).CustomAsync(async (x, context, _) =>
        {
            if (x.Mode == Enums.EModeImport.Modifier)
            {
                int idGroupe = _contextAccessor.HttpContext!.RecupererIdGroupe();

                if (!await _clientServ.ExisteAsync(x.IdPublic, idGroupe))
                    context.AddFailure("idPublic", "Le client n'existe pas");
            }
        });
    }
}
