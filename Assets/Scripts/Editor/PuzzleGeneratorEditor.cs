using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzlePipes
{
    [CustomEditor(typeof(PuzzleGenerator))]
    public class PuzzleGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PuzzleGenerator puzzleGenerator = (PuzzleGenerator)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Generate Puzzle"))
            {
                puzzleGenerator.Generate();
            }
            if (GUILayout.Button("Delete Puzzle"))
            {
                puzzleGenerator.DeleteExistingPuzzle();
            }
        }
    }

}

