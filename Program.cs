using System;
using graph;

namespace MonCal
{
    class Program
    {
        static void Main(string[] args)
        {   
            var settings = Settings.LoadSettings();

            // Initialize Graph
            graph.msgraph.InitializeGraph(settings);
        }
    }
}