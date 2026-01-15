using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Examples.ServiceLocator.Utils
{
    public class SchedulerService : ISchedulerService
    {
        private readonly List<SchedulerTask> _tasks = new();
        private readonly List<SchedulerTask> _willAdd = new();
        private readonly List<int> _willRemoveIds = new();

        private bool _disposed;
        private int _incrementalId = int.MinValue;

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

        public int ScheduleRepeat(Action action, TimeSpan interval, TimeSpan? delay = null)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SchedulerService));
            }

            return Schedule(action, interval, new ScheduleOption
            {
                Delay = delay,
                Repeat = true
            });
        }

        public int ScheduleOnce(Action action, TimeSpan delay)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(SchedulerService));
            }

            return Schedule(action, delay, new ScheduleOption
            {
                Repeat = false
            });
        }

        public void Cancel(int scheduleId)
        {
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

        private int Schedule(Action action, TimeSpan interval, ScheduleOption option = null)
        {
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
        [CanBeNull] private readonly ScheduleOption _option;

        public SchedulerTask(int scheduleId, Action action, TimeSpan interval, [CanBeNull] ScheduleOption option)
        {
            ScheduleId = scheduleId;
            _action = action;
            _interval = interval;
            _option = option;

            if (option is { Delay: not null })
            {
                _nextTime = DateTime.Now + option.Delay.Value;
            }
            else
            {
                _nextTime = DateTime.Now + _interval;
            }
        }

        public bool CanExecute(DateTime now) => !Ended && _nextTime <= now;

        public UniTaskVoid Execute()
        {
            try
            {
                _action();
                if (_option is { Repeat: true })
                {
                    _nextTime = DateTime.Now + _interval;
                }
                else
                {
                    Ended = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return new UniTaskVoid();
        }
    }
}