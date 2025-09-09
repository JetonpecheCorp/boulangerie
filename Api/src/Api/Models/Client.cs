using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Client
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public string? Mail { get; set; }

    public string? Telephone { get; set; }

    public string? Login { get; set; }

    public string? Mdp { get; set; }

    public string Adresse { get; set; } = null!;

    public string AdresseFacturation { get; set; } = null!;

    public string? InfoComplementaire { get; set; }

    public bool ConnexionBloquer { get; set; }

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;
}
