using System;
using JetBrains.Annotations;

namespace Examples.ServiceLocator.Utils
{
    [Service(nameof(ISchedulerService))]
    public interface ISchedulerService : IDisposable
    {
        /// Phải gọi thì mới start update
        void Initialize();

        /// <param name="action">Callback sẽ gọi tới</param>
        /// <param name="interval">Khoảng thời gian sẽ lặp lại</param>
        /// <returns>Schedule ID - use for cancel</returns>
        /// Action mặc định sẽ được call ở Background Thread
        int ScheduleRepeat(Action action, TimeSpan interval);

        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <param name="useMainThread">callback sẽ được gọi bằng MainThread</param>
        int ScheduleRepeat(Action action, TimeSpan interval, bool useMainThread);

        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <param name="delay">Lần fire đầu tiên sẽ tính từ delay thay cho interval</param>
        int ScheduleRepeat(Action action, TimeSpan interval, TimeSpan delay);

        int ScheduleRepeat(Action action, TimeSpan interval, TimeSpan delay, bool useMainThread);
        int ScheduleRepeat(Action action, TimeSpan interval, bool useMainThread, TimeSpan delay);

        /// <returns>Schedule ID - use for cancel</returns>
        /// Action mặc định sẽ được call ở Background Thread
        int ScheduleOnce(Action action, TimeSpan delay, bool useMainThread = false);

        void Cancel(int scheduleId);

        void CancelAll();
    }
}