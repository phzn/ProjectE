using System;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000010 RID: 16
	public class StaticChunkProvider : MonoBehaviour, IChunkProvider
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060000A8 RID: 168 RVA: 0x00006F50 File Offset: 0x00005150
		// (remove) Token: 0x060000A9 RID: 169 RVA: 0x00006F88 File Offset: 0x00005188
		public event Action<ChunkInfo> OnChunkLoaded;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060000AA RID: 170 RVA: 0x00006FC0 File Offset: 0x000051C0
		// (remove) Token: 0x060000AB RID: 171 RVA: 0x00006FF8 File Offset: 0x000051F8
		public event Action<ChunkInfo> OnChunkUnloaded;

		// Token: 0x060000AC RID: 172 RVA: 0x0000702D File Offset: 0x0000522D
		public IEnumerable<ChunkInfo> GetActiveChunks()
		{
			foreach (GameObject gameObject in this.chunkObjects)
			{
				if (gameObject != null)
				{
					yield return StaticChunkProvider.BuildInfo(gameObject);
				}
			}
			List<GameObject>.Enumerator enumerator = default(List<GameObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007040 File Offset: 0x00005240
		private static ChunkInfo BuildInfo(GameObject go)
		{
			Bounds bounds = new Bounds(go.transform.position, Vector3.one * 50f);
			foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(renderer.bounds);
			}
			return new ChunkInfo
			{
				id = go.GetInstanceID().ToString(),
				chunkObject = go,
				bounds = bounds
			};
		}

		// Token: 0x040000AD RID: 173
		[Tooltip("Root GameObjects of each chunk. Each must have a ChunkData component.")]
		public List<GameObject> chunkObjects = new List<GameObject>();
	}
}
