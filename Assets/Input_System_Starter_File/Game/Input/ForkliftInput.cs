using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.LiveObjects;
using Game.Scripts.Player;


public class ForkliftInput : MonoBehaviour
{
    private GameInputs _input;
    [SerializeField]
    private Forklift _forklift;

    private void OnEnable()
    {
        Forklift.onDriveModeEntered += ForkliftControls;
        Forklift.onDriveModeExited += PlayerControls;
    }
    private void OnDisable()
    {
        Forklift.onDriveModeEntered -= ForkliftControls;
        Forklift.onDriveModeExited -= PlayerControls;
    }


    

    // Start is called before the first frame update
    void Start()
    {
        _input = new GameInputs();

        _input.Forklift.Escape.performed += Escape_performed;
    }

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _forklift.ExitDriveMode();
    }

    // Update is called once per frame
    void Update()
    {
        _forklift.Move(_input.Forklift.Movement.ReadValue<Vector2>());
        _forklift.LiftControls(_input.Forklift.RaiseLowerForks.ReadValue<float>());
    }

    private void ForkliftControls() 
    {
        _input.Forklift.Enable();
    }
    private void PlayerControls() 
    {
        _input.Forklift.Disable();
    }
}
