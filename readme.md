## Booking application with ASP.NET Core


This is a Booking application that features:
- [**TKWApi**](TKWApi) - An ASP.NET Core REST API backend using controller APIs
- [**TKWLogic**](TKWApi) - Business Layer Logic
- [**TKWData**](TKWData) - Entity Framework Data Layer
- [**TKWIntegrationTests**](TKWIntegrationTests) - Integrations Tests

It showcases:
- Architecture inspired by [Meanwhile... on the command side of my architecture](https://blogs.cuttingedge.it/steven/posts/2011/meanwhile-on-the-command-side-of-my-architecture/)
- Controller based APIs
- Integration tests

## Prerequisites

### .NET
1. [Install .NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Database

The application uses SQLite and entity framework.

### Running the application

To run the application start the API project