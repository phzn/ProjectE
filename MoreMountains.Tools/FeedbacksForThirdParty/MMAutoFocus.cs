using System;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000324 RID: 804
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMAutoFocus")]
	public class MMAutoFocus : MonoBehaviour
	{
		// Token: 0x040019B3 RID: 6579
		[Header("Bindings")]
		[Tooltip("the position of the camera")]
		public Transform CameraTransform;

		// Token: 0x040019B4 RID: 6580
		[Tooltip("a list of all possible targets")]
		public Transform[] FocusTargets;

		// Token: 0x040019B5 RID: 6581
		[Tooltip("an offset to apply to the focus target")]
		public Vector3 Offset;

		// Token: 0x040019B6 RID: 6582
		[Header("Setup")]
		[Tooltip("the current target of this auto focus")]
		public float FocusTargetID;

		// Token: 0x040019B7 RID: 6583
		[Header("Desired Aperture")]
		[Tooltip("the aperture to work with")]
		[Range(0.1f, 20f)]
		public float Aperture = 0.1f;
	}
}
