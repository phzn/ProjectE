using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200015E RID: 350
	[Serializable]
	public class MMTweenType
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00026525 File Offset: 0x00024725
		public static MMTweenType DefaultEaseInCubic { get; } = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x0600081A RID: 2074 RVA: 0x0002652C File Offset: 0x0002472C
		public MMTweenType(MMTween.MMTweenCurve newCurve)
		{
			this.MMTweenCurve = newCurve;
			this.MMTweenDefinitionType = MMTweenDefinitionTypes.MMTween;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00026594 File Offset: 0x00024794
		public MMTweenType(AnimationCurve newCurve)
		{
			this.Curve = newCurve;
			this.MMTweenDefinitionType = MMTweenDefinitionTypes.AnimationCurve;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x000265F9 File Offset: 0x000247F9
		public float Evaluate(float t)
		{
			return MMTween.Evaluate(t, this);
		}

		// Token: 0x0400097D RID: 2429
		public MMTweenDefinitionTypes MMTweenDefinitionType;

		// Token: 0x0400097E RID: 2430
		public MMTween.MMTweenCurve MMTweenCurve = MMTween.MMTweenCurve.EaseInCubic;

		// Token: 0x0400097F RID: 2431
		public AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000980 RID: 2432
		public bool Initialized;
	}
}
