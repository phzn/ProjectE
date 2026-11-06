using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200000B RID: 11
	[BurstCompile]
	public struct ChainIKConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002777 File Offset: 0x00000977
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000277F File Offset: 0x0000097F
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x06000028 RID: 40 RVA: 0x00002788 File Offset: 0x00000988
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000278C File Offset: 0x0000098C
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num > 0f)
			{
				for (int i = 0; i < this.chain.Length; i++)
				{
					ReadWriteTransformHandle readWriteTransformHandle = this.chain[i];
					this.linkPositions[i] = readWriteTransformHandle.GetPosition(stream);
					this.chain[i] = readWriteTransformHandle;
				}
				int num2 = this.chain.Length - 1;
				if (AnimationRuntimeUtils.SolveFABRIK(ref this.linkPositions, ref this.linkLengths, this.target.GetPosition(stream) + this.targetOffset.translation, this.cache.GetRaw(this.toleranceIdx, 0), this.maxReach, (int)this.cache.GetRaw(this.maxIterationsIdx, 0)))
				{
					float num3 = this.chainRotationWeight.Get(stream) * num;
					for (int j = 0; j < num2; j++)
					{
						Vector3 vector = this.chain[j + 1].GetPosition(stream) - this.chain[j].GetPosition(stream);
						Vector3 vector2 = this.linkPositions[j + 1] - this.linkPositions[j];
						Quaternion rotation = this.chain[j].GetRotation(stream);
						this.chain[j].SetRotation(stream, Quaternion.Lerp(rotation, QuaternionExt.FromToRotation(vector, vector2) * rotation, num3));
					}
				}
				this.chain[num2].SetRotation(stream, Quaternion.Lerp(this.chain[num2].GetRotation(stream), this.target.GetRotation(stream) * this.targetOffset.rotation, this.tipRotationWeight.Get(stream) * num));
				return;
			}
			for (int k = 0; k < this.chain.Length; k++)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.chain[k]);
			}
		}

		// Token: 0x04000010 RID: 16
		public NativeArray<ReadWriteTransformHandle> chain;

		// Token: 0x04000011 RID: 17
		public ReadOnlyTransformHandle target;

		// Token: 0x04000012 RID: 18
		public AffineTransform targetOffset;

		// Token: 0x04000013 RID: 19
		public NativeArray<float> linkLengths;

		// Token: 0x04000014 RID: 20
		public NativeArray<Vector3> linkPositions;

		// Token: 0x04000015 RID: 21
		public FloatProperty chainRotationWeight;

		// Token: 0x04000016 RID: 22
		public FloatProperty tipRotationWeight;

		// Token: 0x04000017 RID: 23
		public CacheIndex toleranceIdx;

		// Token: 0x04000018 RID: 24
		public CacheIndex maxIterationsIdx;

		// Token: 0x04000019 RID: 25
		public AnimationJobCache cache;

		// Token: 0x0400001A RID: 26
		public float maxReach;
	}
}
