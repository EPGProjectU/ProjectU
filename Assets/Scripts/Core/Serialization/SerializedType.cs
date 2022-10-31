using System;
using UnityEngine;
using Type = System.Type;

namespace ProjectU.Core.Serialization
{
    /// <summary>
    /// Allows serialization of System.Type
    /// </summary>
    [Serializable]
    public class SerializedType : ISerializationCallbackReceiver
    {
        [SerializeField]
        private SerializationBundle bundle = new SerializationBundle();

        public Type Type;

        public SerializedType(Type type = null) => Type = type;

        void ISerializationCallbackReceiver.OnBeforeSerialize() => bundle.Serialize(Type);

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Type = (Type)bundle.Deserialize();
            bundle.Clear();
        }
        
        public static implicit operator SerializedType(Type t) => new SerializedType{Type = t};
        public static implicit operator Type(SerializedType t) => t.Type;
    }
}