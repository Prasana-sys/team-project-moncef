using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using System.Security.Cryptography;

public class MSgraph
{
    public static async Task InitializeGraph (IPublicClientApplication app)
    {
        await GraphHelper.InitializeGraphForUserAuth (app);
    }

    // <GreetUserSnippet>
    public static async Task GreetUserAsync()
    {
        try
        {
            var user = await GraphHelper.GetUserAsync();
            Console.WriteLine($"Hello, {user?.DisplayName}!");
            // For Work/school accounts, email is in Mail property
            // Personal accounts, email is in UserPrincipalName
            Console.WriteLine($"Email: {user?.Mail ?? user?.UserPrincipalName ?? ""}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user: {ex.Message}");
        }
    }

    public static async Task CreateEventAsync(string subject, DateTimeTimeZone start, DateTimeTimeZone end, ItemBody body, 
                                              Location location, List<Attendee>? attendees, PatternedRecurrence? recurrence, 
                                              bool AllowNewTimeProposals, bool isAllDay, 
                                              bool isReminderOn, Int32 reminderMinutesBeforeStart
                                             )
    {   
        try
        {   
            await GraphHelper.CreateEvent(subject, start, end, body, location, attendees, recurrence, AllowNewTimeProposals, 
                                          isAllDay, isReminderOn, reminderMinutesBeforeStart
                                         );
            Console.WriteLine("Event created.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error creating event: {ex.Message}");
        }
    }

    public static async Task DeleteEventAsync(string eventID)
    {
        try
        {
            await GraphHelper.deleteEvent(eventID);
            Console.WriteLine("Event deleted.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error deleting event: {ex.Message}");
        }
    }

    public static async Task<Microsoft.Graph.Me.Outlook.SupportedTimeZones.SupportedTimeZonesResponse> GetSupportedTimeZonesAsync ()
    {
        try
        {
            var result = await GraphHelper.getSupportedTimeZones();
            return result;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error getting timezones: {ex.Message}");
            var result = new Microsoft.Graph.Me.Outlook.SupportedTimeZones.SupportedTimeZonesResponse ();
            return result;
        }
    }
}