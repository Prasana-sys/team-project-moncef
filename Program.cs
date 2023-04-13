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
                        Console.WriteLine("Outlook Selected");

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
                                    // string test_prefferedTimeZone = "Eastern Standard Time";
                                    bool test_isAllDay = false;
                                    bool test_isReminderOn = true;
                                    Int32 test_reminderMinutesBeforeStart = 15;
                                    await MSgraph.CreateEventAsync(test_subject, test_start, test_end,  test_itemBody, test_Location, 
                                                                    test_Attendees, test_Recurrence, test_AllowNewTimeProposals, 
                                                                    test_isAllDay, test_isReminderOn, test_reminderMinutesBeforeStart
                                                                );
                                    break;
                                case 3:
                                    //create custom event
                                    var custom_subject = "Untitled";
                                    var custom_start = new DateTimeTimeZone
                                        {
                                            DateTime = "2023-04-13T12:30:00",
                                            TimeZone = "Eastern Standard Time",
                                        };
                                    var custom_end = new DateTimeTimeZone
                                        {
                                            DateTime = "2023-04-13T13:50:00",
                                            TimeZone = "Eastern Standard Time",
                                        };

                                    Console.WriteLine("Type in Event's name (Can leave empty, if left empty \"Test\" is default)");
                                    custom_subject = Console.ReadLine() ?? "Test";
                                    if (custom_subject == "") 
                                        custom_subject = "Test";

                                    Console.WriteLine("Type in time zone (Ex: \"Eastern Standard Time\", EST is default)");
                                    var timeZoneResponse = Console.ReadLine();
                                    var listSupportedTimeZones = await MSgraph.GetSupportedTimeZonesAsync(); // gets list of supported time zones on user's mailbox server
                                    int timeZoneFlag = 0;
                                    var emptyTimeZonesResponse = new Microsoft.Graph.Me.Outlook.SupportedTimeZones.SupportedTimeZonesResponse();
                                    emptyTimeZonesResponse.Value = new List<TimeZoneInformation>();
                                    foreach (var timeZone in listSupportedTimeZones.Value ?? emptyTimeZonesResponse.Value)
                                    {
                                        if (timeZone.Alias == timeZoneResponse){
                                            timeZoneFlag = 1;
                                            break;
                                        }
                                    }
                                    if (timeZoneFlag == 0)
                                    {
                                        Console.WriteLine("Cannot recognize Time Zone, will use default value EST");
                                        timeZoneResponse = "Eastern Standard Time";
                                    }
                                    custom_start.TimeZone = timeZoneResponse ?? "Eastern Standard Time";
                                    custom_end.TimeZone = timeZoneResponse ?? "Eastern Standard Time";

                                    Console.WriteLine("Type in start time (format: yyyy-mm-ddThh:mm:ss)");
                                    var startTimeResponse = Console.ReadLine();
                                    // var timeFormat = @"^\d{4}(-\d{2}){2}T\d{2}(:\d{2}){2}";
                                    // if (System.Text.RegularExpressions.Regex.IsMatch(startTimeResponse, timeFormat))
                                    // {

                                    // }
                                    if (startTimeResponse == "" || startTimeResponse == null)
                                    {
                                        Console.WriteLine("Empty or null, will use \"2023-04-13T12:30:00\"");
                                        custom_start.DateTime = "2023-04-13T12:30:00";
                                    }
                                    else
                                    {
                                        custom_start.DateTime = startTimeResponse;
                                    }

                                    Console.WriteLine("Type in end time (format: yyyy-mm-ddThh:mm:ss)");
                                    var endTimeResponse = Console.ReadLine();
                                    if (endTimeResponse == "" || endTimeResponse == null)
                                    {
                                        Console.WriteLine("Empty or null, will use \"2023-04-13T13:50:00\"");
                                        custom_end.DateTime = "2023-04-13T13:50:00";
                                    }
                                    else
                                    {
                                        custom_end.DateTime = endTimeResponse;
                                    }
                                    
                                    await GraphHelper.CreateCustomtestEventAsync(custom_subject, custom_start, custom_end);
                                    break;
                                case 4:
                                    //Delete Event
                                    Console.WriteLine("Provide event ID of event to delete: ");
                                    var delEventID = Console.ReadLine();
                                    if (delEventID == null || delEventID == "")
                                    {
                                        Console.WriteLine("Event ID empty or null, aborting...");
                                        break;
                                    }
                                    await MSgraph.DeleteEventAsync(delEventID);

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