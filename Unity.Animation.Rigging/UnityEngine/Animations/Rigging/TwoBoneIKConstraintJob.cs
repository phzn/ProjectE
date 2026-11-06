using System;
using Unity.Burst;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200003C RID: 60
	[BurstCompile]
	public struct TwoBoneIKConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000061A1 File Offset: 0x000043A1
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000061A9 File Offset: 0x000043A9
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x0600011A RID: 282 RVA: 0x000061B2 File Offset: 0x000043B2
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000061B4 File Offset: 0x000043B4
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num > 0f)
			{
				AnimationRuntimeUtils.SolveTwoBoneIK(stream, this.root, this.mid, this.tip, this.target, this.hint, this.targetPositionWeight.Get(stream) * num, this.targetRotationWeight.Get(stream) * num, this.hintWeight.Get(stream) * num, this.targetOffset);
				return;
			}
			AnimationRuntimeUtils.PassThrough(stream, this.root);
			AnimationRuntimeUtils.PassThrough(stream, this.mid);
			AnimationRuntimeUtils.PassThrough(stream, this.tip);
		}

		// Token: 0x04000098 RID: 152
		public ReadWriteTransformHandle root;

		// Token: 0x04000099 RID: 153
		public ReadWriteTransformHandle mid;

		// Token: 0x0400009A RID: 154
		public ReadWriteTransformHandle tip;

		// Token: 0x0400009B RID: 155
		public ReadOnlyTransformHandle hint;

		// Token: 0x0400009C RID: 156
		public ReadOnlyTransformHandle target;

		// Token: 0x0400009D RID: 157
		public AffineTransform targetOffset;

		// Token: 0x0400009E RID: 158
		public FloatProperty targetPositionWeight;

		// Token: 0x0400009F RID: 159
		public FloatProperty targetRotationWeight;

		// Token: 0x040000A0 RID: 160
		public FloatProperty hintWeight;
	}
}
