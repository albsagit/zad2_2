using RentalApp.Domain;

namespace RentalApp.Services;

public class RentalPolicy
{
    public int StudentRentalLimit { get; init; } = 2;
    public int EmployeeRentalLimit { get; init; } = 5;
    public decimal PenaltyPerDay { get; init; } = 15m;

    public int GetActiveRentalLimit(UserType userType) => userType switch
    {
        UserType.Student => StudentRentalLimit,
        UserType.Employee => EmployeeRentalLimit,
        _ => throw new ArgumentOutOfRangeException(nameof(userType), userType, "Nieobsługiwany typ użytkownika")
    };

    public decimal CalculatePenalty(int overdueDays) => overdueDays <= 0 ? 0 : overdueDays * PenaltyPerDay;
}
