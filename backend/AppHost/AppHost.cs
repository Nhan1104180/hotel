var builder = DistributedApplication.CreateBuilder(args);

var IdentityDBService = builder.AddProject<Projects.IdentityDBService>("IdentityDBService");
var UserDBService = builder.AddProject<Projects.UserDBService>("UserDBService");
var RoomDBService = builder.AddProject<Projects.RoomDBService>("RoomDBService");
var ServiceDBService = builder.AddProject<Projects.ServiceDBService>("ServiceDBService");
var BookingDBService = builder.AddProject<Projects.BookingDBService>("BookingDBService");
var NotificationDBService = builder.AddProject<Projects.NotificationDBService>("NotificationDBService");
var PaymentDBService = builder.AddProject<Projects.PaymentDBService>("PaymentDBService");
var ReviewDBService = builder.AddProject<Projects.ReviewDBService>("ReviewDBService");


builder.Build().Run();
