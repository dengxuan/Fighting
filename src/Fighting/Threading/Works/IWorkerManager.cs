namespace Fighting.Threading.Works
{
    /// <summary>
    /// Used to manage background workers.
    /// </summary>
    public interface IWorkerManager : IRunnable
    {
        /// <summary>
        /// Adds a new worker. Starts the worker immediately if <see cref="IWorkerManager"/> has started.
        /// </summary>
        /// <param name="worker">
        /// The worker. It should be resolved from IOC.
        /// </param>
        void Add(IWorker worker);
    }
}
