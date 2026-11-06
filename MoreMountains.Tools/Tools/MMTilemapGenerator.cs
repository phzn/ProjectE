using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000084 RID: 132
	[ExecuteAlways]
	public class MMTilemapGenerator : MonoBehaviour
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000EE58 File Offset: 0x0000D058
		public virtual void Generate()
		{
			Random.InitState((int)DateTime.Now.Ticks);
			if (this.RandomizeGlobalSeed)
			{
				this.GlobalSeed = Mathf.Abs(Random.Range(int.MinValue, int.MaxValue));
			}
			foreach (MMTilemapGeneratorLayer mmtilemapGeneratorLayer in this.Layers)
			{
				this.GenerateLayer(mmtilemapGeneratorLayer);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		private void Reset()
		{
			this.Layers = new MMTilemapGeneratorLayerList
			{
				new MMTilemapGeneratorLayer()
			};
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		protected virtual void GenerateLayer(MMTilemapGeneratorLayer layer)
		{
			if (!layer.Active)
			{
				return;
			}
			if (layer.TargetTilemap == null)
			{
				Debug.LogError("Tilemap Generator : you need to specify a Target Tilemap to paint on.");
			}
			if (layer.Tile == null)
			{
				Debug.LogError("Tilemap Generator : you need to specify a Tile to paint with.");
			}
			if (layer.GridWidth == 0)
			{
				Debug.LogError("Tilemap Generator : grid width can't be 0.");
			}
			if (layer.GridHeight == 0)
			{
				Debug.LogError("Tilemap Generator : grid height can't be 0.");
			}
			float num = 0f;
			Random.InitState(this.GlobalSeed);
			int num2 = (layer.OverrideGridSize ? layer.GridWidth : Random.Range(this.GridWidth.x, this.GridWidth.y));
			int num3 = (layer.OverrideGridSize ? layer.GridHeight : Random.Range(this.GridHeight.x, this.GridHeight.y));
			float value = Random.value;
			if (layer.DoNotUseGlobalSeed)
			{
				Random.InitState((int)DateTime.Now.Ticks);
				if (layer.RandomizeSeed)
				{
					layer.Seed = Mathf.Abs(Random.Range(int.MinValue, int.MaxValue));
				}
				Random.InitState(layer.Seed);
				num = Random.value;
			}
			int num4 = (layer.DoNotUseGlobalSeed ? layer.Seed : this.GlobalSeed);
			float num5 = (layer.DoNotUseGlobalSeed ? num : value);
			switch (layer.GenerateMethod)
			{
			case MMTilemapGenerator.GenerateMethods.Full:
				this._grid = MMGridGeneratorFull.Generate(num2, num3, layer.FullGenerationFilled);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.Perlin:
				this._grid = MMGridGeneratorPerlinNoise.Generate(num2, num3, num5);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.PerlinGround:
				this._grid = MMGridGeneratorPerlinNoiseGround.Generate(num2, num3, num5);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.Random:
				this._grid = MMGridGeneratorRandom.Generate(num2, num3, num4, layer.RandomFillPercentage);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.RandomWalk:
				this._grid = MMGridGeneratorRandomWalk.Generate(num2, num3, num4, layer.RandomWalkPercent, layer.RandomWalkStartingPoint, layer.RandomWalkMaxIterations);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.RandomWalkAvoider:
			{
				int[,] array = MMGridGenerator.TilemapToGrid(layer.RandomWalkAvoiderObstaclesTilemap, num2, num3);
				this._grid = MMGridGeneratorRandomWalkAvoider.Generate(num2, num3, num4, layer.RandomWalkAvoiderPercent, layer.RandomWalkAvoiderStartingPoint, array, layer.RandomWalkAvoiderObstaclesDistance, layer.RandomWalkAvoiderMaxIterations);
				layer.Grid = this._grid;
				break;
			}
			case MMTilemapGenerator.GenerateMethods.RandomWalkGround:
				this._grid = MMGridGeneratorRandomWalkGround.Generate(num2, num3, num4, layer.RandomWalkGroundMinHeightDifference, layer.RandomWalkGroundMaxHeightDifference, layer.RandomWalkGroundMinFlatDistance, layer.RandomWalkGroundMaxFlatDistance, layer.RandomWalkGroundMaxHeight);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.Path:
				this._grid = MMGridGeneratorPath.Generate(num2, num3, num4, layer.PathDirection, layer.PathStartPosition, layer.PathMinWidth, layer.PathMaxWidth, layer.PathDirectionChangeDistance, layer.PathWidthChangePercentage, layer.PathDirectionChangePercentage);
				layer.Grid = this._grid;
				break;
			case MMTilemapGenerator.GenerateMethods.Copy:
				layer.TargetTilemap.ClearAllTiles();
				MMTilemapGenerator.DelayedCopy(layer);
				break;
			}
			if (layer.Smooth)
			{
				this._grid = MMGridGenerator.SmoothenGrid(this._grid);
			}
			if (layer.InvertGrid)
			{
				this._grid = MMGridGenerator.InvertGrid(this._grid);
			}
			this._grid = MMGridGenerator.BindGrid(this._grid, layer.BoundsTop, layer.BoundsBottom, layer.BoundsLeft, layer.BoundsRight);
			this._grid = MMGridGenerator.ApplySafeSpots(this._grid, layer.SafeSpots);
			this.RenderGrid(layer);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
		private static async void DelayedCopy(MMTilemapGeneratorLayer layer)
		{
			await Task.Delay(500);
			MMTilemapShadow.Copy(layer.CopyTilemap, layer.TargetTilemap);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000F2D7 File Offset: 0x0000D4D7
		protected virtual void RenderGrid(MMTilemapGeneratorLayer layer)
		{
			MMTilemapGridRenderer.RenderGrid(this._grid, layer, this.SlowRender, this.SlowRenderDuration, this.SlowRenderTweenType, this);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		protected virtual void OnValidate()
		{
			if (this.Layers == null || this.Layers.Count <= 0)
			{
				return;
			}
			foreach (MMTilemapGeneratorLayer mmtilemapGeneratorLayer in this.Layers)
			{
				mmtilemapGeneratorLayer.SetDefaults();
			}
		}

		// Token: 0x040003F8 RID: 1016
		[Header("Grid")]
		[Tooltip("The width of the grid, in cells")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2Int GridWidth = new Vector2Int(50, 50);

		// Token: 0x040003F9 RID: 1017
		[Tooltip("the height of the grid, in cells")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2Int GridHeight = new Vector2Int(50, 50);

		// Token: 0x040003FA RID: 1018
		[Header("Data")]
		[Tooltip("the list of layers that will be used to generate the tilemap")]
		public MMTilemapGeneratorLayerList Layers;

		// Token: 0x040003FB RID: 1019
		[Tooltip("a value between 0 and 1 that will be used by all layers as their random seed. If you generate another map using the same seed, it'll look the same")]
		public int GlobalSeed;

		// Token: 0x040003FC RID: 1020
		[Tooltip("whether or not to randomize the global seed every time a new map is generated")]
		public bool RandomizeGlobalSeed = true;

		// Token: 0x040003FD RID: 1021
		[Header("Slow Render")]
		[Tooltip("turning this to true will (at runtime only) draw the map progressively. This is really just for fun.")]
		public bool SlowRender;

		// Token: 0x040003FE RID: 1022
		[Tooltip("the duration of the slow render, in seconds")]
		public float SlowRenderDuration = 1f;

		// Token: 0x040003FF RID: 1023
		[Tooltip("the tween to use for the slow render")]
		public MMTweenType SlowRenderTweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic);

		// Token: 0x04000400 RID: 1024
		protected int[,] _grid;

		// Token: 0x04000401 RID: 1025
		protected int _width;

		// Token: 0x04000402 RID: 1026
		protected int _height;

		// Token: 0x020003EA RID: 1002
		public enum GenerateMethods
		{
			// Token: 0x04001E68 RID: 7784
			Full,
			// Token: 0x04001E69 RID: 7785
			Perlin,
			// Token: 0x04001E6A RID: 7786
			PerlinGround,
			// Token: 0x04001E6B RID: 7787
			Random,
			// Token: 0x04001E6C RID: 7788
			RandomWalk,
			// Token: 0x04001E6D RID: 7789
			RandomWalkAvoider,
			// Token: 0x04001E6E RID: 7790
			RandomWalkGround,
			// Token: 0x04001E6F RID: 7791
			Path,
			// Token: 0x04001E70 RID: 7792
			Copy
		}
	}
}
