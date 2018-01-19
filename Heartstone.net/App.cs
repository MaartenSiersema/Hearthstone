using System;
using System.Diagnostics;
using Akavache;
using Splat;

namespace Heartstone
{
    static public class App
    {

        public const string ApiURL = "https://omgvamp-hearthstone-v1.p.mashape.com";
        public const string ApiKey = "tZu3XRUjPymsh88nOdc7ggY2JmWap1kAgNnjsnEmfXD9UsvTpZ";

        static public void Initialize()
        {
            BlobCache.ApplicationName = "Heartstone";

            Locator.CurrentMutable.RegisterConstant(new DebugLogger(), typeof(ILogger));
        }

        public class DebugLogger : ILogger
        {
            public LogLevel Level { get; set; }

            public void Write(string message, LogLevel logLevel)
            {
                // Using Debug automatically removes logging when not a DEBUG build
                Debug.WriteLineIf(logLevel >= Level, message);
            }
        }

    }
}
