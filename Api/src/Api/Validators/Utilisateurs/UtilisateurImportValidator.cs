using Api.Extensions;
using Api.ModelsImports.Utilisateurs;
using Api.Services.Utilisateurs;
using FluentValidation;

namespace Api.Validators.Utilisateurs;

public class UtilisateurImportValidator: AbstractValidator<UtilisateurImport>
{
    public UtilisateurImportValidator(IUtilisateurService _utilisateurServ)
    {
        RuleFor(x => x.Mdp).MotDePasse();
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Prenom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Mail).EmailAddress();

        RuleFor(x => x).CustomAsync(async (info, context, _) =>
        {
            if (string.IsNullOrWhiteSpace(info.Mail))
                context.AddFailure("Mail", "Le mail ne peux pas être vide");

            if (await _utilisateurServ.MailExisteAsync(info.Mail, info.IdGroupe))
                context.AddFailure("Mail", "Le mail existe déjà");
        });

        RuleFor(x => x.Telephone).MaximumLength(20);
    }
}
