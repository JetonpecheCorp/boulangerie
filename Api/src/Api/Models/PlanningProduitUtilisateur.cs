using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class PlanningProduitUtilisateur
{
    public int IdPlanning { get; set; }

    public int IdProduit { get; set; }

    public int? IdUtilisateur { get; set; }

    public decimal Quantite { get; set; }

    public virtual Planning IdPlanningNavigation { get; set; } = null!;

    public virtual Produit IdProduitNavigation { get; set; } = null!;

    public virtual Utilisateur? IdUtilisateurNavigation { get; set; }
}
