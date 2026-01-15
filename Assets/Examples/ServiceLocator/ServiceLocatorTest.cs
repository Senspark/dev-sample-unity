using System;
using System.Collections.Generic;
using EasyButtons;
using Examples.ServiceLocator.Utils;
using Examples.Utils;
using UnityEngine;

namespace Examples.ServiceLocator
{
    public class ServiceLocatorTest : MonoBehaviour
    {
        private ISchedulerService _scheduler;

        private List<int> _schedulerIds;

        private void Awake()
        {
            var testService = ServiceLocator.Resolve<ITestService>();
            testService.Hello();

            _scheduler = ServiceLocator.Resolve<ISchedulerService>();

            _schedulerIds = new List<int>();
        }

        [Button]
        private void StartAll()
        {
            _schedulerIds.Add(_scheduler.ScheduleRepeat(RunEvery1Second, TimeSpan.FromSeconds(1), true));
            _schedulerIds.Add(_scheduler.ScheduleRepeat(RunEvery2SecondWithDelay4Seconds, TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4), true));
            _schedulerIds.Add(_scheduler.ScheduleOnce(RunOnceAfter5Seconds, TimeSpan.FromSeconds(5), false));
        }

        [Button]
        private void StopAll()
        {
            foreach (var id in _schedulerIds)
            {
                _scheduler.Cancel(id);
            }
        }

        [Button]
        private void RemoveSelf()
        {
            Destroy(this);
        }

        private void OnDestroy()
        {
            _scheduler.CancelAll();
        }

        /// Function này mong đợi sẽ được gọi từ Main Thread 
        private void RunEvery1Second()
        {
            SimpleDebug.Log("Run Every 1 Second");
            GetComponent<Transform>();
        }

        /// Function này mong đợi sẽ được gọi từ Main Thread 
        private void RunEvery2SecondWithDelay4Seconds()
        {
            SimpleDebug.Log("Run Every 2 Seconds with Delay 4 Seconds");
            GetComponent<Transform>();
        }

        /// Function này mong đợi sẽ được gọi từ Background Thread 
        private void RunOnceAfter5Seconds()
        {
            try
            {
                GetComponent<Transform>();
                SimpleDebug.Log("Run Once After 5 Seconds: Failed");
            }
            catch (Exception e)
            {
                SimpleDebug.Log("Run Once After 5 Seconds: Success");
            }
        }
    }
}