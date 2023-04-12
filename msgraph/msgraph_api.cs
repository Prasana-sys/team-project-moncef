using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

public class MSgraph
{
        // public static void InitializeGraph(Settings settings)
        // {
        //     GraphHelper.InitializeGraphForUserAuth(settings,
        //     (info, cancel) =>
        //     {
        //         // Display the device code message to
        //         // the user. This tells them
        //         // where to go to sign in and provides the
        //         // code to use.
        //         Console.WriteLine(info.Message);
        //         return Task.FromResult(0);
        //     });
        // }

        public static async Task InitializeGraph (Settings settings)
        {
            await GraphHelper.InitializeGraphForUserAuth (settings);
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
        // </GreetUserSnippet>

        // // <DisplayAccessTokenSnippet>
        // async Task DisplayAccessTokenAsync()
        // {
        //     try
        //     {
        //         var userToken = await GraphHelper.GetUserTokenAsync();
        //         Console.WriteLine($"User token: {userToken}");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error getting user access token: {ex.Message}");
        //     }
        // }
        // // </DisplayAccessTokenSnippet>

        public static async Task CreateEventAsync(string subject, ItemBody body, DateTimeTimeZone start, DateTimeTimeZone end, 
                                                  Location location, List<Attendee> attendees, PatternedRecurrence recurrence, 
                                                  string preferredTimeZone, bool AllowNewTimeProposals, bool isAllDay, bool isReminderOn, 
                                                  Int32 reminderMinutesBeforeStart
                                                 )
        {   
            try
            {
                //var user = await GraphHelper.GetUserAsync();
                await GraphHelper.CreateEvent(subject, body, start, end, location, attendees, recurrence, preferredTimeZone, AllowNewTimeProposals, 
                                              isAllDay, isReminderOn, reminderMinutesBeforeStart
                                             );
                Console.WriteLine("Event created.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}");
            }
        }
}