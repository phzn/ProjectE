using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200012E RID: 302
	[Serializable]
	public class MMRadioSignalGeneratorItem
	{
		// Token: 0x04000875 RID: 2165
		public bool Active = true;

		// Token: 0x04000876 RID: 2166
		public MMSignal.SignalType SignalType;

		// Token: 0x04000877 RID: 2167
		[MMEnumCondition("SignalType", new int[] { 9 })]
		public AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000878 RID: 2168
		[MMEnumCondition("SignalType", new int[] { 10 })]
		public MMTween.MMTweenCurve TweenCurve = MMTween.MMTweenCurve.EaseInOutQuartic;

		// Token: 0x04000879 RID: 2169
		public MMRadioSignalGeneratorItem.GeneratorItemModes Mode;

		// Token: 0x0400087A RID: 2170
		[Range(-1f, 1f)]
		public float Phase;

		// Token: 0x0400087B RID: 2171
		[Range(0f, 10f)]
		public float Frequency = 5f;

		// Token: 0x0400087C RID: 2172
		[Range(0f, 1f)]
		public float Amplitude = 1f;

		// Token: 0x0400087D RID: 2173
		[Range(-1f, 1f)]
		public float Offset;

		// Token: 0x0400087E RID: 2174
		public bool Invert;

		// Token: 0x02000444 RID: 1092
		public enum GeneratorItemModes
		{
			// Token: 0x04001FD6 RID: 8150
			Multiply,
			// Token: 0x04001FD7 RID: 8151
			Additive
		}
	}
}
