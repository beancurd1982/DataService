using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

namespace Yang.Data
{
    public interface ITableData
    {
        int TableId { get; }
    }

    public static class DataService
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<int, object>> _registry
            = new ConcurrentDictionary<Type, ConcurrentDictionary<int, object>>();
        private static readonly ConcurrentDictionary<Type, object> _globalRegistry
            = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Registers an instance of the specified data type.
        /// </summary>
        public static void RegisterTableData<T>(T instance) where T : ITableData
        {
            var bucket = _registry.GetOrAdd(typeof(T), _ => new ConcurrentDictionary<int, object>());
            bucket[instance.TableId] = instance;
        }

        /// <summary>
        /// Retrieves a previously registered instance of the specified type by ID.
        /// </summary>
        public static T GetTableData<T>(int id) where T : ITableData
        {
            if (_registry.TryGetValue(typeof(T), out var bucket) &&
                bucket.TryGetValue(id, out var obj))
            {
                return (T)obj;
            }

            throw new KeyNotFoundException($"No instance of {typeof(T).Name} with TableId={id} found.");
        }

        /// <summary>
        /// Registers a single global instance of the specified data type.
        /// Throws if an instance of that type is already registered.
        /// </summary>
        public static void RegisterGlobalData<T>(T instance)
        {
            if (!_globalRegistry.TryAdd(typeof(T), instance))
                throw new InvalidOperationException($"{typeof(T).Name} already registered globally.");
        }

        /// <summary>
        /// Retrieves the global instance of the specified data type.
        /// </summary>
        public static T GetGlobalData<T>()
        {
            if (_globalRegistry.TryGetValue(typeof(T), out var obj))
                return (T)obj;

            throw new KeyNotFoundException($"No global instance of {typeof(T).Name} registered.");
        }
    }

}
