namespace RentalApp.Domain;

public sealed class Student : User
{
    public Student(string firstName, string lastName, string studentIndex)
        : base(firstName, lastName)
    {
        StudentIndex = studentIndex;
    }

    public string StudentIndex { get; }

    public override UserType Type => UserType.Student;
}
