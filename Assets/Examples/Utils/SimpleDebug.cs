using System;
using UnityEngine;

namespace Examples.Utils
{
    public static class SimpleDebug
    {
        public static void Log(string message)
        {
            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
        }

        public static void Error(string message)
        {
            Debug.LogError(message);
        }

        public static void LogError(Exception exception)
        {
            Debug.LogError(exception);
        }
        
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}