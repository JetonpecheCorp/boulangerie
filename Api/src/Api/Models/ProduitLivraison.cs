using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ProduitLivraison
{
    public int IdProduit { get; set; }

    public int IdLivraison { get; set; }

    public decimal PrixHt { get; set; }

    public decimal? Poids { get; set; }

    public decimal Tva { get; set; }

    public decimal Quantite { get; set; }

    public virtual Livraison IdLivraisonNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
