using System;
using Microsoft.Graph.Models;

namespace MonCal
{
    class Program
    {
        static async Task Main(string[] args)
        {   var test_mode = 1; // 1 - MS Graph testing, 2 - GUI testing, 3 - ....

            if (test_mode == 1)
            {
                var settings = Settings.LoadSettings();

                // Initialize Graph
                MSgraph.InitializeGraph(settings);

                await MSgraph.GreetUserAsync();

                Console.WriteLine ("Press 1 to add test event to calendar.");

                if ("1" == Console.ReadLine())
                {   
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

                    Console.WriteLine("Press enter to close.");
                    Console.ReadLine();
                }
            }
        }
    }
}