using System;
using System.Diagnostics;
using EasyButtons;
using UnityEngine;

namespace Examples.ServiceLocator
{
    public class ServiceLocatorTest : MonoBehaviour
    {
        private void Awake()
        {
            var testService = ServiceLocator.Resolve<ITestService>();
            testService.Hello();
        }
    }
}