namespace RentalApp.Domain;

public sealed class Projector : Equipment
{
    public Projector(string name, int lumens, string nativeResolution)
        : base(name)
    {
        Lumens = lumens;
        NativeResolution = nativeResolution;
    }

    public int Lumens { get; }
    public string NativeResolution { get; }

    public override string GetSpecificDescription() => $"Projektor | Jasność: {Lumens} lm | Rozdzielczość: {NativeResolution}";
}
