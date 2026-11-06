using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200126C RID: 4716
	[CreateAssetMenu(fileName = "DetectionConfig", menuName = "Detection/Global Config")]
	public class DetectionConfigSO : ScriptableObject
	{
		// Token: 0x0400973A RID: 38714
		[Header("Global Settings")]
		public LayerMask VisionObstacles;

		// Token: 0x0400973B RID: 38715
		[Tooltip("Time in seconds to detect the player when fully visible and in optimal range")]
		public float TimeToDetect = 2f;

		// Token: 0x0400973C RID: 38716
		[Tooltip("Time in seconds for attention to decay from 100% to 0% when player is lost")]
		public float CooldownTime = 5f;

		// Token: 0x0400973D RID: 38717
		[Header("Detection Rate Multipliers")]
		[Tooltip("Multiplier for visual detection speed. 1.0 = base rate from TimeToDetect")]
		[Range(0.1f, 3f)]
		public float VisualDetectionMultiplier = 1f;

		// Token: 0x0400973E RID: 38718
		[Tooltip("Multiplier for audio detection speed relative to visual. 0.15 = 15% of visual speed")]
		[Range(0.05f, 1f)]
		public float AudioDetectionMultiplier = 0.15f;

		// Token: 0x0400973F RID: 38719
		[Tooltip("If true, audio detection rate scales with distance (closer = faster detection)")]
		public bool AudioScalesWithDistance = true;

		// Token: 0x04009740 RID: 38720
		[Header("Thresholds")]
		[Range(0f, 1f)]
		public float SuspiciousThreshold = 0.3f;

		// Token: 0x04009741 RID: 38721
		[Range(0f, 1f)]
		public float AlertThreshold = 1f;

		// Token: 0x04009742 RID: 38722
		[Header("Movement Layers")]
		public StealthLayerConfig CrouchLayer;

		// Token: 0x04009743 RID: 38723
		public StealthLayerConfig WalkLayer;

		// Token: 0x04009744 RID: 38724
		public StealthLayerConfig RunLayer;

		// Token: 0x04009745 RID: 38725
		[Header("Slow Motion")]
		public float DetectionTimeScale = 0.1f;

		// Token: 0x04009746 RID: 38726
		public float DetectionDuration = 3f;
	}
}
