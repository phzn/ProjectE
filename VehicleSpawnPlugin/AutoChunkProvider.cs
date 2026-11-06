using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x0200000C RID: 12
	public class AutoChunkProvider : MonoBehaviour, IChunkProvider
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000091 RID: 145 RVA: 0x00006C50 File Offset: 0x00004E50
		// (remove) Token: 0x06000092 RID: 146 RVA: 0x00006C88 File Offset: 0x00004E88
		public event Action<ChunkInfo> OnChunkLoaded;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000093 RID: 147 RVA: 0x00006CC0 File Offset: 0x00004EC0
		// (remove) Token: 0x06000094 RID: 148 RVA: 0x00006CF8 File Offset: 0x00004EF8
		public event Action<ChunkInfo> OnChunkUnloaded;

		// Token: 0x06000095 RID: 149 RVA: 0x00006D2D File Offset: 0x00004F2D
		private void OnEnable()
		{
			ChunkData.GlobalChunkEnabled += this.HandleChunkEnabled;
			ChunkData.GlobalChunkDisabled += this.HandleChunkDisabled;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006D51 File Offset: 0x00004F51
		private void OnDisable()
		{
			ChunkData.GlobalChunkEnabled -= this.HandleChunkEnabled;
			ChunkData.GlobalChunkDisabled -= this.HandleChunkDisabled;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006D75 File Offset: 0x00004F75
		public IEnumerable<ChunkInfo> GetActiveChunks()
		{
			foreach (ChunkData chunkData in Object.FindObjectsOfType<ChunkData>())
			{
				yield return AutoChunkProvider.BuildInfo(chunkData);
			}
			ChunkData[] array = null;
			yield break;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006D7E File Offset: 0x00004F7E
		private void HandleChunkEnabled(ChunkData chunk)
		{
			Action<ChunkInfo> onChunkLoaded = this.OnChunkLoaded;
			if (onChunkLoaded == null)
			{
				return;
			}
			onChunkLoaded(AutoChunkProvider.BuildInfo(chunk));
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006D96 File Offset: 0x00004F96
		private void HandleChunkDisabled(ChunkData chunk)
		{
			Action<ChunkInfo> onChunkUnloaded = this.OnChunkUnloaded;
			if (onChunkUnloaded == null)
			{
				return;
			}
			onChunkUnloaded(AutoChunkProvider.BuildInfo(chunk));
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006DB0 File Offset: 0x00004FB0
		private static ChunkInfo BuildInfo(ChunkData chunk)
		{
			Bounds bounds = new Bounds(chunk.transform.position, Vector3.one * 50f);
			foreach (Renderer renderer in chunk.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}
			return new ChunkInfo
			{
				id = chunk.GetInstanceID().ToString(),
				chunkObject = chunk.gameObject,
				bounds = bounds
			};
		}
	}
}
