using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Groupe
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string Adresse { get; set; } = null!;

    public bool? ConnexionBloquer { get; set; }

    public virtual ICollection<Categorie> Categories { get; set; } = new List<Categorie>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Fournisseur> Fournisseurs { get; set; } = new List<Fournisseur>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();

    public virtual ICollection<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();

    public virtual ICollection<Vehicule> Vehicules { get; set; } = new List<Vehicule>();
}
