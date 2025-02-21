using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace AICommand
{
    public class AICommandWindow : EditorWindow
    {
        private string _prompt = "";
        private string _result = "";
        private bool _isRunning = false;

        [MenuItem("Tools/AI Command")]
        public static void ShowWindow()
        {
            GetWindow<AICommandWindow>("AI Command");
        }

        private async void RunGenerator()
        {
            if (_isRunning) return;
            _isRunning = true;
            _result = "";

            string prompt = _prompt; // Construct your prompt as needed
            _result = await OllamaClient.InvokeAsync(prompt);

            if (string.IsNullOrEmpty(_result))
            {
                _result = "No response received.";
            }

            _isRunning = false;
            Repaint(); // Refresh the editor window
        }

        private void OnGUI()
        {
            _prompt = EditorGUILayout.TextArea(_prompt, GUILayout.Height(100));

            if (GUILayout.Button("Run") && !_isRunning)
            {
                RunGenerator();
            }

            if (!string.IsNullOrEmpty(_result))
            {
                EditorGUILayout.LabelField("Result:", EditorStyles.boldLabel);
                EditorGUILayout.TextArea(_result, GUILayout.ExpandHeight(true));
            }
        }
    }
}