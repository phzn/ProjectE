using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012EC RID: 4844
	[Serializable]
	public class StealthModifier
	{
		// Token: 0x04009B63 RID: 39779
		public string SourceName;

		// Token: 0x04009B64 RID: 39780
		[Tooltip("Multiplies the calculated noise radius")]
		public float NoiseMultiplier = 1f;

		// Token: 0x04009B65 RID: 39781
		[Tooltip("Multiplies the calculated visibility")]
		public float VisibilityMultiplier = 1f;
	}
}
