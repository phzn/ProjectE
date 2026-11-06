using System;
using UnityEngine;
using UnityEngine.Animations;

namespace Features.RunModes
{
	// Token: 0x02000CFE RID: 3326
	public struct LongWayHomeStadiumLookAt : IAnimationJob
	{
		// Token: 0x060060C2 RID: 24770 RVA: 0x00004F31 File Offset: 0x00003131
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x002669F0 File Offset: 0x00264BF0
		public void ProcessAnimation(AnimationStream stream)
		{
			if (this.Weight <= 0.0001f || !stream.isHumanStream)
			{
				return;
			}
			AnimationHumanStream animationHumanStream = stream.AsHuman();
			animationHumanStream.SetLookAtPosition(this.Target);
			animationHumanStream.SetLookAtClampWeight(this.ClampWeight);
			animationHumanStream.SetLookAtBodyWeight(this.BodyWeight * this.Weight);
			animationHumanStream.SetLookAtHeadWeight(this.HeadWeight * this.Weight);
			animationHumanStream.SetLookAtEyesWeight(this.EyesWeight * this.Weight);
			animationHumanStream.SolveIK();
		}

		// Token: 0x040062BE RID: 25278
		public Vector3 Target;

		// Token: 0x040062BF RID: 25279
		public float Weight;

		// Token: 0x040062C0 RID: 25280
		public float BodyWeight;

		// Token: 0x040062C1 RID: 25281
		public float HeadWeight;

		// Token: 0x040062C2 RID: 25282
		public float EyesWeight;

		// Token: 0x040062C3 RID: 25283
		public float ClampWeight;
	}
}
