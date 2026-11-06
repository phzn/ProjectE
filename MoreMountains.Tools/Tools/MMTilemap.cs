using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MoreMountains.Tools
{
	// Token: 0x0200008B RID: 139
	public class MMTilemap : MonoBehaviour
	{
		// Token: 0x06000318 RID: 792 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public static Vector2 GetRandomPosition(Tilemap targetTilemap, Grid grid, int width, int height, bool shouldBeFilled = true, int maxIterations = 1000)
		{
			int i = 0;
			Vector3Int vector3Int = Vector3Int.zero;
			while (i < maxIterations)
			{
				vector3Int.x = Random.Range(0, width);
				vector3Int.y = Random.Range(0, height);
				vector3Int += MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
				if (targetTilemap.HasTile(vector3Int) == shouldBeFilled)
				{
					return targetTilemap.CellToWorld(vector3Int) + grid.cellSize / 2f;
				}
				i++;
			}
			return Vector2.zero;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000FD58 File Offset: 0x0000DF58
		public static Vector2 GetRandomPositionOnGround(Tilemap targetTilemap, Grid grid, int width, int height, int startingHeight, int xMin, int xMax, bool shouldBeFilled = true, int maxIterations = 1000)
		{
			int i = 0;
			Vector3Int vector3Int = Vector3Int.zero;
			while (i < maxIterations)
			{
				vector3Int.x = Random.Range(xMin, xMax);
				vector3Int.y = startingHeight;
				vector3Int += MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
				for (int j = height; j > 0; j--)
				{
					int num;
					if (targetTilemap.HasTile(vector3Int) == shouldBeFilled)
					{
						num = vector3Int.y;
						vector3Int.y = num + 1;
						return targetTilemap.CellToWorld(vector3Int) + grid.cellSize / 2f;
					}
					num = vector3Int.y;
					vector3Int.y = num - 1;
				}
				i++;
			}
			return Vector2.zero;
		}
	}
}
