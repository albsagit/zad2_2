namespace RentalApp.Domain;

public class Rental
{
    private static int _nextId = 1;

    public Rental(User user, Equipment equipment, DateTime borrowedAt, int days)
    {
        Id = _nextId++;
        User = user;
        Equipment = equipment;
        BorrowedAt = borrowedAt;
        DueDate = borrowedAt.Date.AddDays(days);
    }

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime BorrowedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal PenaltyFee { get; private set; }

    public bool IsActive => ReturnedAt is null;
    public bool IsOverdue(DateTime now) => IsActive && now.Date > DueDate.Date;

    public int OverdueDays(DateTime returnDate)
    {
        var days = (returnDate.Date - DueDate.Date).Days;
        return days > 0 ? days : 0;
    }

    public void Close(DateTime returnedAt, decimal penaltyFee)
    {
        ReturnedAt = returnedAt;
        PenaltyFee = penaltyFee;
    }

    public override string ToString()
    {
        var status = IsActive ? "Aktywne" : $"Zwrócono: {ReturnedAt:yyyy-MM-dd}";
        return $"Wypożyczenie #{Id} | Użytkownik: {User.FirstName} {User.LastName} | Sprzęt: {Equipment.Name} | Termin zwrotu: {DueDate:yyyy-MM-dd} | Status: {status} | Kara: {PenaltyFee:C}";
    }
}
