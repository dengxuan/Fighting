using System;
using System.Threading.Tasks;

namespace Fighting.Scheduling.Abstractions
{
    /// <summary>
    /// Defines interface of a scheduler.
    /// </summary>
    public interface IScheduler<in TArgs>
    {
        /// <summary>
        /// Executes the scheduler with the <see cref="args"/>.
        /// </summary>
        /// <param name="args">Scheduler arguments.</param>
        Task Run(TArgs args);
    }
}
