using System;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200125E RID: 4702
	[Serializable]
	public class CoverIKOverride
	{
		// Token: 0x0400966C RID: 38508
		[Tooltip("Which limb this override controls")]
		public LimbType limb;

		// Token: 0x0400966D RID: 38509
		[Tooltip("How the IK reference is resolved")]
		public IKReferenceMode referenceMode;

		// Token: 0x0400966E RID: 38510
		[Tooltip("Custom reference bone for this limb (only used when referenceMode = CustomBone)")]
		public Transform customReferenceBone;

		// Token: 0x0400966F RID: 38511
		[Header("Target")]
		[Tooltip("IK target position offset (local space relative to the chosen reference)")]
		public Vector3 localPosition = Vector3.zero;

		// Token: 0x04009670 RID: 38512
		[Tooltip("IK target rotation offset (euler angles, local space relative to the chosen reference)")]
		public Vector3 rotationOffset = Vector3.zero;

		// Token: 0x04009671 RID: 38513
		[Tooltip("How much the IK overrides the animated position (0 = animation only, 1 = full IK)")]
		[Range(0f, 1f)]
		public float targetPositionWeight = 1f;

		// Token: 0x04009672 RID: 38514
		[Tooltip("How much the IK overrides the animated rotation (0 = animation only, 1 = full IK)")]
		[Range(0f, 1f)]
		public float targetRotationWeight = 1f;

		// Token: 0x04009673 RID: 38515
		[Header("Hint")]
		[Tooltip("Hint position offset (local space relative to the chosen reference). Controls elbow/knee direction.")]
		public Vector3 hintLocalPosition = Vector3.zero;

		// Token: 0x04009674 RID: 38516
		[Tooltip("Hint influence weight (0 = no hint, 1 = full hint). Set > 0 to activate hint.")]
		[Range(0f, 1f)]
		public float hintWeight;

		// Token: 0x04009675 RID: 38517
		[NonSerialized]
		public float currentWeight;
	}
}
