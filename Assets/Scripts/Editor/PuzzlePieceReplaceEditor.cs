using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzlePipes
{
    [CustomEditor(typeof(PuzzlePieceReplace))]
    public class PuzzlePieceReplaceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PuzzlePieceReplace replace = (PuzzlePieceReplace)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Replace"))
            {
                replace.ReplacePiece();
            }
        }
    }

}

