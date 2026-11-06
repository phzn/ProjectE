using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public static class ZoneSizeExtensions
{
	// Token: 0x060006FA RID: 1786 RVA: 0x00031E80 File Offset: 0x00030080
	public static Vector2Int GetGridDimensions(this ChunkSize chunkSize)
	{
		switch (chunkSize)
		{
		case ChunkSize.Size1x1:
			return new Vector2Int(1, 1);
		case ChunkSize.Size2x2:
			return new Vector2Int(2, 2);
		case ChunkSize.Size3x3:
			return new Vector2Int(3, 3);
		case ChunkSize.Size4x4:
			return new Vector2Int(4, 4);
		default:
			return new Vector2Int(1, 1);
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00031ECC File Offset: 0x000300CC
	public static Vector2 GetPhysicalSize(this ChunkSize chunkSize, float cellSize = 60f)
	{
		Vector2Int gridDimensions = chunkSize.GetGridDimensions();
		return new Vector2((float)gridDimensions.x * cellSize, (float)gridDimensions.y * cellSize);
	}
}
