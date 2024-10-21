using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Planning
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public virtual ICollection<PlanningProduitUtilisateur> PlanningProduitUtilisateurs { get; set; } = new List<PlanningProduitUtilisateur>();
}
