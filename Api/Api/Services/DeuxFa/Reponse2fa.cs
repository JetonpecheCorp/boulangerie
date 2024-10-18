namespace Services.DeuxFa;

public sealed record Reponse2fa
{
    public required string CleSecret { get; init; } = null!;

    /// <summary>
    /// Se n'est pas une URI / URL
    /// </summary>
    public required string Url2fa { get; init; } = null!;
}
