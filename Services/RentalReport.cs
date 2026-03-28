namespace RentalApp.Services;

public class RentalReport
{
    public required int TotalEquipment { get; init; }
    public required int RentedEquipment { get; init; }
    public required int UnavailableEquipment { get; init; }
    public required int ActiveRentals { get; init; }
    public required decimal TotalCollectedPenalties { get; init; }

    public override string ToString() =>
        $"Sprzęt: łącznie={TotalEquipment}, wypożyczony={RentedEquipment}, niedostępny={UnavailableEquipment}\n" +
        $"Wypożyczenia: aktywne={ActiveRentals}, zebrane kary={TotalCollectedPenalties:C}";
}
