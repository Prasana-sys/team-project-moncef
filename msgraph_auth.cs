using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Me.SendMail;

class GraphHelper
{
    // Settings object
    private static Settings? _settings;
    // User auth token credential
    private static DeviceCodeCredential? _deviceCodeCredential;
    // Client configured with user authentication
    private static GraphServiceClient? _userClient;

    public static void InitializeGraphForUserAuth(Settings settings,
        Func<DeviceCodeInfo, CancellationToken, Task> deviceCodePrompt)
    {
        _settings = settings;

        var options = new DeviceCodeCredentialOptions
        {
            ClientId = settings.ClientId,
            TenantId = settings.TenantId,
            DeviceCodeCallback = deviceCodePrompt,
        };

        _deviceCodeCredential = new DeviceCodeCredential(options);

        _userClient = new GraphServiceClient(_deviceCodeCredential, settings.GraphUserScopes);
    }

    public static async Task<string> GetUserTokenAsync()
    {
        // Ensure credential isn't null
        _ = _deviceCodeCredential ??
            throw new System.NullReferenceException("Graph has not been initialized for user auth");

        // Ensure scopes isn't null
        _ = _settings?.GraphUserScopes ?? throw new System.ArgumentNullException("Argument 'scopes' cannot be null");

        // Request token with given scopes
        var context = new TokenRequestContext(_settings.GraphUserScopes);
        var response = await _deviceCodeCredential.GetTokenAsync(context);
        return response.Token;
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

    public async static Task CreateEvent(string subject, ItemBody body, DateTimeTimeZone start, DateTimeTimeZone end, Location location)
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
                    // new ItemBody
                    // {
                    //     ContentType = BodyType.Html,
                    //     Content = "Does noon work for you?",
                    // },
                    Start = start,
                    // new DateTimeTimeZone
                    // {
                    //     DateTime = "2017-04-15T12:00:00",
                    //     TimeZone = "Pacific Standard Time",
                    // },
                    End = end,
                    // new DateTimeTimeZone
                    // {
                    //     DateTime = "2017-04-15T14:00:00",
                    //     TimeZone = "Pacific Standard Time",
                    // },
                    Location = location,
                    // new Location
                    // {
                    //     DisplayName = "Harry's Bar",
                    // },
                    // Attendees = attendees,
                    // new List<Attendee>
                    // {
                    //     new Attendee
                    //     {
                    //         EmailAddress = new EmailAddress
                    //         {
                    //             Address = "samanthab@contoso.onmicrosoft.com",
                    //             Name = "Samantha Booth",
                    //         },
                    //         Type = AttendeeType.Required,
                    //     },
                    // },
                    
                    // AllowNewTimeProposals = true,
                    // TransactionId = "7E163156-7762-4BEB-A1C6-729EA81755A7",
                };
            await _userClient.Me.Events.PostAsync(requestEvent);//, (requestConfiguration) => 
            //{
	        //    requestConfiguration.Headers.Add("Prefer", "outlook.timezone=\"Eastern Standard Time\"");
            //}
    
        }
        catch(Exception ex)
        {   
            var innerException = ex.InnerException;
            Console.WriteLine($"Error creating event: {innerException?.Message ?? ex.Message}");
        }
    }

}