// using mstest = Microsoft.VisualStudio.TestTools.UnitTesting;

// namespace test_msgraph
// {
//     [mstest.TestClass]
//     public class UnitTest1
//     {
//         [mstest.TestMethod]
//         public void TestMethod1()
//         {
//         }
//     }
// }

using Microsoft.Graph;
using Microsoft.Graph.Models;
using NUnit.Framework;

namespace MonCal.Tests
{
    [TestFixture]
    public class CalendarTests
    {
        private GraphServiceClient _graphClient;

        [SetUp]
        public async void Setup()
        {   
            var app = Microsoft.Identity.Client.PublicClientApplicationBuilder
                    .Create(Settings.ClientId)
                    .WithTenantId(Settings.TenantId)
                    .WithRedirectUri("urn:ietf:wg:oauth:2.0:oob")
                    .Build();
            await MSgraph.InitializeGraph (app);
        }

        [Test]
        public async Task CreateEvent_AddsEventToCalendar()
        {
            // Arrange
            var startDateTime = DateTimeOffset.UtcNow.AddDays(1);
            var endDateTime = startDateTime.AddHours(1);
            var newEvent = new Event()
            {
                Subject = "Test Event",
                Start = new DateTimeTimeZone()
                {
                    DateTime = startDateTime.ToString("o"),
                    TimeZone = TimeZoneInfo.Local.Id
                },
                End = new DateTimeTimeZone()
                {
                    DateTime = endDateTime.ToString("o"),
                    TimeZone = TimeZoneInfo.Local.Id
                }
            };

            // Act
            var createdEvent = await _graphClient.Me.Events.PostAsync(newEvent);

            // Assert
            Assert.NotNull(createdEvent.Id);
            Assert.AreEqual("Test Event", createdEvent.Subject);
            Assert.AreEqual(startDateTime, createdEvent.Start.DateTime);
            Assert.AreEqual(endDateTime, createdEvent.End.DateTime);
        }
    }
}
