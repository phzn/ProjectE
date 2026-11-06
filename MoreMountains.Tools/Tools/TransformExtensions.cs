using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F0 RID: 240
	public static class TransformExtensions
	{
		// Token: 0x06000503 RID: 1283 RVA: 0x00016FD4 File Offset: 0x000151D4
		public static void MMDestroyAllChildren(this Transform transform)
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(transform.GetChild(i).gameObject);
				}
				else
				{
					Object.DestroyImmediate(transform.GetChild(i).gameObject);
				}
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00017020 File Offset: 0x00015220
		public static Transform MMFindDeepChildBreadthFirst(this Transform parent, string transformName)
		{
			Queue<Transform> queue = new Queue<Transform>();
			queue.Enqueue(parent);
			while (queue.Count > 0)
			{
				Transform transform = queue.Dequeue();
				if (transform.name == transformName)
				{
					return transform;
				}
				foreach (object obj in transform)
				{
					Transform transform2 = (Transform)obj;
					queue.Enqueue(transform2);
				}
			}
			return null;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000170A8 File Offset: 0x000152A8
		public static Transform MMFindDeepChildDepthFirst(this Transform parent, string transformName)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (transform.name == transformName)
				{
					return transform;
				}
				Transform transform2 = transform.MMFindDeepChildDepthFirst(transformName);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00017124 File Offset: 0x00015324
		public static void ChangeLayersRecursively(this Transform transform, string layerName)
		{
			transform.gameObject.layer = LayerMask.NameToLayer(layerName);
			foreach (object obj in transform)
			{
				((Transform)obj).ChangeLayersRecursively(layerName);
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00017188 File Offset: 0x00015388
		public static void ChangeLayersRecursively(this Transform transform, int layerIndex)
		{
			transform.gameObject.layer = layerIndex;
			foreach (object obj in transform)
			{
				((Transform)obj).ChangeLayersRecursively(layerIndex);
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000171E8 File Offset: 0x000153E8
		public static IEnumerable<Transform> MMEnumerateAllParents(this Transform targetTransform, bool includeSelf = false)
		{
			if (!includeSelf)
			{
				Transform transform = targetTransform;
				targetTransform = ((transform != null) ? transform.parent : null);
			}
			while (targetTransform)
			{
				yield return targetTransform;
				targetTransform = targetTransform.parent;
			}
			yield break;
		}
	}
}
