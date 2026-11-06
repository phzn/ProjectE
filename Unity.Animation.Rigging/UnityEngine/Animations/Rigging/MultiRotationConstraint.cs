using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200004F RID: 79
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Multi-Rotation Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/MultiRotationConstraint.html")]
	public class MultiRotationConstraint : RigConstraint<MultiRotationConstraintJob, MultiRotationConstraintData, MultiRotationConstraintJobBinder<MultiRotationConstraintData>>
	{
		// Token: 0x060001E1 RID: 481 RVA: 0x00007380 File Offset: 0x00005580
		protected override void OnValidate()
		{
			base.OnValidate();
			WeightedTransformArray sourceObjects = this.m_Data.sourceObjects;
			WeightedTransformArray.OnValidate(ref sourceObjects, 0f, 1f);
			this.m_Data.sourceObjects = sourceObjects;
		}
	}
}
