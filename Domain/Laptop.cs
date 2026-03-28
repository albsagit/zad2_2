namespace RentalApp.Domain;

public sealed class Laptop : Equipment
{
    public Laptop(string name, int ramGb, string cpuModel)
        : base(name)
    {
        RamGb = ramGb;
        CpuModel = cpuModel;
    }

    public int RamGb { get; }
    public string CpuModel { get; }

    public override string GetSpecificDescription() => $"Laptop | RAM: {RamGb} GB | Procesor: {CpuModel}";
}
