# Configure the database connection:

Update the `appsettings.json` file with your SQL Server connection string:

```json
{
    "ConnectionStrings": {
        "AppContext": "Server=YOUR_SERVER;Database=EventManagement;Trusted_Connection=True;"
    }
}
```

Apply migrations and seed the database:

```bash
dotnet ef database update
```

# Run the backend server:

```bash
dotnet run
```


