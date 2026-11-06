using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200004B RID: 75
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Multi-Position Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/MultiPositionConstraint.html")]
	public class MultiPositionConstraint : RigConstraint<MultiPositionConstraintJob, MultiPositionConstraintData, MultiPositionConstraintJobBinder<MultiPositionConstraintData>>
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00007094 File Offset: 0x00005294
		protected override void OnValidate()
		{
			base.OnValidate();
			WeightedTransformArray sourceObjects = this.m_Data.sourceObjects;
			WeightedTransformArray.OnValidate(ref sourceObjects, 0f, 1f);
			this.m_Data.sourceObjects = sourceObjects;
		}
	}
}
