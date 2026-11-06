using System;
using UnityEngine;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009ED RID: 2541
	[Serializable]
	public class FPCameraPreset
	{
		// Token: 0x040047DE RID: 18398
		[Tooltip("Small vertical correction from the anatomical eye midpoint")]
		public float eyeHeightOffset = -0.08f;

		// Token: 0x040047DF RID: 18399
		[Tooltip("Small correction along the pose-derived face direction")]
		public float eyeForwardOffset = 0.045f;

		// Token: 0x040047E0 RID: 18400
		[Tooltip("Horizontal camera anchoring to the torso reference (0 = eyes, 1 = torso). Useful for hunched poses where the head moves far ahead of the chest.")]
		[Range(0f, 1f)]
		public float torsoHorizontalAnchor;

		// Token: 0x040047E1 RID: 18401
		[Tooltip("Distance in front of the torso reference when torso anchoring is active")]
		public float torsoForwardOffset = 0.15f;

		// Token: 0x040047E2 RID: 18402
		[Tooltip("Vertical camera anchoring to the torso reference (0 = eyes, 1 = fixed height above torso)")]
		[Range(0f, 1f)]
		public float torsoVerticalAnchor;

		// Token: 0x040047E3 RID: 18403
		[Tooltip("Height above the torso reference when vertical torso anchoring is active")]
		public float torsoHeightOffset = 0.16f;

		// Token: 0x040047E4 RID: 18404
		[Tooltip("Additional rotation offset applied to the FP camera (pitch, yaw, roll)")]
		public Vector3 rotationOffset = Vector3.zero;
	}
}
