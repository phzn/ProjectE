using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MoreMountains.Tools
{
	// Token: 0x0200007B RID: 123
	public class MMGridGenerator
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000E2A7 File Offset: 0x0000C4A7
		public static int[,] PrepareGrid(ref int width, ref int height)
		{
			return new int[width, height];
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000E2B2 File Offset: 0x0000C4B2
		public static bool SetGridCoordinate(int[,] grid, int x, int y, int value)
		{
			if (x >= 0 && x <= grid.GetUpperBound(0) && y >= 0 && y <= grid.GetUpperBound(1))
			{
				grid[x, y] = value;
				return true;
			}
			return false;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000E2DC File Offset: 0x0000C4DC
		public static int[,] TilemapToGrid(Tilemap tilemap, int width, int height)
		{
			if (tilemap == null)
			{
				Debug.LogError("[MMGridGenerator] You're trying to convert a tilemap into a grid but didn't specify what tilemap to convert.");
				return null;
			}
			int[,] array = new int[width, height];
			Vector3Int vector3Int = Vector3Int.zero;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					vector3Int.x = i;
					vector3Int.y = j;
					vector3Int += MMTilemapGridRenderer.ComputeOffset(width - 1, height - 1);
					array[i, j] = ((tilemap.GetTile(vector3Int) == null) ? 0 : 1);
				}
			}
			return array;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000E364 File Offset: 0x0000C564
		public static void DebugGrid(int[,] grid, int width, int height)
		{
			string text = "";
			for (int i = height - 1; i >= 0; i--)
			{
				text = text + "line " + i.ToString() + " [";
				for (int j = 0; j < width; j++)
				{
					text += grid[j, i].ToString();
					if (j < width - 1)
					{
						text += ", ";
					}
				}
				text += "]\n";
			}
			MMDebug.DebugLogInfo(text, "", 3, true);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		public static int GetValueAtGridCoordinate(int[,] grid, int x, int y, int errorValue)
		{
			if (x >= 0 && x <= grid.GetUpperBound(0) && y >= 0 && y <= grid.GetUpperBound(1))
			{
				return grid[x, y];
			}
			return errorValue;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000E410 File Offset: 0x0000C610
		public static int[,] InvertGrid(int[,] grid)
		{
			for (int i = 0; i <= grid.GetUpperBound(0); i++)
			{
				for (int j = 0; j <= grid.GetUpperBound(1); j++)
				{
					grid[i, j] = ((grid[i, j] == 0) ? 1 : 0);
				}
			}
			return grid;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000E458 File Offset: 0x0000C658
		public static int[,] SmoothenGrid(int[,] grid)
		{
			int upperBound = grid.GetUpperBound(0);
			int upperBound2 = grid.GetUpperBound(1);
			for (int i = 0; i <= upperBound; i++)
			{
				for (int j = 0; j <= upperBound2; j++)
				{
					int adjacentWallsCount = MMGridGenerator.GetAdjacentWallsCount(grid, i, j);
					if (adjacentWallsCount > 4)
					{
						grid[i, j] = 1;
					}
					else if (adjacentWallsCount < 4)
					{
						grid[i, j] = 0;
					}
				}
			}
			return grid;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
		public static int[,] ApplySafeSpots(int[,] grid, List<MMTilemapGeneratorLayer.MMTilemapGeneratorLayerSafeSpot> safeSpots)
		{
			foreach (MMTilemapGeneratorLayer.MMTilemapGeneratorLayerSafeSpot mmtilemapGeneratorLayerSafeSpot in safeSpots)
			{
				Vector2Int vector2Int = mmtilemapGeneratorLayerSafeSpot.Start;
				int x = vector2Int.x;
				vector2Int = mmtilemapGeneratorLayerSafeSpot.End;
				int num = Mathf.Min(x, vector2Int.x);
				vector2Int = mmtilemapGeneratorLayerSafeSpot.Start;
				int x2 = vector2Int.x;
				vector2Int = mmtilemapGeneratorLayerSafeSpot.End;
				int num2 = Mathf.Max(x2, vector2Int.x);
				vector2Int = mmtilemapGeneratorLayerSafeSpot.Start;
				int y = vector2Int.y;
				vector2Int = mmtilemapGeneratorLayerSafeSpot.End;
				int num3 = Mathf.Min(y, vector2Int.y);
				vector2Int = mmtilemapGeneratorLayerSafeSpot.Start;
				int y2 = vector2Int.y;
				vector2Int = mmtilemapGeneratorLayerSafeSpot.End;
				int num4 = Mathf.Max(y2, vector2Int.y);
				for (int i = num; i < num2; i++)
				{
					for (int j = num3; j < num4; j++)
					{
						MMGridGenerator.SetGridCoordinate(grid, i, j, 0);
					}
				}
			}
			return grid;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000E5C4 File Offset: 0x0000C7C4
		public static int[,] BindGrid(int[,] grid, bool top, bool bottom, bool left, bool right)
		{
			int upperBound = grid.GetUpperBound(0);
			int upperBound2 = grid.GetUpperBound(1);
			if (top)
			{
				for (int i = 0; i <= upperBound; i++)
				{
					grid[i, upperBound2] = 1;
				}
			}
			if (bottom)
			{
				for (int j = 0; j <= upperBound; j++)
				{
					grid[j, 0] = 1;
				}
			}
			if (left)
			{
				for (int k = 0; k <= upperBound2; k++)
				{
					grid[0, k] = 1;
				}
			}
			if (right)
			{
				for (int l = 0; l <= upperBound2; l++)
				{
					grid[upperBound, l] = 1;
				}
			}
			return grid;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000E650 File Offset: 0x0000C850
		public static int GetAdjacentWallsCount(int[,] grid, int x, int y)
		{
			int upperBound = grid.GetUpperBound(0);
			int upperBound2 = grid.GetUpperBound(1);
			int num = 0;
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 1; j <= y + 1; j++)
				{
					if (i >= 0 && i <= upperBound && j >= 0 && j <= upperBound2)
					{
						if (i != x || j != y)
						{
							num += grid[i, j];
						}
					}
					else
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
