using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, InputMap.IMovementActions
{
    private InputMap inputMap;

    //EVENTS
    public event Action MoveEvent;
    public event Action MoveCanceledEvent;
    public event Action ToggleLightEvent;
    public event Action LookBigEvent;
    public event Action LookBigCanceledEvent;
    public event Action TurnLightEvent;

    public void EnableMovement()
    {
        inputMap.Movement.Enable();
    }

    public void DisableMovement()
    {
        inputMap.Movement.Disable();
    }

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new InputMap();

            inputMap.Movement.SetCallbacks(this);
            inputMap.Movement.Enable();
        }
    }
    public void OnLookBig(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            LookBigEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            LookBigCanceledEvent?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            MoveEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            MoveCanceledEvent?.Invoke();
        }
    }

    public void OnTurnLight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            TurnLightEvent?.Invoke();
        }
    }

    public void OnToggleLight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ToggleLightEvent?.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
