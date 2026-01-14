using System;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using EasyButtons;
using Examples.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Examples.Thread
{
    public class NetworkThreadTest : MonoBehaviour
    {
        private HttpClient _client;

        private void Awake()
        {
            _client = new HttpClient();
        }

        private void OnDestroy()
        {
            _client.Dispose();
        }

        [Button]
        private void StartTest()
        {
            RunUnityWebRequest();
            RunDotNetWebRequest();
        }

        /// Không sử dụng được trên Background Thread
        private void RunUnityWebRequest()
        {
            UniTask.RunOnThreadPool(async () =>
            {
                try
                {
                    var response = await UnityWebRequest.Get("https://google.com").SendWebRequest();
                    SimpleDebug.Log($"Unity WebRequest: {response.responseCode} - Is Background Thread: {System.Threading.Thread.CurrentThread.IsThreadPoolThread}");
                }
                catch (Exception e)
                {
                    SimpleDebug.LogException(e);
                }
            });
        }

        /// Nghe nói không dùng được cho WebGL
        /// Hoạt động được trên Android - iOS trên thử
        private void RunDotNetWebRequest()
        {
            UniTask.RunOnThreadPool(async () =>
            {
                try
                {
                    var response = await _client.GetAsync("https://google.com");
                    SimpleDebug.Log($"DotNet Web Request: {response.StatusCode} - Is Background Thread: {System.Threading.Thread.CurrentThread.IsThreadPoolThread}");
                }
                catch (Exception e)
                {
                    SimpleDebug.LogException(e);
                }
            });
        }
    }
}