using Fighting.Storaging.Entities.Abstractions;
using Fighting.Timing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fighting.Scheduling.Abstractions
{
    public class Schedule : Entity<long>
    {
        /// <summary>
        /// Maximum length of <see cref="SchedulerType"/>.
        /// Value: 512.
        /// </summary>
        public const int MaxSchedulerTypeLength = 512;

        /// <summary>
        /// Maximum length of <see cref="SchedulerArgs"/>.
        /// Value: 1 MB (1,048,576 bytes).
        /// </summary>
        public const int MaxSchedulerArgsLength = 1024 * 1024;

        /// <summary>
        /// Default duration (as seconds) for the first wait on a failure.
        /// Default value: 60 (1 minutes).
        /// </summary>
        public static int DefaultFirstWaitDuration { get; set; }

        /// <summary>
        /// Default timeout value (as seconds) for a job before it's abandoned (<see cref="IsAbandoned"/>).
        /// Default value: 172,800 (2 days).
        /// </summary>
        public static int DefaultTimeout { get; set; }

        /// <summary>
        /// Default wait factor for execution failures.
        /// This amount is multiplated by last wait time to calculate next wait time.
        /// Default value: 2.0.
        /// </summary>
        public static double DefaultWaitFactor { get; set; }

        /// <summary>
        /// Type of the Scheduler.
        /// It's AssemblyQualifiedName of Scheduler type.
        /// </summary>
        [Required]
        [StringLength(MaxSchedulerTypeLength)]
        public virtual string SchedulerType { get; set; }

        /// <summary>
        /// Scheduler arguments as JSON string.
        /// </summary>
        [Required]
        [MaxLength(MaxSchedulerArgsLength)]
        public virtual string SchedulerArgs { get; set; }

        /// <summary>
        /// Try count of this Scheduler.
        /// A Scheduler is re-tried if it fails.
        /// </summary>
        public virtual short TryCount { get; set; }

        /// <summary>
        /// Next try time of this Scheduler.
        /// </summary>
        public virtual DateTime NextTryTime { get; set; }

        /// <summary>
        /// Last try time of this Scheduler.
        /// </summary>
        public virtual DateTime? LastTryTime { get; set; }

        /// <summary>
        /// This is true if this Scheduler is continously failed and will not be executed again.
        /// </summary>
        public virtual bool IsAbandoned { get; set; }

        /// <summary>
        /// Priority of this Scheduler.
        /// </summary>
        public virtual SchedulerPriority Priority { get; set; }

        static Schedule()
        {
            DefaultFirstWaitDuration = 60;
            DefaultTimeout = 172800;
            DefaultWaitFactor = 2.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class.
        /// </summary>
        public Schedule()
        {
            NextTryTime = Clock.Now;
            Priority = SchedulerPriority.Normal;
        }

        /// <summary>
        /// Calculates next try time if a job fails.
        /// Returns null if it will not wait anymore and job should be abandoned.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime? CalculateNextTryTime()
        {
            var nextWaitDuration = DefaultFirstWaitDuration * (System.Math.Pow(DefaultWaitFactor, (TryCount - 1) > 5 ? 5 : TryCount));
            var nextTryDate = LastTryTime.HasValue ? LastTryTime.Value.AddSeconds(nextWaitDuration) : Clock.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(CreationTime).TotalSeconds > DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}
