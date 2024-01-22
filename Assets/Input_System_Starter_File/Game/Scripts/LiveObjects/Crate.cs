using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.LiveObjects
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private float _punchDelay;
        [SerializeField] private GameObject _wholeCrate, _brokenCrate;
        [SerializeField] private Rigidbody[] _pieces;
        [SerializeField] private BoxCollider _crateCollider;
        [SerializeField] private InteractableZone _interactableZone;
        private bool _isReadyToBreak = false;

        private List<Rigidbody> _brakeOff = new List<Rigidbody>();

        private InteractableZone _zone;
        
        
        private float _holdTime = 0;
        private float _maxHoldTime = 5f;

        private bool _holding = false;
        private void OnEnable()
        {
            InteractableZone.OnHoldFinished += BreakCrate;
            InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
            
        }

        private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
        {
            _zone = zone;
            _holding = true;
            Debug.Log("Holding: " + _holding);
        }

    

        private void BreakCrate(InteractableZone zone)
        {
            _zone = zone;
            
            _holding = false ;
            if (_isReadyToBreak == false && _brakeOff.Count > 0)
            {
                

                _wholeCrate.SetActive(false);
                _brokenCrate.SetActive(true);
                _isReadyToBreak = true;
            }

            if (_isReadyToBreak && _zone.GetZoneID() == 6) //Crate zone            
            {
                if (_brakeOff.Count > 0)
                {                    
                    BreakPart((int)_holdTime);
                    StartCoroutine(PunchDelay());
                }
                else if (_brakeOff.Count == 0)
                {
                    _isReadyToBreak = false;
                    _crateCollider.enabled = false;
                    _interactableZone.CompleteTask(6);
                    Debug.Log("Completely Busted");
                }
            }
            _holdTime = 0;
        }

        private void Start()
        {
            _brakeOff.AddRange(_pieces);
            
        }
        private void Update()
        {
            if (_holding)
            {
                _holdTime += Time.deltaTime;
                _holdTime = Mathf.Clamp(_holdTime, 1, _maxHoldTime);
                
            }
        }


        public void BreakPart(int parts)
        {
            for (int i = 0; i <= parts; i++) 
            {
                if (_brakeOff.Count >= parts) 
                {
                    
                    int rng = Random.Range(0, _brakeOff.Count);
                    _brakeOff[rng].constraints = RigidbodyConstraints.None;
                    _brakeOff[rng].AddForce(new Vector3(1f, 1f, 1f), ForceMode.Force);
                    _brakeOff.Remove(_brakeOff[rng]);
                     
                }
            }
            
                        
        }

        IEnumerator PunchDelay()
        {
            float delayTimer = 0;
            while (delayTimer < _punchDelay)
            {
                yield return new WaitForEndOfFrame();
                delayTimer += Time.deltaTime;
            }

            _interactableZone.ResetAction(6);
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
            InteractableZone.OnHoldFinished -= BreakCrate;
        }
    }
}
