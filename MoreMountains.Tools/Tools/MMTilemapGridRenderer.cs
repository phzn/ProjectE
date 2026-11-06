using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MoreMountains.Tools
{
	// Token: 0x0200008E RID: 142
	public class MMTilemapGridRenderer
	{
		// Token: 0x06000320 RID: 800 RVA: 0x0000FF58 File Offset: 0x0000E158
		public static void RenderGrid(int[,] grid, MMTilemapGeneratorLayer layer, bool slowRender = false, float slowRenderDuration = 1f, MMTweenType slowRenderTweenType = null, MonoBehaviour slowRenderSupport = null)
		{
			if (layer.FusionMode == MMTilemapGeneratorLayer.FusionModes.Normal)
			{
				MMTilemapGridRenderer.ClearTilemap(layer.TargetTilemap);
			}
			TileBase tileBase = layer.Tile;
			if (layer.FusionMode == MMTilemapGeneratorLayer.FusionModes.Combine)
			{
				grid = MMGridGenerator.InvertGrid(grid);
				tileBase = null;
			}
			if (layer.FusionMode == MMTilemapGeneratorLayer.FusionModes.Subtract)
			{
				grid = MMGridGenerator.InvertGrid(grid);
			}
			if (!slowRender || !Application.isPlaying)
			{
				MMTilemapGridRenderer.DrawGrid(grid, layer.TargetTilemap, tileBase, 0, MMTilemapGridRenderer.TotalFilledBlocks(grid));
			}
			else
			{
				slowRenderSupport.StartCoroutine(MMTilemapGridRenderer.SlowRenderGrid(grid, layer.TargetTilemap, tileBase, slowRenderDuration, slowRenderTweenType, 60));
			}
			if (!Application.isPlaying && slowRender)
			{
				Debug.LogWarning("Rendering maps in SlowRender mode is only supported at runtime.");
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000FFF4 File Offset: 0x0000E1F4
		public static IEnumerator SlowRenderGrid(int[,] grid, Tilemap tilemap, TileBase tile, float slowRenderDuration, MMTweenType slowRenderTweenType, int frameRate)
		{
			int totalBlocks = MMTilemapGridRenderer.TotalFilledBlocks(grid);
			totalBlocks = ((totalBlocks == 0) ? 1 : totalBlocks);
			frameRate = ((frameRate == 0) ? 1 : frameRate);
			float refreshFrequency = 1f / (float)frameRate;
			float startedAt = Time.unscaledTime;
			float lastWaitAt = startedAt;
			int drawnBlocks = 0;
			int lastIndex = 0;
			while (Time.unscaledTime - startedAt < slowRenderDuration)
			{
				while (Time.unscaledTime - lastWaitAt < refreshFrequency)
				{
					yield return null;
				}
				float num = (float)(totalBlocks - drawnBlocks);
				float num2 = Time.unscaledTime - startedAt;
				float num3 = slowRenderDuration - num2;
				float num4 = MMMaths.Remap(num2, 0f, slowRenderDuration, 0f, 1f);
				float num5 = MMTween.Tween(num4, 0f, 1f, 0f, 1f, slowRenderTweenType);
				float num6 = 1f - (num4 - num5);
				int num7 = Mathf.RoundToInt(num / num3 * refreshFrequency * num6);
				lastIndex = MMTilemapGridRenderer.DrawGrid(grid, tilemap, tile, lastIndex, num7);
				drawnBlocks += num7;
				lastWaitAt = Time.unscaledTime;
			}
			MMTilemapGridRenderer.DrawGrid(grid, tilemap, tile, lastIndex, totalBlocks - lastIndex);
			yield break;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00010028 File Offset: 0x0000E228
		public static int TotalFilledBlocks(int[,] grid)
		{
			int upperBound = grid.GetUpperBound(0);
			int upperBound2 = grid.GetUpperBound(1);
			int num = 0;
			for (int i = 0; i <= upperBound; i++)
			{
				for (int j = 0; j <= upperBound2; j++)
				{
					if (grid[i, j] == 1)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00010074 File Offset: 0x0000E274
		private static int DrawGrid(int[,] grid, Tilemap tilemap, TileBase tile, int startIndex, int numberOfTilesToDraw)
		{
			int upperBound = grid.GetUpperBound(0);
			int upperBound2 = grid.GetUpperBound(1);
			tilemap.RefreshAllTiles();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i <= upperBound; i++)
			{
				for (int j = 0; j <= upperBound2; j++)
				{
					if (grid[i, j] == 1)
					{
						if (num >= startIndex)
						{
							Vector3Int vector3Int = new Vector3Int(i, j, 0);
							vector3Int += MMTilemapGridRenderer.ComputeOffset(upperBound, upperBound2);
							tilemap.SetTile(vector3Int, tile);
							num2++;
						}
						if (num2 > numberOfTilesToDraw)
						{
							return num;
						}
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00010101 File Offset: 0x0000E301
		public static Vector3Int ComputeOffset(int width, int height)
		{
			Vector3Int vector3Int = new Vector3Int(width + 2, height + 2, 0);
			return -(vector3Int - vector3Int / 2);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00010120 File Offset: 0x0000E320
		public static void ClearTilemap(Tilemap tilemap)
		{
			tilemap.ClearAllTiles();
			tilemap.RefreshAllTiles();
		}
	}
}
