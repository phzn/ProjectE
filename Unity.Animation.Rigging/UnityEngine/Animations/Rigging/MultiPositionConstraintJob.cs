using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000024 RID: 36
	[BurstCompile]
	public struct MultiPositionConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000444F File Offset: 0x0000264F
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004457 File Offset: 0x00002657
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x060000A0 RID: 160 RVA: 0x00004460 File Offset: 0x00002660
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004464 File Offset: 0x00002664
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
			Vector3 position = this.driven.GetPosition(stream);
			Vector3 vector = position;
			Vector3 vector2 = position;
			for (int i = 0; i < this.sourceTransforms.Length; i++)
			{
				float num4 = this.weightBuffer[i] * num3;
				if (num4 >= 1E-05f)
				{
					ReadOnlyTransformHandle readOnlyTransformHandle = this.sourceTransforms[i];
					vector2 += (readOnlyTransformHandle.GetPosition(stream) + this.sourceOffsets[i] - position) * num4;
					this.sourceTransforms[i] = readOnlyTransformHandle;
				}
			}
			AffineTransform identity = AffineTransform.identity;
			if (this.drivenParent.IsValid(stream))
			{
				Vector3 vector3;
				Quaternion quaternion;
				this.drivenParent.GetGlobalTR(stream, out vector3, out quaternion);
				identity = new AffineTransform(vector3, quaternion);
				vector2 = identity.InverseTransform(vector2);
				vector = identity.InverseTransform(vector);
			}
			if (Vector3.Dot(this.axesMask, this.axesMask) < 3f)
			{
				vector2 = AnimationRuntimeUtils.Lerp(vector, vector2, this.axesMask);
			}
			vector2 = identity * (vector2 + this.drivenOffset.Get(stream));
			this.driven.SetPosition(stream, Vector3.Lerp(position, vector2, num));
		}

		// Token: 0x04000053 RID: 83
		private const float k_Epsilon = 1E-05f;

		// Token: 0x04000054 RID: 84
		public ReadWriteTransformHandle driven;

		// Token: 0x04000055 RID: 85
		public ReadOnlyTransformHandle drivenParent;

		// Token: 0x04000056 RID: 86
		public Vector3Property drivenOffset;

		// Token: 0x04000057 RID: 87
		public NativeArray<ReadOnlyTransformHandle> sourceTransforms;

		// Token: 0x04000058 RID: 88
		public NativeArray<PropertyStreamHandle> sourceWeights;

		// Token: 0x04000059 RID: 89
		public NativeArray<Vector3> sourceOffsets;

		// Token: 0x0400005A RID: 90
		public NativeArray<float> weightBuffer;

		// Token: 0x0400005B RID: 91
		public Vector3 axesMask;
	}
}
