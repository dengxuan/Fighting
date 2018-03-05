using System;
using System.Threading.Tasks;

namespace Fighting.Scheduling.Abstractions
{
    public interface ISchedulerManager
    {
        /// <summary>
        /// Enqueues a scheduler to be executed.
        /// </summary>
        /// <typeparam name="TScheduler">Type of the scheduler.</typeparam>
        /// <typeparam name="TArgs">Type of the arguments of scheduler.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="priority">Scheduler priority.</param>
        /// <param name="delay">Scheduler delay (wait duration before first try).</param>
        /// <returns>Unique identifier of a scheduler.</returns>
        Task<string> EnqueueAsync<TScheduler, TArgs>(TArgs args, SchedulerPriority priority = SchedulerPriority.Normal, TimeSpan? delay = null) where TScheduler : IScheduler<TArgs>;

        /// <summary>
        /// Deletes a scheduler with the specified schedulerId.
        /// </summary>
        /// <param name="id">The scheduler Unique Identifier.</param>
        /// <returns><c>True</c> on a successfull state transition, <c>false</c> otherwise.</returns>
        Task<bool> DeleteAsync(string id);
    }
}
