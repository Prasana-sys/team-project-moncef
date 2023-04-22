-Register application for user authentication on Azure Active Directory:
    https://learn.microsoft.com/en-us/graph/tutorials/dotnet?tabs=aad&tutorial-step=1

-Create a .NET console app
    dotnet new console -o MonCal

-get these .NET packages:
    dotnet add package Microsoft.Extensions.Configuration.Binder
    dotnet add package Microsoft.Extensions.Configuration.Json
    dotnet add package Microsoft.Extensions.Configuration.UserSecrets
    dotnet add package Azure.Identity
    dotnet add package Microsoft.Graph

Desktop app that calls web APIs: Acquire a token interactively:
The following example shows minimal code to get a token interactively for reading the user's profile with Microsoft Graph.
The web API is defined by its scopes. Whatever the experience you provide in your application, the pattern to use is:

Systematically attempt to get a token from the token cache by calling AcquireTokenSilent.
If this call fails, use the AcquireToken flow that you want to use, which is represented here by AcquireTokenInteractive.

-Snipped in C#:
```
    string[] scopes = new string[] {"user.read"};
    var app = PublicClientApplicationBuilder.Create(clientId).Build();
    var accounts = await app.GetAccountsAsync();
    AuthenticationResult result;
    try
    {
    result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                .ExecuteAsync();
    }
    catch(MsalUiRequiredException)
    {
    result = await app.AcquireTokenInteractive(scopes)
                .ExecuteAsync();
    }
```

-Documentation to use for Microsoft Graph REST API v1.0:

https://learn.microsoft.com/en-us/graph/api/overview?view=graph-rest-1.0
