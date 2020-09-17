using System;
public static class Log
{
    private static string GetTimestamp()
    {
        return $"{DateTime.Now:d/M/yyyy HH:mm:ss}";
    }

    public static void Info(string message)
    {
        CitizenFX.Core.Debug.WriteLine($"[INFO][{GetTimestamp()}] {message}");
    }
    public static void Warn(string message)
    {
        CitizenFX.Core.Debug.WriteLine($"[WARN][{GetTimestamp()}] {message}");
    }
    public static void Error(Exception message)
    {
        CitizenFX.Core.Debug.WriteLine($"[ERROR][{GetTimestamp()}] {message.Message} - {message.StackTrace}");
    }
    public static void Error(string message)
    {
        CitizenFX.Core.Debug.WriteLine($"[ERROR][{GetTimestamp()}] {message}");
    }
    internal static void Debug(string v)
    {
        CitizenFX.Core.Debug.WriteLine($"[DEBUG][{GetTimestamp()}] {v}");
    }

}