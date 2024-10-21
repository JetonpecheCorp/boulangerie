using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Utilisateur
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string Mdp { get; set; } = null!;

    public string? Telephone { get; set; }

    public bool EstAdmin { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual ICollection<PlanningProduitUtilisateur> PlanningProduitUtilisateurs { get; set; } = new List<PlanningProduitUtilisateur>();
}
