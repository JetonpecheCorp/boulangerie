using Api.Enums;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Api.ModelsImports.Commandes;

public sealed class CommandeFiltreImport: PaginationImport
{
    public DateOnly DateDebut { get; set; }
    public DateOnly DateFin { get; set; }
    public EStatusCommande Status { get; set; }
    public bool? SansLivraison { get; set; }
    public Guid? IdPublicClient { get; set; }

    public bool roleClient = false;
}
