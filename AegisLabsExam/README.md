# AegisLabsExam

### Set Default Connection String With DotNet User Secrets
```shell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=<IPAddress>;Database=<Database>;User Id=<Username>;Password=<Password>;"
```