using System;
using System.Collections.Generic;

namespace Server.Infrastructure.Logging.Loggers
{
    /// <summary>
    ///     A generic logging class that allows printing to the standard console.
    ///     This is useful for debugging information at a glance while developing.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public static LogLevel Level = LogLevel.Debug;

        private Dictionary<LogLevel, ConsoleColor> _colors = new Dictionary<LogLevel, ConsoleColor>()
            {
                {LogLevel.Info, ConsoleColor.Gray},
                {LogLevel.Warn, ConsoleColor.Yellow},
                {LogLevel.Debug, ConsoleColor.White},
                {LogLevel.Error, ConsoleColor.Red},
                {LogLevel.Critical, ConsoleColor.Red}
            };

        public void Warn(string message, params Object[] objects)
        {
            LogAction(LogLevel.Warn, message, objects);
        }

        public void Error(string message, params Object[] objects)
        {
            LogAction(LogLevel.Error, message, objects);
        }

        public void Critical(string message, params object[] objects)
        {
            LogAction(LogLevel.Critical, message, objects);
        }

        public void Debug(string message, params Object[] objects)
        {
            LogAction(LogLevel.Debug, message, objects);
        }

        public void Info(string message, params Object[] objects)
        {
            LogAction(LogLevel.Info, message, objects);
        }

        private void LogAction(LogLevel level, string message, params Object[] objects)
        {
            if (level >= Level)
            {
                var previousColor = Console.ForegroundColor;
                Console.ForegroundColor = _colors[level];
                Console.WriteLine("{0} [{1}] {2}", DateTime.Now, level, string.Format(message, objects));
                Console.ForegroundColor = previousColor;
            }
        }
    }
}