using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Me.SendMail;
using Microsoft.Identity.Client;

class GraphHelper
{
    // Settings object
    private static Settings? _settings;
    
    // Client configured with user authentication
    private static GraphServiceClient? _userClient;
    
    private static InteractiveBrowserCredential? _interactiveBrowserCredential;

    private static AuthenticationRecord? _authRecord;

    public static void InitializeGraphForUserAuth (Settings settings)
    {   
        // string filePath = @"authRecord.json";

        _settings = settings;

        // var app = PublicClientApplicationBuilder.Create(_settings.ClientId).Build();
        // var accounts = await app.GetAccountsAsync();
        // AuthenticationResult result;
        // try
        // {
        // result = await app.AcquireTokenSilent(_settings.GraphUserScopes, accounts.FirstOrDefault())
        //             .ExecuteAsync();
        // }
        // catch(MsalUiRequiredException)
        // {
        // result = await app.AcquireTokenInteractive(_settings.GraphUserScopes)
        //             .ExecuteAsync();
        // }

        try
        {
            // Read (deserialize from existing json file containing Authentication Record if it exists)
            Exception ex = new Exception("test");
            throw ex;

            // using (FileStream fs = File.OpenRead(filePath))
            // {
            //     _authRecord = AuthenticationRecord.Deserialize (fs);
            // }

            // var token = _interactiveBrowserCredential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://graph.microsoft.com/.default" }));

            // _userClient = new GraphServiceClient(_interactiveBrowserCredential, _settings.GraphUserScopes);

        }
        catch (Exception ex)
        {   
            // Else interactively authenticate user via default web browser and store (serialize) auth record to json file

            var innerException = ex.InnerException;
            System.Diagnostics.Debug.WriteLine($"User not initialized previously: {innerException?.Message ?? ex.Message}");

            //_settings = settings;

            var _interactiveBrowserCredentialOptions = new InteractiveBrowserCredentialOptions
            {
                ClientId = settings.ClientId,
                TenantId = settings.TenantId,
                // AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                DisableAutomaticAuthentication = false,
                // MUST be http://localhost or http://localhost:PORT
                // See https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core
                // RedirectUri = new Uri("http://localhost"),
            };

            // https://docs.microsoft.com/dotnet/api/azure.identity.interactivebrowsercredential
            _interactiveBrowserCredential = new InteractiveBrowserCredential(_interactiveBrowserCredentialOptions);
            // Task<AuthenticationRecord> authRecordTask = _interactiveBrowserCredential.AuthenticateAsync ();

            // //Get access token
            // var token = _interactiveBrowserCredential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://graph.microsoft.com/.default" }));

            // Console.WriteLine("Access token: " + token.Token);

            // Console.WriteLine("Expires On: " + token.ExpiresOn);

            //Get graph client based on interactiveCredential and scope.
            _userClient = new GraphServiceClient(_interactiveBrowserCredential, settings.GraphUserScopes);

            // _authRecord = await authRecordTask;
            
            // using (FileStream fs = File.Create(filePath))
            // {
            //     _authRecord.Serialize (fs);
            // }

        }
    }

    // public static async Task<string> GetUserTokenAsync()
    // {
    //     // Ensure credential isn't null
    //     _ = _deviceCodeCredential ??
    //         throw new System.NullReferenceException("Graph has not been initialized for user auth");

    //     // Ensure scopes isn't null
    //     _ = _settings?.GraphUserScopes ?? throw new System.ArgumentNullException("Argument 'scopes' cannot be null");

    //     // Request token with given scopes
    //     var context = new TokenRequestContext(_settings.GraphUserScopes);
    //     var response = await _deviceCodeCredential.GetTokenAsync(context);
    //     return response.Token;
    // }

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
            await _userClient.Me.Events.PostAsync(requestEvent, (requestConfiguration) => 
            {
	           requestConfiguration.Headers.Add("Prefer", "outlook.timezone=\"" + preferredTimeZone + "\"");
            });
    
        }
        catch(Exception ex)
        {   
            var innerException = ex.InnerException;
            Console.WriteLine($"Error creating event: {innerException?.Message ?? ex.Message}");
        }
    }

}