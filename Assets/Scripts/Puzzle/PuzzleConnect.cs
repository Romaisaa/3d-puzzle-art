using UnityEngine;
using System.Collections;

namespace PuzzlePipes
{
    /// <summary>
    /// PuzzleConnect.cs manages each individual connector of the puzzle piece
    /// If a connector is a transmitter, it means that this connector is wired to the start puzzle piece
    /// If a connector is a reveiver, it means that this connector receives 'pipe content' from the transmitter
    /// Transmitters and receivers connect to each other
    /// </summary>
    public class PuzzleConnect : MonoBehaviour
    {
        public bool IsTransmitter;
        public bool IsReceiver;
 
        [HideInInspector]
        [Tooltip("The connector reference that is connected to this connector")]
        public PuzzleConnect connectedConnector;


        
        [HideInInspector]
        public PuzzlePiece puzzlePiece;

        // private variables
        private bool wasReceiver;

        private void Start()
        {
            puzzlePiece = transform.parent.GetComponent<PuzzlePiece>();
        }

        private void Update()
        {
            // if this connector was a receiver but no isn't, remove the receiver reference
            // from the parent puzzle piece
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
                // if this connector touches another connector, add the connected connector reference
                connectedConnector = other.transform.GetComponent<PuzzleConnect>();

                // if the connected connector is a transmitter, set this connector as a receiver
                // and reference this connecter as receiver in the parent puzzle piece
                // and mark this as was receiver, in case it loses the status
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
                // if this connector is a receiver and lost the connection, set the puzzle piece receiver to null
                if(IsReceiver)
                {
                    puzzlePiece.receiver = null;
                }

                // if this connector leaves the other connector, remove the connected connector reference
                // and set IsReceiver to false
                connectedConnector = null;
                IsReceiver = false;
            }
        }
        
    }
}