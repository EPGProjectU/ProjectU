using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ProjectU.Core.Helpers;
using UnityEngine;
using Object = UnityEngine.Object;
using BundleContext = System.Collections.Generic.Dictionary<System.Threading.Thread, ProjectU.Core.Serialization.SerializationBundle>;


namespace ProjectU.Core.Serialization
{
    public partial class SerializationBundle
    {
        [SerializeField]
        private List<Object> unityObjects = new List<Object>();

        [SerializeField]
        private Base64Container data;

        private static BinaryFormatter _formatter = new BinaryFormatter();

        private static BundleContext _bundleThreadContext = new BundleContext();

        private static readonly string UnityReferenceIndexString = "UnityReferenceIndex";

        private class UnityReferenceSerializationSurrogate : ISerializationSurrogate
        {
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                var bundle = ((BundleContext)context.Context)[Thread.CurrentThread];

                var index = bundle.AddUnityReference(obj as Object);

                info.AddValue(UnityReferenceIndexString, index);
            }

            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
            {
                var bundle = ((BundleContext)context.Context)[Thread.CurrentThread];
                var index = info.GetInt32(UnityReferenceIndexString);

                // C# does not have move semantics, so obj can not be assigned in a way that would keep the reference it had when passed as parameter. Does not seem to be a problem.
                obj = bundle.GetUnityReference(index);

                // Object having different type than during serialization is an indicator of it being destroyed or deleted and might result in crash if not cleaned from deserialized data
                // Although returning broken object could result in crash returning non unity object as placeholder seems to cause no problems
                // Could not return null as it is harder to determinate if it should be present and might cause an exception in types like Dictionary where keys cannot repeat
                // Production code will never use raw UnityEngine.Object for anything so this should not be a problem here
                if (obj.GetType() != info.ObjectType)
                {
                    bundle.HasBrokenReferences = true;
                    return new BrokenReference();
                }

                return obj;
            }
        }

        static SerializationBundle()
        {
            var selector = new SurrogateSelector();
            var surrogate = new UnityReferenceSerializationSurrogate();
            var context = new StreamingContext(StreamingContextStates.Other, _bundleThreadContext);

            foreach (var type in typeof(Object).GetDerivedTypes(true))
                selector.AddSurrogate(type, context, surrogate);

            _formatter.SurrogateSelector = selector;
            _formatter.Context = context;
        }

        private void Serialize_Impl(object obj)
        {
            unityObjects.Clear();

            if (obj == null)
            {
                data = null;
                return;
            }

            using var stream = new MemoryStream();

            _bundleThreadContext[Thread.CurrentThread] = this;

            _formatter.Serialize(stream, obj);
            data = stream.GetBuffer();

            _bundleThreadContext.Remove(Thread.CurrentThread);
        }

        private object Deserialize_Impl()
        {
            HasBrokenReferences = false;

            if (data == null)
                return null;

            using var stream = new MemoryStream(data);

            _bundleThreadContext[Thread.CurrentThread] = this;

            var deserializedData = _formatter.Deserialize(stream);

            _bundleThreadContext.Remove(Thread.CurrentThread);

            return deserializedData;
        }


        private int AddUnityReference(Object obj)
        {
            var index = unityObjects.IndexOf(obj);

            if (index >= 0)
                return index;

            index = unityObjects.Count;
            unityObjects.Add(obj);

            return index;
        }

        private Object GetUnityReference(int index)
        {
            return unityObjects[index];
        }

        private void Clear_Impl()
        {
            data = null;
            unityObjects.Clear();
            HasBrokenReferences = false;
        }
    }
}