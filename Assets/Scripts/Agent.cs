using OllamaSharp;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Agent
{
        #nullable enable
            protected OllamaApiClient ollamaClient;
            protected string? name; 
            protected Chat chat;
            protected Logger? logger;


    /// <summary>
    /// Constructor method for the Agent class.
    /// Creates an instance of an OllamaApiClient with the given input model name and optional context. 
    /// </summary>
    /// <param name="modelName"></param>
    /// <param name="contextFilePath"></param>
    public Agent(string name, string modelName, string contextFilePath = "", string loggerName = "",string ollamaUri = "http://localhost:11434") {

        try
        {
            ollamaClient = new OllamaApiClient(ollamaUri);
            ollamaClient.SelectedModel = modelName;
            chat = new Chat(ollamaClient);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        if (!string.IsNullOrEmpty(loggerName)) { logger = new Logger(name); }
        if (!string.IsNullOrEmpty(name)) { this.name = name; }
        if (!string.IsNullOrEmpty(contextFilePath)) { _ = SendMessageAsyncWithoutResponse(LoadConvoContext(contextFilePath)); }
           
        }

        private string LoadConvoContext(string contextFilePath)
        {
            return File.ReadAllText(contextFilePath);
        }

        /// <summary>
        ///  Starts an asynchronous task that sends a message to the instance model and returns the answer.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task<string> SendMessageAsyncWithResponse(string message, bool doLog = true) {
            if (message == string.Empty) {
                 if (doLog&&logger!=null) {logger.Log("User: [EMPTY MESSAGE]");} 
                return "[NO MESSAGE RECEIVED] ";
            }
            string asw = string.Empty;
            try{
                if (doLog&&logger!=null) {logger.Log("User: "+message);} 
                await foreach (var answerToken in chat.SendAsync(message)) { asw += answerToken; }
                if (doLog&&logger!=null) {logger.Log(name+" [AGENT]: "+asw);} 
                } catch(Exception e) { Debug.LogError(e); }
            return asw;

        }

        /// <summary>
        /// Starts an asynchronous task that sends a message to the instance model and does not return the answer.
        /// This method could be useful for debugging or context-change purposes.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task SendMessageAsyncWithoutResponse(string message, bool doLog = true) {
            string asw = string.Empty;
            try{
                if (doLog&&logger!=null) {logger.Log("User: "+message);} 
                await foreach (var answerToken in chat.SendAsync(message)) { asw += answerToken; }
                if (doLog&&logger!=null) {logger.Log("[HIDDEN] "+name+": "+asw);} 
            } catch(Exception e) { Debug.LogError(e); }
        }
        
}



