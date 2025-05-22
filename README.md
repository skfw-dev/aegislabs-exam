# AegisLabsExam

## How to set secret key value

```shell
dotnet user-secrets init
dotnet user-secrets list --json

# Set value
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=<ipAddr>;Database=<database>;User Id=<username>;Password=<password>;Encrypt=true;TrustServerCertificate=true;MultipleActiveResultSets=True;"
```

## How to get secret value by key

```cs
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string secretValue = config["ConnectionStrings:DefaultConnection"];
```
