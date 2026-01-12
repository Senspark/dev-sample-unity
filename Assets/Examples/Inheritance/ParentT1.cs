using System;
using UnityEngine;

namespace Examples.Inheritance
{
    public class ParentT1 : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Parent T1 Awake");
        }
    }
    
    public class ChildT1A1 : ParentT1
    {
    }
    
    public class ChildT1A2 : ParentT1
    {
        private void Awake()
        {
            Debug.Log("Child T1 A2 Awake");
        }
    }
}
