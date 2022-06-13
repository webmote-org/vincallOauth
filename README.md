# Introduction
The project completes Vincall's OAuth 2 function, which is written using the [IdentityServer4] (https://github.com/IdentityServer/IdentityServer4) framework, using .NET core 3.1 technology, providing a programming implementation of the standard OAuth2 four processes, and supporting the OpenID standard.

# Initializing
Clone VincallOAuth repository on your computer, install dependencies using:
```
git clone xxx
```
Then if you want to run the project, go to the directory VincallOAuth.
```
dotnet run 
```

The configuration items of the project are as follows and can be modified according to the actual situation.
```json
{
  "ConnectionStrings": {
    "AuthDB": "Server=192.168.2.215;User=sa;Password=Aa00000000;Database=vincall"
  },
  "InitDB": "true",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },  
  "Domain": "comm100dev.io"
}
```
- ConnectionStrings: The connection string to Microsoft SQL server database.
- InitDB: Whether to recreate the database table structure and initialize the population of data. If the data table structure has already been initialized, it will not be modified again.
- Logging: The definition and type of the log output are changed
- Domain: When you create a cookie, you need the root domain. The cookie is written in the root domain and can be used by all subdomains.

 If you are initializing for the first time, you will need to prepare the MS SQL Server environment. Once you have the database environment ready, you can change the ConnectionString Authdb key and InitDB key.

```
 "ConnectionStrings": {
    "AuthDB": "Server=[your ip];User=[your username];Password=[your passwor];Database=vincall"
  },
  "InitDB": "true",
```
Then to run the program.

```
dotnet run
```
Finally, Let's check database when opening Microsoft SQL server management studio.

That is ok.

