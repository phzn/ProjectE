using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000006 RID: 6
	public static class TerrainExtensions
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00002280 File Offset: 0x00000480
		public static void UpdateGIMaterials(this Terrain terrain)
		{
			bool flag = terrain.terrainData == null;
			if (flag)
			{
				throw new ArgumentException("Invalid terrainData.");
			}
			TerrainExtensions.UpdateGIMaterialsForTerrain(terrain.GetInstanceID(), new Rect(0f, 0f, 1f, 1f));
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000022D0 File Offset: 0x000004D0
		public static void UpdateGIMaterials(this Terrain terrain, int x, int y, int width, int height)
		{
			bool flag = terrain.terrainData == null;
			if (flag)
			{
				throw new ArgumentException("Invalid terrainData.");
			}
			float num = (float)terrain.terrainData.alphamapWidth;
			float num2 = (float)terrain.terrainData.alphamapHeight;
			TerrainExtensions.UpdateGIMaterialsForTerrain(terrain.GetInstanceID(), new Rect((float)x / num, (float)y / num2, (float)width / num, (float)height / num2));
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002335 File Offset: 0x00000535
		[FreeFunction]
		[NativeConditional("INCLUDE_DYNAMIC_GI && ENABLE_RUNTIME_GI")]
		internal static void UpdateGIMaterialsForTerrain(int terrainInstanceID, Rect uvBounds)
		{
			TerrainExtensions.UpdateGIMaterialsForTerrain_Injected(terrainInstanceID, ref uvBounds);
		}

		// Token: 0x0600007F RID: 127
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UpdateGIMaterialsForTerrain_Injected(int terrainInstanceID, ref Rect uvBounds);
	}
}
