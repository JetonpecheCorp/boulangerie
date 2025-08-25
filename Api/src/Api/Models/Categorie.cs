using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Categorie
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public bool EstSupprimer { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
}
