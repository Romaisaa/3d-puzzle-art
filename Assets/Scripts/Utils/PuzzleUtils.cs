using System.Reflection;
using UnityEngine;


    public static class PuzzleUtils
    {
		public static Vector3 GetSize(GameObject gameObject)
		{
			if (gameObject != null)
			{
				var renderer = gameObject.GetComponent<MeshRenderer>();
				if (renderer != null)
				{
					return renderer.bounds.size;
				}
			}

			return Vector3.zero;
		}
	}


