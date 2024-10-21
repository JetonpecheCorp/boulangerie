using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Recette
{
    public int IdIngredient { get; set; }

    public int IdProduit { get; set; }

    public decimal? Quantite { get; set; }

    public virtual Ingredient IdIngredientNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
