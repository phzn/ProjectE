using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001258 RID: 4696
	[Serializable]
	public class CoverPoseBlendShapeSet
	{
		// Token: 0x04009650 RID: 38480
		[Tooltip("Friendly label for this pose variant (e.g. 'Crossed Arms'). Cosmetic only — does not affect runtime.")]
		public string Label;

		// Token: 0x04009651 RID: 38481
		[Tooltip("Explicit Curious rank whitelist. Other ranks retain their clothing requirements.")]
		public CuriousCoverPoseAccess CuriousAccess;

		// Token: 0x04009652 RID: 38482
		public CoverPoseRankRequirement RankRequirement;

		// Token: 0x04009653 RID: 38483
		[Tooltip("Requires the upper garment to be removed. Opening a worn garment never satisfies this rule, at any rank.")]
		public bool RequiresTopRemoved;

		// Token: 0x04009654 RID: 38484
		[Tooltip("Which arm layer(s) drive this pose. The unselected arm passes through to the emotion layer below — useful for half-cover poses like GrabMid.")]
		public CoverArmsMode ArmsMode;

		// Token: 0x04009655 RID: 38485
		[Tooltip("If false, this pose-variant is locked to BoobsCover: the FullCover toggle is ignored while it's active, and selecting it while in FullCover snaps back to BoobsCover. Use for poses like FaceCover where FullCover makes no sense.")]
		public bool AllowFullCover = true;

		// Token: 0x04009656 RID: 38486
		[Tooltip("If true, the controller's MagicaCloth objects are disabled while THIS pose-variant is active (and re-enabled when leaving it). Decided per-pose, not per-emotion: e.g. a chest-hug pose can hide cloth while a casual hands-on-hips pose keeps it on, regardless of whether the player is Embarrassed or Confident.")]
		public bool DisableMagicaCloth = true;

		// Token: 0x04009657 RID: 38487
		[Tooltip("3.2-B: cosa copre questa posa. La posa e' USABILE solo se le parti che copre sono SCOPERTE (evita compenetrazione). CoversTop richiede top scoperto; CoversTopAndBottom richiede anche bottom scoperto.")]
		public CoverPoseCoverage Coverage;

		// Token: 0x04009658 RID: 38488
		[Tooltip("Blend shapes applied while this pose-variant is active. Inactive sets blend back to 0.")]
		public List<CoverBlendShapeEntry> BlendShapes = new List<CoverBlendShapeEntry>();
	}
}
