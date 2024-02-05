using UnityEngine;
using System.Collections;

    public class PuzzleConnect : MonoBehaviour
    {
        public bool IsTransmitter;
        public bool IsReceiver;
 
        [HideInInspector]
        public PuzzleConnect connectedConnector;


        
        [HideInInspector]
        public PuzzlePiece puzzlePiece;

        private bool wasReceiver;

        private void Start()
        {
            puzzlePiece = transform.parent.GetComponent<PuzzlePiece>();
        }

        private void Update()
        {
            if (wasReceiver && !IsReceiver)
            {
                puzzlePiece.receiver = null;
                wasReceiver = false;
            }

        }

        private void OnTriggerStay(Collider other)
        {
            if (transform.parent.name.Contains("Start")) return;

            if (other.CompareTag("PuzzleConnect"))
            {
                connectedConnector = other.transform.GetComponent<PuzzleConnect>();

                if (connectedConnector.IsTransmitter)
                {
                    IsReceiver = true;
                    wasReceiver = true;
                    puzzlePiece.receiver = this;
                }
                else
                {
                    IsReceiver = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (transform.parent.name.Contains("Start")) return;

            if (other.CompareTag("PuzzleConnect"))
            {
                if(IsReceiver)
                {
                    puzzlePiece.receiver = null;
                }
                
                connectedConnector = null;
                IsReceiver = false;
            }
        }
        
    }
