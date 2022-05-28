using System;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;

    public float pickupRange;

    private ActorController actor;

    GameObject camera;

    GameObject player;

    private void Awake()
    {
        actor = GetComponent<ActorController>();
        _playerInput = FindObjectOfType<PlayerInput>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.Find("Character Transforms");
    }

    /// <summary>
    /// Used to determinate if look vector should be equal MovementVector
    /// </summary>
    private bool _useMovementVectorForLook = true;

    /// <summary>
    /// Stores bindings for performed inputs
    /// </summary>
    private readonly Dictionary<string, Action<InputAction.CallbackContext>> _performedInputBindings = new Dictionary<string, Action<InputAction.CallbackContext>>();

    /// <summary>
    /// Stores bindings for canceled inputs
    /// </summary>
    private readonly Dictionary<string, Action<InputAction.CallbackContext>> _canceledInputBindings = new Dictionary<string, Action<InputAction.CallbackContext>>();


    void OnEnable()
    {
        InputSetup();
    }

    private void OnDisable()
    {
        BreakInputBindings();
    }

    private void InputSetup()
    {
        // Cache PlayerInput
        BindInputs();
    }

    /// <summary>
    /// Binds actions to Player Inputs using binding names
    /// </summary>
    private void BindInputs()
    {
        

        _performedInputBindings["Move"] = context =>
        {
            actor.MovementVector = context.ReadValue<Vector2>();

            if (_useMovementVectorForLook)
                actor.LookVector = actor.MovementVector;
        };

        _canceledInputBindings["Move"] = context =>
        {
            actor.running = false;
            actor.MovementVector = Vector2.zero;
        };

        _performedInputBindings["Look"] = context =>
        {
            _useMovementVectorForLook = false;
            actor.LookVector = context.ReadValue<Vector2>();
        };

        _canceledInputBindings["Look"] = context =>
        {
            _useMovementVectorForLook = true;

            if (actor.MovementVector.magnitude > 0f)
                actor.LookVector = actor.MovementVector;
        };

        _performedInputBindings["Cursor"] = context =>
        {
            if (_useMovementVectorForLook)
                return;

            SetLookVectorFromCursor(context.ReadValue<Vector2>());
        };

        _performedInputBindings["Run"] = context => { actor.running = !actor.running; };

        _performedInputBindings["Attack"] = context => { actor.Attack(); };

        //testing
        _performedInputBindings["Interact"] = context =>
        {
            ItemInfo itemInfo = ItemSearcher.findClosestItem(transform.position);

            if (itemInfo.distance != -1 && itemInfo.distance <= pickupRange)
            {
                Pickup(itemInfo.item);
                itemInfo.item.GetComponent<ItemDisplay>().item.Use(gameObject);//using item
                Destroy(itemInfo.item);
            }
        };

        _performedInputBindings["Equipment"] = context =>
        {
            if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
            {
                
                camera.SetActive(false);
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                camera.SetActive(true);
                SceneManager.UnloadSceneAsync(1);

            }
        };

        _performedInputBindings["ToggleCursor"] = context =>
        {
            _useMovementVectorForLook = !_useMovementVectorForLook;

            if (_useMovementVectorForLook)
                actor.LookVector = actor.MovementVector;
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
        if (_playerInput == null)
            return;

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

        actor.LookVector = (cursorPosition - playerScreenPosition).normalized;
    }

    private void Pickup(GameObject item)
    {
        this.GetComponent<Equipment>().items.Add(item.GetComponent<ItemDisplay>().item);
        this.GetComponent<Equipment>().ShowEquipment();
    }
}