using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectU.Core
{
    [DisallowMultipleComponent]
    public class Tags : MonoBehaviour
    {
        public TagList tagCollection;
    }

    [Serializable]
    public class TagList : ISerializationCallbackReceiver, IEnumerable<string>
    {
        public HashSet<string> set = new HashSet<string>();
        
        public static GameObject[] _FindGameObjectsWithTag(string tag)
        {
            return Object.FindObjectsOfType<Tags>()
                .Select(tl => tl.gameObject)
                .Where(o => o._CompareTag(tag))
                .ToArray();
        }

        ////////////////////////// Serialization Bulshit /////////////////////////////////////
        [SerializeField]
        private List<string> tagSerializationList;

        void ISerializationCallbackReceiver.OnBeforeSerialize() => tagSerializationList = set.ToList();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => set = new HashSet<string>(tagSerializationList);

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return set.GetEnumerator();
        }
    }

    public static class GameObjectTagListExtension
    {
        public static void _AddTag(this GameObject gameObject, string tag)
        {
            var tagComponent = gameObject.GetComponent<Tags>();

            if (tagComponent == null)
                tagComponent = gameObject.AddComponent<Tags>();

            tagComponent.tagCollection.set.Add(tag);
        }

        public static void _AddTags(this GameObject gameObject, IEnumerable<string> tags)
        {
            foreach (var @tag in tags)
                gameObject._AddTag(@tag);
        }

        public static void _RemoveTag(this GameObject gameObject, string tag)
        {
            var tagComponent = gameObject.GetComponent<Tags>();

            if (tagComponent == null)
                return;

            tagComponent.tagCollection.set.Remove(tag);
        }

        public static void _RemoveTags(this GameObject gameObject, IEnumerable<string> tags)
        {
            foreach (var @tag in tags)
                gameObject._RemoveTag(@tag);
        }

        public static bool _CompareTag(this GameObject gameObject, string tag)
        {
            var tagComponent = gameObject.GetComponent<Tags>();

            if (tagComponent == null)
                return false;

            return tagComponent.tagCollection.set.Contains(tag);
        }

        public static bool _CompareAnyTag(this GameObject gameObject, IEnumerable<string> tags)
        {
            return tags.Any(t => _CompareTag(gameObject, t));
        }

        public static bool _CompareAllTags(this GameObject gameObject, IEnumerable<string> tags)
        {
            return tags.All(t => _CompareTag(gameObject, t));
        }

        public static void _AddTag(this Component component, string tag) => _AddTag(component.gameObject, tag);

        public static void _RemoveTag(this Component component, string tag) => _RemoveTag(component.gameObject, tag);

        public static bool _CompareTag(this Component component, string tag) => _CompareTag(component.gameObject, tag);

        public static bool _CompareAnyTag(this Component component, IEnumerable<string> tags) => _CompareAnyTag(component.gameObject, tags);

        public static bool _CompareAllTags(this Component component, IEnumerable<string> tags) => _CompareAllTags(component.gameObject, tags);
    }
}