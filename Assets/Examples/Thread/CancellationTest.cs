using System.Threading;
using Cysharp.Threading.Tasks;
using EasyButtons;
using Examples.Utils;
using UnityEngine;

namespace Examples.Thread
{
    public class CancellationTest : MonoBehaviour
    {
        private CancellationTokenSource _cts;

        private void OnDestroy()
        {
            _cts?.Cancel();
            SimpleDebug.Log("Destroy Cancellation Test");
        }

        [Button]
        private void StartTest()
        {
            RunLongTaskWithDestroy();
            RunLongTaskNoDestroy();
        }

        [Button]
        private void StopTest()
        {
            Destroy(this);
        }

        private void RunLongTaskWithDestroy()
        {
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            Run().Forget();
            return;

            async UniTask Run()
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Delay(1000, DelayType.DeltaTime, PlayerLoopTiming.Update, token);
                    SimpleDebug.Log("Task With Destroy running");
                }
            }
        }

        private void RunLongTaskNoDestroy()
        {
            Run().Forget();
            return;

            async UniTask Run()
            {
                while (true)
                {
                    await UniTask.Delay(1000);
                    SimpleDebug.Log("Task No Destroy running");
                }
            }
        }
    }
}