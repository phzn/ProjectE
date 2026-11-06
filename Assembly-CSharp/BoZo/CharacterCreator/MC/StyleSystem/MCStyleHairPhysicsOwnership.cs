using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007F3 RID: 2035
	public sealed class MCStyleHairPhysicsOwnership
	{
		// Token: 0x06003646 RID: 13894 RVA: 0x00138714 File Offset: 0x00136914
		public void Acquire(string partId, GameObject physicalHair, Transform characterRoot, IEnumerable<GameObject> spawnedStyleRoots)
		{
			if (!string.Equals(partId, "Head.Hair", StringComparison.Ordinal) || physicalHair == null || characterRoot == null)
			{
				return;
			}
			if (this._physicalHair != physicalHair)
			{
				this.Release();
			}
			this._physicalHair = physicalHair;
			List<HashSet<Transform>> list = new List<HashSet<Transform>>();
			foreach (MagicaCloth magicaCloth in physicalHair.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth.isActiveAndEnabled && magicaCloth.SerializeData.clothType == ClothProcess.ClothType.BoneCloth)
				{
					HashSet<Transform> hashSet = MCStyleHairPhysicsOwnership.RootSet(magicaCloth);
					if (hashSet.Count > 0)
					{
						list.Add(hashSet);
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			List<Transform> list2 = new List<Transform> { physicalHair.transform };
			if (spawnedStyleRoots != null)
			{
				foreach (GameObject gameObject in spawnedStyleRoots)
				{
					if (gameObject != null)
					{
						list2.Add(gameObject.transform);
					}
				}
			}
			foreach (MagicaCloth magicaCloth2 in characterRoot.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth2.isActiveAndEnabled && !(magicaCloth2.gameObject.name != "HairPhysics") && magicaCloth2.SerializeData.clothType == ClothProcess.ClothType.BoneCloth && !MCStyleHairPhysicsOwnership.IsUnderAny(magicaCloth2.transform, list2))
				{
					HashSet<Transform> hashSet2 = MCStyleHairPhysicsOwnership.RootSet(magicaCloth2);
					if (hashSet2.Count != 0)
					{
						foreach (HashSet<Transform> hashSet3 in list)
						{
							if (hashSet2.SetEquals(hashSet3))
							{
								if (!this._suspended.ContainsKey(magicaCloth2))
								{
									this._suspended.Add(magicaCloth2, magicaCloth2.enabled);
								}
								magicaCloth2.enabled = false;
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x00138910 File Offset: 0x00136B10
		public void Release()
		{
			if (this._physicalHair != null)
			{
				foreach (MagicaCloth magicaCloth in this._physicalHair.GetComponentsInChildren<MagicaCloth>(true))
				{
					if (magicaCloth != null)
					{
						magicaCloth.enabled = false;
					}
				}
			}
			this._physicalHair = null;
			foreach (KeyValuePair<MagicaCloth, bool> keyValuePair in this._suspended)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.enabled = keyValuePair.Value;
				}
			}
			this._suspended.Clear();
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x001389D0 File Offset: 0x00136BD0
		private static HashSet<Transform> RootSet(MagicaCloth cloth)
		{
			HashSet<Transform> hashSet = new HashSet<Transform>();
			if (cloth.SerializeData.rootBones == null)
			{
				return hashSet;
			}
			foreach (Transform transform in cloth.SerializeData.rootBones)
			{
				if (transform == null)
				{
					return new HashSet<Transform>();
				}
				hashSet.Add(transform);
			}
			return hashSet;
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x00138A54 File Offset: 0x00136C54
		private static bool IsUnderAny(Transform candidate, List<Transform> roots)
		{
			foreach (Transform transform in roots)
			{
				if (candidate.IsChildOf(transform))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400363C RID: 13884
		private readonly Dictionary<MagicaCloth, bool> _suspended = new Dictionary<MagicaCloth, bool>();

		// Token: 0x0400363D RID: 13885
		private GameObject _physicalHair;
	}
}
