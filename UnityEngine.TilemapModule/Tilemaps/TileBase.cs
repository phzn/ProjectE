using System;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	// Token: 0x0200000A RID: 10
	[RequiredByNativeCode]
	public abstract class TileBase : ScriptableObject
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002815 File Offset: 0x00000A15
		[RequiredByNativeCode]
		public virtual void RefreshTile(Vector3Int position, ITilemap tilemap)
		{
			tilemap.RefreshTile(position);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002101 File Offset: 0x00000301
		[RequiredByNativeCode]
		public virtual void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002820 File Offset: 0x00000A20
		private TileData GetTileDataNoRef(Vector3Int position, ITilemap tilemap)
		{
			TileData tileData = default(TileData);
			this.GetTileData(position, tilemap, ref tileData);
			return tileData;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002848 File Offset: 0x00000A48
		[RequiredByNativeCode]
		public virtual bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			return false;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000285C File Offset: 0x00000A5C
		private TileAnimationData GetTileAnimationDataNoRef(Vector3Int position, ITilemap tilemap)
		{
			TileAnimationData tileAnimationData = default(TileAnimationData);
			this.GetTileAnimationData(position, tilemap, ref tileAnimationData);
			return tileAnimationData;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002882 File Offset: 0x00000A82
		[RequiredByNativeCode]
		private void GetTileAnimationDataRef(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData, ref bool hasAnimation)
		{
			hasAnimation = this.GetTileAnimationData(position, tilemap, ref tileAnimationData);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002894 File Offset: 0x00000A94
		[RequiredByNativeCode]
		public virtual bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
		{
			return false;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000028A7 File Offset: 0x00000AA7
		[RequiredByNativeCode]
		private void StartUpRef(Vector3Int position, ITilemap tilemap, GameObject go, ref bool startUpInvokedByUser)
		{
			startUpInvokedByUser = this.StartUp(position, tilemap, go);
		}
	}
}
