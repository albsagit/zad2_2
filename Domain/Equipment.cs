namespace RentalApp.Domain;

public abstract class Equipment
{
    private static int _nextId = 1;

    protected Equipment(string name)
    {
        Id = _nextId++;
        Name = name;
        Status = EquipmentStatus.Available;
    }

    public int Id { get; }
    public string Name { get; }
    public EquipmentStatus Status { get; private set; }

    public bool IsAvailable => Status == EquipmentStatus.Available;

    public void MarkAsRented() => Status = EquipmentStatus.Rented;

    public void MarkAsAvailable() => Status = EquipmentStatus.Available;

    public void MarkAsUnavailable() => Status = EquipmentStatus.Unavailable;

    public abstract string GetSpecificDescription();

    public override string ToString()
    {
        var status = Status switch
        {
            EquipmentStatus.Available => "Dostępny",
            EquipmentStatus.Rented => "Wypożyczony",
            EquipmentStatus.Unavailable => "Niedostępny",
            _ => Status.ToString()
        };

        return $"[{Id}] {Name} | Status: {status} | {GetSpecificDescription()}";
    }
}
