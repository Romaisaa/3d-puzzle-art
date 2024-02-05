using System.Collections.Generic;
using UnityEngine;


	public class PuzzleGenerator : MonoBehaviour
	{
		public int widthNum = 3;
		public int heightNum = 4;
		public float distBetweenPieces = 2.1f;

		public GameObject startPiece;
		public GameObject endPiece;
		public List<GameObject> puzzlePieces = new();

		private List<GameObject> Puzzle = new();

		private Vector3 startCoordinates = Vector3.zero;
		private const string START = "start";
		private const string END = "end";
		private const string PIECE = "peice";

		public void Generate()
		{
			DeleteExistingPuzzle();

			startCoordinates = transform.position;

			for (int i = 0; i < heightNum; i++)
			{
				for (int j = 0; j < widthNum; j++)
				{
					if (i == 0 && j == 0)
					{
						SpawnPiece(startPiece, i, j, START);
					}
					else if (i == heightNum - 1 && j == widthNum - 1)
					{
						SpawnPiece(endPiece, i, j, END);
					}
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

			Puzzle.Clear();

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
