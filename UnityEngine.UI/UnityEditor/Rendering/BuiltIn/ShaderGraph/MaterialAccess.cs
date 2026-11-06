using System;
using UnityEngine;

namespace UnityEditor.Rendering.BuiltIn.ShaderGraph
{
	// Token: 0x02000076 RID: 118
	internal static class MaterialAccess
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x0001BF2A File Offset: 0x0001A12A
		internal static int ReadMaterialRawRenderQueue(Material mat)
		{
			return mat.rawRenderQueue;
		}
	}
}
