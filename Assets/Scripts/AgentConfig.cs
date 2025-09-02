using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentConfig", menuName = "Agent/Agent Configuration", order = 1)]
public class AgentConfig : ScriptableObject
{
    public string name = "";
    public string modelName = "";
    public string contextFilePath = "";
    public string ollamaUri = ""; // Do not change unless necessary

    public void LogConfig()
    {
        Debug.Log($"Agent Config Loaded:" +
                  $"\n  Name: {name}" +
                  $"\n  Model Name: {modelName}" +
                  $"\n  Conversation Text: {contextFilePath}" +
                  $"\n  Ollama URI: {ollamaUri}");
    }
}
