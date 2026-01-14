using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Examples.Utils;
using UnityEngine;

namespace Examples.Thread
{
    /// Test background thread
    /// Mỗi giây Unity sẽ Print() 1 lần
    /// Run trên Mobile & exit App thì sẽ thấy Unity không còn Print() nữa
    /// Sau khi resume App, nếu giá trị của Print() đã thay đổi nhiều thì chứng minh rằng Background Thread vẫn hoạt động dưới nền 
    public class ThreadScene : MonoBehaviour
    {
        private float _unityTimer;
        private bool _exited;
        private const int Interval = 1000;
        
        // ---

        private int _unityCounter;
        private int _uniTaskCounter;
        private int _dotnetCounter;

        private void Awake()
        {
            RunDotNetThread();
            RunUniTaskThread();
        }

        private void OnDestroy()
        {
            _exited = true;
        }

        private void Update()
        {
            RunOnUnityPlayerLoop();
        }

        private void RunOnUnityPlayerLoop()
        {
            _unityTimer += Time.deltaTime;
            if (_unityTimer > 1)
            {
                _unityTimer = 0;
                SimpleDebug.Log($"Unity call {++_unityCounter}");
            }
        }

        private void RunDotNetThread()
        {
            Task.Factory.StartNew(() =>
            {
                while (!_exited)
                {
                    System.Threading.Thread.Sleep(Interval);
                    SimpleDebug.Log($"Dotnet Thread call {++_dotnetCounter}");
                    // TryCallUnityApi();
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void RunUniTaskThread()
        {
            UniTask.Void(async () =>
            {
                await UniTask.SwitchToThreadPool();
                while (!_exited)
                {
                    // await UniTask.Delay(Interval);
                    await Task.Delay(Interval);
                    SimpleDebug.Log($"UniTask Thread call {++_uniTaskCounter}");
                    // TryCallUnityApi();
                }
            });
        }

        private void TryCallUnityApi()
        {
            SimpleDebug.Log($"Is Background Thread: {System.Threading.Thread.CurrentThread.IsThreadPoolThread}");

            try
            {
                GetComponent<SpriteRenderer>();
            }
            catch (Exception e)
            {
                SimpleDebug.LogException(e);
            }
        }
    }
}