using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000015 RID: 21
	internal struct MaterialReference
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00006C80 File Offset: 0x00004E80
		public MaterialReference(int index, FontAsset fontAsset, SpriteAsset spriteAsset, Material material, float padding)
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

		// Token: 0x060000BC RID: 188 RVA: 0x00006CE4 File Offset: 0x00004EE4
		public static bool Contains(MaterialReference[] materialReferences, FontAsset fontAsset)
		{
			int instanceID = fontAsset.GetInstanceID();
			int num = 0;
			while (num < materialReferences.Length && materialReferences[num].fontAsset != null)
			{
				bool flag = materialReferences[num].fontAsset.GetInstanceID() == instanceID;
				if (flag)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006D48 File Offset: 0x00004F48
		public static int AddMaterialReference(Material material, FontAsset fontAsset, ref MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int count;
			bool flag = materialReferenceIndexLookup.TryGetValue(instanceID, out count);
			int num;
			if (flag)
			{
				num = count;
			}
			else
			{
				count = materialReferenceIndexLookup.Count;
				materialReferenceIndexLookup[instanceID] = count;
				bool flag2 = count >= materialReferences.Length;
				if (flag2)
				{
					Array.Resize<MaterialReference>(ref materialReferences, Mathf.NextPowerOfTwo(count + 1));
				}
				materialReferences[count].index = count;
				materialReferences[count].fontAsset = fontAsset;
				materialReferences[count].spriteAsset = null;
				materialReferences[count].material = material;
				materialReferences[count].isDefaultMaterial = instanceID == fontAsset.material.GetInstanceID();
				materialReferences[count].referenceCount = 0;
				num = count;
			}
			return num;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00006E08 File Offset: 0x00005008
		public static int AddMaterialReference(Material material, SpriteAsset spriteAsset, ref MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int count;
			bool flag = materialReferenceIndexLookup.TryGetValue(instanceID, out count);
			int num;
			if (flag)
			{
				num = count;
			}
			else
			{
				count = materialReferenceIndexLookup.Count;
				materialReferenceIndexLookup[instanceID] = count;
				bool flag2 = count >= materialReferences.Length;
				if (flag2)
				{
					Array.Resize<MaterialReference>(ref materialReferences, Mathf.NextPowerOfTwo(count + 1));
				}
				materialReferences[count].index = count;
				materialReferences[count].fontAsset = materialReferences[0].fontAsset;
				materialReferences[count].spriteAsset = spriteAsset;
				materialReferences[count].material = material;
				materialReferences[count].isDefaultMaterial = true;
				materialReferences[count].referenceCount = 0;
				num = count;
			}
			return num;
		}

		// Token: 0x0400009A RID: 154
		public int index;

		// Token: 0x0400009B RID: 155
		public FontAsset fontAsset;

		// Token: 0x0400009C RID: 156
		public SpriteAsset spriteAsset;

		// Token: 0x0400009D RID: 157
		public Material material;

		// Token: 0x0400009E RID: 158
		public bool isDefaultMaterial;

		// Token: 0x0400009F RID: 159
		public bool isFallbackMaterial;

		// Token: 0x040000A0 RID: 160
		public Material fallbackMaterial;

		// Token: 0x040000A1 RID: 161
		public float padding;

		// Token: 0x040000A2 RID: 162
		public int referenceCount;
	}
}
