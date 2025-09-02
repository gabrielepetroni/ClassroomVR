#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System;
using System.Linq;
using System.Speech.Synthesis;
#endif

using UnityEngine;

public class TTSManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private SpeechSynthesizer synthesizer;
#endif

    void Start()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        try
        {
            Debug.Log("[TTS] Creating SpeechSynthesizer...");
            synthesizer = new SpeechSynthesizer();

            if (synthesizer == null)
            {
                Debug.LogError("[TTS] Synthesizer is null after creation.");
                return;
            }

            Debug.Log("[TTS] Fetching installed voices...");
            var voices = synthesizer.GetInstalledVoices();
            if (voices == null || voices.Count == 0)
            {
                Debug.LogError("[TTS] No installed voices found.");
                return;
            }

            // Filter out null or invalid voices
            var validVoices = voices.Where(v => v != null && v.VoiceInfo != null).ToList(); 

            if (validVoices.Count == 0)
            {
                Debug.LogError("[TTS] All voices are invalid or missing VoiceInfo.");
                return;
            }

            foreach (var voice in validVoices)
            {
                var info = voice.VoiceInfo;
                Debug.Log($"[TTS] Voice: {info.Name} | Culture: {info.Culture} | Gender: {info.Gender}");
            }


            Debug.Log("[TTS] Selecting voice...");
            synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);

            Debug.Log("[TTS] Speaking sample...");
            Speak("Hola! Me llamo Ana, encantada de conocerte.");
        }
        catch (Exception ex)
        {
            Debug.LogError("[TTS] Initialization failed: " + ex);
        }
#endif
    }



    public void Speak(string text)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (synthesizer == null)
        {
            Debug.LogWarning("[TTS] Synthesizer not initialized.");
            return;
        }

        try
        {
            synthesizer.SpeakAsyncCancelAll();
            synthesizer.SpeakAsync(text);
        }
        catch (Exception ex)
        {
            Debug.LogError("[TTS] Speak failed: " + ex.Message);
        }
#endif
    }

    private void OnDestroy()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (synthesizer != null)
        {
            synthesizer.SpeakAsyncCancelAll();
            synthesizer.Dispose();
            synthesizer = null;
        }
#endif
    }
}
