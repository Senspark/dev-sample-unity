using Cysharp.Threading.Tasks;
using Examples.ServiceLocator.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Examples.ServiceLocator
{
    public class ServiceLocatorInitScene : MonoBehaviour
    {
        private const string WantedSceneKey = "ServiceLocator_WantedScenePath";

        private async void Awake()
        {
            var testService = new TestService();
            ServiceLocator.Provide(testService);

            var scheduler = new SchedulerService();
            scheduler.Initialize();
            ServiceLocator.Provide(scheduler);
            
            Debug.Log("Service Locator Initialized");

            await LoadWantedSceneAsync();
        }

        private async UniTask LoadWantedSceneAsync()
        {
            if (!PlayerPrefs.HasKey(WantedSceneKey))
            {
                Debug.Log("No wanted scene found, staying in init scene");
                return;
            }

            var wantedScenePath = PlayerPrefs.GetString(WantedSceneKey);
            PlayerPrefs.DeleteKey(WantedSceneKey);
            PlayerPrefs.Save();

            if (string.IsNullOrEmpty(wantedScenePath))
            {
                Debug.LogWarning("Wanted scene path is empty");
                return;
            }

            await SceneManager.LoadSceneAsync(wantedScenePath);

            AutoExpandSceneHierarchy(wantedScenePath);
        }

        private static void AutoExpandSceneHierarchy(string wantedScenePath)
        {
#if UNITY_EDITOR
            // Trick: Để giữ cho Scene Hierarchy được expand: select game object đầu tiên của scene đó
            var loadedScene = SceneManager.GetSceneByPath(wantedScenePath);
            if (loadedScene.isLoaded)
            {
                var rootObjects = loadedScene.GetRootGameObjects();
                if (rootObjects.Length > 0)
                {
                    UnityEditor.Selection.activeGameObject = rootObjects[0];
                    UnityEditor.EditorGUIUtility.PingObject(rootObjects[0]);
                }
            }
#endif
        }
    }
}