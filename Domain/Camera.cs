namespace RentalApp.Domain;

public sealed class Camera : Equipment
{
    public Camera(string name, int megapixels, bool supports4K)
        : base(name)
    {
        Megapixels = megapixels;
        Supports4K = supports4K;
    }

    public int Megapixels { get; }
    public bool Supports4K { get; }

    public override string GetSpecificDescription() => $"Aparat | Matryca: {Megapixels} MP | 4K: {(Supports4K ? "Tak" : "Nie")}";
}
