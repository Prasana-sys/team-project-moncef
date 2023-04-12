using System;
using Microsoft.Graph.Models;

namespace MonCal
{
    class Program
    {
        static async Task Main(string[] args)
        {   
            Console.WriteLine("Welcome to MonCal CLI");

            /// <Set up for Outlook>
            // Initializes app with client, tenant ID and redirect URI
            var app = Microsoft.Identity.Client.PublicClientApplicationBuilder
                    .Create(Settings.ClientId)
                    .WithTenantId(Settings.TenantId)
                    .WithRedirectUri("urn:ietf:wg:oauth:2.0:oob")
                    .Build();
            var storageProperties = new Microsoft.Identity.Client.Extensions.Msal.StorageCreationPropertiesBuilder(Settings.CacheFileName, Settings.CacheDir).Build();

            // This hooks up the cross-platform cache into MSAL
            var cacheHelper = await Microsoft.Identity.Client.Extensions.Msal.MsalCacheHelper.CreateAsync(storageProperties);
            cacheHelper.RegisterCache(app.UserTokenCache);
            /// <Finish set up for Outlook>
            
            int choice = -1;

            while (choice != 0)
            {
                Console.WriteLine("Please choose one of the following options:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Login into Outlook");
                Console.WriteLine("2. Login into Google");

                try
                {
                    choice = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                catch (System.FormatException)
                {
                    // Set to invalid value
                    choice = -1;
                }

                switch(choice)
                {
                    case 0:
                        // Exit the program
                        Console.WriteLine("Goodbye...");
                        Console.ReadLine();
                        break;
                    case 1:
                        // Perform event creation for outlook
                        
                        // var settings = Settings.LoadSettings();

                        // Initialize Graph
                        await MSgraph.InitializeGraph(app);

                        await MSgraph.GreetUserAsync();

                        int Outlookchoice = -1;

                        while (Outlookchoice != 0)
                        {

                            Console.WriteLine("Please choose one of the following options:");
                            Console.WriteLine("0. Exit");
                            Console.WriteLine("1. Sign out and Exit");
                            Console.WriteLine("2. Add pre-built test event to calendar");
                            Console.WriteLine("3. Add custom test event to calendar");
                            Console.WriteLine("4. Delete event on calendar");
                            Console.WriteLine("5. Edit event on calendar");

                            try
                            {
                                Outlookchoice = int.Parse(Console.ReadLine() ?? string.Empty);
                            }
                            catch (System.FormatException)
                            {
                                // Set to invalid value
                                Outlookchoice = -1;
                            }

                            switch(Outlookchoice)
                            {
                                case 0:
                                    //Exit outlook menu
                                    Console.WriteLine("Exiting Outlook menu...");
                                    Console.ReadLine();
                                    break;

                                case 1:
                                    // get signed in accounts list
                                    var accounts = (await app.GetAccountsAsync()).ToList();

                                    // clear the cache
                                    while (accounts.Any())
                                    {
                                        await app.RemoveAsync(accounts.First());
                                        accounts = (await app.GetAccountsAsync()).ToList();
                                    }

                                    // Set choice to 0 to exit
                                    Outlookchoice = 0;

                                    //Exit outlook menu
                                    Console.WriteLine("Exiting Outlook menu...");
                                    Console.ReadLine();
                                    break;

                                case 2:
                                    // Pre-built event
                                    string test_subject = "Test";
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
                                    var test_Attendees = new List<Attendee>
                                        {
                                            new Attendee
                                            {
                                                EmailAddress = new EmailAddress
                                                {
                                                    Address = "khairyha@mail.uc.edu",
                                                    Name = "Hamza Khairy",
                                                },
                                                Type = AttendeeType.Required,
                                            },
                                        };
                                    var test_Recurrence = new PatternedRecurrence
                                    {
                                        Pattern = new RecurrencePattern
                                        {
                                            Type = RecurrencePatternType.Weekly,
                                            Interval = 1,
                                            DaysOfWeek = new List<DayOfWeekObject?>
                                            {
                                                DayOfWeekObject.Thursday,
                                            },
                                        },
                                        Range = new RecurrenceRange
                                        {
                                            Type = RecurrenceRangeType.EndDate,
                                            StartDate = new Microsoft.Kiota.Abstractions.Date(DateTime.Parse("2023-03-30")),
                                            EndDate = new Microsoft.Kiota.Abstractions.Date(DateTime.Parse("2023-04-28")),
                                        },
                                    };
                                    var test_AllowNewTimeProposals = true;
                                    string test_prefferedTimeZone = "Eastern Standard Time";
                                    bool test_isAllDay = false;
                                    bool test_isReminderOn = true;
                                    Int32 test_reminderMinutesBeforeStart = 15;
                                    await MSgraph.CreateEventAsync(test_subject, test_itemBody, test_start, test_end, test_Location, 
                                                                    test_Attendees, test_Recurrence, test_prefferedTimeZone, test_AllowNewTimeProposals, 
                                                                    test_isAllDay, test_isReminderOn, test_reminderMinutesBeforeStart
                                                                );
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    break;
                            }
                        }
                        break;
                    case 2:
                        // Perform event creation for google
                        


                        break;
                }
            }
        }
    }
}