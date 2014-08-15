using System.Collections.Generic;

namespace Server.Infrastructure.Logging
{
    /// <summary>
    ///     A master logger which is created and
    /// </summary>
    public class Logger : ILogger
    {
        private static Logger _instance;
        private readonly List<ILogger> _loggers = new List<ILogger>();

        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Logger();
                return _instance;
            }
        }

        public void Warn(string message, params object[] objects)
        {
            foreach (ILogger logger in _loggers)
                logger.Warn(message, objects);
        }

        public void Error(string message, params object[] objects)
        {
            foreach (ILogger logger in _loggers)
                logger.Error(message, objects);
        }

        public void Critical(string message, params object[] objects)
        {
            foreach (ILogger logger in _loggers)
                logger.Critical(message, objects);
        }

        public void Info(string message, params object[] objects)
        {
            foreach (ILogger logger in _loggers)
                logger.Info(message, objects);
        }


        public void Debug(string message, params object[] objects)
        {
            foreach (var logger in _loggers)
                logger.Debug(message, objects);
        }

        public void Trace(string message, params object[] objects)
        {
            _loggers.ForEach(x => x.Trace(message, objects));
        }

        public void AddLogger(ILogger logger)
        {
            _loggers.Add(logger);
        }
    }
}