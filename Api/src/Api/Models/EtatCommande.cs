using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EtatCommande
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public string? CouleurHex { get; set; }

    public bool EstSupprimer { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;
}
