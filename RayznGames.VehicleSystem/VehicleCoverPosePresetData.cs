using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000017 RID: 23
	[Serializable]
	public class VehicleCoverPosePresetData
	{
		// Token: 0x040000D3 RID: 211
		[Tooltip("If enabled, this vehicle preset provides Cover IK offsets by motion state/emotion.")]
		public bool enabled;

		// Token: 0x040000D4 RID: 212
		[Tooltip("Cover IK offsets for Embarrassed emotion, split by motion state.")]
		public VehicleCoverPoseStateOffsets[] embarrassedOffsets = new VehicleCoverPoseStateOffsets[0];

		// Token: 0x040000D5 RID: 213
		[Tooltip("Cover IK offsets for Confident/Exhibit emotion, split by motion state.")]
		public VehicleCoverPoseStateOffsets[] confidentOffsets = new VehicleCoverPoseStateOffsets[0];
	}
}
