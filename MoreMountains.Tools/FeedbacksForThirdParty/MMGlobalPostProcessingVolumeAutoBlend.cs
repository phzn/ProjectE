using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000322 RID: 802
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMGlobalPostProcessingVolumeAutoBlend")]
	public class MMGlobalPostProcessingVolumeAutoBlend : MonoBehaviour
	{
		// Token: 0x0600164A RID: 5706 RVA: 0x0005E8C9 File Offset: 0x0005CAC9
		protected float GetTime()
		{
			if (this.TimeScale != MMGlobalPostProcessingVolumeAutoBlend.TimeScales.Unscaled)
			{
				return Time.time;
			}
			return Time.unscaledTime;
		}

		// Token: 0x040019A2 RID: 6562
		[Header("Blend")]
		[Tooltip("the trigger mode for this MMGlobalPostProcessingVolumeAutoBlend")]
		public MMGlobalPostProcessingVolumeAutoBlend.BlendTriggerModes BlendTriggerMode;

		// Token: 0x040019A3 RID: 6563
		[Tooltip("the duration of the blend (in seconds)")]
		public float BlendDuration = 1f;

		// Token: 0x040019A4 RID: 6564
		[Tooltip("the curve to use to blend")]
		public AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040019A5 RID: 6565
		[Header("Weight")]
		[Tooltip("the weight at the start of the blend")]
		[Range(0f, 1f)]
		public float InitialWeight;

		// Token: 0x040019A6 RID: 6566
		[Tooltip("the desired weight at the end of the blend")]
		[Range(0f, 1f)]
		public float FinalWeight = 1f;

		// Token: 0x040019A7 RID: 6567
		[Header("Behaviour")]
		[Tooltip("the timescale to operate on")]
		public MMGlobalPostProcessingVolumeAutoBlend.TimeScales TimeScale = MMGlobalPostProcessingVolumeAutoBlend.TimeScales.Unscaled;

		// Token: 0x040019A8 RID: 6568
		[Tooltip("whether or not the associated volume should be disabled at 0")]
		public bool DisableVolumeOnZeroWeight = true;

		// Token: 0x040019A9 RID: 6569
		[Tooltip("whether or not this blender should disable itself at 0")]
		public bool DisableSelfAfterEnd = true;

		// Token: 0x040019AA RID: 6570
		[Tooltip("whether or not this blender can be interrupted")]
		public bool Interruptable = true;

		// Token: 0x040019AB RID: 6571
		[Tooltip("whether or not this blender should pick the current value as its starting point")]
		public bool StartFromCurrentValue = true;

		// Token: 0x040019AC RID: 6572
		[Tooltip("reset to initial value on end ")]
		public bool ResetToInitialValueOnEnd;

		// Token: 0x040019AD RID: 6573
		[Header("Tests")]
		[Tooltip("test blend button")]
		[MMFInspectorButton("Blend")]
		public bool TestBlend;

		// Token: 0x040019AE RID: 6574
		[Tooltip("test blend back button")]
		[MMFInspectorButton("BlendBack")]
		public bool TestBlendBackwards;

		// Token: 0x040019AF RID: 6575
		protected float _initial;

		// Token: 0x040019B0 RID: 6576
		protected float _destination;

		// Token: 0x040019B1 RID: 6577
		protected float _startTime;

		// Token: 0x040019B2 RID: 6578
		protected bool _blending;

		// Token: 0x02000571 RID: 1393
		public enum TimeScales
		{
			// Token: 0x040024AB RID: 9387
			Scaled,
			// Token: 0x040024AC RID: 9388
			Unscaled
		}

		// Token: 0x02000572 RID: 1394
		public enum BlendTriggerModes
		{
			// Token: 0x040024AE RID: 9390
			OnEnable,
			// Token: 0x040024AF RID: 9391
			Script
		}
	}
}
