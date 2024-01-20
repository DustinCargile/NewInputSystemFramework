using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects
{
    public class Drone : MonoBehaviour
    {
        private enum Tilt
        {
            NoTilt, Forward, Back, Left, Right
        }

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        private bool _inFlightMode = false;
        [SerializeField]
        private Animator _propAnim;
        [SerializeField]
        private CinemachineVirtualCamera _droneCam;
        [SerializeField]
        private InteractableZone _interactableZone;

        private GameInputs _input;

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode;
        }

        private void Start()
        {
            _input = new GameInputs();
        }

        private void EnterFlightMode(InteractableZone zone)
        {
            if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                _interactableZone.CompleteTask(4);
               
                _input.Drone.Enable();
            }
        }

        private void ExitFlightMode()
        {            
            _droneCam.Priority = 9;
            _inFlightMode = false;
            UIManager.Instance.DroneView(false);
            _input.Drone.Disable();
            
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            //gravity
            _rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
        
        }

       

        public void UpAndDownThrust(float direction) 
        {
            if (_inFlightMode) 
            {
                _rigidbody.AddForce(transform.up * _speed * direction, ForceMode.Acceleration);
            }
                
        }
        public void Rotate(float direction) 
        {
            if (_inFlightMode) 
            {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y += direction * (_speed / 3);
                transform.localRotation = Quaternion.Euler(tempRot);
            }
        }

        /*private void CalculateTilt()
        {
            if (Input.GetKey(KeyCode.A)) 
                transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            else if (Input.GetKey(KeyCode.D))
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            else if (Input.GetKey(KeyCode.W))
                transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            else if (Input.GetKey(KeyCode.S))
                transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            else 
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        }*/
        public void Move(Vector2 direction)
        {
            if (_inFlightMode) 
            {   

                var move = new Vector3(direction.y,0,direction.x);
                _rigidbody.AddForce(direction * _speed, ForceMode.Acceleration);
                
                transform.rotation = Quaternion.Euler(move.x * 30,transform.localRotation.eulerAngles.y,-move.z * 30);
            }

                
        }
        public void EscapePressed() 
        {
            _inFlightMode = false;
            onExitFlightmode?.Invoke();
            ExitFlightMode();
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
        }
    }
}
