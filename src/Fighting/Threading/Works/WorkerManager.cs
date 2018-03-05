using System;
using System.Collections.Generic;

namespace Fighting.Threading.Works
{
    /// <summary>
    /// Implements <see cref="IWorkerManager"/>.
    /// </summary>
    public class WorkerManager : Runner, IWorkerManager, IDisposable
    {
        private readonly IServiceProvider _iocResolver;
        private readonly List<IWorker> _workers;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerManager"/> class.
        /// </summary>
        public WorkerManager(IServiceProvider iocResolver)
        {
            _iocResolver = iocResolver;
            _workers = new List<IWorker>();
        }

        public override void Start()
        {
            base.Start();

            _workers.ForEach(job => job.Start());
        }

        public override void Stop()
        {
            _workers.ForEach(job => job.Stop());

            base.Stop();
        }

        public void Add(IWorker worker)
        {
            _workers.Add(worker);

            if (IsRunning)
            {
                worker.Start();
            }
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _workers.Clear();
        }
    }
}
