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

    private float _duration = 0;
    private float _holdTime = 0;
    private bool _holding = false;

    public static event Action<int> OnBreakCrate;

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
        _input.Player.HoldInteraction.performed += HoldInteraction_performed;
        _input.Player.HoldInteraction.canceled += HoldInteraction_canceled;
        _input.Player.HoldInteraction.started += HoldInteraction_started;
        _input.Drone.Escape.performed += Escape_performed;
        _input.Drone.Escape.canceled += Escape_canceled;
        _input.Forklift.Escape.performed += Escape_performed;
        _input.Forklift.Escape.canceled += Escape_canceled;

        
    }

   

    private void HoldInteraction_started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        
    }

    private void HoldInteraction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnBreakCrate?.Invoke(Mathf.Clamp((int)context.duration, 1, 4));


    }

    private void HoldInteraction_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

        OnBreakCrate?.Invoke(Mathf.Clamp((int)context.duration,1,4));
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
        _holdTime += Time.deltaTime;
        
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

    //-----------------Crate Interactions------------------------
    public int GetDuration() { return (int)_duration; }
}
