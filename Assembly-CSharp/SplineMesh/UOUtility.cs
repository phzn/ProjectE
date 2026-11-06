using System;
using System.Linq;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x0200019F RID: 415
	public static class UOUtility
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00057850 File Offset: 0x00055A50
		public static GameObject Create(string name, GameObject parent, params Type[] components)
		{
			return new GameObject(name, components)
			{
				transform = 
				{
					parent = parent.transform,
					localPosition = Vector3.zero,
					localScale = Vector3.one,
					localRotation = Quaternion.identity
				}
			};
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x000578A5 File Offset: 0x00055AA5
		public static GameObject Instantiate(GameObject prefab, Transform parent)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(prefab, parent);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			return gameObject;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00027EE2 File Offset: 0x000260E2
		public static void Destroy(GameObject go)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(go);
				return;
			}
			Object.DestroyImmediate(go);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00027EE2 File Offset: 0x000260E2
		public static void Destroy(Component comp)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(comp);
				return;
			}
			Object.DestroyImmediate(comp);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x000578E0 File Offset: 0x00055AE0
		public static void DestroyChildren(GameObject go)
		{
			foreach (Transform transform in go.transform.Cast<Transform>().ToList<Transform>())
			{
				UOUtility.Destroy(transform.gameObject);
			}
		}
	}
}
