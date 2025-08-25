using Api.Extensions;
using Api.ModelsImports.Utilisateurs;
using Api.Services.Utilisateurs;
using FluentValidation;

namespace Api.Validators.Utilisateurs;

public sealed class UtilisateurModifierImportValidator: AbstractValidator<UtilisateurModifierImport>
{
    public UtilisateurModifierImportValidator(IUtilisateurService _utilisateurServ, IHttpContextAccessor _httpContextAccessor)
    {
        int idGroupe = _httpContextAccessor.HttpContext!.RecupererIdGroupe();

        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Prenom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Mail).EmailAddress();
        RuleFor(x => x.IdPublic)
            .NotEmpty()
            .MustAsync(async (idPublic, _) => await _utilisateurServ.ExisteAsync(idPublic, idGroupe));

        RuleFor(x => x).CustomAsync(async (info, context, _) =>
        {
            if (string.IsNullOrWhiteSpace(info.Mail))
                context.AddFailure("Mail", "Le mail ne peux pas être vide");

            if (await _utilisateurServ.MailExisteAsync(info.Mail, info.IdPublic))
                context.AddFailure("Mail", "Le mail existe déjà");
        });

        RuleFor(x => x.Telephone).MaximumLength(20);
    }
}
