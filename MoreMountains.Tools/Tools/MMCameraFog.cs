using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200001E RID: 30
	[ExecuteAlways]
	public class MMCameraFog : MonoBehaviour
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00003608 File Offset: 0x00001808
		protected void Awake()
		{
			this._previousSettings = new FogSettings();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003618 File Offset: 0x00001818
		protected virtual void OnPreRender()
		{
			this._previousSettings.FogEnabled = RenderSettings.fog;
			this._previousSettings.FogColor = RenderSettings.fogColor;
			this._previousSettings.FogDensity = RenderSettings.fogDensity;
			this._previousSettings.FogMode = RenderSettings.fogMode;
			RenderSettings.fog = this.Settings.FogEnabled;
			RenderSettings.fogColor = this.Settings.FogColor;
			RenderSettings.fogDensity = this.Settings.FogDensity;
			RenderSettings.fogMode = this.Settings.FogMode;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000036A8 File Offset: 0x000018A8
		protected virtual void OnPostRender()
		{
			RenderSettings.fog = this._previousSettings.FogEnabled;
			RenderSettings.fogColor = this._previousSettings.FogColor;
			RenderSettings.fogDensity = this._previousSettings.FogDensity;
			RenderSettings.fogMode = this._previousSettings.FogMode;
		}

		// Token: 0x040000A3 RID: 163
		public FogSettings Settings;

		// Token: 0x040000A4 RID: 164
		protected FogSettings _previousSettings;
	}
}
