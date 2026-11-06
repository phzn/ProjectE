using System;
using System.Diagnostics;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine
{
	// Token: 0x02000009 RID: 9
	[MovedFrom("UnityEngine.Experimental.TerrainAPI")]
	public static class TerrainCallbacks
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000085 RID: 133 RVA: 0x00002354 File Offset: 0x00000554
		// (remove) Token: 0x06000086 RID: 134 RVA: 0x00002388 File Offset: 0x00000588
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event TerrainCallbacks.HeightmapChangedCallback heightmapChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000087 RID: 135 RVA: 0x000023BC File Offset: 0x000005BC
		// (remove) Token: 0x06000088 RID: 136 RVA: 0x000023F0 File Offset: 0x000005F0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event TerrainCallbacks.TextureChangedCallback textureChanged;

		// Token: 0x06000089 RID: 137 RVA: 0x00002424 File Offset: 0x00000624
		[RequiredByNativeCode]
		internal static void InvokeHeightmapChangedCallback(TerrainData terrainData, RectInt heightRegion, bool synched)
		{
			bool flag = TerrainCallbacks.heightmapChanged != null;
			if (flag)
			{
				foreach (Terrain terrain in terrainData.users)
				{
					TerrainCallbacks.heightmapChanged(terrain, heightRegion, synched);
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002468 File Offset: 0x00000668
		[RequiredByNativeCode]
		internal static void InvokeTextureChangedCallback(TerrainData terrainData, string textureName, RectInt texelRegion, bool synched)
		{
			bool flag = TerrainCallbacks.textureChanged != null;
			if (flag)
			{
				foreach (Terrain terrain in terrainData.users)
				{
					TerrainCallbacks.textureChanged(terrain, textureName, texelRegion, synched);
				}
			}
		}

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x0600008C RID: 140
		public delegate void HeightmapChangedCallback(Terrain terrain, RectInt heightRegion, bool synched);

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x06000090 RID: 144
		public delegate void TextureChangedCallback(Terrain terrain, string textureName, RectInt texelRegion, bool synched);
	}
}
