using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000055 RID: 85
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Twist Correction")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/TwistCorrection.html")]
	public class TwistCorrection : RigConstraint<TwistCorrectionJob, TwistCorrectionData, TwistCorrectionJobBinder<TwistCorrectionData>>
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000770C File Offset: 0x0000590C
		protected override void OnValidate()
		{
			base.OnValidate();
			WeightedTransformArray twistNodes = this.m_Data.twistNodes;
			WeightedTransformArray.OnValidate(ref twistNodes, -1f, 1f);
			this.m_Data.twistNodes = twistNodes;
		}
	}
}
