using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System.Threading.Tasks;
using Oculus.Voice.Dictation;
using Meta.WitAi;
using Meta.WitAi.Json;
using Meta.WitAi.Dictation;
using Meta.WitAi.Events;
using TMPro;


// This AgentManager script is not complete! May cause errors and unexpected behaviour.
// Only use with supported and tested Agents.

[System.Serializable] public class AgentManager : MonoBehaviour
{
    [Header("Agents configurations")]
    public List<AgentConfig> agentsConfig = new List<AgentConfig>();
    [Header("Categorizer configuration")]
    public AgentConfig categorizerConfig;
    [Header("TTS Agent")]
    public TTSSpeaker ttsSpeaker;

    [Header("Meta Voice Experience")]
    public AppDictationExperience dictationExperience;
    public TextMeshPro dictationOutput;
    public TextMeshPro playerEmotions;

    private EmotionData acumulado = new EmotionData();
    private int totalMensajes = 0;

    [System.Serializable]
    public class EmotionData
    {
        public float felicidad;
        public float tristeza;
        public float miedo;
        public float ira;
        public float asco;
        public float sorpresa;
    }

   

    private List<Agent> activeAgents = new List<Agent>();
    private Agent categorizerAgent;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" - STARTING AGENT MANAGER - ");
        Debug.Log("persistentDataPath: " + Application.persistentDataPath);


        Debug.Log("> Instatiating agents from configuration... ");
        foreach (var agentCfg in agentsConfig)
        {
            try
            {
                activeAgents.Add(new Agent(agentCfg.name, agentCfg.modelName, agentCfg.contextFilePath, agentCfg.ollamaUri));
                Debug.Log("Successfully initialized Agent: " + agentCfg.name + " with model: "+ agentCfg.modelName);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        Debug.Log("> Instatiating categorizer from configuration... ");
        try
        {
            categorizerAgent = new Agent(categorizerConfig.name, categorizerConfig.modelName, categorizerConfig.contextFilePath, categorizerConfig.ollamaUri);
            Debug.Log("Successfully initialized Agent: " + categorizerConfig.name + " with model: " + categorizerConfig.modelName);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        Debug.Log("> Checking TTSSpeaker object reference... ");
        if (ttsSpeaker == null)
            Debug.LogError("Assign TTSSpeaker in inspector!");
        else
        {
            ttsSpeaker.Speak("Comienzo de la simulacion");
        }

        if(dictationExperience != null)
        {
            dictationExperience.DictationEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
            dictationExperience.DictationEvents.OnFullTranscription.AddListener(OnFullTranscription);
            dictationExperience.DictationEvents.OnStartListening.AddListener(OnStartListening);
            dictationExperience.DictationEvents.OnStoppedListening.AddListener(OnStoppedListening);
        }
    }

    public void StartDictation()
    {
        if (!dictationExperience.Active)
        {
            Debug.Log("Dictation started.");
            dictationExperience.Activate();
        }
    }

    public void StopDictation()
    {
        if (dictationExperience.Active)
        {
            Debug.Log("Dictation stopped.");
            dictationExperience.Deactivate();
        }
    }

    private async void OnFullTranscription(string text)
    {
        Debug.Log("OnFullTranscription called: " + text);
        if (dictationOutput) dictationOutput.text = text;

        try
        {
            // Ejecutar ambas tareas en paralelo
            var responseTask = activeAgents[0].SendMessageAsyncWithResponse(text);
            var categorizationTask = categorizerAgent.SendMessageAsyncWithResponse(text);

            // Esperar ambas respuestas
            await Task.WhenAll(responseTask, categorizationTask);

            string response = responseTask.Result;
            string categorization = categorizationTask.Result;

            Debug.Log("Response received!");

            ttsSpeaker.Speak(response); // Text-to-speech en hilo principal
            Debug.Log("Categorization JSON: " + categorization);
            AddNewEmotionEntry(categorization);
        }
        catch (Exception ex)
        {
            Debug.LogError("Ollama request failed: " + ex.Message);
        }
    }

    public void AddNewEmotionEntry(string json)
    {
        EmotionData nueva = JsonConvert.DeserializeObject<EmotionData>(json);
        totalMensajes++;

        // Actualizar promedios acumulados
        acumulado.felicidad = ((acumulado.felicidad * (totalMensajes - 1)) + nueva.felicidad) / totalMensajes;
        acumulado.tristeza = ((acumulado.tristeza * (totalMensajes - 1)) + nueva.tristeza) / totalMensajes;
        acumulado.miedo = ((acumulado.miedo * (totalMensajes - 1)) + nueva.miedo) / totalMensajes;
        acumulado.ira = ((acumulado.ira * (totalMensajes - 1)) + nueva.ira) / totalMensajes;
        acumulado.asco = ((acumulado.asco * (totalMensajes - 1)) + nueva.asco) / totalMensajes;
        acumulado.sorpresa = ((acumulado.sorpresa * (totalMensajes - 1)) + nueva.sorpresa) / totalMensajes;

        UpdateEmotions();
    }

    private void UpdateEmotions()
    {
        playerEmotions.text =
            $"<b>Promedio emocional</b>\n" +
            $"Felicidad: {acumulado.felicidad:F2}\n" +
            $"Tristeza: {acumulado.tristeza:F2}\n" +
            $"Miedo: {acumulado.miedo:F2}\n" +
            $"Ira: {acumulado.ira:F2}\n" +
            $"Asco: {acumulado.asco:F2}\n" +
            $"Sorpresa: {acumulado.sorpresa:F2}";
    }

    private void OnPartialTranscription(string text)
    {
        Debug.Log("Partial Transcription: " + text);
        if (dictationOutput) dictationOutput.text = text;
    }

    private void OnStartListening()
    {
        Debug.Log("Dictation Listening...");
    }

    private void OnStoppedListening()
    {
        Debug.Log("Dictation Stopped Listening.");
    }

    private void OnDictationComplete(string text)
    {
        Debug.Log("Dictation Complete: " + text);
    }

    private void OnDictationError(string error, string message)
    {
        Debug.LogError($"Dictation Error: {error} - {message}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
