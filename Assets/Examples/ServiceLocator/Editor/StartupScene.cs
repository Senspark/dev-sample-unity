using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Examples.ServiceLocator.Editor
{
    /// Tự động chạy trên Editor:
    /// Scene `ServiceLocatorInitScene` sẽ luôn tự khởi động trước và sau đó mới load scene cần thiết.
    /// Nếu không muốn điều đó xảy ra, đặt tên scene có đuôi là `_NoInit`.
    public class StartupScene
    {
        private const string InitScenePath = "Assets/Examples/ServiceLocator/ServiceLocatorInitScene.unity";
        private const string WantedSceneKey = "ServiceLocator_WantedScenePath";

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                var currentScene = SceneManager.GetActiveScene();

                // Skip scenes with "_NoInit" suffix
                if (currentScene.name.EndsWith("_NoInit"))
                {
                    Debug.Log($"Scene '{currentScene.name}' has _NoInit suffix, skipping init scene");
                    return;
                }

                // Only save the current scene if it's not the init scene itself
                if (currentScene.path != InitScenePath)
                {
                    PlayerPrefs.SetString(WantedSceneKey, currentScene.path);
                    PlayerPrefs.Save();
                }

                // Force the init scene to be the play mode start scene
                var initSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(InitScenePath);
                if (initSceneAsset)
                {
                    EditorSceneManager.playModeStartScene = initSceneAsset;
                }
            }
        }
    }
}