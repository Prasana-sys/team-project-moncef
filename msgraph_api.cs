using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

public class MSgraph
{
        public static void InitializeGraph(Settings settings)
        {
            GraphHelper.InitializeGraphForUserAuth(settings,
            (info, cancel) =>
            {
                // Display the device code message to
                // the user. This tells them
                // where to go to sign in and provides the
                // code to use.
                Console.WriteLine(info.Message);
                return Task.FromResult(0);
            });
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

        public static async Task CreateEventAsync(int test_mode = 0)
        {   
            try
            {
                var user = await GraphHelper.GetUserAsync();
                if (test_mode == 1)
                {
                    var test_itemBody = new ItemBody
                        {
                            ContentType = BodyType.Html,
                            Content = "This is a test.",
                        };
                    var test_start = new DateTimeTimeZone
                        {
                            DateTime = "2023-03-30T12:30:00",
                            TimeZone = "Eastern Standard Time",
                        };
                    var test_end = new DateTimeTimeZone
                        {
                            DateTime = "2023-03-30T13:50:00",
                            TimeZone = "Eastern Standard Time",
                        };
                    var test_Location = new Location
                        {
                            DisplayName = "SWIFT 500",
                        };
                
                    await GraphHelper.CreateEvent("Test", test_itemBody, test_start, test_end, test_Location);
                    Console.WriteLine("Event created.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}");
            }
        }
}