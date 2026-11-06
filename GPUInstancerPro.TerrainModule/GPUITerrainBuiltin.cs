using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000015 RID: 21
	[ExecuteInEditMode]
	[RequireComponent(typeof(Terrain))]
	[DefaultExecutionOrder(-200)]
	[DisallowMultipleComponent]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_Terrain")]
	public class GPUITerrainBuiltin : GPUITerrain
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00007BA5 File Offset: 0x00005DA5
		public override void LoadTerrain()
		{
			if (this._terrain == null)
			{
				this._terrain = base.GetComponent<Terrain>();
			}
			base.LoadTerrain();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007BC8 File Offset: 0x00005DC8
		public override bool LoadTerrainData()
		{
			if (!base.LoadTerrainData())
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find TerrainData for terrain: " + this._terrain.name, base.gameObject);
				return false;
			}
			if (this._terrain.terrainData != null)
			{
				this._treePrototypes = this._terrain.terrainData.treePrototypes;
				base.DetermineTreePrototypeIndexes(base.TreeManager);
				this._detailPrototypes = this._terrain.terrainData.detailPrototypes;
				base.DetermineDetailPrototypeIndexes(base.DetailManager);
				this._detailScatterMode = this._terrain.terrainData.detailScatterMode;
				if (this._terrain.treeDistance > 0f)
				{
					this._terrainTreeDistance = this._terrain.treeDistance;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00007C97 File Offset: 0x00005E97
		internal override void SetTerrainDetailObjectDistance(float value)
		{
			base.SetTerrainDetailObjectDistance(value);
			if (this._terrain == null)
			{
				return;
			}
			this._terrain.detailObjectDistance = value;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00007CBB File Offset: 0x00005EBB
		internal override void SetTerrainTreeDistance(float value)
		{
			base.SetTerrainTreeDistance(value);
			if (this._terrain == null)
			{
				return;
			}
			this._terrain.treeDistance = value;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00007CDF File Offset: 0x00005EDF
		protected override RenderTexture LoadHeightmapTexture()
		{
			if (this._terrain == null || this._terrain.terrainData == null)
			{
				return null;
			}
			return this._terrain.terrainData.heightmapTexture;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00007D14 File Offset: 0x00005F14
		protected override void LoadDetailDensityTextures()
		{
			if (this._terrain == null || this._terrain.terrainData == null)
			{
				return;
			}
			this._detailPrototypes = this._terrain.terrainData.detailPrototypes;
			base.DetermineDetailPrototypeIndexes(base.DetailManager);
			int num = ((base.DetailPrototypes == null) ? 0 : base.DetailPrototypes.Length);
			base.ResizeDetailDensityTextureArray(num);
			if (num == 0)
			{
				return;
			}
			this._detailScatterMode = this._terrain.terrainData.detailScatterMode;
			string name = this._terrain.terrainData.name;
			for (int i = 0; i < num; i++)
			{
				this.CreateDetailTexture(name, i);
				if (!base.IsReadTerrainDetails(i))
				{
					this._detailDensityTextures[i].ClearRenderTexture();
				}
				else if (this._isBakedDetailTextures && (this._isCustomBakedDetailTextures || Application.isPlaying))
				{
					base.BlitBakedDetailTexture(i);
				}
				else
				{
					this.CaptureTerrainDetailsToRenderTexture(this._detailDensityTextures[i], i, false);
				}
			}
			base.ExecuteProceduralDetails();
			if (base.DetailManager != null)
			{
				base.DetailManager.RequireUpdate(!Application.isPlaying, false);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007E30 File Offset: 0x00006030
		private void CaptureTerrainDetailsToRenderTexture(RenderTexture rt, int detailLayer, bool captureWithComputeDetailInstanceTransforms = false)
		{
			if (captureWithComputeDetailInstanceTransforms)
			{
				GPUITerrainUtility.CaptureTerrainDetailToRenderTextureWithComputeDetailInstanceTransforms(this._terrain.terrainData, detailLayer, (base.DetailPrototypes[detailLayer].useDensityScaling ? this._terrain.detailObjectDensity : 1f) * base.DetailPrototypes[detailLayer].density, rt, this.terrainHolesSampleMode == GPUITerrain.GPUITerrainHolesSampleMode.Initialization);
				return;
			}
			GPUITerrainUtility.CaptureTerrainDetailToRenderTexture(this._terrain.terrainData, detailLayer, rt, this.terrainHolesSampleMode == GPUITerrain.GPUITerrainHolesSampleMode.Initialization);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00007EA8 File Offset: 0x000060A8
		public void SetDetailLayer(int layer, int[,] details)
		{
			int num = this._terrain.terrainData.detailPrototypes.Length;
			if (layer >= num)
			{
				return;
			}
			base.ResizeDetailDensityTextureArray(num);
			int detailResolution = this._terrain.terrainData.detailResolution;
			this.CreateDetailTexture(this._terrain.terrainData.name, layer);
			GPUITerrainUtility.CaptureTerrainDetailToRenderTexture(this._terrain.terrainData, detailResolution, details, this._detailDensityTextures[layer], this.terrainHolesSampleMode == GPUITerrain.GPUITerrainHolesSampleMode.Initialization);
			base.IsDetailDensityTexturesLoaded = true;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007F28 File Offset: 0x00006128
		[ContextMenu("Save Detail Density Changes")]
		public void SaveDetailChangesToTerrainData()
		{
			for (int i = 0; i < base.GetDetailTextureCount(); i++)
			{
				GPUITerrainUtility.UpdateTerrainDetailWithRenderTexture(this._terrain, i, base.GetDetailDensityTexture(i));
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00007F59 File Offset: 0x00006159
		[ContextMenu("Reset Detail Density Changes")]
		public void ResetDetailChanges()
		{
			base.CreateDetailTextures();
			if (base.DetailManager != null)
			{
				base.DetailManager.RequireUpdate();
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00007F7C File Offset: 0x0000617C
		protected override void LoadTreeInstances()
		{
			if (this._terrain != null && this._terrain.terrainData != null)
			{
				this._treeInstances = this._terrain.terrainData.treeInstances;
				if (base.TreeManager != null)
				{
					base.ConvertToGPUITreeData(base.TreeManager);
				}
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00007FDC File Offset: 0x000061DC
		protected override void SaveTreePrototypesToTerrainData()
		{
			if (this._terrain == null || this._terrain.terrainData == null)
			{
				return;
			}
			this._terrain.terrainData.treePrototypes = this._treePrototypes;
			this._terrain.terrainData.RefreshPrototypes();
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008034 File Offset: 0x00006234
		protected override void SaveDetailPrototypesToTerrainData()
		{
			if (this._terrain == null || this._terrain.terrainData == null)
			{
				return;
			}
			this._terrain.terrainData.detailPrototypes = this._detailPrototypes;
			this._terrain.terrainData.RefreshPrototypes();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00008089 File Offset: 0x00006289
		protected override void OnRemoveTreePrototypesAtIndexes(List<int> terrainPrototypeIndexes)
		{
			this._terrain.terrainData.treeInstances = this._treeInstances;
			this._terrain.terrainData.treePrototypes = this._treePrototypes;
			this._terrain.terrainData.RefreshPrototypes();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000080C8 File Offset: 0x000062C8
		protected override void OnRemoveDetailPrototypesAtIndexes(List<int> terrainPrototypeIndexes)
		{
			if (terrainPrototypeIndexes.Count == 0)
			{
				return;
			}
			DetailPrototype[] detailPrototypes = this._terrain.terrainData.detailPrototypes;
			List<int[,]> list = new List<int[,]>();
			for (int i = 0; i < detailPrototypes.Length; i++)
			{
				if (!terrainPrototypeIndexes.Contains(i))
				{
					list.Add(this._terrain.terrainData.GetDetailLayer(0, 0, this._terrain.terrainData.detailResolution, this._terrain.terrainData.detailResolution, i));
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this._terrain.terrainData.SetDetailLayer(0, 0, j, list[j]);
			}
			this._terrain.terrainData.detailPrototypes = this._detailPrototypes;
			this._terrain.terrainData.RefreshPrototypes();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008196 File Offset: 0x00006396
		public override int GetHeightmapResolution()
		{
			return this._terrain.terrainData.heightmapResolution;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000081A8 File Offset: 0x000063A8
		public override bool SetTerrainBounds(bool forceNew = false)
		{
			if (this._terrain == null || this._terrain.terrainData == null)
			{
				return false;
			}
			Vector3 size = this._terrain.terrainData.size;
			Bounds bounds = new Bounds(size / 2f, size);
			if (forceNew || this._bounds != bounds)
			{
				this._bounds = bounds;
				return true;
			}
			return false;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008217 File Offset: 0x00006417
		public override Vector3 GetSize()
		{
			return this._terrain.terrainData.size;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008229 File Offset: 0x00006429
		protected override Vector3 GetCalculatedTerrainPosition()
		{
			return this._cachedLTW.GetPosition();
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008236 File Offset: 0x00006436
		public Terrain GetTerrain()
		{
			if (this._terrain == null)
			{
				this.LoadTerrain();
			}
			return this._terrain;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008252 File Offset: 0x00006452
		public override float GetTerrainTreeDistance()
		{
			return this._terrainTreeDistance;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000825A File Offset: 0x0000645A
		public override bool IsBakedDetailTextures()
		{
			return this._isBakedDetailTextures;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008264 File Offset: 0x00006464
		public override float GetDetailDensity(int prototypeIndex)
		{
			float num = (base.DetailPrototypes[prototypeIndex].useDensityScaling ? this._terrain.detailObjectDensity : 1f);
			if (this._detailScatterMode == DetailScatterMode.CoverageMode)
			{
				num *= this._terrain.terrainData.ComputeDetailCoverage(prototypeIndex);
				int detailResolution = this._terrain.terrainData.detailResolution;
				Vector3 size = this.GetSize();
				float num2 = math.sqrt(size.x / (float)detailResolution * (size.z / (float)detailResolution));
				return num * num2;
			}
			return num * 255f;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000082ED File Offset: 0x000064ED
		public override Color GetWavingGrassTint()
		{
			if (this._terrain == null || this._terrain.terrainData == null)
			{
				return base.GetWavingGrassTint();
			}
			return this._terrain.terrainData.wavingGrassTint;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008327 File Offset: 0x00006527
		public override void SetBakedDetailTexture(int index, Texture2D texture)
		{
			base.SetBakedDetailTexture(index, texture);
			this._isBakedDetailTextures = true;
			this._isCustomBakedDetailTextures = true;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008340 File Offset: 0x00006540
		public override void SetTreeInstances(TreeInstance[] treeInstances, bool applyToTerrainData = false)
		{
			base.SetTreeInstances(treeInstances, applyToTerrainData);
			if (applyToTerrainData)
			{
				TerrainCollider component = this._terrain.GetComponent<TerrainCollider>();
				if (component != null)
				{
					component.enabled = false;
				}
				this._terrain.terrainData.treeInstances = treeInstances;
				if (component != null)
				{
					component.enabled = true;
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008395 File Offset: 0x00006595
		protected override int GetDetailResolution()
		{
			return this._terrain.terrainData.detailResolution;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000083A7 File Offset: 0x000065A7
		public override Texture GetHolesTexture()
		{
			return this._terrain.terrainData.holesTexture;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000083B9 File Offset: 0x000065B9
		public override int GetAlphamapTextureCount()
		{
			return this._terrain.terrainData.alphamapTextureCount;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000083CB File Offset: 0x000065CB
		public override Texture2D[] GetAlphamapTextures()
		{
			return this._terrain.terrainData.alphamapTextures;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000083DD File Offset: 0x000065DD
		public override TerrainLayer[] GetTerrainLayers()
		{
			return this._terrain.terrainData.terrainLayers;
		}

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		internal float _terrainTreeDistance = 5000f;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		internal bool _isBakedDetailTextures;

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		protected bool _isCustomBakedDetailTextures;

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		private Terrain _terrain;

		// Token: 0x040000C9 RID: 201
		[NonSerialized]
		private DetailScatterMode _detailScatterMode;
	}
}
