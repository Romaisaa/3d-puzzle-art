using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using Utils;

namespace PuzzlePipes
{
    /// <summary>
    /// This script is attached to the each individual puzzle piece and manages
    /// its connection status and rotates on click
    /// </summary>
    public class PuzzlePiece : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public PuzzlePieceScriptable puzzlePiece;

        [HideInInspector]
        [Tooltip("This puzzle piece child connectors")]
        public List<PuzzleConnect> connectors = new();
        [HideInInspector]
        public PuzzleConnect receiver;
        public UnityEvent onClick;


        // private variables
        private PuzzleMaster puzzleMaster;
        private Material pieceMaterial;
        private PuzzleUtils.Axis axisToRotate;
        private float rotDegrees;
        private float rotTime;
        private float verticalMoveTime;
        private float rotWaitTime;
        private float moveDownWaitTime;
        private float distUp;
        private AudioClip moveUpSound;
        private AudioClip rotateSound;
        private AudioClip moveDownSound;

        private bool isRotating;
        private Vector3 startPos;
        private Vector3 upPos;

        private AudioSource audioSource;

        private void Start()
        {

            audioSource = GetComponent<AudioSource>();

            GetScriptableData();

            puzzleMaster = transform.parent.GetComponent<PuzzleMaster>();

            // find the geometry gameobject in children
            foreach (Transform child in transform)
                if (child.CompareTag("Geometry"))
                    pieceMaterial = child.GetComponent<MeshRenderer>().material;

            ChangeAlbedoColour(pieceMaterial, puzzleMaster.DefaultPieceColour);

            if (name.Contains("Start"))
            {
                ChangeEmissionColour(pieceMaterial, puzzleMaster.ConnectedPipeColour);
                return;
            }

            // set start position and the up position 
            startPos = transform.position;
            upPos = transform.position + Vector3.up * distUp;

            // add child connectors to connectors
            foreach (Transform child in transform)
                if (child.CompareTag("PuzzleConnect") || child.CompareTag("EndConnector"))
                    connectors.Add(child.GetComponent<PuzzleConnect>());
        }

        // this method is called when the object with collider on it is clicked
        public void OnPointerClick(PointerEventData eventData)
        {
            if (puzzleMaster.completed) return;
            // if the piece is rotating, we can't click on it and rotate again
            if(!isRotating)
            {
                onClick.Invoke();
                // move vertically up
                StartCoroutine(MoveVertically(startPos, upPos, verticalMoveTime));
                // rotate 90 degrees
                StartCoroutine(Rotate(rotDegrees, rotWaitTime, rotTime));
                // move vertically down
                StartCoroutine(MoveVertically(upPos, startPos, verticalMoveTime, moveDownWaitTime));
                isRotating = true;
            }
        }

        // this method is called when the pointer enters this object's collider
        public void OnPointerEnter(PointerEventData eventData)
        {
            ChangeAlbedoColour(pieceMaterial, puzzleMaster.SelectedPieceColour);
        }

        // this method is called when the pointer exits this object's collider
        public void OnPointerExit(PointerEventData eventData)
        {
            ChangeAlbedoColour(pieceMaterial, puzzleMaster.DefaultPieceColour);
        }

        // keep checking the connections in Update method
        private void Update()
        {
            if (name.Contains("Start")) return;
            CheckForConnections();
            ChangePieceColour();
        }
        #region CONNECTIONS
        /// <summary>
        /// Checks the connections of the connectors
        /// </summary>
        private void CheckForConnections()
        {
            // if this piece has no connectors return
            if (connectors.Count <= 0) return;

            // check in connectors: if connector is receiver but the connector it is connected to is no longer a transmitter, then turn of
            // the receiver and set the receiver to null
            foreach (var connector in connectors)
            {
                if(connector.IsReceiver && !connector.connectedConnector.IsTransmitter)
                {
                    connector.IsReceiver = false;
                    connector.puzzlePiece.receiver = null;
                }
            }

            // if this piece has a receiver, mark the other connectors of this piece as transmitters
            if (receiver != null)
            {
                foreach (var connector in connectors)
                {
                    if (connector == receiver) continue;

                    connector.IsTransmitter = true;
                }
            }
            else
            {
                foreach (var connector in connectors)
                {
                    connector.IsTransmitter = false;
                }
            }

        }
        #endregion

