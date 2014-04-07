using System;

namespace ConfigBuddy.Core
{
    public enum LogLevel
    {
        Debug,
        Warning,
        Error
    }

    public static class Logger
    {
        static Logger()
        {
            LogAction = (msg, level) => { };
        }

        public static void Error(string msg, params object[] args)
        {
            Log(LogLevel.Error, msg, args);
        }

        public static void Warning(string msg, params object[] args)
        {
            Log(LogLevel.Warning, msg, args);
        }

        public static void Debug(string msg, params object[] args)
        {
            Log(LogLevel.Debug, msg, args);
        }

        public static void Log(LogLevel level, string msg, params object[] args)
        {
            LogAction(String.Format(msg, args), level);
        }

        public static Action<string, LogLevel> LogAction { get; set; }
    }
}