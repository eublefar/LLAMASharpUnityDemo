using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using LLama;
using LLama.Native;
using LLama.Common;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class LLamaSharpTestScript : MonoBehaviour
{
    public string ModelPath = "models/mistral-7b-instruct-v0.1.Q4_K_M.gguf"; // change it to your own model path
    public TMP_Text Output;
    public TMP_InputField Input;
    public Button Submit;

    private string _submittedText = "";

    async UniTaskVoid Start()
    {
        Submit.interactable = false;
        Submit.onClick.AddListener(() =>
        {
            _submittedText = Input.text;
            Input.text = "";
        });
        Output.text = "User: ";
        var bobPrompt = "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Bob.\r\nBob: Hello. How may I help you today?\r\nUser: Please tell me the largest city in Europe.\r\nBob: Sure. The largest city in Europe is Moscow, the capital of Russia.\r\nUser:"; // use the "chat-with-bob" prompt here.
        // Load a model
        var parameters = new ModelParams(Application.streamingAssetsPath + "/" + ModelPath)
        {
            ContextSize = 4096,
            Seed = 1337,
            GpuLayerCount = 35
        };
        // Switch to the thread pool for long-running operations
        await UniTask.SwitchToThreadPool();
        using var model = LLamaWeights.LoadFromFile(parameters);
        await UniTask.SwitchToMainThread();
        // Initialize a chat session
        using var context = model.CreateContext(parameters);
        var ex = new InteractiveExecutor(context);
        ChatSession session = new ChatSession(ex);

        Submit.interactable = true;
        // run the inference in a loop to chat with LLM
        while (bobPrompt != "stop")
        {
            await foreach (var token in ChatConcurrent(
                session.ChatAsync(
                    bobPrompt, 
                    new InferenceParams() 
                    { 
                        Temperature = 0.6f, 
                        AntiPrompts = new List<string> { "User:" } 
                    }
                )
            ))
            {
                Output.text += token;
            }
            await UniTask.WaitUntil(() => _submittedText != "");
            bobPrompt = _submittedText;
            _submittedText = "";
            Output.text += " " + bobPrompt+"\n";
        }
        Submit.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Wraps AsyncEnumerable with transition to the thread pool. 
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns>IAsyncEnumerable computed on a thread pool</returns>
    private async IAsyncEnumerable<string> ChatConcurrent(IAsyncEnumerable<string> tokens)
    {
        await UniTask.SwitchToThreadPool();
        await foreach (var token in tokens)
        {
            yield return token;
        }
    }
}