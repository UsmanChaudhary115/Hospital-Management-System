# Hospital Management System (Console, C#)

Console-based HMS covering patient enrollment and appointment scheduling.
Two persistence modes:
- File storage (JSON)
- Database (SQL Server)

## Run

### File mode
dotnet run --project src/HMS.Console -- --mode file

### DB mode
Set env var:
  HMS_CONN="Data Source=hms.db"
Then:
  dotnet run --project src/HMS.Console -- --mode db

## Features
- Register/search patients
- Create/cancel appointments
- Prevent overlapping slots per doctor
- Basic reports (daily schedule)

## Structure
Domain / Application / Infrastructure / Presentation layered. Repositories abstract storage.

## Development
- .NET 8
- xUnit tests in HMS.Tests
- Logging to logs/app.log

## Notes
Educational project. Data is mock/demo only.
