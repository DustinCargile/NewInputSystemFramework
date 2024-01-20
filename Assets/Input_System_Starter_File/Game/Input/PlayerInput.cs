using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using Game.Scripts.Player;
using Game.Scripts.LiveObjects;

public class PlayerInput : MonoBehaviour
{
    private GameInputs _input;
    [SerializeField]
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _input = new GameInputs();

        _input.Player.Enable();

        Drone.OnEnterFlightMode += DisablePlayerControl;
        Drone.onExitFlightmode += EnablePlayerControl;

        Forklift.onDriveModeEntered += DisablePlayerControl;
        Forklift.onDriveModeExited += EnablePlayerControl;

    }

    // Update is called once per frame
    void Update()
    {
        var move = _input.Player.Movement.ReadValue<Vector2>();
        _player.Move(move);
    }

    void DisablePlayerControl() 
    {
        _input.Player.Disable();    
    }

    void EnablePlayerControl() 
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        Drone.OnEnterFlightMode -= DisablePlayerControl;
        Drone.onExitFlightmode -= EnablePlayerControl;

        Forklift.onDriveModeEntered -= DisablePlayerControl;
        Forklift.onDriveModeExited -= EnablePlayerControl;
    }
}
