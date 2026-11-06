using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000004 RID: 4
	[DisallowMultipleComponent]
	public abstract class GPUITerrain : MonoBehaviour, IEquatable<GPUITerrain>
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		public GPUITreeManager TreeManager { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020D1 File Offset: 0x000002D1
		public TreePrototype[] TreePrototypes
		{
			get
			{
				return this._treePrototypes;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020D9 File Offset: 0x000002D9
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020E1 File Offset: 0x000002E1
		internal int[] TreePrototypeIndexes { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020EA File Offset: 0x000002EA
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000020F2 File Offset: 0x000002F2
		public GPUIDetailManager DetailManager { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020FB File Offset: 0x000002FB
		public DetailPrototype[] DetailPrototypes
		{
			get
			{
				return this._detailPrototypes;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002103 File Offset: 0x00000303
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000210B File Offset: 0x0000030B
		internal int[] DetailPrototypeIndexes { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002114 File Offset: 0x00000314
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000211C File Offset: 0x0000031C
		public bool IsInitialized { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002125 File Offset: 0x00000325
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000212D File Offset: 0x0000032D
		public bool IsDetailDensityTexturesLoaded { get; protected set; }

		// Token: 0x06000011 RID: 17 RVA: 0x00002136 File Offset: 0x00000336
		protected virtual void Awake()
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				base.enabled = false;
				return;
			}
			this.LoadTerrain();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002150 File Offset: 0x00000350
		protected virtual void OnEnable()
		{
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
			if (this.DetailManager != null)
			{
				this.DetailManager.RequireUpdate();
			}
			if (this.TreeManager != null)
			{
				this.TreeManager.RequireUpdate();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000219D File Offset: 0x0000039D
		protected virtual void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021A8 File Offset: 0x000003A8
		public virtual void LoadTerrain()
		{
			if (this._cachedTransform == null)
			{
				this._cachedTransform = base.transform;
			}
			if (this._treePrototypes == null)
			{
				this._treePrototypes = new TreePrototype[0];
			}
			if (this._detailPrototypes == null)
			{
				this._detailPrototypes = new DetailPrototype[0];
			}
			this.SetTerrainBounds(false);
			this.NotifyTransformChanges();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002205 File Offset: 0x00000405
		public virtual bool LoadTerrainData()
		{
			this.LoadTerrain();
			return true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002210 File Offset: 0x00000410
		public virtual void ReloadTerrainData()
		{
			if (this.IsInitialized)
			{
				this.Initialize();
			}
			if (this.DetailManager != null)
			{
				this.DetailManager.OnTerrainsModified();
				this.DetailManager.RequireUpdate();
			}
			if (this.TreeManager != null)
			{
				this.TreeManager.OnTerrainsModified();
				this.TreeManager.RequireUpdate();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002274 File Offset: 0x00000474
		protected virtual void Initialize()
		{
			this.Dispose();
			if (!this.LoadTerrainData())
			{
				return;
			}
			this.CreateHeightmapTexture();
			this.IsInitialized = true;
			if (this.TreeManager != null)
			{
				if (!this.TreeManager.AddTerrain(this))
				{
					this.SetTreeManager(this.TreeManager);
				}
			}
			else if (Application.isPlaying && this.isAutoFindTreeManager)
			{
				this.AutoFindTreeManager();
			}
			if (this.DetailManager != null)
			{
				if (!this.DetailManager.AddTerrain(this))
				{
					this.SetDetailManager(this.DetailManager);
					return;
				}
			}
			else if (Application.isPlaying && this.isAutoFindDetailManager)
			{
				this.AutoFindDetailManager();
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000231C File Offset: 0x0000051C
		protected virtual void Dispose()
		{
			this.IsInitialized = false;
			this.DisposeDetailDensityTextures();
			this.DisposeHeightmapTexture();
			this.DisposeHolesTexture();
			this.DisposeTreeColliders();
			this._treeInstances = null;
			if (this.TreeManager != null)
			{
				this.TreeManager.RemoveTerrain(this);
			}
			else if (GPUITerrain._terrainsSearchingForTreeManager != null)
			{
				int num = GPUITerrain._terrainsSearchingForTreeManager.IndexOf(this);
				if (num >= 0)
				{
					GPUITerrain._terrainsSearchingForTreeManager.RemoveAt(num);
				}
			}
			if (this.DetailManager != null)
			{
				this.DetailManager.RemoveTerrain(this);
			}
			else if (GPUITerrain._terrainsSearchingForDetailManager != null)
			{
				int num2 = GPUITerrain._terrainsSearchingForDetailManager.IndexOf(this);
				if (num2 >= 0)
				{
					GPUITerrain._terrainsSearchingForDetailManager.RemoveAt(num2);
				}
			}
			if (GPUITerrain.DUMMY_HOLES_TEXTURE != null)
			{
				GPUITerrain.DUMMY_HOLES_TEXTURE.DestroyRenderTexture();
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023E4 File Offset: 0x000005E4
		public void AutoFindTreeManager()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			if (this.TreeManager == null)
			{
				foreach (GPUIManager gpuimanager in GPUIRenderingSystem.Instance.ActiveGPUIManagers)
				{
					GPUITreeManager gpuitreeManager = gpuimanager as GPUITreeManager;
					if (gpuitreeManager != null && gpuitreeManager != null)
					{
						this.TreeManager = gpuitreeManager;
						if (!this.TreeManager.AddTerrain(this))
						{
							this.SetTreeManager(this.TreeManager);
						}
						return;
					}
				}
				if (GPUITerrain._terrainsSearchingForTreeManager == null)
				{
					GPUITerrain._terrainsSearchingForTreeManager = new List<GPUITerrain>();
				}
				GPUITerrain._terrainsSearchingForTreeManager.Add(this);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024A0 File Offset: 0x000006A0
		public void AutoFindDetailManager()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			if (this.DetailManager == null)
			{
				foreach (GPUIManager gpuimanager in GPUIRenderingSystem.Instance.ActiveGPUIManagers)
				{
					GPUIDetailManager gpuidetailManager = gpuimanager as GPUIDetailManager;
					if (gpuidetailManager != null && gpuidetailManager != null)
					{
						this.DetailManager = gpuidetailManager;
						if (!this.DetailManager.AddTerrain(this))
						{
							this.SetDetailManager(this.DetailManager);
						}
						return;
					}
				}
				if (GPUITerrain._terrainsSearchingForDetailManager == null)
				{
					GPUITerrain._terrainsSearchingForDetailManager = new List<GPUITerrain>();
				}
				GPUITerrain._terrainsSearchingForDetailManager.Add(this);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000255C File Offset: 0x0000075C
		protected void DisposeDetailDensityTextures()
		{
			this.IsDetailDensityTexturesLoaded = false;
			if (this._detailDensityTextures != null)
			{
				for (int i = 0; i < this._detailDensityTextures.Length; i++)
				{
					this.DisposeDetailDensityTexture(i);
				}
				this._detailDensityTextures = null;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void DisposeHeightmapTexture()
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void DisposeHolesTexture()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000259C File Offset: 0x0000079C
		protected void DisposeDetailDensityTexture(int index)
		{
			RenderTexture renderTexture = this._detailDensityTextures[index];
			if (renderTexture != null && renderTexture.name.Contains("_GPUIDetailLayer_"))
			{
				renderTexture.DestroyRenderTexture();
				this._detailDensityTextures[index] = null;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002599 File Offset: 0x00000799
		internal virtual void SetTerrainDetailObjectDistance(float value)
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002599 File Offset: 0x00000799
		internal virtual void SetTerrainTreeDistance(float value)
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000025DC File Offset: 0x000007DC
		public void CreateHeightmapTexture()
		{
			this._heightmapTexture = this.LoadHeightmapTexture();
		}

		// Token: 0x06000022 RID: 34
		protected abstract RenderTexture LoadHeightmapTexture();

		// Token: 0x06000023 RID: 35 RVA: 0x000025EA File Offset: 0x000007EA
		public void CreateDetailTextures()
		{
			this.LoadDetailDensityTextures();
			this.IsDetailDensityTexturesLoaded = true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000025FC File Offset: 0x000007FC
		protected virtual void LoadDetailDensityTextures()
		{
			this.DetermineDetailPrototypeIndexes(this.DetailManager);
			int num = ((this.DetailPrototypes == null) ? 0 : this.DetailPrototypes.Length);
			this.ResizeDetailDensityTextureArray(num);
			if (num == 0)
			{
				return;
			}
			bool flag = this.IsBakedDetailTextures();
			for (int i = 0; i < num; i++)
			{
				this.CreateDetailTexture(base.name, i);
				if (!this.IsReadTerrainDetails(i))
				{
					this._detailDensityTextures[i].ClearRenderTexture();
				}
				else if (flag)
				{
					this.BlitBakedDetailTexture(i);
				}
			}
			this.ExecuteProceduralDetails();
			this.RequireDetailUpdate(!Application.isPlaying, false);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000268B File Offset: 0x0000088B
		protected void ExecuteProceduralDetails()
		{
			if (this.DetailManager != null)
			{
				this.DetailManager.ExecuteProceduralDetails(this);
			}
			if (this._proceduralDetailModifier != null)
			{
				this._proceduralDetailModifier.Execute(this);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000026BB File Offset: 0x000008BB
		protected virtual void CreateDetailTexture(string terrainName, int index)
		{
			if (this._detailDensityTextures[index] == null)
			{
				this._detailDensityTextures[index] = GPUITerrainUtility.CreateDetailRenderTexture(this.GetDetailResolution(), terrainName + "_GPUIDetailLayer_" + index.ToString());
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026F4 File Offset: 0x000008F4
		protected void ResizeDetailDensityTextureArray(int detailCount)
		{
			int detailResolution = this.GetDetailResolution();
			if (this._detailDensityTextures == null)
			{
				this._detailDensityTextures = new RenderTexture[detailCount];
			}
			else if (this._detailDensityTextures.Length != detailCount)
			{
				for (int i = detailCount; i < this._detailDensityTextures.Length; i++)
				{
					this.DisposeDetailDensityTexture(i);
				}
				Array.Resize<RenderTexture>(ref this._detailDensityTextures, detailCount);
			}
			if (this.IsBakedDetailTextures())
			{
				this.ResizeBakedDetailTextureArray(detailCount);
			}
			for (int j = 0; j < detailCount; j++)
			{
				RenderTexture renderTexture = this._detailDensityTextures[j];
				if (renderTexture != null && renderTexture.width != detailResolution)
				{
					this.DisposeDetailDensityTexture(j);
					renderTexture = null;
				}
				if (renderTexture == null)
				{
					this.CreateDetailTexture(base.name, j);
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027A4 File Offset: 0x000009A4
		protected virtual void ResizeBakedDetailTextureArray(int detailCount)
		{
			if (this._bakedDetailTextures == null)
			{
				this._bakedDetailTextures = new Texture2D[detailCount];
				return;
			}
			if (this._bakedDetailTextures.Length != detailCount)
			{
				Array.Resize<Texture2D>(ref this._bakedDetailTextures, detailCount);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000027D4 File Offset: 0x000009D4
		protected void BlitBakedDetailTexture(int index)
		{
			Texture2D bakedDetailTexture = this.GetBakedDetailTexture(index);
			if (bakedDetailTexture != null)
			{
				GPUITextureUtility.CopyTextureSamplerWithComputeShader(bakedDetailTexture, this._detailDensityTextures[index]);
				return;
			}
			this._detailDensityTextures[index].ClearRenderTexture();
		}

		// Token: 0x0600002A RID: 42
		protected abstract int GetDetailResolution();

		// Token: 0x0600002B RID: 43 RVA: 0x0000280E File Offset: 0x00000A0E
		protected virtual void DisposeTreeColliders()
		{
			if (this._treeCollidersParent != null)
			{
				this._treeCollidersParent.gameObject.DestroyGeneric();
			}
			this._treeCollidersParent = null;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002838 File Offset: 0x00000A38
		internal void GenerateVegetation(GPUIDetailPrototypeData detailPrototypeData, GraphicsBuffer transformBuffer, GPUIDataBuffer<uint> counterBuffer, Vector3 cameraPos, float detailObjectDistance, Texture2D healthyDryNoiseTexture, int[] sizeAndIndexes, bool isCheckTerrainBounds = false)
		{
			if (!this.IsInitialized)
			{
				return;
			}
			if (this._heightmapTexture == null)
			{
				this.CreateHeightmapTexture();
				if (this._heightmapTexture == null)
				{
					return;
				}
			}
			if (!this.IsDetailDensityTexturesLoaded)
			{
				this.CreateDetailTextures();
			}
			if (this._detailDensityTextures == null)
			{
				return;
			}
			Vector3 position = this.GetPosition();
			if (isCheckTerrainBounds && !this.IsTerrainWithinViewDistance(cameraPos, detailObjectDistance))
			{
				return;
			}
			Vector3 size = this.GetSize();
			if (this.DetailPrototypeIndexes == null)
			{
				this.DetermineDetailPrototypeIndexes(this.DetailManager);
			}
			int num = sizeAndIndexes[1];
			int subSettingCount = detailPrototypeData.GetSubSettingCount();
			Texture texture = null;
			if (this.terrainHolesSampleMode == GPUITerrain.GPUITerrainHolesSampleMode.Runtime)
			{
				texture = this.GetHolesTexture();
			}
			bool flag = false;
			Matrix4x4 matrix4x = GPUIConstants.IDENTITY_Matrix4x4;
			if (this.HasRotationSupport())
			{
				matrix4x = this.GetRotationMatrix();
				flag = !matrix4x.EqualsMatrix4x4(GPUIConstants.IDENTITY_Matrix4x4);
			}
			ComputeShader cs_VegetationGenerator = GPUITerrainConstants.CS_VegetationGenerator;
			if (this.HasTwoChannelHeightmap())
			{
				cs_VegetationGenerator.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TWO_CHANNEL_HEIGHTMAP);
			}
			else
			{
				cs_VegetationGenerator.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TWO_CHANNEL_HEIGHTMAP);
			}
			int num2 = 0;
			while (num2 < this.DetailPrototypeIndexes.Length && num2 < this._detailDensityTextures.Length)
			{
				int num3 = this.DetailPrototypeIndexes[num2];
				if (num3 % 1000 == num && detailPrototypeData.TryGetParameterBufferIndex(out sizeAndIndexes[2]))
				{
					int num4 = num3 / 1000;
					if (subSettingCount <= num4 || !detailPrototypeData.GetSubSettings(num4).TryGetParameterBufferIndex(out sizeAndIndexes[3]))
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find Detail Prototype Sub Setting parameter buffer index.");
					}
					else
					{
						RenderTexture renderTexture = this._detailDensityTextures[num2];
						if (!(renderTexture == null))
						{
							int width = renderTexture.width;
							if (this.enableDetailDensityReduction && detailPrototypeData.isUseDensityReduction && detailPrototypeData.densityReduceDistance < detailObjectDistance)
							{
								cs_VegetationGenerator.EnableKeyword(GPUITerrainConstants.Kw_GPUI_DETAIL_DENSITY_REDUCE);
							}
							else
							{
								cs_VegetationGenerator.DisableKeyword(GPUITerrainConstants.Kw_GPUI_DETAIL_DENSITY_REDUCE);
							}
							if (this.terrainHolesSampleMode == GPUITerrain.GPUITerrainHolesSampleMode.Runtime && texture != null)
							{
								cs_VegetationGenerator.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
								cs_VegetationGenerator.SetTexture(0, GPUITerrainConstants.PROP_terrainHoleTexture, texture);
							}
							else
							{
								cs_VegetationGenerator.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
							}
							cs_VegetationGenerator.SetBuffer(0, GPUIConstants.PROP_gpuiTransformBuffer, transformBuffer);
							cs_VegetationGenerator.SetBuffer<uint>(0, GPUITerrainConstants.PROP_detailCounterBuffer, counterBuffer);
							cs_VegetationGenerator.SetBuffer<float>(0, GPUIConstants.PROP_parameterBuffer, GPUIRenderingSystem.Instance.ParameterBuffer);
							cs_VegetationGenerator.SetTexture(0, GPUITerrainConstants.PROP_terrainDetailTexture, renderTexture);
							cs_VegetationGenerator.SetTexture(0, GPUITerrainConstants.PROP_heightmapTexture, this._heightmapTexture);
							cs_VegetationGenerator.SetInt(GPUITerrainConstants.PROP_detailTextureSize, width);
							cs_VegetationGenerator.SetInt(GPUITerrainConstants.PROP_heightmapTextureSize, this._heightmapTexture.width);
							cs_VegetationGenerator.SetVector(GPUITerrainConstants.PROP_startPosition, position);
							cs_VegetationGenerator.SetVector(GPUITerrainConstants.PROP_terrainSize, size);
							cs_VegetationGenerator.SetInts(GPUIConstants.PROP_sizeAndIndexes, sizeAndIndexes);
							cs_VegetationGenerator.SetVector(GPUITerrainConstants.PROP_cameraPos, cameraPos);
							cs_VegetationGenerator.SetFloat(GPUITerrainConstants.PROP_density, this.GetDetailDensity(num2));
							cs_VegetationGenerator.SetFloat(GPUITerrainConstants.PROP_detailObjectDistance, detailObjectDistance);
							cs_VegetationGenerator.SetTexture(0, GPUITerrainConstants.PROP_healthyDryNoiseTexture, healthyDryNoiseTexture);
							if (flag)
							{
								cs_VegetationGenerator.EnableKeyword("GPUI_TRANSFORM_OFFSET");
								cs_VegetationGenerator.SetMatrix(GPUIConstants.PROP_gpuiTransformOffset, matrix4x);
							}
							else
							{
								cs_VegetationGenerator.DisableKeyword("GPUI_TRANSFORM_OFFSET");
							}
							cs_VegetationGenerator.DispatchXY(0, width, width);
						}
					}
				}
				num2++;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B7C File Offset: 0x00000D7C
		public bool IsTerrainWithinViewDistance(Vector3 cameraPos, float detailObjectDistance)
		{
			Bounds terrainWorldBounds = this.GetTerrainWorldBounds();
			return terrainWorldBounds.Contains(cameraPos) || Mathf.Sqrt(terrainWorldBounds.SqrDistance(cameraPos)) <= detailObjectDistance;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002BB0 File Offset: 0x00000DB0
		public virtual void NotifyTransformChanges()
		{
			int frameCount = Time.frameCount;
			if (this._lastTerrainTransformChangeCheckFrame == frameCount)
			{
				return;
			}
			this._lastTerrainTransformChangeCheckFrame = frameCount;
			Matrix4x4 cachedLTW = this._cachedLTW;
			this._cachedLTW = this._cachedTransform.localToWorldMatrix;
			if (this._cachedLTW.EqualsMatrix4x4(cachedLTW))
			{
				return;
			}
			bool flag = this.HasRotationSupport();
			if (flag)
			{
				this._cachedRotationMatrix = GPUIUtility.GetRotationMatrix(this.GetTerrainRotation());
			}
			this._cachedTerrainPosition = this.GetCalculatedTerrainPosition();
			this._cachedWorldBounds = this.GetCalculatedTerrainWorldBounds();
			bool flag2 = false;
			if (this.HasScalingSupport() && !cachedLTW.EqualsMatrix4x4(Matrix4x4.zero) && cachedLTW.lossyScale != this._cachedLTW.lossyScale)
			{
				flag2 = true;
				if (this._treeCollidersParent != null)
				{
					this.CreateTreeColliders();
				}
				this.RequireTreeUpdate(false);
				this.RequireDetailUpdate(true, false);
			}
			if (!flag2)
			{
				if (flag)
				{
					this.deltaLTW = this._cachedLTW * cachedLTW.inverse;
				}
				else
				{
					this.deltaLTW = GPUIConstants.IDENTITY_Matrix4x4;
					(ref this.deltaLTW).SetPosition(this._cachedLTW.GetPosition() - cachedLTW.GetPosition());
				}
				if (this.TreeManager != null)
				{
					this.TreeManager.RequireTerrainTransformChange(frameCount);
				}
				if (this.DetailManager != null)
				{
					this.DetailManager.RequireTerrainTransformChange(frameCount);
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void LoadTreeInstances()
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002D04 File Offset: 0x00000F04
		protected void ConvertToGPUITreeData(GPUITreeManager treeManager)
		{
			if (treeManager._enableTreeInstanceColors)
			{
				TreeInstance[] treeInstances = this.GetTreeInstances(false);
				for (int i = 0; i < treeInstances.Length; i++)
				{
					TreeInstance treeInstance = treeInstances[i];
					Color color = treeInstance.color;
					treeInstance.color = GPUITerrain.DecodeFloatRGBA(color);
					treeInstances[i] = treeInstance;
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002D60 File Offset: 0x00000F60
		private static Color32 DecodeFloatRGBA(Vector4 enc)
		{
			byte[] bytes = BitConverter.GetBytes(Vector4.Dot(enc, GPUITerrain._kDecodeDot));
			return new Color32(bytes[0], bytes[1], bytes[2], bytes[3]);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D8F File Offset: 0x00000F8F
		public void RequireTreeUpdate(bool reloadTreeInstances = false)
		{
			if (this.TreeManager != null)
			{
				this.TreeManager.RequireUpdate(reloadTreeInstances);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002DAB File Offset: 0x00000FAB
		public void RequireDetailUpdate(bool forceImmediateUpdate = false, bool reloadTerrainDetailTextures = false)
		{
			if (this.DetailManager != null)
			{
				this.DetailManager.RequireUpdate(forceImmediateUpdate, reloadTerrainDetailTextures);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void CreateTreeColliders()
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public void DetermineTreePrototypeIndexes(GPUITreeManager treeManager)
		{
			if (treeManager == null)
			{
				return;
			}
			if (this.TreePrototypes == null)
			{
				if (this.TreePrototypeIndexes == null || this.TreePrototypeIndexes.Length != 0)
				{
					this.TreePrototypeIndexes = new int[0];
				}
				return;
			}
			if (this.TreePrototypeIndexes == null || this.TreePrototypeIndexes.Length != this.TreePrototypes.Length)
			{
				this.TreePrototypeIndexes = new int[this.TreePrototypes.Length];
			}
			for (int i = 0; i < this.TreePrototypes.Length; i++)
			{
				this.TreePrototypeIndexes[i] = treeManager.DetermineTreePrototypeIndex(this.TreePrototypes[i]);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E5C File Offset: 0x0000105C
		public void DetermineDetailPrototypeIndexes(GPUIDetailManager detailManager)
		{
			if (detailManager == null)
			{
				return;
			}
			if (this.DetailPrototypes == null)
			{
				this.DetailPrototypeIndexes = new int[0];
				return;
			}
			if (this.DetailPrototypeIndexes == null || this.DetailPrototypeIndexes.Length != this.DetailPrototypes.Length)
			{
				this.DetailPrototypeIndexes = new int[this.DetailPrototypes.Length];
			}
			for (int i = 0; i < this.DetailPrototypes.Length; i++)
			{
				this.DetailPrototypeIndexes[i] = detailManager.DetermineDetailPrototypeIndex(this.DetailPrototypes[i]);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002EE0 File Offset: 0x000010E0
		public int GetFristTerrainTreePrototypeIndex(int managerPrototypeIndex)
		{
			if (this.TreePrototypeIndexes == null)
			{
				return -1;
			}
			for (int i = 0; i < this.TreePrototypeIndexes.Length; i++)
			{
				if (this.TreePrototypeIndexes[i] == managerPrototypeIndex)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F18 File Offset: 0x00001118
		public void GetTerrainTreePrototypeIndexes(int managerPrototypeIndex, ref List<int> terrainPrototypeIndexes)
		{
			terrainPrototypeIndexes.Clear();
			if (this.TreePrototypeIndexes == null)
			{
				return;
			}
			for (int i = 0; i < this.TreePrototypeIndexes.Length; i++)
			{
				if (this.TreePrototypeIndexes[i] == managerPrototypeIndex)
				{
					terrainPrototypeIndexes.Add(i);
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002F5C File Offset: 0x0000115C
		public int GetFirstTerrainDetailPrototypeIndex(int managerPrototypeIndex)
		{
			if (this.DetailPrototypeIndexes == null)
			{
				return -1;
			}
			for (int i = 0; i < this.DetailPrototypeIndexes.Length; i++)
			{
				if (this.DetailPrototypeIndexes[i] % 1000 == managerPrototypeIndex)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002F9C File Offset: 0x0000119C
		public void GetTerrainDetailPrototypeIndexes(int managerPrototypeIndex, ref List<int> terrainPrototypeIndexes)
		{
			if (terrainPrototypeIndexes == null)
			{
				if (terrainPrototypeIndexes == null)
				{
					terrainPrototypeIndexes = new List<int>();
				}
			}
			else
			{
				terrainPrototypeIndexes.Clear();
			}
			if (this.DetailPrototypeIndexes == null)
			{
				return;
			}
			for (int i = 0; i < this.DetailPrototypeIndexes.Length; i++)
			{
				if (this.DetailPrototypeIndexes[i] % 1000 == managerPrototypeIndex)
				{
					terrainPrototypeIndexes.Add(i);
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002FF8 File Offset: 0x000011F8
		protected bool IsUnorderedTreePrototypeIndexes(GPUITreeManager treeManager)
		{
			if (this.TreePrototypeIndexes == null)
			{
				this.DetermineTreePrototypeIndexes(treeManager);
			}
			for (int i = 0; i < this.TreePrototypeIndexes.Length; i++)
			{
				if (i != this.TreePrototypeIndexes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003038 File Offset: 0x00001238
		public string GetTreePrototypeIndexesToString()
		{
			if (this.TreePrototypeIndexes == null)
			{
				return null;
			}
			string text = "";
			for (int i = 0; i < this.TreePrototypeIndexes.Length; i++)
			{
				if (i > 0)
				{
					text += ", ";
				}
				text += this.TreePrototypeIndexes[i].ToString();
			}
			return text;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003094 File Offset: 0x00001294
		public string GetDetailPrototypeIndexesToString()
		{
			if (this.DetailPrototypeIndexes == null)
			{
				return null;
			}
			string text = "";
			for (int i = 0; i < this.DetailPrototypeIndexes.Length; i++)
			{
				if (i > 0)
				{
					text += ", ";
				}
				text = string.Concat(new string[]
				{
					text,
					(this.DetailPrototypeIndexes[i] % 1000).ToString(),
					"[",
					(this.DetailPrototypeIndexes[i] / 1000).ToString(),
					"]"
				});
			}
			return text;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003128 File Offset: 0x00001328
		public virtual void AddTreePrototypeToTerrain(GameObject pickerGameObject, int overwriteIndex)
		{
			this.LoadTerrainData();
			if (this._treePrototypes == null)
			{
				return;
			}
			if (overwriteIndex >= 0)
			{
				List<int> list = new List<int>();
				this.GetTerrainTreePrototypeIndexes(overwriteIndex, ref list);
				using (List<int>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int num = enumerator.Current;
						if (num >= 0 && num < this._treePrototypes.Length)
						{
							this._treePrototypes[num].prefab = pickerGameObject;
						}
					}
					goto IL_0087;
				}
			}
			this._treePrototypes = this._treePrototypes.AddAndReturn(new TreePrototype
			{
				prefab = pickerGameObject
			});
			IL_0087:
			this.DetermineTreePrototypeIndexes(this.TreeManager);
			this.SaveTreePrototypesToTerrainData();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000031E0 File Offset: 0x000013E0
		public virtual void AddDetailPrototypeToTerrain(Object pickerObject, int overwriteIndex, int noiseSeed = 0)
		{
			this.LoadTerrainData();
			if (this._detailPrototypes == null)
			{
				return;
			}
			if (pickerObject is Texture2D)
			{
				if (overwriteIndex >= 0)
				{
					List<int> list = new List<int>();
					this.GetTerrainDetailPrototypeIndexes(overwriteIndex, ref list);
					using (List<int>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							int num = enumerator.Current;
							if (num >= 0 && num < this._detailPrototypes.Length)
							{
								this._detailPrototypes[num].prototype = null;
								this._detailPrototypes[num].prototypeTexture = (Texture2D)pickerObject;
								this._detailPrototypes[num].renderMode = DetailRenderMode.GrassBillboard;
								this._detailPrototypes[num].usePrototypeMesh = false;
							}
						}
						goto IL_0217;
					}
				}
				this._detailPrototypes = this._detailPrototypes.AddAndReturn(new DetailPrototype
				{
					usePrototypeMesh = false,
					prototypeTexture = (Texture2D)pickerObject,
					renderMode = DetailRenderMode.GrassBillboard,
					noiseSeed = ((noiseSeed == 0) ? Random.Range(100, 100000) : noiseSeed)
				});
			}
			else
			{
				GameObject gameObject = pickerObject as GameObject;
				if (gameObject != null)
				{
					if (gameObject.GetComponentInChildren<MeshRenderer>() == null)
					{
						return;
					}
					if (overwriteIndex >= 0)
					{
						List<int> list2 = new List<int>();
						this.GetTerrainDetailPrototypeIndexes(overwriteIndex, ref list2);
						using (List<int>.Enumerator enumerator = list2.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								int num2 = enumerator.Current;
								if (num2 >= 0 && num2 < this._detailPrototypes.Length)
								{
									this._detailPrototypes[num2].prototype = gameObject.GetComponentInChildren<MeshRenderer>().gameObject;
									this._detailPrototypes[num2].prototypeTexture = null;
									this._detailPrototypes[num2].renderMode = DetailRenderMode.VertexLit;
									this._detailPrototypes[num2].usePrototypeMesh = true;
								}
							}
							goto IL_0217;
						}
					}
					this._detailPrototypes = this._detailPrototypes.AddAndReturn(new DetailPrototype
					{
						usePrototypeMesh = true,
						prototype = gameObject.GetComponentInChildren<MeshRenderer>().gameObject,
						renderMode = DetailRenderMode.VertexLit,
						noiseSeed = ((noiseSeed == 0) ? Random.Range(100, 100000) : noiseSeed),
						healthyColor = Color.white,
						dryColor = Color.white,
						useInstancing = true
					});
				}
			}
			IL_0217:
			this.DetermineDetailPrototypeIndexes(this.DetailManager);
			this.SaveDetailPrototypesToTerrainData();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003434 File Offset: 0x00001634
		[ContextMenu("Add Tree Prototypes From Manager")]
		public void AddAllTreePrototypesFromTreeManager()
		{
			if (this.TreeManager == null)
			{
				return;
			}
			this.DetermineTreePrototypeIndexes(this.TreeManager);
			int prototypeCount = this.TreeManager.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.TreePrototypeIndexes.Length; j++)
				{
					if (i == this.TreePrototypeIndexes[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.AddTreePrototypeToTerrain(this.TreeManager.GetPrototype(i).prefabObject, -1);
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000034B4 File Offset: 0x000016B4
		[ContextMenu("Add Detail Prototypes From Manager")]
		public void AddAllDetailPrototypesFromDetailManager()
		{
			if (this.DetailManager == null)
			{
				return;
			}
			this.DetermineDetailPrototypeIndexes(this.DetailManager);
			int prototypeCount = this.DetailManager.GetPrototypeCount();
			int num = this._detailPrototypes.Length;
			for (int i = 0; i < prototypeCount; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.DetailPrototypeIndexes.Length; j++)
				{
					if (i == this.DetailPrototypeIndexes[j] % 1000)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GPUIDetailPrototypeData prototypeData = this.DetailManager.GetPrototypeData(i);
					DetailPrototype detailPrototype;
					if (prototypeData.detailTexture != null)
					{
						this.AddDetailPrototypeToTerrain(prototypeData.detailTexture, -1, prototypeData.GetFirstNoiseSeed());
						detailPrototype = this._detailPrototypes[num];
						detailPrototype.renderMode = (prototypeData.isBillboard ? DetailRenderMode.GrassBillboard : DetailRenderMode.Grass);
					}
					else
					{
						this.AddDetailPrototypeToTerrain(this.DetailManager.GetPrototype(i).prefabObject, -1, prototypeData.GetFirstNoiseSeed());
						detailPrototype = this._detailPrototypes[num];
						detailPrototype.renderMode = DetailRenderMode.VertexLit;
					}
					detailPrototype.dryColor = prototypeData.dryColor;
					detailPrototype.healthyColor = prototypeData.healthyColor;
					detailPrototype.noiseSpread = prototypeData.noiseSpread;
					if (prototypeData.GetSubSettingCount() > 0)
					{
						GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings subSettings = prototypeData.GetSubSettings(0);
						detailPrototype.alignToGround = subSettings.alignToGround;
						if (subSettings.maxHeight > 0f)
						{
							detailPrototype.maxHeight = subSettings.maxHeight;
						}
						if (subSettings.maxWidth > 0f)
						{
							detailPrototype.maxWidth = subSettings.maxWidth;
						}
						if (subSettings.minHeight > 0f)
						{
							detailPrototype.minHeight = subSettings.minHeight;
						}
						if (subSettings.maxHeight > 0f)
						{
							detailPrototype.maxHeight = subSettings.maxHeight;
						}
						if (subSettings.noiseSeed != 0)
						{
							detailPrototype.noiseSeed = subSettings.noiseSeed;
						}
					}
					this.SaveDetailPrototypesToTerrainData();
					num++;
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void SaveTreePrototypesToTerrainData()
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002599 File Offset: 0x00000799
		protected virtual void SaveDetailPrototypesToTerrainData()
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000369C File Offset: 0x0000189C
		public void RemoveTreePrototypeAtIndex(int index)
		{
			this.LoadTerrainData();
			if (this._treePrototypes == null || this._treePrototypes.Length == 0)
			{
				return;
			}
			List<int> list = new List<int>();
			this.GetTerrainTreePrototypeIndexes(index, ref list);
			this.LoadTreeInstances();
			foreach (int num in list)
			{
				this.RemoveTerrainTreePrototypeAtIndex(num);
			}
			this.OnRemoveTreePrototypesAtIndexes(list);
			this.DetermineTreePrototypeIndexes(this.TreeManager);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000372C File Offset: 0x0000192C
		private void RemoveTerrainTreePrototypeAtIndex(int terrainPrototypeIndex)
		{
			this._treePrototypes = this._treePrototypes.RemoveAtAndReturn<TreePrototype>(terrainPrototypeIndex);
			if (this._treeInstances == null || this._treeInstances.Length == 0)
			{
				return;
			}
			List<TreeInstance> list = new List<TreeInstance>(this._treeInstances);
			for (int i = 0; i < list.Count; i++)
			{
				TreeInstance treeInstance = list[i];
				if (treeInstance.prototypeIndex >= terrainPrototypeIndex)
				{
					if (treeInstance.prototypeIndex == terrainPrototypeIndex)
					{
						list.RemoveAt(i);
						i--;
					}
					else if (treeInstance.prototypeIndex > terrainPrototypeIndex)
					{
						treeInstance.prototypeIndex--;
						list[i] = treeInstance;
					}
				}
			}
			this._treeInstances = list.ToArray();
		}

		// Token: 0x06000046 RID: 70
		protected abstract void OnRemoveTreePrototypesAtIndexes(List<int> terrainPrototypeIndexes);

		// Token: 0x06000047 RID: 71 RVA: 0x000037CC File Offset: 0x000019CC
		public void RemoveDetailPrototypeAtIndex(int index)
		{
			this.DisposeDetailDensityTextures();
			List<int> list = new List<int>();
			this.GetTerrainDetailPrototypeIndexes(index, ref list);
			foreach (int num in list)
			{
				this._detailPrototypes = this._detailPrototypes.RemoveAtAndReturn<DetailPrototype>(num);
			}
			this.OnRemoveDetailPrototypesAtIndexes(list);
			this.DetermineDetailPrototypeIndexes(this.DetailManager);
		}

		// Token: 0x06000048 RID: 72
		protected abstract void OnRemoveDetailPrototypesAtIndexes(List<int> terrainPrototypeIndexes);

		// Token: 0x06000049 RID: 73 RVA: 0x00003850 File Offset: 0x00001A50
		internal void SetTreeManager(GPUITreeManager treeManager)
		{
			if (this.TreeManager != null && this.TreeManager != treeManager)
			{
				this.TreeManager.RemoveTerrain(this);
			}
			this.TreeManager = treeManager;
			this.SetTerrainTreeDistance(0f);
			this.DetermineTreePrototypeIndexes(treeManager);
			this.LoadTreeInstances();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000038A8 File Offset: 0x00001AA8
		internal void SetDetailManager(GPUIDetailManager detailManager)
		{
			if (this.DetailManager != null && this.DetailManager != detailManager)
			{
				this.DetailManager.RemoveTerrain(this);
			}
			this.DetailManager = detailManager;
			this.SetTerrainDetailObjectDistance(0f);
			this.DetermineDetailPrototypeIndexes(detailManager);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000038F7 File Offset: 0x00001AF7
		internal void RemoveTreeManager()
		{
			if (this.TreeManager != null && (!Application.isPlaying || this.TreeManager.isEnableDefaultRenderingWhenDisabled))
			{
				this.SetTerrainTreeDistance(this.GetTerrainTreeDistance());
			}
			this.TreeManager = null;
			this._treeInstances = null;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003935 File Offset: 0x00001B35
		internal void RemoveDetailManager()
		{
			if (this.DetailManager != null && (!Application.isPlaying || this.DetailManager.isEnableDefaultRenderingWhenDisabled))
			{
				this.SetTerrainDetailObjectDistance(this.DetailManager.detailObjectDistance);
			}
			this.DetailManager = null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003971 File Offset: 0x00001B71
		public virtual float GetTerrainTreeDistance()
		{
			return 5000f;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003978 File Offset: 0x00001B78
		public RenderTexture GetHeightmapTexture()
		{
			if (this._heightmapTexture == null)
			{
				this.CreateHeightmapTexture();
			}
			return this._heightmapTexture;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003994 File Offset: 0x00001B94
		public virtual void SetHeightmapTexture(RenderTexture heightmapTexture)
		{
			this._heightmapTexture = heightmapTexture;
			this.RequireTreeUpdate(false);
			this.RequireDetailUpdate(true, false);
		}

		// Token: 0x06000050 RID: 80
		public abstract int GetHeightmapResolution();

		// Token: 0x06000051 RID: 81 RVA: 0x000039AC File Offset: 0x00001BAC
		public virtual bool SetTerrainBounds(bool forceNew = false)
		{
			if (forceNew || this._bounds == default(Bounds))
			{
				this._bounds = this._cachedTransform.gameObject.GetBounds(true);
				this._bounds.center = this._bounds.center - this._cachedTransform.position;
				if (this.HasScalingSupport())
				{
					Vector3 vector = this._cachedTransform.lossyScale.Reciprocal();
					this._bounds.extents = Vector3.Scale(this._bounds.extents, vector);
					this._bounds.center = Vector3.Scale(this._bounds.center, vector);
					if (this._bounds.extents == Vector3.zero)
					{
						this._bounds.extents = Vector3.one;
					}
				}
				this.RequireTreeUpdate(false);
				this.RequireDetailUpdate(false, false);
				return true;
			}
			return false;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003A98 File Offset: 0x00001C98
		protected virtual Vector3 GetCalculatedTerrainPosition()
		{
			Vector3 vector = this._bounds.min;
			if (this.HasScalingSupport())
			{
				vector = Vector3.Scale(vector, this.GetTerrainScale());
			}
			if (this.HasRotationSupport())
			{
				vector = this.GetTerrainRotation() * vector;
			}
			return this._cachedLTW.GetPosition() + vector;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003AEC File Offset: 0x00001CEC
		public virtual Vector3 GetPosition()
		{
			return this._cachedTerrainPosition;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003AF4 File Offset: 0x00001CF4
		protected Bounds GetCalculatedTerrainWorldBounds()
		{
			Bounds bounds = this._bounds;
			if (this.HasScalingSupport())
			{
				Vector3 terrainScale = this.GetTerrainScale();
				bounds.center = Vector3.Scale(bounds.center, terrainScale);
				bounds.extents = Vector3.Scale(bounds.extents, terrainScale);
			}
			if (this.HasRotationSupport())
			{
				bounds = bounds.GetMatrixAppliedBounds(this.GetRotationMatrix());
			}
			bounds.center += this._cachedTransform.position;
			return bounds;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003B72 File Offset: 0x00001D72
		public virtual Bounds GetTerrainWorldBounds()
		{
			return this._cachedWorldBounds;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003B7A File Offset: 0x00001D7A
		public virtual bool IsBakedDetailTextures()
		{
			return true;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003B80 File Offset: 0x00001D80
		public virtual Vector3 GetSize()
		{
			Vector3 vector = this._bounds.size;
			if (this.HasScalingSupport())
			{
				vector = Vector3.Scale(vector, this.GetTerrainScale());
			}
			return vector;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003BAF File Offset: 0x00001DAF
		public virtual float GetDetailDensity(int prototypeIndex)
		{
			return 255f;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003BB6 File Offset: 0x00001DB6
		public int GetDetailTextureCount()
		{
			if (this._detailDensityTextures == null)
			{
				return 0;
			}
			return this._detailDensityTextures.Length;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003BCA File Offset: 0x00001DCA
		public RenderTexture GetDetailDensityTexture(int index)
		{
			if (this._detailDensityTextures == null || index < 0 || this._detailDensityTextures.Length <= index)
			{
				return null;
			}
			return this._detailDensityTextures[index];
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003BED File Offset: 0x00001DED
		public virtual int GetBakedDetailTextureCount()
		{
			if (this._bakedDetailTextures == null)
			{
				return 0;
			}
			return this._bakedDetailTextures.Length;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003C01 File Offset: 0x00001E01
		public virtual Texture2D GetBakedDetailTexture(int index)
		{
			if (this._bakedDetailTextures == null || index < 0 || this._bakedDetailTextures.Length <= index)
			{
				return null;
			}
			return this._bakedDetailTextures[index];
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003C24 File Offset: 0x00001E24
		public virtual void SetBakedDetailTexture(int index, Texture2D texture)
		{
			if (this.DetailPrototypes == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Detail prototypes are not set.");
				return;
			}
			if (this._bakedDetailTextures == null)
			{
				this._bakedDetailTextures = new Texture2D[this.DetailPrototypes.Length];
			}
			if (index < 0 || index > this._bakedDetailTextures.Length)
			{
				Debug.LogError(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					"SetBakedDetailTexture error: given index [",
					index.ToString(),
					"] is out of bounds. Detail prototype count: ",
					this._bakedDetailTextures.Length.ToString()
				}));
				return;
			}
			this._bakedDetailTextures[index] = texture;
			if (this.IsDetailDensityTexturesLoaded)
			{
				this.CreateDetailTextures();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003CD8 File Offset: 0x00001ED8
		public virtual void SetDetailDensityTexture(int index, RenderTexture renderTexture)
		{
			if (this.DetailPrototypes == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Detail prototypes are not set.");
				return;
			}
			if (!this.IsDetailDensityTexturesLoaded)
			{
				this.CreateDetailTextures();
			}
			if (this._detailDensityTextures[index] != null)
			{
				this._detailDensityTextures[index].Release();
			}
			this._detailDensityTextures[index] = renderTexture;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003D36 File Offset: 0x00001F36
		public TreeInstance[] GetTreeInstances(bool reloadTreeInstances = false)
		{
			if (reloadTreeInstances)
			{
				this.LoadTreeInstances();
			}
			if (this._treeInstances == null)
			{
				return GPUITerrain.EMPTY_TREE_INSTANCES;
			}
			return this._treeInstances;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003D55 File Offset: 0x00001F55
		public virtual void SetTreeInstances(TreeInstance[] treeInstances, bool applyToTerrainData = false)
		{
			this._treeInstances = treeInstances;
			if (this.TreeManager != null)
			{
				this.ConvertToGPUITreeData(this.TreeManager);
				this.TreeManager.RequireUpdate();
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003D83 File Offset: 0x00001F83
		public virtual Color GetWavingGrassTint()
		{
			return Color.white;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003D8A File Offset: 0x00001F8A
		public bool Equals(GPUITerrain other)
		{
			return !(other == null) && this.GetInstanceIDGPUI() == other.GetInstanceIDGPUI();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003DA8 File Offset: 0x00001FA8
		public override bool Equals(object obj)
		{
			GPUITerrain gpuiterrain = obj as GPUITerrain;
			if (gpuiterrain != null)
			{
				return this.Equals(gpuiterrain);
			}
			return base.Equals(obj);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003DCE File Offset: 0x00001FCE
		public override int GetHashCode()
		{
			return this.GetInstanceIDGPUI();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public virtual Texture GetHolesTexture()
		{
			if (GPUITerrain.DUMMY_HOLES_TEXTURE == null)
			{
				GPUITerrain.DUMMY_HOLES_TEXTURE = new RenderTexture(1, 1, 0, GPUITerrainConstants.R8_RenderTextureFormat, RenderTextureReadWrite.Linear)
				{
					isPowerOfTwo = false,
					enableRandomWrite = true,
					filterMode = FilterMode.Point,
					useMipMap = false,
					autoGenerateMips = false
				};
				GPUITerrain.DUMMY_HOLES_TEXTURE.Create();
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.SetPixel(0, 0, Color.white);
				GPUITextureUtility.CopyTextureSamplerWithComputeShader(texture2D, GPUITerrain.DUMMY_HOLES_TEXTURE);
				texture2D.DestroyGeneric();
			}
			return GPUITerrain.DUMMY_HOLES_TEXTURE;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003E5C File Offset: 0x0000205C
		public virtual int GetAlphamapTextureCount()
		{
			return 0;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003E5F File Offset: 0x0000205F
		public virtual Texture2D[] GetAlphamapTextures()
		{
			return null;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003E5F File Offset: 0x0000205F
		public virtual TerrainLayer[] GetTerrainLayers()
		{
			return null;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003E5C File Offset: 0x0000205C
		public virtual bool HasScalingSupport()
		{
			return false;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003E5C File Offset: 0x0000205C
		public virtual bool HasRotationSupport()
		{
			return false;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003E62 File Offset: 0x00002062
		public virtual Vector3 GetTerrainScale()
		{
			return Vector3.one;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003E69 File Offset: 0x00002069
		public virtual Quaternion GetTerrainRotation()
		{
			return GPUIConstants.IDENTITY_Quaternion;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003E70 File Offset: 0x00002070
		internal Matrix4x4 GetRotationMatrix()
		{
			return this._cachedRotationMatrix;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003E69 File Offset: 0x00002069
		public virtual Quaternion GetSavedTerrainRotation()
		{
			return GPUIConstants.IDENTITY_Quaternion;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003E78 File Offset: 0x00002078
		public virtual bool HasTwoChannelHeightmap()
		{
			return !GPUIRuntimeSettings.Instance.API_HAS_GUARANTEED_R8_SUPPORT;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003E87 File Offset: 0x00002087
		public void SetProceduralDetailModifier(IGPUIProceduralDetailModifier detailModifier)
		{
			this._proceduralDetailModifier = detailModifier;
			this.RequireDetailUpdate(false, true);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003E98 File Offset: 0x00002098
		protected bool IsReadTerrainDetails(int terrainDetailPrototypeIndex)
		{
			return (this._proceduralDetailModifier == null || this._proceduralDetailModifier.IsReadTerrainDetails(terrainDetailPrototypeIndex)) && (!(this.DetailManager != null) || this.DetailPrototypeIndexes == null || this.DetailPrototypeIndexes.Length <= terrainDetailPrototypeIndex || this.DetailManager.IsReadTerrainDetails(this.DetailPrototypeIndexes[terrainDetailPrototypeIndex] % 1000));
		}

		// Token: 0x04000006 RID: 6
		[SerializeField]
		protected Bounds _bounds;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		protected Texture2D[] _bakedDetailTextures;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		public bool isAutoFindTreeManager = true;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		public bool isAutoFindDetailManager = true;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		public GPUITerrain.GPUITerrainHolesSampleMode terrainHolesSampleMode;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		public bool enableDetailDensityReduction = true;

		// Token: 0x0400000C RID: 12
		[NonSerialized]
		protected Transform _cachedTransform;

		// Token: 0x0400000D RID: 13
		[NonSerialized]
		protected Matrix4x4 _cachedLTW = Matrix4x4.zero;

		// Token: 0x0400000E RID: 14
		[NonSerialized]
		protected Vector3 _cachedTerrainPosition = Vector3.zero;

		// Token: 0x0400000F RID: 15
		[NonSerialized]
		protected Matrix4x4 _cachedRotationMatrix = Matrix4x4.identity;

		// Token: 0x04000010 RID: 16
		[NonSerialized]
		public Matrix4x4 deltaLTW = Matrix4x4.identity;

		// Token: 0x04000011 RID: 17
		[NonSerialized]
		protected Bounds _cachedWorldBounds;

		// Token: 0x04000012 RID: 18
		[NonSerialized]
		protected RenderTexture _heightmapTexture;

		// Token: 0x04000013 RID: 19
		[NonSerialized]
		protected TreeInstance[] _treeInstances;

		// Token: 0x04000014 RID: 20
		[NonSerialized]
		protected RenderTexture[] _detailDensityTextures;

		// Token: 0x04000015 RID: 21
		[NonSerialized]
		protected IGPUIProceduralDetailModifier _proceduralDetailModifier;

		// Token: 0x04000017 RID: 23
		protected TreePrototype[] _treePrototypes;

		// Token: 0x0400001A RID: 26
		protected DetailPrototype[] _detailPrototypes;

		// Token: 0x0400001E RID: 30
		public static readonly TreeInstance[] EMPTY_TREE_INSTANCES = new TreeInstance[0];

		// Token: 0x0400001F RID: 31
		public static RenderTexture DUMMY_HOLES_TEXTURE;

		// Token: 0x04000020 RID: 32
		internal static List<GPUITerrain> _terrainsSearchingForTreeManager;

		// Token: 0x04000021 RID: 33
		internal static List<GPUITerrain> _terrainsSearchingForDetailManager;

		// Token: 0x04000022 RID: 34
		[NonSerialized]
		protected Transform _treeCollidersParent;

		// Token: 0x04000023 RID: 35
		private int _lastTerrainTransformChangeCheckFrame = -1;

		// Token: 0x04000024 RID: 36
		private static readonly Vector4 _kDecodeDot = new Vector4(1f, 0.003921569f, 1.53787E-05f, 6.030863E-08f);

		// Token: 0x02000005 RID: 5
		public enum GPUITerrainHolesSampleMode
		{
			// Token: 0x04000026 RID: 38
			Initialization,
			// Token: 0x04000027 RID: 39
			Runtime,
			// Token: 0x04000028 RID: 40
			None
		}

		// Token: 0x02000006 RID: 6
		public abstract class GPUITerrainPaintingProxy : MonoBehaviour
		{
			// Token: 0x06000074 RID: 116
			public abstract void FinalizePainting(bool saveTerrainData);
		}
	}
}
