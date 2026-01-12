using EasyButtons;
using UnityEngine;

namespace Examples.Inheritance
{
    public class InheritanceManager : MonoBehaviour
    {
        // ================================================================
        // ================================================================

        [Button]
        public void AddParentT1()
        {
            var go = new GameObject("Parent T1");
            go.AddComponent<ParentT1>();
        }

        [Button]
        public void AddChildT1A1()
        {
            var go = new GameObject("Child T1 - A1");
            go.AddComponent<ChildT1A1>();
        }

        [Button]
        public void AddChildT1A2()
        {
            var go = new GameObject("Child T2 - A2");
            go.AddComponent<ChildT1A2>();
        }

        // ================================================================
        [Button("===============")]
        public void Separate2()
        {
        }
        // ================================================================

        [Button]
        public void AddParentT2()
        {
            var go = new GameObject("Parent T2");
            go.AddComponent<ParentT2>();
        }

        [Button]
        public void AddChildT2A1()
        {
            var go = new GameObject("Child T2 - A1");
            go.AddComponent<ChildT2A1>();
        }

        [Button]
        public void AddChildT2A2()
        {
            var go = new GameObject("Child T2 - A2");
            go.AddComponent<ChildT2A2>();
        }
        
        [Button]
        public void AddChildT2A3()
        {
            var go = new GameObject("Child T2 - A3");
            go.AddComponent<ChildT2A3>();
        }

        // ================================================================
        [Button("===============")]
        public void Separate3()
        {
        }
        // ================================================================
        
        [Button]
        public void AddParentT3()
        {
            var go = new GameObject("Parent T3");
            go.AddComponent<ParentT3>();
        }

        [Button]
        public void AddChildT3A1()
        {
            var go = new GameObject("Child T3 - A1");
            go.AddComponent<ChildT3A1>();
        }
    }
}