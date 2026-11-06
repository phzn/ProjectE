using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem
{
	// Token: 0x02000008 RID: 8
	public static class LODGroupHelper
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002A09 File Offset: 0x00000C09
		public static int Count(this LODGroup group, Func<Renderer, bool> filter)
		{
			return group.GetLODs().Count(filter);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A18 File Offset: 0x00000C18
		public static int Count(this LOD[] lods, Func<Renderer, bool> filter)
		{
			int num = 0;
			for (int i = 0; i < lods.Length; i++)
			{
				foreach (Renderer renderer in lods[i].renderers)
				{
					if (!(renderer == null) && filter(renderer))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A70 File Offset: 0x00000C70
		public static bool ContainsAny(this LOD[] lods, Func<Renderer, bool> filter)
		{
			for (int i = 0; i < lods.Length; i++)
			{
				Renderer[] renderers = lods[i].renderers;
				for (int j = 0; j < renderers.Length; j++)
				{
					if (filter(renderers[j]))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
