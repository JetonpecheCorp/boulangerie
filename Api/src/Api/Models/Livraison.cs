using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Livraison
{
    public int Id { get; set; }

    public int IdVehicule { get; set; }

    public int IdUtilisateur { get; set; }

    public string Numero { get; set; } = null!;

    public decimal Frais { get; set; }

    public DateOnly Date { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;

    public virtual Vehicule IdVehiculeNavigation { get; set; } = null!;

    public virtual ICollection<Commande> IdCommandes { get; set; } = new List<Commande>();
}