        #region ROTATION
        IEnumerator Rotate(float angle, float delayTime, float duration = 1.0f)
        {
            // wait for delayTime till the piece is moved vertically up
            yield return new WaitForSeconds(delayTime);

            // play sound
            if (!audioSource.isPlaying && rotateSound != null)
                audioSource.PlayOneShot(rotateSound);

            // normally we want to rotate the pieces on Y axis, but just in case
            // we want the puzzle positioned differently, we can choose the rotation axis in the inspector
            // the switch statement below checks which axis is selected

            Vector3 axis = Vector3.right;

            switch(axisToRotate)
            {
                case PuzzleUtils.Axis.x:
                    axis = Vector3.right;
                    break;
                case PuzzleUtils.Axis.y:
                    axis = Vector3.up;
                    break;
                case PuzzleUtils.Axis.z:
                    axis = Vector3.forward;
                    break;
            }

            Quaternion from = transform.rotation;
            Quaternion to = transform.rotation;
            to *= Quaternion.Euler(axis * angle);

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = to;
            isRotating = false;
        }
        #endregion

        #region VERTICALMOVEMENT
        IEnumerator MoveVertically(Vector3 from, Vector3 to, float duration, float delayTime = 0f)
        {
            // if moving down, wait for moving up and rotating
            if (delayTime != 0)
                yield return new WaitForSeconds(delayTime);

            // play sound
            if (to == upPos)
            {
                if (!audioSource.isPlaying && moveUpSound != null)
                    audioSource.PlayOneShot(moveUpSound);
            }
            else
            {
                if (!audioSource.isPlaying && moveDownSound != null)
                    audioSource.PlayOneShot(moveDownSound);
            }


            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, to, elapsed / duration); 
                elapsed += Time.deltaTime;
                yield return null;
            }
            // make sure we reached the destination
            transform.position = to;
        }
        #endregion

        /// <summary>
        /// Changes the piece colour if it is connected
        /// </summary>
        private void ChangePieceColour()
        {
            if (receiver != null)
            {
                ChangeEmissionColour(pieceMaterial, puzzleMaster.ConnectedPipeColour);
            }
            else
            {
                ChangeEmissionColour(pieceMaterial, puzzleMaster.DefaultPipeColour);
            }
        }

        /// <summary>
        /// Sets the colour of the material emission that is atteched to this GameObject
        /// </summary>
        private void ChangeEmissionColour(Material mat, Color color) { mat.SetColor("_Emission_Tint", color); }
        /// <summary>
        /// Sets the colour of the material albedo that is atteched to this GameObject
        /// </summary>
        private void ChangeAlbedoColour(Material mat, Color color) { mat.SetColor("_Albedo_Tint", color); }

        /// <summary>
        /// Extracts data from the scriptable object
        /// </summary>
        private void GetScriptableData()
        {
            axisToRotate = puzzlePiece.axisToRotate;
            rotDegrees = puzzlePiece.rotDegrees;
            rotTime = puzzlePiece.rotTime;
            verticalMoveTime = puzzlePiece.verticalMoveTime;
            rotWaitTime = puzzlePiece.rotWaitTime;
            moveDownWaitTime = puzzlePiece.moveDownWaitTime;
            distUp = puzzlePiece.distUp;
            moveUpSound = puzzlePiece.moveUpSFX;
            moveDownSound = puzzlePiece.moveDownSFX;
            rotateSound = puzzlePiece.rotateSFX;
        }
    }
}