using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ProduitCommande
{
    public int IdProduit { get; set; }

    public int IdCommande { get; set; }

    public int Quantite { get; set; }

    public decimal PrixHt { get; set; }

    public virtual Commande IdCommandeNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
