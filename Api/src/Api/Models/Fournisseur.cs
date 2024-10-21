using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Fournisseur
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Nom { get; set; } = null!;

    public string? Adresse { get; set; }

    public string? Telephone { get; set; }

    public string? Mail { get; set; }

    public bool EstSupprimer { get; set; }

    public DateOnly DateCreation { get; set; }

    public DateOnly? DateModification { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual ICollection<Ingredient> IdIngredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Produit> IdProduits { get; set; } = new List<Produit>();
}
