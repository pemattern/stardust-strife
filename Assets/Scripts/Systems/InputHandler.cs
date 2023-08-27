using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IUnitController
{
    public static InputHandler Instance;

    public Vector3 Rotation { get; private set; }
    public Vector3 Movement {get; private set; }

    public bool Fire { get; private set; }
    public bool AlternateFire {get; private set; }
    public bool Boost { get; private set; }

    private InputActions _inputActions;

    void Awake()
    {
        Instance = this;
        _inputActions = new InputActions();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.Pitch.performed += OnPitchPerformed;
        _inputActions.Player.Pitch.canceled += OnPitchCancelled;

        _inputActions.Player.Yaw.performed += OnYawPerformed;
        _inputActions.Player.Yaw.canceled += OnYawCancelled;

        _inputActions.Player.Roll.performed += OnRollPerformed;
        _inputActions.Player.Roll.canceled += OnRollCancelled;

        _inputActions.Player.Thrust.performed += OnThrustPerformed;
        _inputActions.Player.Thrust.canceled += OnThrustCancelled;

        _inputActions.Player.Fire.performed += OnFirePerformed;
        _inputActions.Player.Fire.canceled += OnFireCancelled;

        _inputActions.Player.AlternateFire.performed += OnAlternateFirePerformed;
        _inputActions.Player.AlternateFire.canceled += OnAlternateFireCancelled;

        _inputActions.Player.Boost.performed += OnBoostPerformed;
        _inputActions.Player.Boost.canceled += OnBoostCancelled;

        _inputActions.Player.Quit.performed += Quit;
    }

    void OnDisable()
    {
        _inputActions.Disable();

        _inputActions.Player.Pitch.performed -= OnPitchPerformed;
        _inputActions.Player.Pitch.canceled -= OnPitchCancelled;

        _inputActions.Player.Yaw.performed -= OnYawPerformed;
        _inputActions.Player.Yaw.canceled -= OnYawCancelled;

        _inputActions.Player.Roll.performed -= OnRollPerformed;
        _inputActions.Player.Roll.canceled -= OnRollCancelled;

        _inputActions.Player.Thrust.performed -= OnThrustPerformed;
        _inputActions.Player.Thrust.canceled -= OnThrustCancelled;

        _inputActions.Player.Fire.performed -= OnFirePerformed;
        _inputActions.Player.Fire.canceled -= OnFireCancelled;

        _inputActions.Player.AlternateFire.performed -= OnAlternateFirePerformed;
        _inputActions.Player.AlternateFire.canceled -= OnAlternateFireCancelled;

        _inputActions.Player.Boost.performed -= OnBoostPerformed;
        _inputActions.Player.Boost.canceled -= OnBoostCancelled;

        _inputActions.Player.Quit.performed -= Quit;
    }

    void OnPitchPerformed(InputAction.CallbackContext context) => Rotation = new Vector3(context.ReadValue<float>(), Rotation.y, Rotation.z);
    void OnPitchCancelled(InputAction.CallbackContext context) => Rotation = new Vector3(0f, Rotation.y, Rotation.z);

    void OnYawPerformed(InputAction.CallbackContext context) => Rotation = new Vector3(Rotation.x, context.ReadValue<float>(), Rotation.z);
    void OnYawCancelled(InputAction.CallbackContext context) => Rotation = new Vector3(Rotation.x, 0f, Rotation.z);

    void OnRollPerformed(InputAction.CallbackContext context) => Rotation = new Vector3(Rotation.x, Rotation.y, context.ReadValue<float>());
    void OnRollCancelled(InputAction.CallbackContext context) => Rotation = new Vector3(Rotation.x, Rotation.y, 0f);

    void OnThrustPerformed(InputAction.CallbackContext context) => Movement = new Vector3(0f, 0f, context.ReadValue<float>());
    void OnThrustCancelled(InputAction.CallbackContext context) => Movement = Vector3.zero;

    void OnFirePerformed(InputAction.CallbackContext context) => Fire = context.ReadValue<float>() > 0f ? true : false;
    void OnFireCancelled(InputAction.CallbackContext context) => Fire = false;

    void OnAlternateFirePerformed(InputAction.CallbackContext context) => AlternateFire = context.ReadValue<float>() > 0f ? true : false;
    void OnAlternateFireCancelled(InputAction.CallbackContext context) => AlternateFire = false;

    void OnBoostPerformed(InputAction.CallbackContext context) => Boost = context.ReadValue<float>() > 0f ? true : false;
    void OnBoostCancelled(InputAction.CallbackContext context) => Boost = false;

    void Quit(InputAction.CallbackContext context) => Application.Quit();
}
