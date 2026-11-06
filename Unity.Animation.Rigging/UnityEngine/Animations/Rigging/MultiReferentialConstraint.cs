using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200004D RID: 77
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Multi-Referential Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/MultiReferentialConstraint.html")]
	public class MultiReferentialConstraint : RigConstraint<MultiReferentialConstraintJob, MultiReferentialConstraintData, MultiReferentialConstraintJobBinder<MultiReferentialConstraintData>>
	{
		// Token: 0x060001CD RID: 461 RVA: 0x0000720C File Offset: 0x0000540C
		protected override void OnValidate()
		{
			base.OnValidate();
			this.m_Data.UpdateDriver();
		}
	}
}
