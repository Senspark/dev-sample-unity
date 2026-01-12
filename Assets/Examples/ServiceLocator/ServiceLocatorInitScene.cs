using UnityEngine;

namespace Examples.ServiceLocator
{
    public class ServiceLocatorInitScene : MonoBehaviour
    {
        private void Awake()
        {
            var testService = new TestService();
            ServiceLocator.Provide(testService);
            Debug.Log("Service Locator Initialized");
        }
    }
}