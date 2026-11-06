using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200002A RID: 42
	[BurstCompile]
	public struct MultiRotationConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004A8E File Offset: 0x00002C8E
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00004A96 File Offset: 0x00002C96
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x060000BA RID: 186 RVA: 0x00004A9F File Offset: 0x00002C9F
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num <= 0f)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.driven);
				return;
			}
			AnimationStreamHandleUtility.ReadFloats(stream, this.sourceWeights, this.weightBuffer);
			float num2 = AnimationRuntimeUtils.Sum(this.weightBuffer);
			if (num2 < 1E-05f)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.driven);
				return;
			}
			float num3 = ((num2 > 1f) ? (1f / num2) : 1f);
			float num4 = 0f;
			Quaternion quaternion = QuaternionExt.zero;
			for (int i = 0; i < this.sourceTransforms.Length; i++)
			{
				float num5 = this.weightBuffer[i] * num3;
				if (num5 >= 1E-05f)
				{
					ReadOnlyTransformHandle readOnlyTransformHandle = this.sourceTransforms[i];
					quaternion = QuaternionExt.Add(quaternion, QuaternionExt.Scale(readOnlyTransformHandle.GetRotation(stream) * this.sourceOffsets[i], num5));
					this.sourceTransforms[i] = readOnlyTransformHandle;
					num4 += num5;
				}
			}
			quaternion = QuaternionExt.NormalizeSafe(quaternion);
			if (num4 < 1f)
			{
				quaternion = Quaternion.Lerp(this.driven.GetRotation(stream), quaternion, num4);
			}
			if (this.drivenParent.IsValid(stream))
			{
				quaternion = Quaternion.Inverse(this.drivenParent.GetRotation(stream)) * quaternion;
			}
			Quaternion localRotation = this.driven.GetLocalRotation(stream);
			if (Vector3.Dot(this.axesMask, this.axesMask) < 3f)
			{
				quaternion = Quaternion.Euler(AnimationRuntimeUtils.Lerp(localRotation.eulerAngles, quaternion.eulerAngles, this.axesMask));
			}
			Vector3 vector = this.drivenOffset.Get(stream);
			if (Vector3.Dot(vector, vector) > 0f)
			{
				quaternion *= Quaternion.Euler(vector);
			}
			this.driven.SetLocalRotation(stream, Quaternion.Lerp(localRotation, quaternion, num));
		}

		// Token: 0x04000063 RID: 99
		private const float k_Epsilon = 1E-05f;

		// Token: 0x04000064 RID: 100
		public ReadWriteTransformHandle driven;

		// Token: 0x04000065 RID: 101
		public ReadOnlyTransformHandle drivenParent;

		// Token: 0x04000066 RID: 102
		public Vector3Property drivenOffset;

		// Token: 0x04000067 RID: 103
		public NativeArray<ReadOnlyTransformHandle> sourceTransforms;

		// Token: 0x04000068 RID: 104
		public NativeArray<PropertyStreamHandle> sourceWeights;

		// Token: 0x04000069 RID: 105
		public NativeArray<Quaternion> sourceOffsets;

		// Token: 0x0400006A RID: 106
		public NativeArray<float> weightBuffer;

		// Token: 0x0400006B RID: 107
		public Vector3 axesMask;
	}
}
