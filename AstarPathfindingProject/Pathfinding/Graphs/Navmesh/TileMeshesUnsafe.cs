using System;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001D3 RID: 467
	public struct TileMeshesUnsafe
	{
		// Token: 0x06000D17 RID: 3351 RVA: 0x0004B240 File Offset: 0x00049440
		public TileMeshesUnsafe(NativeArray<TileMesh.TileMeshUnsafe> tileMeshes, IntRect tileRect, Vector2 tileWorldSize)
		{
			this.tileMeshes = tileMeshes;
			this.tileRect = tileRect;
			this.tileWorldSize = tileWorldSize;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0004B258 File Offset: 0x00049458
		public TileMeshes ToManaged()
		{
			TileMesh[] array = new TileMesh[this.tileMeshes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.tileMeshes[i].ToManaged();
			}
			return new TileMeshes
			{
				tileMeshes = array,
				tileRect = this.tileRect,
				tileWorldSize = this.tileWorldSize
			};
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0004B2CC File Offset: 0x000494CC
		public void Dispose(Allocator allocator)
		{
			if (!this.tileMeshes.IsCreated)
			{
				return;
			}
			for (int i = 0; i < this.tileMeshes.Length; i++)
			{
				this.tileMeshes[i].Dispose(allocator);
			}
			this.tileMeshes.Dispose();
		}

		// Token: 0x040008CB RID: 2251
		public NativeArray<TileMesh.TileMeshUnsafe> tileMeshes;

		// Token: 0x040008CC RID: 2252
		public IntRect tileRect;

		// Token: 0x040008CD RID: 2253
		public Vector2 tileWorldSize;
	}
}
