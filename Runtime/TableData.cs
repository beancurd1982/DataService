namespace Yang.Data
{
    public class TableData
    {
        /// <summary>
        /// Expose the concrete type so writers can call Config.AsWrite() with no cast.
        /// UI can still call AsRead() or read properties/subscribe to events.
        /// </summary>
        public TableConfig Config { get; } = new();

        /// <summary>
        /// Status now follows the same read/write split pattern internally.
        /// </summary>
        public TableStatus Status { get; } = new();
    }
}
