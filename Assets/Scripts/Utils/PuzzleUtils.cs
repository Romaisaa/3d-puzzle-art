using System.Reflection;
using UnityEngine;

namespace Utils
{
    public static class PuzzleUtils
    {
#if UNITY_EDITOR
		/// <summary>
		/// Clears log
		/// </summary>
		public static void ClearLog()
		{
			var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			var type = assembly.GetType("UnityEditor.LogEntries");
			var method = type.GetMethod("Clear");
			method.Invoke(new object(), null);
		}
#endif
		/// <summary>
		/// Get the size of the gameobject
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
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

		[System.Serializable]
		public enum Axis
		{
			x, y, z
		}
	}
}

