using Api.Enums;

namespace Api.ModelsImports.Commandes;

public sealed record CommandeModifierStatusImport
{
    public required string Numero { get; init; }
    public required EStatusCommandeModifier Status { get; init; }
}
