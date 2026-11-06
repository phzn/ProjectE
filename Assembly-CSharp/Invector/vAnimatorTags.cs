using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000464 RID: 1124
	public static class vAnimatorTags
	{
		// Token: 0x04001D33 RID: 7475
		[Tooltip("Use to lock the controller movement to use the root movement instead")]
		public const string LockMovement = "LockMovement";

		// Token: 0x04001D34 RID: 7476
		[Tooltip("Use to lock the controller rotation to use the root rotation instead")]
		public const string LockRotation = "LockRotation";

		// Token: 0x04001D35 RID: 7477
		[Tooltip("Use for Generic Actions like push lever, it will lock the players input, movement and rotation and use the animation root motion")]
		public const string CustomAction = "CustomAction";

		// Token: 0x04001D36 RID: 7478
		[Tooltip("Use to identify if this is a Airborne animation")]
		public const string Airborne = "Airborne";

		// Token: 0x04001D37 RID: 7479
		[Tooltip("Use to Ignore the Headtrack")]
		public const string IgnoreHeadtrack = "IgnoreHeadtrack";

		// Token: 0x04001D38 RID: 7480
		[Tooltip("Use to identify a Death animation")]
		public const string Dead = "Dead";

		// Token: 0x04001D39 RID: 7481
		[Tooltip("Use to identify Attack animations")]
		public const string Attack = "Attack";

		// Token: 0x04001D3A RID: 7482
		[Tooltip("Use to identify Equipping animations")]
		public const string IsEquipping = "IsEquipping";

		// Token: 0x04001D3B RID: 7483
		[Tooltip("Use to identify Reloading animations")]
		public const string IsReloading = "IsReloading";

		// Token: 0x04001D3C RID: 7484
		[Tooltip("Use to Ignore IK while this animation is being played")]
		public const string IgnoreIK = "IgnoreIK";

		// Token: 0x04001D3D RID: 7485
		[Tooltip("Use to Ignore only support hand IK while this animation is being played")]
		public const string IgnoreSupportHandIK = "IgnoreSupportHandIK";

		// Token: 0x04001D3E RID: 7486
		[Tooltip("Use to identify a Shooter Upperbody Pose animation")]
		public const string UpperbodyPose = "Upperbody Pose";

		// Token: 0x04001D3F RID: 7487
		[Tooltip("Use to identify a Throw animation")]
		public const string IsThrowing = "IsThrowing";
	}
}
