using UnityEngine;

namespace PuzzlePipes
{
    /// <summary>
    /// Replace Puzzle piece with another one quickly and easily
    /// </summary>
    public class PuzzlePieceReplace : MonoBehaviour
    {
        public Pieces ReplaceWith;

        public GameObject Start;
        public GameObject Cross;
        public GameObject T;
        public GameObject L;
        public GameObject Line;
        public GameObject End;

        public void ReplacePiece()
        {
            switch (ReplaceWith)
            {
                case Pieces.Start:
                    InstantiateNewPiece(Start);
                    break;
                case Pieces.CrossShaped:
                    InstantiateNewPiece(Cross);
                    break;
                case Pieces.T_Shaped:
                    InstantiateNewPiece(T);
                    break;
                case Pieces.L_Shaped:
                    InstantiateNewPiece(L);
                    break;
                case Pieces.LineShaped:
                    InstantiateNewPiece(Line);
                    break;
                case Pieces.End:
                    InstantiateNewPiece(End);
                    break;
            }
        }

        private void InstantiateNewPiece(GameObject go)
        {
            GameObject newGo = Instantiate(go, transform.position, Quaternion.identity);
            if(newGo != null)
            {
                // parent this gameobject to the Puzzle main parent
                newGo.transform.parent = FindObjectOfType<PuzzleMaster>().transform;
                DestroyImmediate(gameObject);
            }
                
        }

        [System.Serializable]
        public enum Pieces
        {
            Start,
            CrossShaped,
            T_Shaped,
            L_Shaped,
            LineShaped,
            End
        }
    }

}
