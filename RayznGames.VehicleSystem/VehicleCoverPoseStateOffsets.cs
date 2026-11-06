using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class VehicleCoverPoseStateOffsets
	{
		// Token: 0x040000CC RID: 204
		public VehiclePoseState state;

		// Token: 0x040000CD RID: 205
		[Header("Left Hand")]
		public Vector3 leftHandLocalPositionOffset = Vector3.zero;

		// Token: 0x040000CE RID: 206
		public Vector3 leftHandRotationOffset = Vector3.zero;

		// Token: 0x040000CF RID: 207
		public Vector3 leftHandHintLocalOffset = Vector3.zero;

		// Token: 0x040000D0 RID: 208
		[Header("Right Hand")]
		public Vector3 rightHandLocalPositionOffset = Vector3.zero;

		// Token: 0x040000D1 RID: 209
		public Vector3 rightHandRotationOffset = Vector3.zero;

		// Token: 0x040000D2 RID: 210
		public Vector3 rightHandHintLocalOffset = Vector3.zero;
	}
}
