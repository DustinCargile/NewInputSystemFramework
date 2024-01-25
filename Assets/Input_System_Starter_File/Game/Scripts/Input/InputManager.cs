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
    }

    private void InitalizeInput() 
    {
        _input = new GameInputActions();
        _input.Player.Enable();
        _input.Player.Interaction.performed += Interaction_performed;
        _input.Player.Interaction.canceled += Interaction_canceled;

        _input.Player.Escape.performed += Escape_performed;
        _input.Player.Escape.canceled += Escape_canceled;
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
  
    

}
