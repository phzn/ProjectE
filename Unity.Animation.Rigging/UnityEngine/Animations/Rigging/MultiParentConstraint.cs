using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000049 RID: 73
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Multi-Parent Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/MultiParentConstraint.html")]
	public class MultiParentConstraint : RigConstraint<MultiParentConstraintJob, MultiParentConstraintData, MultiParentConstraintJobBinder<MultiParentConstraintData>>
	{
		// Token: 0x060001AD RID: 429 RVA: 0x00006EF4 File Offset: 0x000050F4
		protected override void OnValidate()
		{
			base.OnValidate();
			WeightedTransformArray sourceObjects = this.m_Data.sourceObjects;
			WeightedTransformArray.OnValidate(ref sourceObjects, 0f, 1f);
			this.m_Data.sourceObjects = sourceObjects;
		}
	}
}
