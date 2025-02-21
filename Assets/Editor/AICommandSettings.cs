using UnityEngine;
using UnityEditor;
namespace AICommand
{
    public class AICommandSettings : ScriptableObject
    {
        public string model = "deepseek-coder-v2:16b"; // Default model name, adjust as needed

        private static AICommandSettings _instance;

        public static AICommandSettings instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<AICommandSettings>("AICommandSettings");
                    if (_instance == null)
                    {
                        _instance = CreateInstance<AICommandSettings>();
                        AssetDatabase.CreateAsset(_instance, "Assets/Resources/AICommandSettings.asset");
                        AssetDatabase.SaveAssets();
                    }
                }
                return _instance;
            }
        }
    }
}