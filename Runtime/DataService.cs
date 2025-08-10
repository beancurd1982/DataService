using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

namespace Yang.Data
{
    /// <summary>
    /// Represents a table data entity with a unique TableId.
    /// </summary>
    public interface ITableData
    {
        /// <summary>
        /// Gets the unique identifier for the table data.
        /// </summary>
        uint TableId { get; }
    }

    /// <summary>
    /// Provides registration and retrieval services for table data and global data instances.
    /// </summary>
    public static class DataService
    {
        /// <summary>
        /// Stores registered table data instances, keyed by their unique TableId.
        /// </summary>
        private static readonly ConcurrentDictionary<uint, ITableData> TableDataRegistry = new ConcurrentDictionary<uint, ITableData>();

        /// <summary>
        /// Stores registered global data instances, keyed by their type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> GlobalDataRegistry = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Registers a table data instance by its unique <see cref="ITableData.TableId"/>.
        /// Only one instance can be registered for each TableId at any time.
        /// Throws if an instance with the same TableId is already registered.
        /// </summary>
        /// <typeparam name="T">The type of the table data, implementing <see cref="ITableData"/>.</typeparam>
        /// <param name="instance">The table data instance to register.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="instance"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a different instance with the same TableId is already registered.
        /// </exception>
        public static void RegisterTableData<T>(T instance) where T : ITableData, new()
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance), "Instance cannot be null.");
            if (!TableDataRegistry.TryAdd(instance.TableId, instance))
                throw new InvalidOperationException($"An instance of {typeof(T).Name} with TableId={instance.TableId} is already registered.");
        }

        /// <summary>
        /// Unregisters the table data instance associated with the specified TableId.
        /// If no instance is registered with the given TableId, this method does nothing.
        /// </summary>
        /// <param name="tableId">The unique TableId of the table data to unregister.</param>
        public static void UnregisterTableData(uint tableId)
        {
            TableDataRegistry.TryRemove(tableId, out _);
        }

        /// <summary>
        /// Retrieves a registered table data instance of the specified type by its TableId.
        /// </summary>
        /// <typeparam name="T">The expected type of the table data, implementing <see cref="ITableData"/>.</typeparam>
        /// <param name="id">The unique TableId of the table data to retrieve.</param>
        /// <returns>The registered table data instance of type <typeparamref name="T"/>.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no instance is registered with the specified TableId.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown if the registered instance is not of the expected type <typeparamref name="T"/>.
        /// </exception>
        public static T GetTableData<T>(uint id) where T : ITableData
        {
            if (TableDataRegistry.TryGetValue(id, out var obj))
            {
                if (obj is T data)
                    return data;
                throw new InvalidCastException($"Instance with ID {id} is not of type {typeof(T).Name}.");
            }
            throw new KeyNotFoundException($"No instance of {typeof(T).Name} registered with ID {id}.");
        }

        /// <summary>
        /// Registers a single global instance of the specified data type.
        /// Only one global instance per type can be registered at any time.
        /// Throws if an instance of that type is already registered.
        /// </summary>
        /// <typeparam name="T">The type of the global data instance.</typeparam>
        /// <param name="instance">The global data instance to register.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="instance"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a global instance of the specified type is already registered.
        /// </exception>
        public static void RegisterGlobalData<T>(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance), "Instance cannot be null.");
            if (!GlobalDataRegistry.TryAdd(typeof(T), instance))
                throw new InvalidOperationException($"{typeof(T).Name} already registered globally.");
        }

        /// <summary>
        /// Retrieves the global instance of the specified data type.
        /// </summary>
        /// <typeparam name="T">The type of the global data instance.</typeparam>
        /// <returns>The registered global data instance of type <typeparamref name="T"/>.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no global instance of the specified type is registered.
        /// </exception>
        public static T GetGlobalData<T>()
        {
            if (GlobalDataRegistry.TryGetValue(typeof(T), out var obj))
                return (T)obj;

            throw new KeyNotFoundException($"No global instance of {typeof(T).Name} registered.");
        }
    }
}
