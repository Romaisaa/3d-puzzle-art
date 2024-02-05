using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

    public class PuzzleMaster : MonoBehaviour
    {
        public bool completed;

        public Color DefaultPipeColour = Color.grey;
        public Color DefaultPieceColour = Color.grey;
        public Color ConnectedPipeColour = Color.blue;
        public Color SelectedPieceColour = Color.yellow;

        public List<PuzzleConnect> endConnectors;
        public UnityEvent onPuzzleComplete;
        private List<GameObject> endConnectorsGO;
       

        private void Start()
        {
            endConnectorsGO = new List<GameObject>(GameObject.FindGameObjectsWithTag("EndConnector"));
            endConnectors = GetPuzzleConnectFromGOList(endConnectorsGO);
        }

        // check for puzzle complete
        private void Update()
        {
            CheckForPuzzleComplete();
        }

        private void CheckForPuzzleComplete()
        {
            completed = true;
            foreach (var endConnector in endConnectors)
            {
                if (!endConnector.IsReceiver)
                {
                    // if at least one connector is not a receiver then it means the puzzle is not completed and break 
                    completed = false;
                    return;
                }
            }

            if(completed)
            {
                onPuzzleComplete.Invoke();
             
            }

        }


        private List<PuzzleConnect> GetPuzzleConnectFromGOList(List<GameObject> list)
        {
            int length = list.Count;
            if (length <= 0) return new();

            List<PuzzleConnect> newList = new();

            for (int i = 0; i < length; i++)
            {
                newList.Add(list[i].GetComponent<PuzzleConnect>());
            }
            return newList;
        }
    }
