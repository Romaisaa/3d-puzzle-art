using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace PuzzlePipes
{
	public class PuzzleGenerator : MonoBehaviour
	{
		[Header("Puzzle Settings")]
		[Tooltip("Number of puzzle pieces horizonally")]
		public int widthNum = 3;
		[Tooltip("Number of puzzle pieces vertically")]
		public int heightNum = 4;
		[Tooltip("Distance between each puzzle piece. Keep in mind that the bigger distance will not guarantee detection of connections")]
		public float distBetweenPieces = 2.1f;

		[Header("Puzzle Pieces")]
		public GameObject startPiece;
		public GameObject endPiece;
		public List<GameObject> puzzlePieces = new();

		private List<GameObject> Puzzle = new();

		private Vector3 startCoordinates = Vector3.zero;
		private const string START = "start";
		private const string END = "end";
		private const string PIECE = "peice";

		/// <summary>
		/// Generates the puzzle
		/// </summary>
		public void Generate()
		{
			// delete if there is any puzzly in the scene
			DeleteExistingPuzzle();

			// start coordinates is where we build the puzzle, normally we want it where the PuzzleGenerator gameobject is
			startCoordinates = transform.position;


			// puzzle generating process
			for (int i = 0; i < heightNum; i++)
			{
				for (int j = 0; j < widthNum; j++)
				{
					// if this is the start, spawn a start piece here
					if (i == 0 && j == 0)
					{
						SpawnPiece(startPiece, i, j, START);
					}
					// if this is the end, spawn an end piece here
					else if (i == heightNum - 1 && j == widthNum - 1)
					{
						SpawnPiece(endPiece, i, j, END);
					}
					// otherwise spawn a random puzzle piece
					else
					{
						System.Random rand = new System.Random();
						int randIndex = rand.Next(0, puzzlePieces.Count);
						//int randIndex = GetRandomIndex();
						SpawnPiece(puzzlePieces[randIndex], i, j);
					}
				}
			}
		}

		public void DeleteExistingPuzzle()
		{
			// clear everything
#if UNITY_EDITOR
			PuzzleUtils.ClearLog();
#endif
			Puzzle.Clear();

			// delete children of this gameobject, i.e. puzzle pieces
			var tempArray = new GameObject[transform.childCount];

			for (int i = 0; i < tempArray.Length; i++)
			{
				tempArray[i] = transform.GetChild(i).gameObject;
			}

			foreach (var child in tempArray)
			{
				DestroyImmediate(child);
			}
		}

		/// <summary>
		/// Method for spawing a puzzle piece
		/// </summary>
		/// <param name="puzzlePiece"></param>
		/// GameObject to spawn
		/// <param name="i"></param>
		/// vertical position to spawn on
		/// <param name="j"></param>
		/// horizonal position to spawn on
		/// <param name="pieceType"></param>
		/// Start, End or normal piece
		private void SpawnPiece(GameObject puzzlePiece, int i, int j, string pieceType = "def")
		{
			// random rotation for the normal piece
			int[] rotDegrees = new int[] { 0, 90, 180, -90 };

			Vector3 ramdomRot = Vector3.zero;
			System.Random randStart = new System.Random();
			int randIndex = randStart.Next(0, rotDegrees.Length);
			ramdomRot.y = rotDegrees[randIndex];

			Vector3 pos = new(startCoordinates.x + i * (distBetweenPieces + PuzzleUtils.GetSize(startPiece).x), startCoordinates.y, startCoordinates.z + j * (distBetweenPieces + PuzzleUtils.GetSize(puzzlePiece).z));
			GameObject go = Instantiate(puzzlePiece, pos, Quaternion.Euler(ramdomRot), transform);

			if (go == null) // if instantiating failed
			{
				Debug.Log("instantiation failed");
				//SpawnPiece(puzzlePiece, i, j, pieceType);
				return;
			}

			if (pieceType == START)
			{
				if (randIndex <= 1)
					go.transform.LookAt(-Vector3.forward);
				else
					go.transform.LookAt(Vector3.right);
			}
            else if (pieceType == END)
            {
                if (randIndex <= 1)
					go.transform.eulerAngles = new Vector3(0f, -90f, 0f);
				else
					go.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			}

            Puzzle.Add(go);
		}
	}

}
