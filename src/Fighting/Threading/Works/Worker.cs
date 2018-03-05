using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fighting.Threading.Works
{
    /// <summary>
    /// Base class that can be used to implement <see cref="IWorker"/>.
    /// </summary>
    public abstract class Worker : Runner, IWorker
    {
        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected Worker()
        {
            Logger = NullLogger.Instance;
        }

        public override void Start()
        {
            base.Start();
            Logger.LogDebug("Start background worker: {0}", ToString());
        }

        public override void Stop()
        {
            base.Stop();
            Logger.LogDebug("Stop background worker: {0}", ToString());
        }

        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}
