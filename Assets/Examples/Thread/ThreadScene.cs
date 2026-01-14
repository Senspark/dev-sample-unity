using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using EasyButtons;
using UnityEngine;

namespace Examples.Thread
{
    /// Test background thread
    /// Mỗi giây Unity sẽ Print() 1 lần
    /// Run trên Mobile & exit App thì sẽ thấy Unity không còn Print() nữa
    /// Sau khi resume App, nếu giá trị của Print() đã thay đổi nhiều thì chứng minh rằng Background Thread vẫn hoạt động dưới nền 
    public class ThreadScene : MonoBehaviour
    {
        private int _dotnetCounter;
        private int _uniTaskCounter;
        private int _unityCounter;

        private float _unityTimer;

        private bool _exited;
        private const int Interval = 1000;

        private void Awake()
        {
            RunDotNetThread();
            RunUniTaskThread();
        }

        private void OnDestroy()
        {
            _exited = true;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Print();
        }

        private void Update()
        {
            // Mỗi giây print 1 lần
            _unityTimer += Time.deltaTime;
            if (_unityTimer > 1)
            {
                _unityTimer = 0;
                _unityCounter++;
                Print();
            }
        }

        [Button]
        private void Print()
        {
            Debug.Log($"Unity: {_unityCounter} - Dotnet: {_dotnetCounter} - UniTask: {_uniTaskCounter}");
        }

        private void RunDotNetThread()
        {
            _dotnetCounter = 0;
            Task.Factory.StartNew(() =>
            {
                while (!_exited)
                {
                    System.Threading.Thread.Sleep(Interval);
                    _dotnetCounter++;

                    // TryCallUnityApi();
                    
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// UniTask sẽ binding vào lifetime cycle của Unity
        /// Làm sao biết được UniTask có thật sự running ở Background Thread hay không?
        private void RunUniTaskThread()
        {
            _uniTaskCounter = 0;
            Run().Forget();
            return;

            async UniTask Run()
            {
                await UniTask.SwitchToThreadPool();
                while (!_exited)
                {
                    // await UniTask.Delay(Interval);
                    await Task.Delay(Interval);
                    _uniTaskCounter++;

                    TryCallUnityApi();
                }
            }
        }

        /// Làm sao biết được có thật sự running ở Background Thread hay không?
        /// Gọi thử Unity API nếu bị throw error là biết
        private void TryCallUnityApi()
        {
            Debug.Log($"Is Background Thread: {System.Threading.Thread.CurrentThread.IsThreadPoolThread}");
            
            try
            {
                GetComponent<SpriteRenderer>();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}