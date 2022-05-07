using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectU.Core
{
    [DisallowMultipleComponent]
    public class TagList : MonoBehaviour, ISerializationCallbackReceiver
    {
        public HashSet<string> tagSet = new HashSet<string>();

        ////////////////////////// Serialization Bulshit /////////////////////////////////////
        [SerializeField]
        private List<string> tagSerializationList;

        void ISerializationCallbackReceiver.OnBeforeSerialize() => tagSerializationList = tagSet.ToList();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => tagSet = new HashSet<string>(tagSerializationList);
    }
}