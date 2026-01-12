using UnityEngine;

namespace Examples.ServiceLocator
{
    [Service(nameof(ITestService))]
    public interface ITestService
    {
        void Hello();
    }
    
    public class TestService : ITestService
    {
        public void Hello()
        {
            Debug.Log("TestService: Hello");
        }
    }
}