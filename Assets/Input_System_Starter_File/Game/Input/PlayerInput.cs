using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using Game.Scripts.Player;

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

        
    }

    // Update is called once per frame
    void Update()
    {
        var move = _input.Player.Movement.ReadValue<Vector2>();
        _player.Move(move);
    }
}
