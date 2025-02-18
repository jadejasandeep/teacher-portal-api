# API

# Technologies Used
.NET 8: Framework for building cross-platform applications with the latest features and improvements.
Entity Framework (EF) Core: ORM (Object-Relational Mapper) for database operations using the Code First approach.
SQL Server: Relational database for storing and retrieving data.
JWT (JSON Web Token): For secure user authentication and session management.
ASP.NET Core: Web framework for building the API.

# Prerequisites
.NET 8 SDK
SQL Server (locally installed or a cloud instance)


# Clone the repository:
```
git clone https://github.com/jadejasandeep/teacher-portal-api.git
cd teacher-portal-api
```

# Install dependencies:
```
dotnet restore
```

# Set up environment variables: Create an appsettings.json file in the API project directory and add the following variables:
```
json
{
  "ConnectionStrings": {
    "SqlConnectionString": "Server=localhost\\SQLEXPRESS;Database=TeacherPortalDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "http://localhost",
    "Audience": "http://localhost",
    "SecretKey": "YourSuperSecretKeyForDev123!YourSuperSecretKeyForDev123!YourSuperSecretKeyForDev123!"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "AllowedHosts": "*"
}
```
# Run database migrations:
```
Migration running is in built into Application Run. As soon as application started any pending migrations will be applied
```

# Run the server:

```
dotnet run
```
The API should now be running on https://localhost:7002.


# Improvements or TO DO Things
- More Coverage of Testing currently its around 73%
- More validations and unit tests for Edge Cases
- Any possible caching implementation for specific scenario
  
