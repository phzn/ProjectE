using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Rendering;

namespace UnityEngine.UI
{
	// Token: 0x02000039 RID: 57
	public static class StencilMaterial
	{
		// Token: 0x06000437 RID: 1079 RVA: 0x00014BCD File Offset: 0x00012DCD
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Material.Add instead.", true)]
		public static Material Add(Material baseMat, int stencilID)
		{
			return null;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00014BD0 File Offset: 0x00012DD0
		public static Material Add(Material baseMat, int stencilID, StencilOp operation, CompareFunction compareFunction, ColorWriteMask colorWriteMask)
		{
			return StencilMaterial.Add(baseMat, stencilID, operation, compareFunction, colorWriteMask, 255, 255);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00014BE7 File Offset: 0x00012DE7
		private static void LogWarningWhenNotInBatchmode(string warning, Object context)
		{
			if (!Application.isBatchMode)
			{
				Debug.LogWarning(warning, context);
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00014BF8 File Offset: 0x00012DF8
		public static Material Add(Material baseMat, int stencilID, StencilOp operation, CompareFunction compareFunction, ColorWriteMask colorWriteMask, int readMask, int writeMask)
		{
			if ((stencilID <= 0 && colorWriteMask == ColorWriteMask.All) || baseMat == null)
			{
				return baseMat;
			}
			if (!baseMat.HasProperty("_Stencil"))
			{
				StencilMaterial.LogWarningWhenNotInBatchmode("Material " + baseMat.name + " doesn't have _Stencil property", baseMat);
				return baseMat;
			}
			if (!baseMat.HasProperty("_StencilOp"))
			{
				StencilMaterial.LogWarningWhenNotInBatchmode("Material " + baseMat.name + " doesn't have _StencilOp property", baseMat);
				return baseMat;
			}
			if (!baseMat.HasProperty("_StencilComp"))
			{
				StencilMaterial.LogWarningWhenNotInBatchmode("Material " + baseMat.name + " doesn't have _StencilComp property", baseMat);
				return baseMat;
			}
			if (!baseMat.HasProperty("_StencilReadMask"))
			{
				StencilMaterial.LogWarningWhenNotInBatchmode("Material " + baseMat.name + " doesn't have _StencilReadMask property", baseMat);
				return baseMat;
			}
			if (!baseMat.HasProperty("_StencilWriteMask"))
			{
				StencilMaterial.LogWarningWhenNotInBatchmode("Material " + baseMat.name + " doesn't have _StencilWriteMask property", baseMat);
				return baseMat;
			}
			if (!baseMat.HasProperty("_ColorMask"))
			{
				StencilMaterial.LogWarningWhenNotInBatchmode("Material " + baseMat.name + " doesn't have _ColorMask property", baseMat);
				return baseMat;
			}
			int count = StencilMaterial.m_List.Count;
			for (int i = 0; i < count; i++)
			{
				StencilMaterial.MatEntry matEntry = StencilMaterial.m_List[i];
				if (matEntry.baseMat == baseMat && matEntry.stencilId == stencilID && matEntry.operation == operation && matEntry.compareFunction == compareFunction && matEntry.readMask == readMask && matEntry.writeMask == writeMask && matEntry.colorMask == colorWriteMask)
				{
					matEntry.count++;
					return matEntry.customMat;
				}
			}
			StencilMaterial.MatEntry matEntry2 = new StencilMaterial.MatEntry();
			matEntry2.count = 1;
			matEntry2.baseMat = baseMat;
			matEntry2.customMat = new Material(baseMat);
			matEntry2.customMat.hideFlags = HideFlags.HideAndDontSave;
			matEntry2.stencilId = stencilID;
			matEntry2.operation = operation;
			matEntry2.compareFunction = compareFunction;
			matEntry2.readMask = readMask;
			matEntry2.writeMask = writeMask;
			matEntry2.colorMask = colorWriteMask;
			matEntry2.useAlphaClip = operation != StencilOp.Keep && writeMask > 0;
			matEntry2.customMat.name = string.Format("Stencil Id:{0}, Op:{1}, Comp:{2}, WriteMask:{3}, ReadMask:{4}, ColorMask:{5} AlphaClip:{6} ({7})", new object[] { stencilID, operation, compareFunction, writeMask, readMask, colorWriteMask, matEntry2.useAlphaClip, baseMat.name });
			matEntry2.customMat.SetFloat("_Stencil", (float)stencilID);
			matEntry2.customMat.SetFloat("_StencilOp", (float)operation);
			matEntry2.customMat.SetFloat("_StencilComp", (float)compareFunction);
			matEntry2.customMat.SetFloat("_StencilReadMask", (float)readMask);
			matEntry2.customMat.SetFloat("_StencilWriteMask", (float)writeMask);
			matEntry2.customMat.SetFloat("_ColorMask", (float)colorWriteMask);
			matEntry2.customMat.SetFloat("_UseUIAlphaClip", matEntry2.useAlphaClip ? 1f : 0f);
			if (matEntry2.useAlphaClip)
			{
				matEntry2.customMat.EnableKeyword("UNITY_UI_ALPHACLIP");
			}
			else
			{
				matEntry2.customMat.DisableKeyword("UNITY_UI_ALPHACLIP");
			}
			StencilMaterial.m_List.Add(matEntry2);
			return matEntry2.customMat;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00014F3C File Offset: 0x0001313C
		public static void Remove(Material customMat)
		{
			if (customMat == null)
			{
				return;
			}
			int count = StencilMaterial.m_List.Count;
			for (int i = 0; i < count; i++)
			{
				StencilMaterial.MatEntry matEntry = StencilMaterial.m_List[i];
				if (!(matEntry.customMat != customMat))
				{
					StencilMaterial.MatEntry matEntry2 = matEntry;
					int num = matEntry2.count - 1;
					matEntry2.count = num;
					if (num == 0)
					{
						Misc.DestroyImmediate(matEntry.customMat);
						matEntry.baseMat = null;
						StencilMaterial.m_List.RemoveAt(i);
					}
					return;
				}
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00014FB8 File Offset: 0x000131B8
		public static void ClearAll()
		{
			int count = StencilMaterial.m_List.Count;
			for (int i = 0; i < count; i++)
			{
				StencilMaterial.MatEntry matEntry = StencilMaterial.m_List[i];
				Misc.DestroyImmediate(matEntry.customMat);
				matEntry.baseMat = null;
			}
			StencilMaterial.m_List.Clear();
		}

		// Token: 0x0400016E RID: 366
		private static List<StencilMaterial.MatEntry> m_List = new List<StencilMaterial.MatEntry>();

		// Token: 0x020000B1 RID: 177
		private class MatEntry
		{
			// Token: 0x04000316 RID: 790
			public Material baseMat;

			// Token: 0x04000317 RID: 791
			public Material customMat;

			// Token: 0x04000318 RID: 792
			public int count;

			// Token: 0x04000319 RID: 793
			public int stencilId;

			// Token: 0x0400031A RID: 794
			public StencilOp operation;

			// Token: 0x0400031B RID: 795
			public CompareFunction compareFunction = CompareFunction.Always;

			// Token: 0x0400031C RID: 796
			public int readMask;

			// Token: 0x0400031D RID: 797
			public int writeMask;

			// Token: 0x0400031E RID: 798
			public bool useAlphaClip;

			// Token: 0x0400031F RID: 799
			public ColorWriteMask colorMask;
		}
	}
}
