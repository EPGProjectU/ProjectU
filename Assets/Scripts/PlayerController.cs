using System;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    private PlayerInput _playerInput;

    /// <summary>
    /// Stores bindings for performed inputs
    /// </summary>
    private readonly Dictionary<string, Action<InputAction.CallbackContext>> _performedInputBindings = new Dictionary<string, Action<InputAction.CallbackContext>>();

    /// <summary>
    /// Stores bindings for canceled inputs
    /// </summary>
    private readonly Dictionary<string, Action<InputAction.CallbackContext>> _canceledInputBindings = new Dictionary<string, Action<InputAction.CallbackContext>>();

    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
        InputSetup();
    }

    private void OnDestroy()
    {
        BreakInputBindings();
    }

    private void InputSetup()
    {
        // Cache PlayerInput
        _playerInput = GetComponent<PlayerInput>();
        BindInputs();
    }

    /// <summary>
    /// Binds actions to Player Inputs using binding names
    /// </summary>
    private void BindInputs()
    {
        _performedInputBindings["Move"] = context =>
        {
            MovementVector = context.ReadValue<Vector2>();

            if (useMovementVectorForLook)
                LookVector = MovementVector;
        };

        _canceledInputBindings["Move"] = context =>
        {
            running = false;
            MovementVector = Vector2.zero;
        };

        _performedInputBindings["Look"] = context =>
        {
            useMovementVectorForLook = false;
            LookVector = context.ReadValue<Vector2>();
        };

        _canceledInputBindings["Look"] = context =>
        {
            useMovementVectorForLook = true;

            if (MovementVector.magnitude > 0f)
                LookVector = MovementVector;
        };

        _performedInputBindings["Cursor"] = context =>
        {
            if (useMovementVectorForLook)
                return;

            SetLookVectorFromCursor(context.ReadValue<Vector2>());
        };

        _performedInputBindings["Run"] = context => { running = !running; };

        _performedInputBindings["Attack"] = context => { Attack(); };

        _performedInputBindings["ToggleCursor"] = context =>
        {
            useMovementVectorForLook = !useMovementVectorForLook;

            if (useMovementVectorForLook)
                LookVector = MovementVector;
            else
                SetLookVectorFromCursor(Mouse.current.position.ReadValue());
        };


        foreach (var inputBinding in _performedInputBindings)
        {
            _playerInput.actions[inputBinding.Key].performed += inputBinding.Value;
        }

        foreach (var inputBinding in _canceledInputBindings)
        {
            _playerInput.actions[inputBinding.Key].canceled += inputBinding.Value;
        }
    }

    /// <summary>
    /// Removes all input bindings
    /// </summary>
    private void BreakInputBindings()
    {
        foreach (var inputBinding in _performedInputBindings)
        {
            _playerInput.actions[inputBinding.Key].performed -= inputBinding.Value;
        }

        foreach (var inputBinding in _canceledInputBindings)
        {
            _playerInput.actions[inputBinding.Key].canceled -= inputBinding.Value;
        }
    }

    /// <summary>
    /// Calculates look vector using mouse position
    /// </summary>
    /// <param name="cursorPosition"></param>
    private void SetLookVectorFromCursor(Vector2 cursorPosition)
    {
        Debug.Assert(Camera.main != null, "Main camera is missing tag \"MainCamera\" or does not exist");
        Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        LookVector = (cursorPosition - playerScreenPosition).normalized;
    }

    /// <summary>
    /// Rebinds inputs after HotReload
    /// </summary>
    [AfterHotReload]
    private static void HotReloadRebinding()
    {
        var player = FindObjectOfType<PlayerController>();

        player.InputSetup();
    }
}