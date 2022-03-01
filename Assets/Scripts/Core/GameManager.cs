using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace ProjectU.Core
{
    /// <summary>Base class for callback attributes</summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CallbackAttribute: Attribute
    {
        protected CallbackAttribute() {}
    }

    /// <summary>Invokes method during Awake phase</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    /// <remarks>Attribute is called only once for the lifetime of the application</remarks>
    public class Awake: CallbackAttribute {}

    /// <summary>Invokes method during Start phase</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    /// <remarks>Attribute is called only once for the lifetime of the application</remarks>
    public class Start: CallbackAttribute {}

    /// <summary>Invokes method during Update phase</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    public class OnUpdate: CallbackAttribute {}

    /// <summary>Invokes method after a new scene has been loaded</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    public class OnSceneLoaded: CallbackAttribute {}

    /// <summary>Invokes method on Entering Edit Mode</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    /// <remarks>Works only in Editor</remarks>
    public class OnEnteredEditMode: CallbackAttribute {}

    /// <summary>Invokes method on Exiting Edit Mode</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    /// <remarks>Works only in Editor</remarks>
    public class OnExitingEditMode: CallbackAttribute {}

    /// <summary>Invokes method on Entering Play Mode</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    /// <remarks>Works only in Editor</remarks>
    public class OnEnteredPlayMode: CallbackAttribute {}

    /// <summary>Invokes method on Exiting Play Mode</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    /// <remarks>Works only in Editor</remarks>
    public class OnExitingPlayMode: CallbackAttribute {}


    // Wrapper class to prevent GameManger from appearing in component list
    public abstract class WrapperClass
    {
        /// <summary>
        /// Invokes methods with callback attributes
        /// </summary>
        public class CallbackAttributeController: MonoBehaviour
        {

            /// <summary>
            /// Prepares StaticCallbackController
            /// </summary>
            [RuntimeInitializeOnLoadMethod]
            private static void Init()
            {
                LoadAttributeReferences();
                CreateInstance();
            }
            
            /// <summary>
            /// Stores methods with callback attributes
            /// </summary>
            private static readonly Dictionary<Type, Array> AttributeMethods = new Dictionary<Type, Array>();

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
                SceneManager.sceneLoaded += (scene, mode) => InvokeMethodsWithAttribute(typeof(OnSceneLoaded));
#if UNITY_EDITOR
                EditorApplication.playModeStateChanged += InvokePlayModeStateChange;
#endif
            }

#if UNITY_EDITOR
            /// <summary>
            /// Invokes callbacks for change in playmode state
            /// </summary>
            /// <param name="state"></param>
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
#endif

            /// <summary>
            /// Gets references of methods that have custom attributes and saves them into dictionary
            /// </summary>
            /// TODO Check if there is a need for monitoring loading of assemblies that were not available during method execution

#if UNITY_EDITOR
            [UnityEditor.Callbacks.DidReloadScripts]
#endif
            private static void LoadAttributeReferences()
            {
                // Get current assembly
                var thisAssembly = typeof(CallbackAttributeController).Assembly;

                // Get assemblies that directly reference current assembly
                var assemblies = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    where assembly.GetReferencedAssemblies().Any(name => name.ToString().Equals(thisAssembly.GetName().ToString()))
                    select assembly;

                // Add current assembly to collection
                assemblies = assemblies.Append(thisAssembly);

                // Get all types in the founded assemblies
                var types = from assembly in assemblies
                    from type in assembly.GetTypes()
                    select type;

                // Get all static methods from the founded types
                var staticMethods = (from type in types
                    from method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                    select method).ToArray();

                // Load static methods with given attributes into the dictionary
                LoadMethodsWithAttribute(typeof(Awake), staticMethods);
                LoadMethodsWithAttribute(typeof(Start), staticMethods);
                LoadMethodsWithAttribute(typeof(OnUpdate), staticMethods);
                LoadMethodsWithAttribute(typeof(OnSceneLoaded), staticMethods);
                LoadMethodsWithAttribute(typeof(OnEnteredEditMode), staticMethods);
                LoadMethodsWithAttribute(typeof(OnExitingEditMode), staticMethods);
                LoadMethodsWithAttribute(typeof(OnEnteredPlayMode), staticMethods);
                LoadMethodsWithAttribute(typeof(OnExitingPlayMode), staticMethods);
            }

            /// <summary>
            /// Filters out methods that contain given attribute and saves their references in the dictionary
            /// </summary>
            /// <param name="attributeType">Attribute that is searched for</param>
            /// <param name="methods">Methods that are to be searched through</param>
            private static void LoadMethodsWithAttribute(Type attributeType, IEnumerable<MethodInfo> methods)
            {
                AttributeMethods[attributeType] = (from method in methods
                    where method.GetCustomAttributes(attributeType, false).Length > 0
                    select method).ToArray();
            }

            /// <summary>
            /// Calls all static methods that have given attribute
            /// </summary>
            /// <param name="attributeType"></param>
            private static void InvokeMethodsWithAttribute(Type attributeType)
            {
                foreach (MethodInfo method in AttributeMethods[attributeType])
                {
                    method.Invoke(null, null);
                }
            }

            //--------------- Callbacks for MonoBehaviour methods ------------
            private void Awake() => InvokeMethodsWithAttribute(typeof(Awake));
            private void Start() => InvokeMethodsWithAttribute(typeof(Start));
            private void Update() => InvokeMethodsWithAttribute(typeof(OnUpdate));
            //----------------------------------------------------------------
        }
    }
}