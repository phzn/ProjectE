using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000045 RID: 69
	[DisallowMultipleComponent]
	[AddComponentMenu("Animation Rigging/Damped Transform")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/constraints/DampedTransform.html")]
	public class DampedTransform : RigConstraint<DampedTransformJob, DampedTransformData, DampedTransformJobBinder<DampedTransformData>>
	{
		// Token: 0x0600016D RID: 365 RVA: 0x00006995 File Offset: 0x00004B95
		protected override void OnValidate()
		{
			base.OnValidate();
			this.m_Data.dampPosition = Mathf.Clamp01(this.m_Data.dampPosition);
			this.m_Data.dampRotation = Mathf.Clamp01(this.m_Data.dampRotation);
		}
	}
}
