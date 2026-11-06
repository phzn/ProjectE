using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace Features.Flash
{
	// Token: 0x0200120A RID: 4618
	public static class DonorSkeleton
	{
		// Token: 0x06008EA4 RID: 36516 RVA: 0x003A52AC File Offset: 0x003A34AC
		public static int Detach(GameObject instance, IReadOnlyDictionary<string, Transform> liveBones, Object logContext = null)
		{
			if (instance == null || liveBones == null)
			{
				return 0;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in instance.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				foreach (Transform transform in skinnedMeshRenderer.bones)
				{
					if (transform != null)
					{
						hashSet.Add(transform);
					}
				}
				if (skinnedMeshRenderer.rootBone != null)
				{
					hashSet.Add(skinnedMeshRenderer.rootBone);
				}
			}
			foreach (MagicaCloth magicaCloth in instance.GetComponentsInChildren<MagicaCloth>(true))
			{
				hashSet.UnionWith(magicaCloth.GetUsedTransform());
			}
			List<Transform> list = new List<Transform>();
			DonorSkeleton.CollectRoots(instance.transform, liveBones, hashSet, list);
			foreach (Transform transform2 in list)
			{
				transform2.SetParent(null, false);
				Object.Destroy(transform2.gameObject);
			}
			if (list.Count > 0)
			{
				Debug.Log(string.Format("[DonorSkeleton] '{0}': scheletro donatore rimosso dalla gerarchia di Ciel ({1} root, es. '{2}').", instance.name, list.Count, list[0].name), logContext);
			}
			return list.Count;
		}

		// Token: 0x06008EA5 RID: 36517 RVA: 0x003A5404 File Offset: 0x003A3604
		private static void CollectRoots(Transform parent, IReadOnlyDictionary<string, Transform> liveBones, HashSet<Transform> inUse, List<Transform> result)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				Transform transform2;
				if (liveBones.TryGetValue(transform.name, out transform2) && transform2 != transform && transform.GetComponentsInChildren<Renderer>(true).Length == 0 && !DonorSkeleton.SubtreeContainsAny(transform, inUse))
				{
					result.Add(transform);
				}
				else
				{
					DonorSkeleton.CollectRoots(transform, liveBones, inUse, result);
				}
			}
		}

		// Token: 0x06008EA6 RID: 36518 RVA: 0x003A5498 File Offset: 0x003A3698
		private static bool SubtreeContainsAny(Transform root, HashSet<Transform> set)
		{
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (set.Contains(transform))
				{
					return true;
				}
			}
			return false;
		}
	}
}
