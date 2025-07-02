using UnityEngine;

public class ConsoleLogHandler : ILogHandler
{
    private readonly ILogHandler defaultLogHandler;
    private readonly CommandConsole console;

    public ConsoleLogHandler(CommandConsole console)
    {
        this.console = console;
        this.defaultLogHandler = Debug.unityLogger.logHandler;
    }

    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
        // Mantain native functionality
        defaultLogHandler.LogFormat(logType, context, format, args);
        
        // Add to our console
        if (console != null)
        {
            string message = string.Format(format, args);
            string formattedMessage = FormatLogMessage(logType, message);
            console.LogToConsole(formattedMessage, logType);
        }
    }

    public void LogException(System.Exception exception, Object context)
    {
        // Mantain native functionality
        defaultLogHandler.LogException(exception, context);
        
        // Add to our console
        if (console != null)
        {
            string message = $"Exception: {exception.Message}\n{exception.StackTrace}";
            console.LogToConsole(message, LogType.Exception);
        }
    }

    private string FormatLogMessage(LogType logType, string message)
    {
        string prefix = logType switch
        {
            LogType.Error => "[ERROR]",
            LogType.Warning => "[WARNING]",
            LogType.Log => "[INFO]",
            LogType.Exception => "[EXCEPTION]",
            LogType.Assert => "[ASSERT]",
            _ => "[LOG]"
        };

        return $"{prefix} {message}";
    }

    public void RestoreOriginalHandler()
    {
        Debug.unityLogger.logHandler = defaultLogHandler;
    }
}