using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.LiveObjects;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private GameInputActions _input;
    private bool _interactionPressed = false;

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
        Laptop.onHackComplete += PlayerToLaptop;
        Laptop.onHackEnded += LaptopToPlayer;
    }
    // Start is called before the first frame update
    void Start()
    {
        _input = new GameInputActions();
        _input.Player.Enable();
        _input.Player.Interaction.performed += Interaction_performed;
        _input.Player.Interaction.canceled += Interaction_canceled;
        
    }

    private void Interaction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _interactionPressed = false;
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _interactionPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2 GetPlayerMovement() 
    {
        return _input.Player.Movement.ReadValue<Vector2>();
    }
    public bool IsInteractionPressed() 
    {
        return _interactionPressed; 
    }

    private void PlayerToLaptop() 
    {
        _input.Player.Disable();
        _input.Laptop.Enable();
    }
    private void LaptopToPlayer() 
    {
        _input.Laptop.Disable();
        _input.Player.Enable();
    }


}
