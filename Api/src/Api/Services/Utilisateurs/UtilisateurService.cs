using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Utilisateurs;

public class UtilisateurService(BoulangerieContext _context) : IUtilisateurService
{
    public async Task AjouterAsync(Utilisateur _utilisateur)
    {
        _context.Add(_utilisateur);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> MailExiste(string _mail, int _idGroupe = 0)
    {
        if(string.IsNullOrWhiteSpace(_mail))
            return false;

        var requete = _context.Utilisateurs.Where(x => x.Mail == _mail);

        if(_idGroupe > 0)
            requete = requete.Where(x => x.IdGroupe == _idGroupe);

        return await requete.AnyAsync();
    }
}
