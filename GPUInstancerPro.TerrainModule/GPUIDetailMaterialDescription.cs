using System;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000009 RID: 9
	[CreateAssetMenu(menuName = "Rendering/GPU Instancer Pro/Detail Material Description", order = 711)]
	public class GPUIDetailMaterialDescription : GPUIMPBDescription
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00004B60 File Offset: 0x00002D60
		public void SetPropertyIDs()
		{
			this._mainTexturePropertyID = Shader.PropertyToID(this.mainTextureProperty);
			this._healthyColorPropertyID = Shader.PropertyToID(this.healthyColorProperty);
			if (!string.IsNullOrEmpty(this.dryColorProperty))
			{
				this._dryColorPropertyID = Shader.PropertyToID(this.dryColorProperty);
			}
			if (!string.IsNullOrEmpty(this.wavingTintProperty))
			{
				this._wavingTintPropertyID = Shader.PropertyToID(this.wavingTintProperty);
			}
			if (!string.IsNullOrEmpty(this.isBillboardProperty))
			{
				this._isBillboardPropertyID = Shader.PropertyToID(this.isBillboardProperty);
			}
			if (!string.IsNullOrEmpty(this.healthyDryNoiseTextureProperty))
			{
				this._healthyDryNoiseTexturePropertyID = Shader.PropertyToID(this.healthyDryNoiseTextureProperty);
			}
			this._isDefaultShader = GPUITerrainConstants.IsDefaultDetailShader(this.shader);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004C18 File Offset: 0x00002E18
		public override void SetMPBValues(GPUIRenderSourceGroup rsg, GPUIManager manager, int prototypeIndex)
		{
			GPUIDetailManager gpuidetailManager = manager as GPUIDetailManager;
			if (gpuidetailManager != null)
			{
				if (this._mainTexturePropertyID == 0 || !this._isDefaultShader)
				{
					this.SetPropertyIDs();
				}
				GPUIDetailPrototypeData prototypeData = gpuidetailManager.GetPrototypeData(prototypeIndex);
				if (prototypeData != null && prototypeData.detailTexture != null)
				{
					rsg.AddMaterialPropertyOverride(this._mainTexturePropertyID, prototypeData.detailTexture, -1, -1, true);
					rsg.AddMaterialPropertyOverride(this._healthyColorPropertyID, prototypeData.healthyColor, -1, -1, true);
					if (this.isDryColorActive && !string.IsNullOrEmpty(this.dryColorProperty))
					{
						rsg.AddMaterialPropertyOverride(this._dryColorPropertyID, prototypeData.dryColor, -1, -1, true);
					}
					if (this.isWavingTintActive && !string.IsNullOrEmpty(this.wavingTintProperty))
					{
						rsg.AddMaterialPropertyOverride(this._wavingTintPropertyID, prototypeData.windWaveTintColor, -1, -1, true);
					}
					if (this.isBillboardActive && !string.IsNullOrEmpty(this.isBillboardProperty))
					{
						rsg.AddMaterialPropertyOverride(this._isBillboardPropertyID, prototypeData.isBillboard ? 1 : 0, -1, -1, true);
					}
					if (this.isHealthyDryNoiseTextureActive && !string.IsNullOrEmpty(this.healthyDryNoiseTextureProperty))
					{
						if (prototypeData.isOverrideHealthyDryNoiseTexture && prototypeData.healthyDryNoiseTexture != null)
						{
							rsg.AddMaterialPropertyOverride(this._healthyDryNoiseTexturePropertyID, prototypeData.healthyDryNoiseTexture, -1, -1, true);
						}
						else if (gpuidetailManager.healthyDryNoiseTexture != null)
						{
							rsg.AddMaterialPropertyOverride(this._healthyDryNoiseTexturePropertyID, gpuidetailManager.healthyDryNoiseTexture, -1, -1, true);
						}
					}
				}
				if (this._isDefaultShader)
				{
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_WindWaveSize, prototypeData.windWaveSize, -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_AmbientOcclusion, prototypeData.ambientOcclusion, -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_NoiseSpread, prototypeData.GetNoiseSpread(), -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_WindVector, gpuidetailManager.windVector, -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_WindWavesOn, prototypeData.windWavesOn ? 1 : 0, -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_WindWaveSway, prototypeData.windWaveSway, -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_WindIdleSway, prototypeData.windIdleSway, -1, -1, true);
					rsg.AddMaterialPropertyOverride(GPUIDetailMaterialDescription.PROP_GradientContrastRatioTint, new Vector4(prototypeData.gradientPower, prototypeData.contrast, prototypeData.healthyDryRatio, prototypeData.windWaveTint), -1, -1, true);
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004E80 File Offset: 0x00003080
		public void SetDefaultValues()
		{
			this.shader = GPUITerrainConstants.GetDefaultDetailShader();
			this.mainTextureProperty = "_MainTex";
			this.healthyColorProperty = "_HealthyColor";
			this.isDryColorActive = true;
			this.dryColorProperty = "_DryColor";
			this.isWavingTintActive = true;
			this.wavingTintProperty = "_WindWaveTintColor";
			this.isBillboardActive = true;
			this.isBillboardProperty = "_IsBillboard";
			this.isHealthyDryNoiseTextureActive = true;
			this.healthyDryNoiseTextureProperty = "_HealthyDryNoiseTexture";
		}

		// Token: 0x04000034 RID: 52
		public string mainTextureProperty;

		// Token: 0x04000035 RID: 53
		public string healthyColorProperty;

		// Token: 0x04000036 RID: 54
		public bool isDryColorActive;

		// Token: 0x04000037 RID: 55
		public string dryColorProperty;

		// Token: 0x04000038 RID: 56
		public bool isWavingTintActive;

		// Token: 0x04000039 RID: 57
		public string wavingTintProperty;

		// Token: 0x0400003A RID: 58
		public bool isBillboardActive;

		// Token: 0x0400003B RID: 59
		public string isBillboardProperty;

		// Token: 0x0400003C RID: 60
		public bool isHealthyDryNoiseTextureActive;

		// Token: 0x0400003D RID: 61
		public string healthyDryNoiseTextureProperty;

		// Token: 0x0400003E RID: 62
		public static readonly int PROP_WindWaveSize = Shader.PropertyToID("_WindWaveSize");

		// Token: 0x0400003F RID: 63
		public static readonly int PROP_AmbientOcclusion = Shader.PropertyToID("_AmbientOcclusion");

		// Token: 0x04000040 RID: 64
		public static readonly int PROP_NoiseSpread = Shader.PropertyToID("_NoiseSpread");

		// Token: 0x04000041 RID: 65
		public static readonly int PROP_WindVector = Shader.PropertyToID("_WindVector");

		// Token: 0x04000042 RID: 66
		public static readonly int PROP_WindWavesOn = Shader.PropertyToID("_WindWavesOn");

		// Token: 0x04000043 RID: 67
		public static readonly int PROP_WindWaveSway = Shader.PropertyToID("_WindWaveSway");

		// Token: 0x04000044 RID: 68
		public static readonly int PROP_WindIdleSway = Shader.PropertyToID("_WindIdleSway");

		// Token: 0x04000045 RID: 69
		public static readonly int PROP_GradientContrastRatioTint = Shader.PropertyToID("_GradientContrastRatioTint");

		// Token: 0x04000046 RID: 70
		private int _mainTexturePropertyID;

		// Token: 0x04000047 RID: 71
		private int _healthyColorPropertyID;

		// Token: 0x04000048 RID: 72
		private int _dryColorPropertyID;

		// Token: 0x04000049 RID: 73
		private int _wavingTintPropertyID;

		// Token: 0x0400004A RID: 74
		private int _isBillboardPropertyID;

		// Token: 0x0400004B RID: 75
		private int _healthyDryNoiseTexturePropertyID;

		// Token: 0x0400004C RID: 76
		private bool _isDefaultShader;
	}
}
