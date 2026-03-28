namespace RentalApp.Domain;

public sealed class Employee : User
{
    public Employee(string firstName, string lastName, string department)
        : base(firstName, lastName)
    {
        Department = department;
    }

    public string Department { get; }

    public override UserType Type => UserType.Employee;
}
