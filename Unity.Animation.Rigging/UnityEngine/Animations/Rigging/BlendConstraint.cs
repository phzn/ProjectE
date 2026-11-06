using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000041 RID: 65
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Blend Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/BlendConstraint.html")]
	public class BlendConstraint : RigConstraint<BlendConstraintJob, BlendConstraintData, BlendConstraintJobBinder<BlendConstraintData>>
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0000664C File Offset: 0x0000484C
		protected override void OnValidate()
		{
			base.OnValidate();
			this.m_Data.positionWeight = Mathf.Clamp01(this.m_Data.positionWeight);
			this.m_Data.rotationWeight = Mathf.Clamp01(this.m_Data.rotationWeight);
		}
	}
}
