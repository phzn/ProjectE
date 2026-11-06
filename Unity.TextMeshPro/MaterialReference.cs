using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200000A RID: 10
	public struct MaterialReference
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000025AC File Offset: 0x000007AC
		public MaterialReference(int index, TMP_FontAsset fontAsset, TMP_SpriteAsset spriteAsset, Material material, float padding)
		{
			this.index = index;
			this.fontAsset = fontAsset;
			this.spriteAsset = spriteAsset;
			this.material = material;
			this.isDefaultMaterial = material.GetInstanceID() == fontAsset.material.GetInstanceID();
			this.isFallbackMaterial = false;
			this.fallbackMaterial = null;
			this.padding = padding;
			this.referenceCount = 0;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002614 File Offset: 0x00000814
		public static bool Contains(MaterialReference[] materialReferences, TMP_FontAsset fontAsset)
		{
			int instanceID = fontAsset.GetInstanceID();
			int num = 0;
			while (num < materialReferences.Length && materialReferences[num].fontAsset != null)
			{
				if (materialReferences[num].fontAsset.GetInstanceID() == instanceID)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002664 File Offset: 0x00000864
		public static int AddMaterialReference(Material material, TMP_FontAsset fontAsset, ref MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int count;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out count))
			{
				return count;
			}
			count = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = count;
			if (count >= materialReferences.Length)
			{
				Array.Resize<MaterialReference>(ref materialReferences, Mathf.NextPowerOfTwo(count + 1));
			}
			materialReferences[count].index = count;
			materialReferences[count].fontAsset = fontAsset;
			materialReferences[count].spriteAsset = null;
			materialReferences[count].material = material;
			materialReferences[count].isDefaultMaterial = instanceID == fontAsset.material.GetInstanceID();
			materialReferences[count].referenceCount = 0;
			return count;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002710 File Offset: 0x00000910
		public static int AddMaterialReference(Material material, TMP_SpriteAsset spriteAsset, ref MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int count;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out count))
			{
				return count;
			}
			count = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = count;
			if (count >= materialReferences.Length)
			{
				Array.Resize<MaterialReference>(ref materialReferences, Mathf.NextPowerOfTwo(count + 1));
			}
			materialReferences[count].index = count;
			materialReferences[count].fontAsset = materialReferences[0].fontAsset;
			materialReferences[count].spriteAsset = spriteAsset;
			materialReferences[count].material = material;
			materialReferences[count].isDefaultMaterial = true;
			materialReferences[count].referenceCount = 0;
			return count;
		}

		// Token: 0x04000010 RID: 16
		public int index;

		// Token: 0x04000011 RID: 17
		public TMP_FontAsset fontAsset;

		// Token: 0x04000012 RID: 18
		public TMP_SpriteAsset spriteAsset;

		// Token: 0x04000013 RID: 19
		public Material material;

		// Token: 0x04000014 RID: 20
		public bool isDefaultMaterial;

		// Token: 0x04000015 RID: 21
		public bool isFallbackMaterial;

		// Token: 0x04000016 RID: 22
		public Material fallbackMaterial;

		// Token: 0x04000017 RID: 23
		public float padding;

		// Token: 0x04000018 RID: 24
		public int referenceCount;
	}
}
