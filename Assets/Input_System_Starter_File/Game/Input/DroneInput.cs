using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.LiveObjects;

public class DroneInput : MonoBehaviour
{
    private GameInputs _input;

    [SerializeField]
    private Drone _drone;


    // Start is called before the first frame update
    void Start()
    {
        _input = new GameInputs();
        _input.Drone.Enable();
        _input.Drone.Escape.performed += Escape_performed;
    }

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _drone.EscapePressed();
    }

    // Update is called once per frame
    void Update()
    {
        _drone.Move(_input.Drone.Movement.ReadValue<Vector2>());
        _drone.Rotate(_input.Drone.Rotate.ReadValue<float>());
    }

    private void FixedUpdate()
    {
        _drone.UpAndDownThrust(_input.Drone.Thrust.ReadValue<float>());
    }
}
