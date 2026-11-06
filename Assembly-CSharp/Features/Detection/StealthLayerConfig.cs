using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200126B RID: 4715
	[Serializable]
	public class StealthLayerConfig
	{
		// Token: 0x04009737 RID: 38711
		public string LayerName;

		// Token: 0x04009738 RID: 38712
		[Range(0f, 1f)]
		public float BaseVisibility = 0.5f;

		// Token: 0x04009739 RID: 38713
		public float BaseNoiseRadius = 5f;
	}
}
