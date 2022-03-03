using System;

namespace ProjectU.Core
{
    /// <summary>Base class for callback attributes</summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CallbackAttribute: Attribute
    {
        protected CallbackAttribute() {}
    }

    /// <summary>Invokes method during Awake phase</summary>
    /// <remarks>Only for use with static methods with no parameters
    /// <br/>Attribute is called only once for the lifetime of the application</remarks>
    public class Awake: CallbackAttribute {}

    /// <summary>Invokes method during Start phase</summary>
    /// <remarks>Only for use with static methods with no parameters
    /// <br/>Attribute is called only once for the lifetime of the application</remarks>
    public class Start: CallbackAttribute {}

    /// <summary>Invokes method during Update phase</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    public class OnUpdate: CallbackAttribute {}

    /// <summary>Invokes method after a new scene has been loaded</summary>
    /// <remarks>Only for use with static methods with no parameters</remarks>
    public class OnSceneLoaded: CallbackAttribute {}

    /// <summary>Invokes method on Entering Edit Mode</summary>
    /// <remarks>Only for use with static methods with no parameters
    /// <br/>Works only in Editor</remarks>
    public class OnEnteredEditMode: CallbackAttribute {}

    /// <summary>Invokes method on Exiting Edit Mode</summary>
    /// <remarks>Only for use with static methods with no parameters
    /// <br/>Works only in Editor</remarks>
    public class OnExitingEditMode: CallbackAttribute {}

    /// <summary>Invokes method on Entering Play Mode</summary>
    /// <remarks>Only for use with static methods with no parameters
    /// <br/>Works only in Editor</remarks>
    public class OnEnteredPlayMode: CallbackAttribute {}

    /// <summary>Invokes method on Exiting Play Mode</summary>
    /// <remarks>Only for use with static methods with no parameters
    /// <br/>Works only in Editor</remarks>
    public class OnExitingPlayMode: CallbackAttribute {}
}