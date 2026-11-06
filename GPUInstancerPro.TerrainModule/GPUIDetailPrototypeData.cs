using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public class GPUIDetailPrototypeData : GPUIPrototypeData
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00004F88 File Offset: 0x00003188
		public void ReadFromDetailPrototypeData(DetailPrototype detailPrototype, int subSettingIndex, GPUIDetailManager detailManager, int prototypeIndex)
		{
			GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings subSettings = this.GetSubSettings(subSettingIndex);
			bool flag = this.healthyColor != detailPrototype.healthyColor;
			this.healthyColor = detailPrototype.healthyColor;
			flag |= this.dryColor != detailPrototype.dryColor;
			this.dryColor = detailPrototype.dryColor;
			flag |= this.detailTexture != detailPrototype.prototypeTexture;
			this.detailTexture = detailPrototype.prototypeTexture;
			flag |= subSettings.minWidth != detailPrototype.minWidth;
			subSettings.minWidth = detailPrototype.minWidth;
			flag |= subSettings.maxWidth != detailPrototype.maxWidth;
			subSettings.maxWidth = detailPrototype.maxWidth;
			flag |= subSettings.minHeight != detailPrototype.minHeight;
			subSettings.minHeight = detailPrototype.minHeight;
			flag |= subSettings.maxHeight != detailPrototype.maxHeight;
			subSettings.maxHeight = detailPrototype.maxHeight;
			flag |= subSettings.noiseSeed != detailPrototype.noiseSeed;
			subSettings.noiseSeed = detailPrototype.noiseSeed;
			subSettings.detailUniqueValue = 0f;
			flag |= this.noiseSpread != detailPrototype.noiseSpread;
			this.noiseSpread = detailPrototype.noiseSpread;
			flag |= subSettings.alignToGround != detailPrototype.alignToGround;
			subSettings.alignToGround = detailPrototype.alignToGround;
			flag |= this.isBillboard != (detailPrototype.renderMode == DetailRenderMode.GrassBillboard);
			this.isBillboard = detailPrototype.renderMode == DetailRenderMode.GrassBillboard;
			if (detailPrototype.prototype == null && this.mpbDescription == null)
			{
				this.mpbDescription = GPUITerrainConstants.DefaultDetailMaterialDescription;
			}
			if (this.initialBufferSize <= 0)
			{
				this.initialBufferSize = 1024;
			}
			if (!detailManager.IsInitialized || !flag)
			{
				return;
			}
			this.SetParameterBufferData();
			subSettings.SetParameterBufferData();
			GPUIRenderSourceGroup gpuirenderSourceGroup;
			if (this.detailTexture != null && GPUIRenderingSystem.TryGetRenderSourceGroup(detailManager.GetRenderKey(prototypeIndex), out gpuirenderSourceGroup))
			{
				this.SetMPBValues(detailManager, prototypeIndex, gpuirenderSourceGroup);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005188 File Offset: 0x00003388
		public bool IsMatchingPrefabAndTexture(DetailPrototype detailPrototype, GPUIPrototype prototype, bool checkPropertyValues = true)
		{
			return (prototype.prototypeType == GPUIPrototypeType.Prefab && detailPrototype.usePrototypeMesh && prototype.prefabObject.EqualOrParentOf(detailPrototype.prototype)) || (prototype.prototypeType == GPUIPrototypeType.MeshAndMaterial && !detailPrototype.usePrototypeMesh && detailPrototype.prototypeTexture == this.detailTexture && (!checkPropertyValues || (detailPrototype.healthyColor.Approximately(this.healthyColor, false, 0.002f) && detailPrototype.dryColor.Approximately(this.dryColor, false, 0.002f) && detailPrototype.noiseSpread == this.noiseSpread && detailPrototype.renderMode == DetailRenderMode.GrassBillboard == this.isBillboard)));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005238 File Offset: 0x00003438
		public bool HasSameSettingsWith(DetailPrototype detailPrototype, int subSettingIndex)
		{
			GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings gpuidetailPrototypeSubSettings = this._prototypeSubSettings[subSettingIndex];
			return detailPrototype.minWidth == gpuidetailPrototypeSubSettings.minWidth && detailPrototype.maxWidth == gpuidetailPrototypeSubSettings.maxWidth && detailPrototype.minHeight == gpuidetailPrototypeSubSettings.minHeight && detailPrototype.maxHeight == gpuidetailPrototypeSubSettings.maxHeight && detailPrototype.alignToGround == gpuidetailPrototypeSubSettings.alignToGround;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005297 File Offset: 0x00003497
		public void SetMPBValues(GPUIDetailManager detailManager, int prototypeIndex, GPUIRenderSourceGroup rsg)
		{
			if (this.mpbDescription != null)
			{
				this.mpbDescription.SetMPBValues(rsg, detailManager, prototypeIndex);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000052B8 File Offset: 0x000034B8
		private void CreateSubSettingAtIndex(int subSettingIndex)
		{
			if (this._prototypeSubSettings == null)
			{
				this._prototypeSubSettings = new GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings[subSettingIndex + 1];
				for (int i = 0; i <= subSettingIndex; i++)
				{
					this._prototypeSubSettings[i] = new GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings();
				}
				return;
			}
			if (this._prototypeSubSettings.Length <= subSettingIndex)
			{
				int num = this._prototypeSubSettings.Length;
				Array.Resize<GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings>(ref this._prototypeSubSettings, subSettingIndex + 1);
				for (int j = num; j <= subSettingIndex; j++)
				{
					this._prototypeSubSettings[j] = new GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings();
				}
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000532E File Offset: 0x0000352E
		public GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings GetSubSettings(int subSettingIndex)
		{
			this.CreateSubSettingAtIndex(subSettingIndex);
			return this._prototypeSubSettings[subSettingIndex];
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000533F File Offset: 0x0000353F
		public int GetSubSettingCount()
		{
			if (this._prototypeSubSettings != null)
			{
				return this._prototypeSubSettings.Length;
			}
			return 0;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005353 File Offset: 0x00003553
		public float GetNoiseSpread()
		{
			return this.noiseSpread * this.noiseSpreadAdjustment;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005362 File Offset: 0x00003562
		public int GetFirstNoiseSeed()
		{
			if (this._prototypeSubSettings != null && this._prototypeSubSettings.Length != 0 && this._prototypeSubSettings[0] != null)
			{
				return this._prototypeSubSettings[0].noiseSeed;
			}
			return 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005390 File Offset: 0x00003590
		public override void SetParameterBufferData()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIDataBuffer<float> parameterBuffer = GPUIRenderingSystem.Instance.ParameterBuffer;
			int num;
			if (this.TryGetParameterBufferIndex(out num))
			{
				parameterBuffer[num] = this.densityReduceDistance;
				parameterBuffer[num + 1] = this.densityReduceMultiplier;
				parameterBuffer[num + 2] = this.densityReduceMaxScale;
				parameterBuffer[num + 3] = this.densityReduceHeightScale;
				parameterBuffer[num + 4] = this.densityAdjustment;
				parameterBuffer[num + 5] = (this.isBillboard ? 1f : 0f);
				parameterBuffer[num + 6] = (float)this.maxDetailInstanceCountPerUnit;
				parameterBuffer[num + 7] = this.GetNoiseSpread();
				parameterBuffer[num + 8] = this.healthyDryScaleAdjustment;
			}
			else
			{
				GPUIRenderingSystem.Instance.ParameterBufferIndexes.Add(this, parameterBuffer.Length);
				parameterBuffer.Add(new float[]
				{
					this.densityReduceDistance,
					this.densityReduceMultiplier,
					this.densityReduceMaxScale,
					this.densityReduceHeightScale,
					this.densityAdjustment,
					this.isBillboard ? 1f : 0f,
					(float)this.maxDetailInstanceCountPerUnit,
					this.GetNoiseSpread(),
					this.healthyDryScaleAdjustment
				});
			}
			if (this._prototypeSubSettings != null)
			{
				foreach (GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings gpuidetailPrototypeSubSettings in this._prototypeSubSettings)
				{
					if (gpuidetailPrototypeSubSettings.noiseSeedAdjustment != this.noiseSeedAdjustment)
					{
						gpuidetailPrototypeSubSettings.detailUniqueValue = 0f;
						gpuidetailPrototypeSubSettings.noiseSeedAdjustment = this.noiseSeedAdjustment;
					}
					gpuidetailPrototypeSubSettings.SetParameterBufferData();
				}
			}
		}

		// Token: 0x0400004D RID: 77
		public GPUIDetailMaterialDescription mpbDescription;

		// Token: 0x0400004E RID: 78
		public Texture2D detailTexture;

		// Token: 0x0400004F RID: 79
		public float noiseSpread = 0.1f;

		// Token: 0x04000050 RID: 80
		public Color healthyColor = new Color(0.2627f, 0.9765f, 0.1647f);

		// Token: 0x04000051 RID: 81
		public Color dryColor = new Color(0.8039f, 0.7373f, 0.102f);

		// Token: 0x04000052 RID: 82
		public bool isBillboard = true;

		// Token: 0x04000053 RID: 83
		[Range(0f, 1f)]
		public float ambientOcclusion = 0.2f;

		// Token: 0x04000054 RID: 84
		[Range(0f, 1f)]
		public float gradientPower = 0.5f;

		// Token: 0x04000055 RID: 85
		public Color windWaveTintColor = new Color(0.69803923f, 0.6f, 0.5019608f);

		// Token: 0x04000056 RID: 86
		public bool isOverrideHealthyDryNoiseTexture;

		// Token: 0x04000057 RID: 87
		public Texture2D healthyDryNoiseTexture;

		// Token: 0x04000058 RID: 88
		[Range(0f, 1f)]
		public float windIdleSway = 0.6f;

		// Token: 0x04000059 RID: 89
		public bool windWavesOn = true;

		// Token: 0x0400005A RID: 90
		[Range(0f, 1f)]
		public float windWaveSize = 0.8f;

		// Token: 0x0400005B RID: 91
		[Range(0f, 1f)]
		public float windWaveTint = 0.5f;

		// Token: 0x0400005C RID: 92
		[Range(0f, 1f)]
		public float windWaveSway = 0.5f;

		// Token: 0x0400005D RID: 93
		[Range(0f, 4f)]
		public float contrast = 1f;

		// Token: 0x0400005E RID: 94
		[Range(0f, 4f)]
		public float healthyDryRatio = 1f;

		// Token: 0x0400005F RID: 95
		public bool isUseDensityReduction = true;

		// Token: 0x04000060 RID: 96
		public float densityReduceDistance = 200f;

		// Token: 0x04000061 RID: 97
		[Range(1f, 128f)]
		public float densityReduceMultiplier = 16f;

		// Token: 0x04000062 RID: 98
		[Range(0f, 64f)]
		public float densityReduceMaxScale;

		// Token: 0x04000063 RID: 99
		[Range(0f, 1f)]
		public float densityReduceHeightScale;

		// Token: 0x04000064 RID: 100
		public int initialBufferSize = 1024;

		// Token: 0x04000065 RID: 101
		[Range(1f, 255f)]
		public int maxDetailInstanceCountPerUnit = 16;

		// Token: 0x04000066 RID: 102
		[Range(0f, 4f)]
		public float detailBufferSizePercentageDifferenceForReduction = 0.5f;

		// Token: 0x04000067 RID: 103
		[Range(0.05f, 1f)]
		public float detailExtraBufferSizePercentage = 0.2f;

		// Token: 0x04000068 RID: 104
		[Range(0.0625f, 16f)]
		public float densityAdjustment = 1f;

		// Token: 0x04000069 RID: 105
		[Range(-4f, 4f)]
		public float healthyDryScaleAdjustment;

		// Token: 0x0400006A RID: 106
		public int noiseSeedAdjustment;

		// Token: 0x0400006B RID: 107
		[Range(0f, 4f)]
		public float noiseSpreadAdjustment = 1f;

		// Token: 0x0400006C RID: 108
		public GPUIProceduralDetailObject proceduralDensityData;

		// Token: 0x0400006D RID: 109
		[NonSerialized]
		internal Bounds _bounds;

		// Token: 0x0400006E RID: 110
		[NonSerialized]
		private GPUIDetailPrototypeData.GPUIDetailPrototypeSubSettings[] _prototypeSubSettings;

		// Token: 0x0200000B RID: 11
		public class GPUIDetailPrototypeSubSettings : IGPUIParameterBufferData
		{
			// Token: 0x060000BB RID: 187 RVA: 0x0000565C File Offset: 0x0000385C
			public void SetParameterBufferData()
			{
				if (!GPUIRenderingSystem.IsActive)
				{
					return;
				}
				GPUIDataBuffer<float> parameterBuffer = GPUIRenderingSystem.Instance.ParameterBuffer;
				int num;
				if (this.TryGetParameterBufferIndex(out num))
				{
					parameterBuffer[num] = this.minWidth;
					parameterBuffer[num + 1] = this.maxWidth;
					parameterBuffer[num + 2] = this.minHeight;
					parameterBuffer[num + 3] = this.maxHeight;
					parameterBuffer[num + 4] = (float)this.noiseSeed;
					parameterBuffer[num + 5] = this.alignToGround;
					parameterBuffer[num + 6] = this.GetUniqueValue();
					return;
				}
				GPUIRenderingSystem.Instance.ParameterBufferIndexes.Add(this, parameterBuffer.Length);
				parameterBuffer.Add(new float[]
				{
					this.minWidth,
					this.maxWidth,
					this.minHeight,
					this.maxHeight,
					(float)this.noiseSeed,
					this.alignToGround,
					this.GetUniqueValue()
				});
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00005751 File Offset: 0x00003951
			public bool TryGetParameterBufferIndex(out int index)
			{
				return GPUIRenderingSystem.Instance.ParameterBufferIndexes.TryGetValue(this, out index);
			}

			// Token: 0x060000BD RID: 189 RVA: 0x00005764 File Offset: 0x00003964
			public float GetUniqueValue()
			{
				if (this.detailUniqueValue == 0f)
				{
					Random.InitState(this.noiseSeed + this.noiseSeedAdjustment);
					this.detailUniqueValue = (float)Math.Round((double)Random.Range(0.001f, 0.999f), 3);
				}
				return this.detailUniqueValue;
			}

			// Token: 0x0400006F RID: 111
			public float minWidth;

			// Token: 0x04000070 RID: 112
			public float maxWidth;

			// Token: 0x04000071 RID: 113
			public float minHeight;

			// Token: 0x04000072 RID: 114
			public float maxHeight;

			// Token: 0x04000073 RID: 115
			public int noiseSeed;

			// Token: 0x04000074 RID: 116
			[Range(0f, 1f)]
			public float alignToGround;

			// Token: 0x04000075 RID: 117
			public float detailUniqueValue;

			// Token: 0x04000076 RID: 118
			public int noiseSeedAdjustment;
		}
	}
}
