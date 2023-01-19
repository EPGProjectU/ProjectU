using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace ProjectU.Core.Serialization
{
    /// <summary>
    /// Dictionary that fully serializes with support for polymorphism and unity references
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <remarks>
    /// After deserialization all broken UnityEngine.Objects are replaced by <see cref="ProjectU.Core.Serialization.SerializationBundle.BrokenReference"/>s and need to be manually removed as a validation
    /// </remarks>
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Stores dictionary data in format that Unity is able to serialize
        /// </summary>
        [SerializeField]
        private SerializationBundle bundle = new SerializationBundle();

        public SerializedDictionary() {}

        public SerializedDictionary(Dictionary<TKey, TValue> dictionary) => AddKeyValuePairsFrom(dictionary);

        /// <summary>
        /// Copies keys and values from a dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        public void AddKeyValuePairsFrom(Dictionary<TKey, TValue> dictionary)
        {
            foreach (var kvp in dictionary)
                Add(kvp.Key, kvp.Value);
        }

        // Calls deserialization constructor of base Dictionary
        protected SerializedDictionary(SerializationInfo info, StreamingContext context) : base(info, context) {}

        // Serializes as if this was normal Dictionary, bundle will not be serialized
        void ISerializationCallbackReceiver.OnBeforeSerialize() => bundle.Serialize(this);

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            AddKeyValuePairsFrom(bundle.Deserialize() as Dictionary<TKey, TValue>);
            bundle.Clear();
        }
    }
}