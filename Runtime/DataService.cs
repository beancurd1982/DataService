using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

namespace Yang.Data
{
    public interface ITableData
    {
        uint TableId { get; }
    }

    public static class DataService
    {
        private static readonly ConcurrentDictionary<uint, ITableData> TableDataRegistry = new ConcurrentDictionary<uint, ITableData>();
        private static readonly ConcurrentDictionary<Type, object> GlobalDataRegistry = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Registers an instance of the specified data type.
        /// </summary>
        public static void RegisterTableData<T>(T instance) where T : ITableData, new()
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance), "Instance cannot be null.");
            if (!TableDataRegistry.TryAdd(instance.TableId, instance))
                throw new InvalidOperationException($"An instance of {typeof(T).Name} with TableId={instance.TableId} is already registered.");
        }

        /// <summary>
        /// Unregisters an instance of the specified data type by its TableId.
        /// </summary>
        /// <param name="tableId"></param>
        public static void UnregisterTableData(uint tableId)
        {
            TableDataRegistry.TryRemove(tableId, out _);
        }

        /// <summary>
        /// Retrieves a previously registered instance of the specified type by ID.
        /// </summary>
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
        /// Throws if an instance of that type is already registered.
        /// </summary>
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
        public static T GetGlobalData<T>()
        {
            if (GlobalDataRegistry.TryGetValue(typeof(T), out var obj))
                return (T)obj;

            throw new KeyNotFoundException($"No global instance of {typeof(T).Name} registered.");
        }
    }

}
