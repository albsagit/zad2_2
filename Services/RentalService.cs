using RentalApp.Domain;

namespace RentalApp.Services;

public class RentalService
{
    private readonly RentalPolicy _policy;
    private readonly List<User> _users = [];
    private readonly List<Equipment> _equipment = [];
    private readonly List<Rental> _rentals = [];

    public RentalService(RentalPolicy policy)
    {
        _policy = policy;
    }

    public IReadOnlyList<User> Users => _users;
    public IReadOnlyList<Equipment> Equipment => _equipment;
    public IReadOnlyList<Rental> Rentals => _rentals;

    public User AddUser(User user)
    {
        _users.Add(user);
        return user;
    }

    public Equipment AddEquipment(Equipment item)
    {
        _equipment.Add(item);
        return item;
    }

    public IEnumerable<User> GetAllUsers() => _users;

    public IEnumerable<Equipment> GetAllEquipment() => _equipment;

    public IEnumerable<Rental> GetUserActiveRentals(int userId) =>
        _rentals.Where(r => r.IsActive && r.User.Id == userId);

    public OperationResult MarkEquipmentAsUnavailable(int equipmentId)
    {
        var item = FindEquipment(equipmentId);
        if (item is null)
        {
            return OperationResult.Fail("Nie znaleziono sprzętu.");
        }

        if (item.Status == EquipmentStatus.Rented)
        {
            return OperationResult.Fail("Nie można oznaczyć wypożyczonego sprzętu jako niedostępny.");
        }

        item.MarkAsUnavailable();
        return OperationResult.Ok("Oznaczono sprzęt jako niedostępny.");
    }

    public OperationResult BorrowEquipment(int userId, int equipmentId, int days, DateTime borrowedAt)
    {
        if (days <= 0)
        {
            return OperationResult.Fail("Czas wypożyczenia musi być większy niż 0 dni.");
        }

        var user = FindUser(userId);
        if (user is null)
        {
            return OperationResult.Fail("Nie znaleziono użytkownika.");
        }

        var item = FindEquipment(equipmentId);
        if (item is null)
        {
            return OperationResult.Fail("Nie znaleziono sprzętu.");
        }

        if (item.Status == EquipmentStatus.Unavailable)
        {
            return OperationResult.Fail("Sprzęt jest niedostępny i nie może zostać wypożyczony.");
        }

        if (!item.IsAvailable)
        {
            return OperationResult.Fail("Sprzęt jest już wypożyczony.");
        }

        var activeCount = GetUserActiveRentals(userId).Count();
        var limit = _policy.GetActiveRentalLimit(user.Type);

        if (activeCount >= limit)
        {
            return OperationResult.Fail($"Użytkownik osiągnął limit aktywnych wypożyczeń ({limit}).");
        }

        var rental = new Rental(user, item, borrowedAt, days);
        _rentals.Add(rental);
        item.MarkAsRented();

        return OperationResult.Ok($"Wypożyczono pomyślnie. Id wypożyczenia: {rental.Id}");
    }

    public OperationResult ReturnEquipment(int rentalId, DateTime returnedAt)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental is null)
        {
            return OperationResult.Fail("Nie znaleziono wypożyczenia.");
        }

        if (!rental.IsActive)
        {
            return OperationResult.Fail("Wypożyczenie jest już zamknięte.");
        }

        var overdueDays = rental.OverdueDays(returnedAt);
        var penalty = _policy.CalculatePenalty(overdueDays);

        rental.Close(returnedAt, penalty);
        rental.Equipment.MarkAsAvailable();

        if (penalty > 0)
        {
            return OperationResult.Ok($"Zwrócono z opóźnieniem ({overdueDays} dni). Kara: {penalty:C}");
        }

        return OperationResult.Ok("Zwrócono w terminie. Brak kary.");
    }

    public RentalReport GenerateReport()
    {
        var totalPenalties = _rentals.Sum(r => r.PenaltyFee);

        return new RentalReport
        {
            TotalEquipment = _equipment.Count,
            RentedEquipment = _equipment.Count(e => e.Status == EquipmentStatus.Rented),
            UnavailableEquipment = _equipment.Count(e => e.Status == EquipmentStatus.Unavailable),
            ActiveRentals = _rentals.Count(r => r.IsActive),
            TotalCollectedPenalties = totalPenalties
        };
    }

    private User? FindUser(int userId) => _users.FirstOrDefault(u => u.Id == userId);

    private Equipment? FindEquipment(int equipmentId) => _equipment.FirstOrDefault(e => e.Id == equipmentId);
}
