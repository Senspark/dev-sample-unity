using System;

namespace Examples.ServiceLocator.Utils
{
    [Service(nameof(ISchedulerService))]
    public interface ISchedulerService : IDisposable
    {
        /// Phải gọi thì mới start update
        void Initialize();

        /// <param name="action">Callback sẽ gọi tới</param>
        /// <param name="interval">Khoảng thời gian sẽ lặp lại</param>
        /// <param name="delay">Lần fire đầu tiên sẽ tính từ delay thay cho interval</param>
        /// <returns>Schedule ID - use for cancel</returns>
        /// Action mặc định sẽ được call ở Background Thread
        int ScheduleRepeat(Action action, TimeSpan interval, TimeSpan? delay = null);

        /// <returns>Schedule ID - use for cancel</returns>
        /// Action mặc định sẽ được call ở Background Thread
        int ScheduleOnce(Action action, TimeSpan delay);

        void Cancel(int scheduleId);

        void CancelAll();
    }

    public class ScheduleOption
    {
        public TimeSpan? Delay;
        public bool? Repeat;
    }
}