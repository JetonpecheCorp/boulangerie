using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public string? CodeInterne { get; set; }

    public decimal Stock { get; set; }

    public decimal StockAlert { get; set; }

    public bool EstSupprimer { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual ICollection<Recette> Recettes { get; set; } = new List<Recette>();

    public virtual ICollection<Fournisseur> IdFournisseurs { get; set; } = new List<Fournisseur>();
}
