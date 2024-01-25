using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Scripts.LiveObjects;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private GameInputActions _input;
    private bool _interactionPressed = false;

    public static Action<bool> OnInteract;
    public static Action<bool> OnEscape;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else 
        {
            Instance = this;
        }

        
        
       
    }
    private void OnEnable()
    {
        InitalizeInput();

        Drone.OnEnterFlightMode += ActivateDroneControls;
        Drone.onExitFlightmode += DeactivateDroneControls;

        Forklift.onDriveModeEntered += ActivateForkliftControls;
        Forklift.onDriveModeExited += DeactivateForkliftContols;
    }

    private void InitalizeInput() 
    {
        _input = new GameInputActions();
        _input.Player.Enable();
        _input.Player.Interaction.performed += Interaction_performed;
        _input.Player.Interaction.canceled += Interaction_canceled;

        _input.Player.Escape.performed += Escape_performed;
        _input.Player.Escape.canceled += Escape_canceled;

        _input.Drone.Escape.performed += Escape_performed;

        
    }

    //=================Escape methods=========================
    private void Escape_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnEscape?.Invoke(false);
    }

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnEscape?.Invoke(true);
    }
    //================Interaction methods=================================
    private void Interaction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(false);
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteract?.Invoke(true);
    }

    //==============Start and Update=================================

    // Start is called before the first frame update
    void Start()
    {
        
       
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //=====================Public Methods======================
    public Vector2 GetPlayerMovement() 
    {
        return _input.Player.Movement.ReadValue<Vector2>();
    }

    ///////////////////////DroneMovement//////////////////
    public Vector2 GetDroneInput() 
    {
        
        return _input.Drone.Movement.ReadValue<Vector2>(); 
    }
    public float GetDroneRotate() 
    {
        return _input.Drone.Rotate.ReadValue<float>();
    }
    public float GetDroneLift() 
    { 
        return _input.Drone.Lift.ReadValue<float>();
    }

    //|-------------Forklift Movemnt---------------------|
    public Vector2 GetForkliftMovement() { return _input.Forklift.Movement.ReadValue<Vector2>(); }

    public float GetForkliftRaiseLower() {return _input.Forklift.LiftControls.ReadValue<float>(); }
    //=====================Private Methods====================

    ///////////Drone Controls////////////
    private void ActivateDroneControls() 
    {
        _input.Drone.Enable();
        _input.Player.Disable();
    }
    private void DeactivateDroneControls() 
    {
        _input.Drone.Disable();
        _input.Player.Enable();
    }


    //|----------Forklift Controls--------------------------|
    private void ActivateForkliftControls() { _input.Forklift.Enable(); _input.Player.Disable(); }
    private void DeactivateForkliftContols() { _input.Forklift.Disable(); _input.Player.Enable(); }
    
    

    //====================Disable Method============================
    private void OnDisable()
    {
        Drone.OnEnterFlightMode -= ActivateDroneControls;
        Drone.onExitFlightmode -= DeactivateDroneControls;
    }
}
