using UnityEngine;

namespace Examples.Inheritance
{
    public class ParentT2 : MonoBehaviour
    {
        protected void Awake()
        {
            Debug.Log("Parent T2 Awake");
        }
    }
    
    public class ChildT2A1 : ParentT2
    {
        private void Awake()
        {
            Debug.Log("Child T2 A1 Awake");
        }
    }
    
    public class ChildT2A2 : ParentT2
    {
        protected void Awake()
        {
            Debug.Log("Parent T2 A2 Awake");
        }
    }
    
    public class ChildT2A3 : ParentT2
    {
        protected new void Awake()
        {
            Debug.Log("Parent T2 A3 Awake");
        }
    }
}