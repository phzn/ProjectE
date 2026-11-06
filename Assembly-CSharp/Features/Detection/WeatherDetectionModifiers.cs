using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200130F RID: 4879
	public readonly struct WeatherDetectionModifiers
	{
		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x0600974F RID: 38735 RVA: 0x003E0F47 File Offset: 0x003DF147
		public float ViewRadiusMultiplier { get; }

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06009750 RID: 38736 RVA: 0x003E0F4F File Offset: 0x003DF14F
		public float VisualRateMultiplier { get; }

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06009751 RID: 38737 RVA: 0x003E0F57 File Offset: 0x003DF157
		public float FogBlend { get; }

		// Token: 0x06009752 RID: 38738 RVA: 0x003E0F5F File Offset: 0x003DF15F
		public WeatherDetectionModifiers(float viewRadiusMultiplier, float visualRateMultiplier, float fogBlend)
		{
			this.ViewRadiusMultiplier = Mathf.Clamp01(viewRadiusMultiplier);
			this.VisualRateMultiplier = Mathf.Clamp01(visualRateMultiplier);
			this.FogBlend = Mathf.Clamp01(fogBlend);
		}

		// Token: 0x04009C87 RID: 40071
		public static readonly WeatherDetectionModifiers Clear = new WeatherDetectionModifiers(1f, 1f, 0f);
	}
}
