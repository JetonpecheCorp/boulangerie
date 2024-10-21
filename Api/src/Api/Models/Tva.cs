using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Tva
{
    public int Id { get; set; }

    public decimal Valeur { get; set; }

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
}
