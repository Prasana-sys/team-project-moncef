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
                    await MSgraph.CreateEventAsync(test_mode);
                }
            }
        }
    }
}