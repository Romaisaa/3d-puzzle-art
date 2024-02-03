using UnityEngine;
using Utils;

namespace PuzzlePipes
{
    [CreateAssetMenu(fileName = "NewPuzzlePiece", menuName = "ScriptableObjects/NewPuzzlePiece", order = 1)]
    public class PuzzlePieceScriptable : ScriptableObject
    {
        [Tooltip("Axis to rotate")]
        public PuzzleUtils.Axis axisToRotate;
        [Tooltip("Degrees to rotate")]
        public float rotDegrees = 90f;
        [Tooltip("Rotation Time")]
        public float rotTime = 0.8f;
        [Tooltip("Vertical movement time")]
        public float verticalMoveTime = 0.3f;
        [Tooltip("wait to rotate time")]
        public float rotWaitTime = 1f;
        [Tooltip("wait to move vertically down time")]
        public float moveDownWaitTime = 2f;
        [Tooltip("Distance to go up")]
        public float distUp = 1f;
        [Tooltip("Move Up SFX")]
        public AudioClip moveUpSFX;
        [Tooltip("Rotate SFX")]
        public AudioClip rotateSFX;
        [Tooltip("Move Down SFX")]
        public AudioClip moveDownSFX;
    }
}