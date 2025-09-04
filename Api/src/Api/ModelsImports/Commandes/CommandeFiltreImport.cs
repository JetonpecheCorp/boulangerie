using Api.Enums;

namespace Api.ModelsImports.Commandes;

public sealed class CommandeFiltreImport: PaginationImport
{
    public DateOnly DateDebut { get; set; }
    public DateOnly DateFin { get; set; }
    public EStatusCommande Status { get; set; }
    public bool? SansLivraison { get; set; }
    public Guid? IdPublicClient { get; set; }
}
