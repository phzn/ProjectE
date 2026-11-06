using System;
using Unity.Burst;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000008 RID: 8
	[BurstCompile]
	public struct BlendConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000244C File Offset: 0x0000064C
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002454 File Offset: 0x00000654
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x06000018 RID: 24 RVA: 0x0000245D File Offset: 0x0000065D
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002460 File Offset: 0x00000660
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num <= 0f)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.driven);
				return;
			}
			if (this.blendPosition.Get(stream))
			{
				Vector3 vector = Vector3.Lerp(this.sourceA.GetPosition(stream) + this.sourceAOffset.translation, this.sourceB.GetPosition(stream) + this.sourceBOffset.translation, this.positionWeight.Get(stream));
				this.driven.SetPosition(stream, Vector3.Lerp(this.driven.GetPosition(stream), vector, num));
			}
			else
			{
				this.driven.SetLocalPosition(stream, this.driven.GetLocalPosition(stream));
			}
			if (this.blendRotation.Get(stream))
			{
				Quaternion quaternion = Quaternion.Lerp(this.sourceA.GetRotation(stream) * this.sourceAOffset.rotation, this.sourceB.GetRotation(stream) * this.sourceBOffset.rotation, this.rotationWeight.Get(stream));
				this.driven.SetRotation(stream, Quaternion.Lerp(this.driven.GetRotation(stream), quaternion, num));
				return;
			}
			this.driven.SetLocalRotation(stream, this.driven.GetLocalRotation(stream));
		}

		// Token: 0x04000004 RID: 4
		private const int k_BlendTranslationMask = 1;

		// Token: 0x04000005 RID: 5
		private const int k_BlendRotationMask = 2;

		// Token: 0x04000006 RID: 6
		public ReadWriteTransformHandle driven;

		// Token: 0x04000007 RID: 7
		public ReadOnlyTransformHandle sourceA;

		// Token: 0x04000008 RID: 8
		public ReadOnlyTransformHandle sourceB;

		// Token: 0x04000009 RID: 9
		public AffineTransform sourceAOffset;

		// Token: 0x0400000A RID: 10
		public AffineTransform sourceBOffset;

		// Token: 0x0400000B RID: 11
		public BoolProperty blendPosition;

		// Token: 0x0400000C RID: 12
		public BoolProperty blendRotation;

		// Token: 0x0400000D RID: 13
		public FloatProperty positionWeight;

		// Token: 0x0400000E RID: 14
		public FloatProperty rotationWeight;
	}
}
