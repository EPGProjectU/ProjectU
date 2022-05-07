using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectU.Core
{
    [DisallowMultipleComponent]
    public class TagList : MonoBehaviour, ISerializationCallbackReceiver
    {
        public HashSet<string> tagSet = new HashSet<string>();
        
        public static GameObject[] _FindGameObjectsWithTag(string tag)
        {
            return FindObjectsOfType<TagList>()
                .Select(tl => tl.gameObject)
                .Where(o => o._CompareTag(tag))
                .ToArray();
        }

        ////////////////////////// Serialization Bulshit /////////////////////////////////////
        [SerializeField]
        private List<string> tagSerializationList;

        void ISerializationCallbackReceiver.OnBeforeSerialize() => tagSerializationList = tagSet.ToList();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => tagSet = new HashSet<string>(tagSerializationList);
    }

    public static class GameObjectTagListExtension
    {
        public static void _AddTag(this GameObject gameObject, string tag)
        {
            var tagList = gameObject.GetComponent<TagList>();

            if (tagList == null)
                tagList = gameObject.AddComponent<TagList>();

            tagList.tagSet.Add(tag);
        }

        public static void _RemoveTag(this GameObject gameObject, string tag)
        {
            var tagList = gameObject.GetComponent<TagList>();

            if (tagList == null)
                return;

            tagList.tagSet.Remove(tag);
        }

        public static bool _CompareTag(this GameObject gameObject, string tag)
        {
            var tagList = gameObject.GetComponent<TagList>();

            if (tagList == null)
                return false;

            return tagList.tagSet.Contains(tag);
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