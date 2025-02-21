using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace AICommand
{
    [System.Serializable]
    public class OllamaResponse
    {
        public string response; // Matches Ollama's /api/generate "response" field
    }

    public class OllamaClient : MonoBehaviour
    {
        private const string BASE_URL = "http://localhost:11434/api/generate"; // Default Ollama URL

        public static async Task<string> InvokeAsync(string prompt)
        {
            // Load model name from settings
            var settings = AICommandSettings.instance;
            if (string.IsNullOrEmpty(settings.model))
            {
                Debug.LogError("Ollama model name not set in AICommand settings.");
                return null;
            }

            // Construct the payload
            string jsonPayload = JsonUtility.ToJson(new
            {
                model = settings.model,
                prompt = prompt,
                stream = false // Adjust based on your needs
            });

            // Send the request
            using (var request = new UnityWebRequest(BASE_URL, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Ollama request failed: {request.error}");
                    return null;
                }

                string responseText = request.downloadHandler.text;
                if (string.IsNullOrEmpty(responseText))
                if (request.result != UnityWebRequest.Result.Success)
{
    Debug.LogError($"Ollama request failed: {request.error}\nURL: {request.url}\nResponse: {request.downloadHandler?.text}");
    return null;
}

                // Parse the response
                var ollamaResponse = JsonUtility.FromJson<OllamaResponse>(responseText);
                return ollamaResponse?.response;
            }
        }
    }
}