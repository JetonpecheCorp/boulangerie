using Api.ModelsExports.Categories;
using Api.ModelsExports.Clients;
using Api.ModelsExports.Fournisseurs;
using Api.ModelsExports.Ingredients;
using Api.ModelsExports.Livraisons;
using Api.ModelsExports.Produits;
using Api.ModelsExports.Utilisateurs;
using Api.ModelsExports.Vehicules;
using System.Text.Json.Serialization;

namespace Api.ModelsExports;

public sealed record PaginationExport<T> where T : class
{
    public required T[] Liste { get; init; }

    public int NumPage { get; init; }

    public int NbParPage { get; init; }

    public int Total { get; init; }

    public bool AUneProchainePage => Total > (NumPage * NbParPage);
}

[JsonSerializable(typeof(PaginationExport<IngredientExport>))]
[JsonSerializable(typeof(PaginationExport<ProduitExport>))]
[JsonSerializable(typeof(PaginationExport<ProduitLegerExport>))]
[JsonSerializable(typeof(PaginationExport<CategorieExport>))]
[JsonSerializable(typeof(PaginationExport<VehiculeExport>))]
[JsonSerializable(typeof(PaginationExport<FournisseurExport>))]
[JsonSerializable(typeof(PaginationExport<ClientExport>))]
[JsonSerializable(typeof(PaginationExport<ClientLegerExport>))]
[JsonSerializable(typeof(PaginationExport<UtilisateurLegerExport>))]
[JsonSerializable(typeof(PaginationExport<LivraisonExport>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class PaginationExportContext: JsonSerializerContext { }
