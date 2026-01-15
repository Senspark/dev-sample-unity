using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Examples.ServiceLocator.Utils
{
    public class SchedulerService : ISchedulerService
    {
        private readonly List<SchedulerTask> _tasks = new();
        private readonly List<SchedulerTask> _willAdd = new();
        private readonly List<int> _willRemoveIds = new();

        private bool _disposed;
        private int _incrementalId;

        private bool _initialized;

        public void Dispose()
        {
            _disposed = true;
        }

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            Update().Forget();
        }

        public int ScheduleRepeat(Action action, TimeSpan interval)
        {
            return Schedule(action, interval, new SchedulerOption
            {
                Delay = TimeSpan.Zero,
                Repeat = true,
                UseMainThread = false
            });
        }

        public int ScheduleRepeat(Action action, TimeSpan interval, bool useMainThread)
        {
            return Schedule(action, interval, new SchedulerOption
            {
                Delay = TimeSpan.Zero,
                Repeat = true,
                UseMainThread = useMainThread
            });
        }

        public int ScheduleRepeat(Action action, TimeSpan interval, TimeSpan delay)
        {
            return Schedule(action, interval, new SchedulerOption
            {
                Delay = delay,
                Repeat = true,
                UseMainThread = false
            });
        }

        public int ScheduleRepeat(Action action, TimeSpan interval, TimeSpan delay, bool useMainThread)
        {
            return Schedule(action, interval, new SchedulerOption
            {
                Delay = delay,
                Repeat = true,
                UseMainThread = useMainThread
            });
        }

        public int ScheduleRepeat(Action action, TimeSpan interval, bool useMainThread, TimeSpan delay)
        {
            return Schedule(action, interval, new SchedulerOption
            {
                Delay = delay,
                Repeat = true,
                UseMainThread = useMainThread
            });
        }

        public int ScheduleOnce(Action action, TimeSpan delay, bool useMainThread = false)
        {
            return Schedule(action, delay, new SchedulerOption
            {
                Delay = TimeSpan.Zero,
                Repeat = false,
                UseMainThread = useMainThread
            });
        }

        public void Cancel(int scheduleId)
        {
            if (scheduleId <= 0) return;

            UniTask.RunOnThreadPool(() =>
            {
                lock (_willRemoveIds)
                {
                    _willRemoveIds.Add(scheduleId);
                }
            });
        }

        public void CancelAll()
        {
            UniTask.RunOnThreadPool(() =>
            {
                var allIds = _tasks.Select(e => e.ScheduleId);
                lock (_willRemoveIds)
                {
                    _willRemoveIds.AddRange(allIds);
                }
            });
        }

        private int Schedule(Action action, TimeSpan interval, SchedulerOption option = null)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SchedulerService));
            }

            var id = _incrementalId++;
            UniTask.RunOnThreadPool(() =>
            {
                lock (_willAdd)
                {
                    _willAdd.Add(new SchedulerTask(id, action, interval, option));
                }
            });
            return id;
        }

        private async UniTask Update()
        {
            while (!_disposed)
            {
                await UniTask.SwitchToThreadPool();

                CheckAndRemove();
                CheckAndAdd();
                TryExecute();
                RemoveEnded();

                await UniTask.Yield();
            }

            CleanUp();
        }

        private void CleanUp()
        {
            lock (_willAdd)
            {
                _willAdd.Clear();
            }

            lock (_willRemoveIds)
            {
                _willRemoveIds.Clear();
            }

            lock (_tasks)
            {
                _tasks.Clear();
            }
        }

        private void TryExecute()
        {
            var now = DateTime.Now;
            foreach (var task in _tasks)
            {
                if (task.CanExecute(now))
                {
                    task.Execute().Forget();
                }
            }
        }

        private void CheckAndRemove()
        {
            List<int> idsToRemove;
            lock (_willRemoveIds)
            {
                if (_willRemoveIds.Count == 0) return;
                idsToRemove = new List<int>(_willRemoveIds);
                _willRemoveIds.Clear();
            }

            _tasks.RemoveAll(t => idsToRemove.Contains(t.ScheduleId));
        }

        private void CheckAndAdd()
        {
            List<SchedulerTask> tasksToAdd;
            lock (_willAdd)
            {
                if (_willAdd.Count == 0) return;
                tasksToAdd = new List<SchedulerTask>(_willAdd);
                _willAdd.Clear();
            }

            _tasks.AddRange(tasksToAdd);
        }

        private void RemoveEnded()
        {
            _tasks.RemoveAll(t => t.Ended);
        }
    }


    internal class SchedulerTask
    {
        public readonly int ScheduleId;
        public bool Ended { get; private set; }

        private DateTime _nextTime;
        private readonly Action _action;
        private readonly TimeSpan _interval;
        private readonly SchedulerOption _option;

        public SchedulerTask(int scheduleId, Action action, TimeSpan interval, SchedulerOption option)
        {
            ScheduleId = scheduleId;
            _action = action;
            _interval = interval;
            _option = option;

            if (option.Delay.Ticks > 0)
            {
                _nextTime = DateTime.Now + option.Delay;
            }
            else
            {
                _nextTime = DateTime.Now + _interval;
            }
        }

        public bool CanExecute(DateTime now) => !Ended && _nextTime <= now;

        public async UniTaskVoid Execute()
        {
            try
            {
                if (_option.Repeat)
                {
                    _nextTime = DateTime.Now + _interval;
                }
                else
                {
                    Ended = true;
                }

                if (_option.UseMainThread)
                {
                    await UniTask.SwitchToMainThread();
                }

                _action();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    internal class SchedulerOption
    {
        public TimeSpan Delay;
        public bool Repeat;
        public bool UseMainThread;
    }
}