using RentalApp.ConsoleUI;
using RentalApp.Services;

var policy = new RentalPolicy
{
    StudentRentalLimit = 2,
    EmployeeRentalLimit = 5,
    PenaltyPerDay = 20m
};

var service = new RentalService(policy);
var app = new ConsoleApplication(service);
app.Run();
