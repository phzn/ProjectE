using System;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001128 RID: 4392
	[Serializable]
	public class WearableBlendshape
	{
		// Token: 0x040087F8 RID: 34808
		[Tooltip("Name of the blendshape (resolved by name across all SkinnedMeshRenderers known to BlendShapeManager).")]
		public string blendshapeName;

		// Token: 0x040087F9 RID: 34809
		[Tooltip("Weight when the wearable / effect is in its 'on' state (0-100).")]
		[Range(0f, 100f)]
		public float wornValue = 100f;

		// Token: 0x040087FA RID: 34810
		[Tooltip("Weight when the wearable / effect is in its 'off' state (0-100).")]
		[Range(0f, 100f)]
		public float unwornValue;

		// Token: 0x040087FB RID: 34811
		[Tooltip("After unwear, let animation drive this shape again when no other override owns it. Use for facial expressions; body fit shapes normally keep their unworn baseline.")]
		public bool releaseToAnimationOnUnwear;
	}
}
