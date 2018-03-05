using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Fighting.Threading.Works
{
    public abstract class CyclicityWorker : Worker
    {
        private AutoResetEvent _toSignal = new AutoResetEvent(false);

        private AutoResetEvent _toWaitOn = new AutoResetEvent(false);

        /// <summary>
        /// 周期
        /// </summary>
        private int _interval { get; }

        public CyclicityWorker(int interval = 1000)
        {
            _interval = interval;
        }

        public override void Start()
        {
            base.Start();
            while (IsRunning)
            {
                try
                {
                    Logger.LogInformation("开始运行任务:{0}", ToString());
                    OnRunning();
                    Logger.LogInformation("结束运行任务:{0}", ToString());
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ToString());
                }
                finally
                {
                    WaitHandle.SignalAndWait(_toSignal, _toWaitOn, _interval, false);
                }
            }
        }
        public override void Stop()
        {
            base.Stop();
            _toWaitOn.Set();
            WaitHandle.WaitAll(new[] { _toSignal });
        }
        public abstract void OnRunning();
    }
}
