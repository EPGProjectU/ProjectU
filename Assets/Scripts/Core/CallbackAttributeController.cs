﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectU.Core
{
    /// <summary>
    /// Invokes methods with callback attributes
    /// </summary>
    public class CallbackAttributeController : MonoBehaviour, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Indicates playmode status
        /// </summary>
        private static bool _isInPlaymode;

        /// <summary>
        /// Used for serialization of <see cref="_isInPlaymode"/>
        /// </summary>
        private bool _isInPlaymodeSerialization;

        /// <summary>
        /// Stores cached delegates of methods with callback attributes
        /// </summary>
        private static readonly Dictionary<Type, Delegate> AttributeMethods = new Dictionary<Type, Delegate>();


        /// <summary>
        /// Prepares StaticCallbackController
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _isInPlaymode = true;
            CreateInstance();
        }

        [OnExitingPlayMode]
        private static void OnExit() => _isInPlaymode = false;

        /// <summary>
        /// Creates a single instance of <see cref="CallbackAttributeController"/> that persist for the duration of playtime
        /// </summary>
        private static void CreateInstance()
        {
            var gm = new GameObject("CallbackAttributeController")
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            DontDestroyOnLoad(gm);

            gm.AddComponent<CallbackAttributeController>();
        }

        /// <summary>
        /// Links callbacks to UnityAPI
        /// </summary>
        /// <remarks>Static constructor guarantees that callbacks are bound even after a hot reload</remarks>
        static CallbackAttributeController()
        {
            CacheAttributes();
            // Will invoke methods with OnSceneLoaded attribute when receiving event from SceneManager
            SceneManager.sceneLoaded += (scene, mode) => InvokeMethodsWithAttribute(typeof(OnSceneLoaded));

#if UNITY_EDITOR
            AssemblyReloadEvents.beforeAssemblyReload += () => InvokeMethodsWithAttribute(typeof(BeforeAssemblyReload));
            AssemblyReloadEvents.afterAssemblyReload += () => InvokeMethodsWithAttribute(typeof(AfterAssemblyReload));

            EditorApplication.playModeStateChanged += InvokePlayModeStateChange;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Invokes callbacks for change in playmode state
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>Editor only functionality</remarks>
        private static void InvokePlayModeStateChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    InvokeMethodsWithAttribute(typeof(OnEnteredEditMode));
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    InvokeMethodsWithAttribute(typeof(OnExitingEditMode));
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    InvokeMethodsWithAttribute(typeof(OnEnteredPlayMode));
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    InvokeMethodsWithAttribute(typeof(OnExitingPlayMode));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        /// <summary>
        /// Invokes BeforeHotReload during BeforeAssemblyReload if in playmode
        /// </summary>
        [BeforeAssemblyReload]
        private static void InvokeBeforeHotReload()
        {
            if (_isInPlaymode)
                InvokeMethodsWithAttribute(typeof(BeforeHotReload));
        }

        /// <summary>
        /// Invokes AfterHotReload during AfterAssemblyReload if in playmode
        /// </summary>
        [AfterAssemblyReload]
        private static void InvokeAfterHotReload()
        {
            if (_isInPlaymode)
                InvokeMethodsWithAttribute(typeof(AfterHotReload));
        }
#endif

        /// <summary>
        /// Gets delegates of methods that have custom attributes and caches them into dictionary
        /// </summary>
        /// TODO Check if there is a need for monitoring loading of assemblies that were not available during method execution
#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
#endif
        private static void CacheAttributes()
        {
            // Get current assembly
            var thisAssembly = typeof(CallbackAttributeController).Assembly;

            // Get assemblies that directly reference current assembly
            var assemblies = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                where assembly.GetReferencedAssemblies().Any(name => name.ToString().Equals(thisAssembly.GetName().ToString()))
                select assembly;

            // Add current assembly to collection
            assemblies = assemblies.Append(thisAssembly);

            // Get all types in the found assemblies
            var types = (from assembly in assemblies
                from type in assembly.GetTypes()
                select type).ToArray();

            // Cache all types deriving from CallbackAttribute
            var callbackAttributeTypes = (from t in types
                where t.BaseType == typeof(CallbackAttribute)
                select t).ToArray();


            // Get all static methods from the found types
            var staticMethods = (from type in types
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                select method).ToArray();


            // Load static methods with callback attributes into the dictionary
            foreach (var type in callbackAttributeTypes)
            {
                CacheMethodsWithAttribute(staticMethods, type);
            }
        }

        /// <summary>
        /// Filters out methods that contain given attribute and caches them as delegates in the dictionary
        /// </summary>
        /// <param name="methodInfos">Methods that are to be searched through</param>
        /// <param name="attributeType">Attribute that is searched for</param>
        private static void CacheMethodsWithAttribute(IEnumerable<MethodInfo> methodInfos, Type attributeType)
        {
            // Filter out methods without the attribute
            var filteredMethodInfos = (from methodInfo in methodInfos
                where methodInfo.GetCustomAttributes(attributeType, false).Length > 0
                select methodInfo).ToArray();

            // Convert filtered method info into combined delegate
            AttributeMethods[attributeType] = filteredMethodInfos.Any() ? filteredMethodInfos.Select(methodInfo => Delegate.CreateDelegate(typeof(Action), methodInfo)).Aggregate(Delegate.Combine) : null;
        }

        /// <summary>
        /// Invokes cached delegate for given attribute
        /// </summary>
        /// <param name="attributeType"></param>
        private static void InvokeMethodsWithAttribute(Type attributeType)
        {
            AttributeMethods[attributeType]?.DynamicInvoke();
        }

        //--------------- Callbacks for MonoBehaviour methods ------------
        private void Awake() => InvokeMethodsWithAttribute(typeof(Awake));
        private void Start() => InvokeMethodsWithAttribute(typeof(Start));

        private void Update() => InvokeMethodsWithAttribute(typeof(OnUpdate));
        //----------------------------------------------------------------

        public void OnBeforeSerialize() => _isInPlaymodeSerialization = _isInPlaymode;

        public void OnAfterDeserialize() => _isInPlaymode = _isInPlaymodeSerialization;
    }
}