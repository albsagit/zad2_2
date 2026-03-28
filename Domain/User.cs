namespace RentalApp.Domain;

public abstract class User
{
    private static int _nextId = 1;

    protected User(string firstName, string lastName)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public abstract UserType Type { get; }

    public override string ToString()
    {
        var type = Type switch
        {
            UserType.Student => "Student",
            UserType.Employee => "Pracownik",
            _ => Type.ToString()
        };

        return $"[{Id}] {FirstName} {LastName} ({type})";
    }
}
