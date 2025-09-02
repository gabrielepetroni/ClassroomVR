using System;
using System.IO;
using UnityEngine;

public class Logger
{
    protected string logFileName = string.Empty;
    protected string loggerPath = string.Empty;

    public Logger(string name)
    {
        logFileName = $"{name}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log";

        try
        {
            string basePath = Path.Combine(Application.persistentDataPath, "Logs");

            if (!Directory.Exists(basePath))
            {
                Debug.Log($"{basePath} does not exist! Creating directory...");
                Directory.CreateDirectory(basePath);
            }
            else
            {
                Debug.Log($"{basePath} exists!");
            }

            loggerPath = Path.Combine(basePath, logFileName);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Cannot initialize logger at path: {loggerPath}. Error: {ex.Message}");
        }
    }

    public void Log(string message, string level = "INFO")
    {
        var log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
        try
        {
            File.AppendAllText(loggerPath, log + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Logger failed to write: {ex.Message}");
        }
    }
}
