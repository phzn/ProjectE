using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x0200000D RID: 13
	public class ChunkData : MonoBehaviour
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600009C RID: 156 RVA: 0x00006E40 File Offset: 0x00005040
		// (remove) Token: 0x0600009D RID: 157 RVA: 0x00006E74 File Offset: 0x00005074
		public static event Action<ChunkData> GlobalChunkEnabled;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600009E RID: 158 RVA: 0x00006EA8 File Offset: 0x000050A8
		// (remove) Token: 0x0600009F RID: 159 RVA: 0x00006EDC File Offset: 0x000050DC
		public static event Action<ChunkData> GlobalChunkDisabled;

		// Token: 0x060000A0 RID: 160 RVA: 0x00006F0F File Offset: 0x0000510F
		private void OnEnable()
		{
			Action<ChunkData> globalChunkEnabled = ChunkData.GlobalChunkEnabled;
			if (globalChunkEnabled == null)
			{
				return;
			}
			globalChunkEnabled(this);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006F21 File Offset: 0x00005121
		private void OnDisable()
		{
			Action<ChunkData> globalChunkDisabled = ChunkData.GlobalChunkDisabled;
			if (globalChunkDisabled == null)
			{
				return;
			}
			globalChunkDisabled(this);
		}

		// Token: 0x040000A5 RID: 165
		[Tooltip("All traffic lanes (TrackData) that belong to this chunk.")]
		public List<TrackData> tracks = new List<TrackData>();

		// Token: 0x040000A6 RID: 166
		[Tooltip("Optional per-chunk spawn profile. Overrides the global profile when set.")]
		public SpawnProfile localSpawnProfile;

		// Token: 0x040000A7 RID: 167
		[Tooltip("Total vehicles allowed across all tracks in this chunk.")]
		[Range(1f, 200f)]
		public int maxVehiclesInChunk = 30;
	}
}
