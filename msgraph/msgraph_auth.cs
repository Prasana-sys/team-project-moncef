using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

class GraphHelper
{
    // // Settings object
    // private static Settings? _settings;
    
    // Client configured with user authentication
    private static GraphServiceClient? _userClient;
    
    // private static InteractiveBrowserCredential? _interactiveBrowserCredential;

    private static HttpClient? httpClient;

    public static async Task InitializeGraphForUserAuth (IPublicClientApplication app)
    {   
        #pragma warning disable 8600 // _settings will never be null
        string[] scopes = Settings.GraphUserScopes;
        #pragma warning restore 8600

        var accounts = await app.GetAccountsAsync();
        AuthenticationResult result;

        try
        {   
            //Console.WriteLine(accounts.First().Username);
            result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

        }
        catch(MsalUiRequiredException authRequiredEx)
        {   
            Console.WriteLine(authRequiredEx.Message);
            //Console.WriteLine(authRequiredEx.InnerException.Message);
            result = await app.AcquireTokenInteractive(scopes)
                        .ExecuteAsync();
        }

        // Succesfully creates a GraphServiceClient to use protected web API calls
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        _userClient = new GraphServiceClient (httpClient);

        //     //_settings = settings;

        //     var _interactiveBrowserCredentialOptions = new InteractiveBrowserCredentialOptions
        //     {
        //         ClientId = settings.ClientId,
        //         TenantId = settings.TenantId,
        //         // AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        //         DisableAutomaticAuthentication = false,
        //         // MUST be http://localhost or http://localhost:PORT
        //         // See https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core
        //         // RedirectUri = new Uri("http://localhost"),
        //     };

        //     // https://docs.microsoft.com/dotnet/api/azure.identity.interactivebrowsercredential
        //     _interactiveBrowserCredential = new InteractiveBrowserCredential(_interactiveBrowserCredentialOptions);
        //     // Task<AuthenticationRecord> authRecordTask = _interactiveBrowserCredential.AuthenticateAsync ();

        //     // //Get access token
        //     // var token = _interactiveBrowserCredential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://graph.microsoft.com/.default" }));

        //     // Console.WriteLine("Access token: " + token.Token);

        //     // Console.WriteLine("Expires On: " + token.ExpiresOn);

        //     //Get graph client based on interactiveCredential and scope.
        //     _userClient = new GraphServiceClient(_interactiveBrowserCredential, settings.GraphUserScopes);
    }

    public static Task<User?> GetUserAsync()
        {
            // Ensure client isn't null
            _ = _userClient ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");

            return _userClient.Me.GetAsync((config) =>
            {
                // Only request specific properties
                config.QueryParameters.Select = new[] {"displayName", "mail", "userPrincipalName" };
            });
        }

    public async static Task CreateEvent(string subject, ItemBody body, DateTimeTimeZone start, DateTimeTimeZone end, 
                                         Location location, List<Attendee> attendees, PatternedRecurrence recurrence, 
                                         string preferredTimeZone, bool allowNewTimeProposals, bool isAllDay, bool isReminderOn, 
                                         Int32 reminderMinutesBeforeStart
                                        )
    {
        // Ensure client isn't null
            _ = _userClient ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");

        // Ensure new event is not null
            _ = subject ??
                throw new System.NullReferenceException("New Event subject is null");

        try
        {
            var requestEvent = new Event
                {
                    Subject = subject,
                    Body = body,
                    Start = start,
                    End = end,
                    Location = location,
                    Attendees = attendees,
                    Recurrence = recurrence,
                    AllowNewTimeProposals = allowNewTimeProposals,
                    IsAllDay = isAllDay,
                    IsReminderOn = isReminderOn,
                    ReminderMinutesBeforeStart = reminderMinutesBeforeStart
                };
            var result = await _userClient.Me.Events.PostAsync(requestEvent, (requestConfiguration) => 
            {
	           requestConfiguration.Headers.Add("Prefer", "outlook.timezone=\"" + preferredTimeZone + "\"");
            });

            Console.WriteLine("Event created succesfully");
            #pragma warning disable 8602
            Console.WriteLine("Event ID: " + result.Id);
            #pragma warning restore 8602
    
        }
        catch(Exception ex)
        {   
            var innerException = ex.InnerException;
            Console.WriteLine($"Error creating event: {innerException?.Message ?? ex.Message}");
        }
    }

    public async static Task deleteEvent(string eventID)
    {
        // Ensure client isn't null
            _ = _userClient ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");

        // Ensure eventID is not null
            _ = eventID ??
                throw new System.NullReferenceException("New Event subject is null");

        await _userClient.Me.Events[eventID].DeleteAsync();
    }

}