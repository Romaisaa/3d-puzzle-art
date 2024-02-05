using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;


    public class PuzzlePiece : MonoBehaviour
    {

        [HideInInspector]
        public List<PuzzleConnect> connectors = new();
        [HideInInspector]
        public PuzzleConnect receiver;


        // private variables
        private PuzzleMaster puzzleMaster;
        private Material pieceMaterial;
        private float rotDegrees = 90f;
        private float rotTime = 0.8f;
        private float verticalMoveTime = 0.3f;
        private float rotWaitTime = 1f;
        private float moveDownWaitTime = 2f;
        private float distUp = 0.5f;
        private bool isRotating;
        private Vector3 startPos;
        private Vector3 upPos;


        private void Start()
        {



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

        
        public void OnSelect(){
            if (puzzleMaster.completed) return;
            if (name.Contains("Start")) return;
            if (name.Contains("End")) return;
            if(!isRotating)
            {
                StartCoroutine(MoveVertically(startPos, upPos, verticalMoveTime));
                StartCoroutine(Rotate(rotDegrees, rotWaitTime, rotTime));
                StartCoroutine(MoveVertically(upPos, startPos, verticalMoveTime, moveDownWaitTime));
                isRotating = true;
            }
        }

        private void Update()
        {
            if (name.Contains("Start")) return;
            CheckForConnections();
            ChangePieceColour();
        }
        private void CheckForConnections()
        {
            if (connectors.Count <= 0) return;


            foreach (var connector in connectors)
            {
                if(connector.IsReceiver && !connector.connectedConnector.IsTransmitter)
                {
                    connector.IsReceiver = false;
                    connector.puzzlePiece.receiver = null;
                }
            }
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

        IEnumerator Rotate(float angle, float delayTime, float duration = 1.0f)
        {
            yield return new WaitForSeconds(delayTime);

            


            Vector3 axis = Vector3.up;
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
        

        IEnumerator MoveVertically(Vector3 from, Vector3 to, float duration, float delayTime = 0f)
        {
            if (delayTime != 0)
                yield return new WaitForSeconds(delayTime);

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(from, to, elapsed / duration); 
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = to;
        }
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

        private void ChangeEmissionColour(Material mat, Color color) { mat.SetColor("_Emission_Tint", color); }

        private void ChangeAlbedoColour(Material mat, Color color) { mat.SetColor("_Albedo_Tint", color); }

        
    }
