using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Produit
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public int IdCategorie { get; set; }

    public int IdTva { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public decimal PrixHt { get; set; }

    public string? Alergene { get; set; }

    public string? CodeInterne { get; set; }

    public decimal? Poids { get; set; }

    public bool EstSupprimer { get; set; }

    public DateOnly DateCreation { get; set; }

    public DateOnly? DateModification { get; set; }

    public decimal Stock { get; set; }

    public decimal StockAlert { get; set; }

    public virtual Categorie IdCategorieNavigation { get; set; } = null!;

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual Tva IdTvaNavigation { get; set; } = null!;

    public virtual ICollection<PlanningProduitUtilisateur> PlanningProduitUtilisateurs { get; set; } = new List<PlanningProduitUtilisateur>();

    public virtual ICollection<ProduitLivraison> ProduitLivraisons { get; set; } = new List<ProduitLivraison>();

    public virtual ICollection<Recette> Recettes { get; set; } = new List<Recette>();

    public virtual ICollection<Fournisseur> IdFournisseurs { get; set; } = new List<Fournisseur>();
}
