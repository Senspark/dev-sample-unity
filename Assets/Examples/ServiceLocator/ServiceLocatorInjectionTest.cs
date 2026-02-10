using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Examples.ServiceLocator
{
    public class ServiceLocatorInjectionTest : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Provide(new TestService1());
            ServiceLocator.Provide(new TestService2());
            this.AddComponent<MonoDerived1>();
            this.AddComponent<MonoDerived2>();
        }
    }
    
    // ====================================================
    
    public class MonoBase2 : MonoBehaviour
    {
        [Inject] private ITestService1 _testService1;
        
        private void Start()
        {
            // Base ko cần ResolveInjection
            // Children đã ResolveInjection ở Awake rồi
            // Tuy flow này bị ngược - không được viết như vậy
            // - nhưng vẫn ví dụ để chứng minh là nó vẫn worked
            _testService1.Print(nameof(MonoBase2));
        }
    }

    public class MonoDerived2 : MonoBase2
    {
        [Inject] private ITestService2 _testService2;

        private void Awake()
        {
            ServiceLocator.ResolveInjection(this);
            _testService2.Print(nameof(MonoDerived2));
        }
    }
    
    // ====================================================

    public class MonoBase1 : MonoBehaviour
    {
        [Inject] private ITestService1 _testService1;
        
        protected virtual void Awake()
        {
            ServiceLocator.ResolveInjection(this);
            _testService1.Print(nameof(MonoBase1));
        }
    }

    public class MonoDerived1 : MonoBase1
    {
        [Inject] private ITestService2 _testService2;

        private void Start()
        {
            // Children ko cần ResolveInjection nữa
            // Base đã ResolveInjection ở Awake rồi
            
            _testService2.Print(nameof(MonoDerived1));
        }
    }

    // ====================================================

    [Service(nameof(ITestService1))]
    public interface ITestService1
    {
        void Print(string msg);
    }

    [Service(nameof(ITestService2))]
    public interface ITestService2
    {
        void Print(string msg);
    }

    public class TestService1 : ITestService1
    {
        public void Print(string msg)
        {
            Debug.Log($"TestService1: {msg}");
        }
    }

    public class TestService2 : ITestService2
    {
        public void Print(string msg)
        {
            Debug.Log($"TestService2: {msg}");
        }
    }
}