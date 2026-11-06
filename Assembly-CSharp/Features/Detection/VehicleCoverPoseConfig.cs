using System;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x0200125B RID: 4699
	[Serializable]
	public struct VehicleCoverPoseConfig
	{
		// Token: 0x04009660 RID: 38496
		[Tooltip("If true, cover pose is allowed while riding this vehicle")]
		public bool allowCover;

		// Token: 0x04009661 RID: 38497
		[Tooltip("Which arm(s) cover pose can control while riding")]
		public VehicleCoverArmMode armMode;

		// Token: 0x04009662 RID: 38498
		public IVehicleAnchorProvider anchorProvider;

		// Token: 0x04009663 RID: 38499
		public VehiclePoseDriver poseDriver;

		// Token: 0x04009664 RID: 38500
		public VehiclePosePreset posePreset;
	}
}
