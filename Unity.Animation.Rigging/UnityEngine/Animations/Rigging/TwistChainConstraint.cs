using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000053 RID: 83
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Twist Chain Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/TwistChainConstraint.html")]
	public class TwistChainConstraint : RigConstraint<TwistChainConstraintJob, TwistChainConstraintData, TwistChainConstraintJobBinder<TwistChainConstraintData>>
	{
	}
}
