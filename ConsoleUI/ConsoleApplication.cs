using RentalApp.Domain;
using RentalApp.Services;

namespace RentalApp.ConsoleUI;

public class ConsoleApplication
{
    private readonly RentalService _service;

    public ConsoleApplication(RentalService service)
    {
        _service = service;
        SeedData();
    }

    public void Run()
    {
        Console.WriteLine("=== System Wypożyczania Sprzętu ===");

        while (true)
        {
            ShowMenu();
            var option = (Console.ReadLine() ?? string.Empty).Trim();

            switch (option)
            {
                case "1": AddUser(); break;
                case "2": AddEquipment(); break;
                case "3": PrintEquipment(_service.GetAllEquipment()); break;
                case "4": BorrowEquipment(); break;
                case "5": ReturnEquipment(); break;
                case "6": MarkEquipmentUnavailable(); break;
                case "7": ShowUserActiveRentals(); break;
                case "8": ShowReport(); break;
                case "9": ShowAllUsers(); break;
                case "0": return;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Wybierz operację:");
        Console.WriteLine("1. Dodaj użytkownika");
        Console.WriteLine("2. Dodaj sprzęt");
        Console.WriteLine("3. Pokaż cały sprzęt");
        Console.WriteLine("4. Wypożycz sprzęt");
        Console.WriteLine("5. Zwróć sprzęt");
        Console.WriteLine("6. Oznacz sprzęt jako niedostępny");
        Console.WriteLine("7. Pokaż aktywne wypożyczenia użytkownika");
        Console.WriteLine("8. Wygeneruj raport");
        Console.WriteLine("9. Pokaż wszystkich użytkowników");
        Console.WriteLine("0. Zakończ");
        Console.Write("Opcja: ");
    }

    private void AddUser()
    {
        Console.Write("Typ (student/pracownik): ");
        var type = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

        Console.Write("Imię: ");
        var firstName = Console.ReadLine() ?? string.Empty;

        Console.Write("Nazwisko: ");
        var lastName = Console.ReadLine() ?? string.Empty;

        User? user = type switch
        {
            "student" => CreateStudent(firstName, lastName),
            "pracownik" => CreateEmployee(firstName, lastName),
            _ => null
        };

        if (user is null)
        {
            Console.WriteLine("Nieobsługiwany typ użytkownika.");
            return;
        }

        _service.AddUser(user);
        Console.WriteLine($"Dodano użytkownika: {user}");
    }

    private static Student CreateStudent(string firstName, string lastName)
    {
        Console.Write("Numer indeksu: ");
        var index = Console.ReadLine() ?? string.Empty;
        return new Student(firstName, lastName, index);
    }

    private static Employee CreateEmployee(string firstName, string lastName)
    {
        Console.Write("Dział: ");
        var department = Console.ReadLine() ?? string.Empty;
        return new Employee(firstName, lastName, department);
    }

    private void AddEquipment()
    {
        Console.Write("Typ (laptop/projektor/aparat): ");
        var type = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

        Console.Write("Nazwa: ");
        var name = Console.ReadLine() ?? string.Empty;

        if (type is not ("laptop" or "projektor" or "aparat"))
        {
            Console.WriteLine("Nieobsługiwany typ sprzętu.");
            return;
        }

        Equipment? equipment = type switch
        {
            "laptop" => CreateLaptop(name),
            "projektor" => CreateProjector(name),
            "aparat" => CreateCamera(name),
            _ => null
        };

        if (equipment is null)
        {
            return;
        }

        _service.AddEquipment(equipment);
        Console.WriteLine($"Dodano sprzęt: {equipment}");
    }

    private static Laptop? CreateLaptop(string name)
    {
        if (!TryReadNaturalNumber("RAM (GB): ", "RAM", out var ram))
        {
            return null;
        }

        Console.Write("Model CPU: ");
        var cpu = Console.ReadLine() ?? "Nieznany";
        return new Laptop(name, ram, cpu);
    }

    private static Projector? CreateProjector(string name)
    {
        if (!TryReadNaturalNumber("Jasność (lumeny): ", "Jasność", out var lumens))
        {
            return null;
        }

        Console.Write("Rozdzielczość natywna: ");
        var resolution = Console.ReadLine() ?? "1920x1080";
        return new Projector(name, lumens, resolution);
    }

    private static Camera? CreateCamera(string name)
    {
        if (!TryReadNaturalNumber("Megapiksele: ", "Megapiksele", out var megapixels))
        {
            return null;
        }

        Console.Write("Obsługuje 4K (t/n): ");
        var supports4K = (Console.ReadLine() ?? "n").Trim().ToLowerInvariant() == "t";
        return new Camera(name, megapixels, supports4K);
    }

    private void BorrowEquipment()
    {
        if (!TryReadNaturalNumber("Id użytkownika: ", "Id użytkownika", out var userId))
        {
            return;
        }

        if (!TryReadNaturalNumber("Id sprzętu: ", "Id sprzętu", out var equipmentId))
        {
            return;
        }

        if (!TryReadNaturalNumber("Liczba dni: ", "Liczba dni", out var days))
        {
            return;
        }

        var result = _service.BorrowEquipment(userId, equipmentId, days, DateTime.Now);
        Console.WriteLine(result.Message);
    }

    private void ReturnEquipment()
    {
        if (!TryReadNaturalNumber("Id wypożyczenia: ", "Id wypożyczenia", out var rentalId))
        {
            return;
        }

        var result = _service.ReturnEquipment(rentalId, DateTime.Now);
        Console.WriteLine(result.Message);
    }

    private void MarkEquipmentUnavailable()
    {
        if (!TryReadNaturalNumber("Id sprzętu: ", "Id sprzętu", out var equipmentId))
        {
            return;
        }

        var result = _service.MarkEquipmentAsUnavailable(equipmentId);
        Console.WriteLine(result.Message);
    }

    private void ShowUserActiveRentals()
    {
        if (!TryReadNaturalNumber("Id użytkownika: ", "Id użytkownika", out var userId))
        {
            return;
        }

        var rentals = _service.GetUserActiveRentals(userId).ToList();
        if (rentals.Count == 0)
        {
            Console.WriteLine("Brak aktywnych wypożyczeń dla tego użytkownika.");
            return;
        }

        foreach (var rental in rentals)
        {
            Console.WriteLine(rental.ToString());
        }
    }

    private void ShowReport()
    {
        var report = _service.GenerateReport();
        Console.WriteLine(report.ToString());
    }

    private void ShowAllUsers()
    {
        var users = _service.GetAllUsers().ToList();
        if (users.Count == 0)
        {
            Console.WriteLine("Brak użytkowników do wyświetlenia.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine(user.ToString());
        }
    }

    private void PrintEquipment(IEnumerable<Equipment> equipment)
    {
        var list = equipment.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("Brak sprzętu do wyświetlenia.");
            return;
        }

        foreach (var item in list)
        {
            Console.WriteLine(item.ToString());
        }
    }

    private void SeedData()
    {
        _service.AddUser(new Student("Jan", "Kowalski", "s12345"));
        _service.AddUser(new Employee("Anna", "Nowak", "IT"));

        _service.AddEquipment(new Laptop("Dell Latitude 5520", 16, "Intel i7"));
        _service.AddEquipment(new Projector("Epson EB-FH52", 4000, "1920x1080"));
        _service.AddEquipment(new Camera("Sony A6400", 24, true));
    }

    private static bool TryReadNaturalNumber(string prompt, string fieldName, out int value)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();

        if (!int.TryParse(input, out value) || value <= 0)
        {
            Console.WriteLine($"{fieldName} musi być liczbą naturalną.");
            return false;
        }

        return true;
    }
}
