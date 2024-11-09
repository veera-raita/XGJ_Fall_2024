using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, InputMap.IMovementActions, InputMap.IDialogueActions, InputMap.IGameOverActions
{
    private InputMap inputMap;

    //EVENTS
    public event Action MoveEvent;
    public event Action MoveCanceledEvent;
    public event Action ToggleLightEvent;
    public event Action LookBigEvent;
    public event Action LookBigCanceledEvent;
    public event Action TurnLightEvent;

    public event Action NextDialogueEvent;
    public event Action SkipDialogueEvent;

    public event Action RestartEvent;
    public event Action QuitEvent;

    public void EnableMovement()
    {
        inputMap.Movement.Enable();
        inputMap.Dialogue.Disable();
    }

    public void DisableMovement()
    {
        inputMap.Movement.Disable();
    }

    public void EnableDialogue()
    {
        inputMap.Movement.Disable();
        inputMap.Dialogue.Enable();
    }

    public void SetGameOver()
    {
        inputMap.Dialogue.Disable();
        inputMap.GameOver.Enable();
    }

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new InputMap();

            inputMap.Movement.SetCallbacks(this);
            inputMap.Dialogue.SetCallbacks(this);
            inputMap.GameOver.SetCallbacks(this);
            EnableDialogue();
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

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            NextDialogueEvent?.Invoke();
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            SkipDialogueEvent?.Invoke();
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            RestartEvent?.Invoke();
        }
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            QuitEvent?.Invoke();
        }
    }
}
