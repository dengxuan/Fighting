namespace Fighting.Scheduling.Abstractions
{
    /// <summary>
    /// Priority of a Scheduler.
    /// </summary>
    public enum SchedulerPriority : byte
    {
        /// <summary>
        /// Low.
        /// </summary>
        Low = 5,

        /// <summary>
        /// Below normal.
        /// </summary>
        BelowNormal = 10,

        /// <summary>
        /// Normal (default).
        /// </summary>
        Normal = 15,

        /// <summary>
        /// Above normal.
        /// </summary>
        AboveNormal = 20,

        /// <summary>
        /// High.
        /// </summary>
        High = 25
    }
}
