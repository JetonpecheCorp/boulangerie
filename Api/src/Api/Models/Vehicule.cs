using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Vehicule
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public Guid IdPublic { get; set; }

    public string Immatriculation { get; set; } = null!;

    public string? InfoComplementaire { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual ICollection<Livraison> Livraisons { get; set; } = new List<Livraison>();
}
