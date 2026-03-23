using UnityEngine;

namespace Examples.AIAgent
{
    public class AiAgentTestBug
    {
        public void LogBug()
        {
            var a = 0;
            var b = 1;

            var c = b / a;
            Debug.Log($"{a}, {b}, {c}");
        }
    }
}