using System;
using UnityEngine;

namespace UnityEditor.Rendering.Universal
{
	// Token: 0x02000075 RID: 117
	internal static class MaterialAccess
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x0001BF22 File Offset: 0x0001A122
		internal static int ReadMaterialRawRenderQueue(Material mat)
		{
			return mat.rawRenderQueue;
		}
	}
}
