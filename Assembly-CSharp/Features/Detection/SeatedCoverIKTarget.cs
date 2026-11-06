using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200125F RID: 4703
	[Serializable]
	public struct SeatedCoverIKTarget
	{
		// Token: 0x04009676 RID: 38518
		[Tooltip("IK target position offset (local space relative to the chosen reference)")]
		public Vector3 localPosition;

		// Token: 0x04009677 RID: 38519
		[Tooltip("IK target rotation offset (euler angles, local space relative to the chosen reference)")]
		public Vector3 rotationOffset;

		// Token: 0x04009678 RID: 38520
		[Tooltip("Hint position offset (local space relative to the chosen reference). Controls elbow direction.")]
		public Vector3 hintLocalPosition;
	}
}
