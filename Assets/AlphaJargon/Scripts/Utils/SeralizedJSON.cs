using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace SeralizedJSONSystem{
    public static class SeralizedJSON<T> where T : ScriptableObject
    {
        // adding JSON Serialization (copy + paste from texticon.cs)
        internal static void LoadFromJSON(string path, out T instance){
            instance = ScriptableObject.CreateInstance<T>();
            JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), instance);
            instance.hideFlags = HideFlags.HideAndDontSave;
        }

        internal static void LoadFromResources(string filename, out T instance){
            instance = (T)Resources.Load(filename);
            instance.hideFlags = HideFlags.HideAndDontSave;
        }
        public static string RemoveFileFromDirectory(string directory) {
            // Split the directory into its parts
            string[] parts = directory.Split(Path.DirectorySeparatorChar);

            // Check if the last part of the directory is a file
            if (Path.HasExtension(parts[parts.Length - 1])) {
                // If it is a file, remove it from the list of parts
                parts = parts.Take(parts.Length - 1).ToArray();
            }

            // Join the parts back together and return the result
            return string.Join(Path.DirectorySeparatorChar.ToString(), parts);
        }

        internal static void SaveToJSON(T obj, string path) {
            if (!Directory.Exists(RemoveFileFromDirectory(path))) 
            {
                // Create the directory if it does not exist
                Directory.CreateDirectory(RemoveFileFromDirectory(path));
            }
            System.IO.File.WriteAllText(path, JsonUtility.ToJson(obj, true));
        }

        // public
        public static void LoadScriptableObject(string filename, out T _instance)
        {
            string jsonPath = System.IO.Path.Combine(Application.persistentDataPath,UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,$"{filename}.json");
            string resourcesPath = "Computer/Icon/" + filename;

            T instance = ScriptableObject.CreateInstance<T>();
            if (System.IO.File.Exists(jsonPath))
            {
                LoadFromJSON(jsonPath, out _instance);
            }
            else
            {
                try
                {
                    Debug.LogError($"Could not load {typeof(T)} from ({jsonPath})\nLoading most recent from {resourcesPath} instead.");
                    LoadFromResources(resourcesPath, out _instance);
                }
                catch(System.Exception e)
                {
                    Debug.LogAssertion($"No {typeof(T)} file found.\n{e.Message}");
                    _instance = null;
                }
            }
        }
        
        public static void SaveScriptableObject(T scriptableObject, string filename) {
            string jsonPath = System.IO.Path.Combine(Application.persistentDataPath,UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,$"{filename}.json");
            SaveToJSON(scriptableObject, jsonPath);
        }
    }
}