using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CB5 RID: 3253
	[CreateAssetMenu(menuName = "Run Modes/Long Way Home/Face Animation")]
	public sealed class LongWayHomeFaceAnimationSO : ScriptableObject
	{
		// Token: 0x06005E6C RID: 24172 RVA: 0x00255A80 File Offset: 0x00253C80
		public float SampleTime(float elapsed)
		{
			if (this.duration <= 0f)
			{
				return 0f;
			}
			elapsed = Mathf.Max(0f, elapsed);
			if (!this.loop)
			{
				return Mathf.Min(elapsed, this.duration);
			}
			return Mathf.Repeat(elapsed, this.duration);
		}

		// Token: 0x04005F79 RID: 24441
		public AnimationClip sourceClip;

		// Token: 0x04005F7A RID: 24442
		[Min(0f)]
		public float duration = 1f;

		// Token: 0x04005F7B RID: 24443
		public bool loop = true;

		// Token: 0x04005F7C RID: 24444
		public LongWayHomeFaceAnimationSO.CurveBinding[] curves = Array.Empty<LongWayHomeFaceAnimationSO.CurveBinding>();

		// Token: 0x02000CB6 RID: 3254
		[Serializable]
		public sealed class CurveBinding
		{
			// Token: 0x04005F7D RID: 24445
			public string blendShapeName;

			// Token: 0x04005F7E RID: 24446
			public AnimationCurve curve;
		}
	}
}
