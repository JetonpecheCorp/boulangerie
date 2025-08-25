using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Commande
{
    public int Id { get; set; }

    public int IdGroupe { get; set; }

    public int? IdClient { get; set; }

    public int? IdLivraison { get; set; }

    public string Numero { get; set; } = null!;

    public decimal? PrixTotalHt { get; set; }

    public bool EstLivraison { get; set; }

    public int? OrdreLivraison { get; set; }

    public DateTime DateCreation { get; set; }

    public DateTime DatePourLe { get; set; }

    public DateTime? DateValidation { get; set; }

    public DateTime? DatLivraison { get; set; }

    public DateTime? DateAnnulation { get; set; }

    public virtual Client? IdClientNavigation { get; set; }

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual Livraison? IdLivraisonNavigation { get; set; }

    public virtual ICollection<ProduitCommande> ProduitCommandes { get; set; } = new List<ProduitCommande>();
}
