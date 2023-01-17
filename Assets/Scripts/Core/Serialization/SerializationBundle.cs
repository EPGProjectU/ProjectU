using System;

namespace ProjectU.Core.Serialization
{
    /// <summary>
    /// Allows serialization of mixed general types, polymorphism and references to Unity objects
    /// </summary>
    [Serializable]
    public partial class SerializationBundle
    {
        /// <summary>
        /// Class used as placeholder for broken UnityEngine.Object references
        /// </summary>
        /// <remarks>
        /// Might want to remove it from deserialized data, but in some cases should not cause any problems
        /// </remarks>
        public class BrokenReference {}

        /// <summary>
        /// Indicates that there were some <see cref="BrokenReference"/>s put into deserialized data as placeholders for broken Unity Objects
        /// </summary>
        public bool HasBrokenReferences { get; private set; }

        /// <summary>
        /// Serializes obj and stores its data internally
        /// </summary>
        /// <param name="obj"></param>
        /// <remarks>
        /// All stored data is overwritten each time this method is called
        /// </remarks>
        /// <remarks>Should be able to serialize any type, including references to unity objects, dictionaries</remarks>
        public void Serialize(object obj) => Serialize_Impl(obj);

        /// <summary>
        /// Deserializes last object that was passed to <see cref="ProjectU.Core.Serialization.SerializationBundle.Serialize"/>
        /// </summary>
        public object Deserialize() => Deserialize_Impl();

        /// <summary>
        /// Clears fields to free memory
        /// </summary>
        /// <remarks>
        /// Should be called after deserialization is done
        /// </remarks>
        public void Clear() => Clear_Impl();
    }
}