using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001129 RID: 4393
	[Serializable]
	public class WearableSecondaryEffect
	{
		// Token: 0x040087FC RID: 34812
		[Tooltip("Id matching WearableActionDefinition.secondaryEffectId.")]
		public string effectId;

		// Token: 0x040087FD RID: 34813
		[Header("Transition")]
		[Range(0f, 5f)]
		public float transitionDuration = 0.4f;

		// Token: 0x040087FE RID: 34814
		public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040087FF RID: 34815
		[Header("Opacity Fade on outfit materials (LilToon Transparent / One Pass)")]
		public bool useOpacityFade;

		// Token: 0x04008800 RID: 34816
		public string mainColorProperty = "_Color";

		// Token: 0x04008801 RID: 34817
		[Tooltip("Alpha when the effect is active (e.g. 0.5 to make the bag semi-transparent).")]
		[Range(0f, 1f)]
		public float activeAlpha = 0.5f;

		// Token: 0x04008802 RID: 34818
		[Tooltip("Alpha when the effect is inactive (typically 1 = fully opaque).")]
		[Range(0f, 1f)]
		public float inactiveAlpha = 1f;

		// Token: 0x04008803 RID: 34819
		[Header("Blendshapes (driven via BlendShapeManager)")]
		[Tooltip("wornValue is treated as the active value, unwornValue as the inactive value.")]
		public List<WearableBlendshape> blendshapes = new List<WearableBlendshape>();
	}
}
