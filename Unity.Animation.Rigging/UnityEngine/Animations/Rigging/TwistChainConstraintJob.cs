using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000036 RID: 54
	[BurstCompile]
	public struct TwistChainConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005BAB File Offset: 0x00003DAB
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005BB3 File Offset: 0x00003DB3
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x06000102 RID: 258 RVA: 0x00005BBC File Offset: 0x00003DBC
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005BC0 File Offset: 0x00003DC0
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num > 0f)
			{
				Quaternion rotation = this.rootTarget.GetRotation(stream);
				Quaternion rotation2 = this.tipTarget.GetRotation(stream);
				this.chain[0].SetRotation(stream, Quaternion.Lerp(this.chain[0].GetRotation(stream), rotation, num));
				for (int i = 1; i < this.chain.Length - 1; i++)
				{
					this.chain[i].SetRotation(stream, Quaternion.Lerp(this.chain[i].GetRotation(stream), this.rotations[i] * Quaternion.Lerp(rotation, rotation2, this.weights[i]), num));
				}
				this.chain[this.chain.Length - 1].SetRotation(stream, Quaternion.Lerp(this.chain[this.chain.Length - 1].GetRotation(stream), rotation2, num));
				return;
			}
			for (int j = 0; j < this.chain.Length; j++)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.chain[j]);
			}
		}

		// Token: 0x04000089 RID: 137
		public ReadWriteTransformHandle rootTarget;

		// Token: 0x0400008A RID: 138
		public ReadWriteTransformHandle tipTarget;

		// Token: 0x0400008B RID: 139
		public NativeArray<ReadWriteTransformHandle> chain;

		// Token: 0x0400008C RID: 140
		public NativeArray<float> steps;

		// Token: 0x0400008D RID: 141
		public NativeArray<float> weights;

		// Token: 0x0400008E RID: 142
		public NativeArray<Quaternion> rotations;
	}
}
