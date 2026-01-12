using UnityEngine;

namespace Examples.Inheritance
{
    public class ParentT3 : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Hello();
        }

        private void Hello()
        {
            Debug.Log("Parent T3 Hello");
        }
    }
    
    public class ChildT3A1 : ParentT3
    {
        protected override void Awake()
        {
            base.Awake();
            Hello();
        }
        
        private void Hello()
        {
            Debug.Log("Child T3 A1 Hello");
        }
    }
}