using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class CommandeInterne
{
    public DateOnly Date { get; set; }

    public int IdProduit { get; set; }

    public int Quantite { get; set; }

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
