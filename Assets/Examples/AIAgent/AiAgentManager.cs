using System;
using System.IO;
using UnityEngine;

namespace Examples.AIAgent
{
    public class AiAgentManager : MonoBehaviour
    {
        [SerializeField] private AiAgentUseUI aiUI;

        private FileLogWriter _logWriter;

        private void Awake()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "game.log");
            _logWriter = new FileLogWriter(filePath);
            Application.logMessageReceivedThreaded += _logWriter.WriteLog;
            Debug.Log($"Log file: {filePath}");
        }

        private void Start()
        {
            Debug.Log($"Function Started");
            var t = new AiAgentTestBug();
            t.LogBug();
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= _logWriter.WriteLog;
            _logWriter.Dispose();
        }
    }
}