using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000115 RID: 277
	[Serializable]
	public class AnimationCurveParameter : VolumeParameter<AnimationCurve>
	{
		// Token: 0x06000853 RID: 2131 RVA: 0x00027554 File Offset: 0x00025754
		public AnimationCurveParameter(AnimationCurve value, bool overrideState = false)
			: base(value, overrideState)
		{
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002755E File Offset: 0x0002575E
		public override void Interp(AnimationCurve lhsCurve, AnimationCurve rhsCurve, float t)
		{
			this.m_Value = lhsCurve;
			KeyframeUtility.InterpAnimationCurve(ref this.m_Value, rhsCurve, t);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00027574 File Offset: 0x00025774
		public override void SetValue(VolumeParameter parameter)
		{
			this.m_Value.CopyFrom(((AnimationCurveParameter)parameter).m_Value);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002758C File Offset: 0x0002578C
		public override object Clone()
		{
			return new AnimationCurveParameter(new AnimationCurve(base.GetValue<AnimationCurve>().keys), this.overrideState);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x000275AC File Offset: 0x000257AC
		public override int GetHashCode()
		{
			return this.overrideState.GetHashCode() * 23 + this.value.GetHashCode();
		}
	}
}
