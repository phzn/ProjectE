using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000043 RID: 67
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Chain IK Constraint")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/ChainIKConstraint.html")]
	public class ChainIKConstraint : RigConstraint<ChainIKConstraintJob, ChainIKConstraintData, ChainIKConstraintJobBinder<ChainIKConstraintData>>
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000683C File Offset: 0x00004A3C
		protected override void OnValidate()
		{
			base.OnValidate();
			this.m_Data.chainRotationWeight = Mathf.Clamp01(this.m_Data.chainRotationWeight);
			this.m_Data.tipRotationWeight = Mathf.Clamp01(this.m_Data.tipRotationWeight);
			this.m_Data.maxIterations = Mathf.Clamp(this.m_Data.maxIterations, 1, 50);
			this.m_Data.tolerance = Mathf.Clamp(this.m_Data.tolerance, 0f, 0.01f);
		}
	}
}
