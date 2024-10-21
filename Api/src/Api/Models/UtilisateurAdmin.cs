using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class UtilisateurAdmin
{
    public int Id { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Mdp { get; set; } = null!;

    public string? Telephone { get; set; }
}
