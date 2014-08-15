using System;

namespace Server.Infrastructure.Logging
{
    /// <summary>
    ///     Represents a logger that can log events to various different mediums. Useful for debugging and
    ///     tracking information.
    /// </summary>
    public interface ILogger
    {
        void Warn(string message, params Object[] objects);
        void Error(string message, params Object[] objects);
        void Critical(string message, params Object[] objects);
        void Info(string message, params Object[] objects);
        void Debug(string message, params Object[] objects);

        void Trace(string trace, params Object[] objects);
    }
}