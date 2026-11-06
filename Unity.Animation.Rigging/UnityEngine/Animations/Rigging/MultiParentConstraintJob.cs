using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000021 RID: 33
	[BurstCompile]
	public struct MultiParentConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003F3A File Offset: 0x0000213A
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00003F42 File Offset: 0x00002142
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x0600008E RID: 142 RVA: 0x00003F4B File Offset: 0x0000214B
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003F50 File Offset: 0x00002150
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
			AffineTransform affineTransform = new AffineTransform(Vector3.zero, QuaternionExt.zero);
			for (int i = 0; i < this.sourceTransforms.Length; i++)
			{
				ReadOnlyTransformHandle readOnlyTransformHandle = this.sourceTransforms[i];
				float num5 = this.weightBuffer[i] * num3;
				if (num5 >= 1E-05f)
				{
					Vector3 vector;
					Quaternion quaternion;
					readOnlyTransformHandle.GetGlobalTR(stream, out vector, out quaternion);
					AffineTransform affineTransform2 = new AffineTransform(vector, quaternion);
					affineTransform2 *= this.sourceOffsets[i];
					affineTransform.translation += affineTransform2.translation * num5;
					affineTransform.rotation = QuaternionExt.Add(affineTransform.rotation, QuaternionExt.Scale(affineTransform2.rotation, num5));
					this.sourceTransforms[i] = readOnlyTransformHandle;
					num4 += num5;
				}
			}
			Vector3 vector2;
			Quaternion quaternion2;
			this.driven.GetGlobalTR(stream, out vector2, out quaternion2);
			AffineTransform affineTransform3 = new AffineTransform(vector2, quaternion2);
			affineTransform.rotation = QuaternionExt.NormalizeSafe(affineTransform.rotation);
			if (num4 < 1f)
			{
				affineTransform.translation += vector2 * (1f - num4);
				affineTransform.rotation = Quaternion.Lerp(quaternion2, affineTransform.rotation, num4);
			}
			AffineTransform identity = AffineTransform.identity;
			if (this.drivenParent.IsValid(stream))
			{
				Vector3 vector3;
				Quaternion quaternion3;
				this.drivenParent.GetGlobalTR(stream, out vector3, out quaternion3);
				identity = new AffineTransform(vector3, quaternion3);
				affineTransform = identity.InverseMul(affineTransform);
				affineTransform3 = identity.InverseMul(affineTransform3);
			}
			if (Vector3.Dot(this.positionAxesMask, this.positionAxesMask) < 3f)
			{
				affineTransform.translation = AnimationRuntimeUtils.Lerp(affineTransform3.translation, affineTransform.translation, this.positionAxesMask);
			}
			if (Vector3.Dot(this.rotationAxesMask, this.rotationAxesMask) < 3f)
			{
				affineTransform.rotation = Quaternion.Euler(AnimationRuntimeUtils.Lerp(affineTransform3.rotation.eulerAngles, affineTransform.rotation.eulerAngles, this.rotationAxesMask));
			}
			affineTransform = identity * affineTransform;
			this.driven.SetGlobalTR(stream, Vector3.Lerp(vector2, affineTransform.translation, num), Quaternion.Lerp(quaternion2, affineTransform.rotation, num), false);
		}

		// Token: 0x04000049 RID: 73
		private const float k_Epsilon = 1E-05f;

		// Token: 0x0400004A RID: 74
		public ReadWriteTransformHandle driven;

		// Token: 0x0400004B RID: 75
		public ReadOnlyTransformHandle drivenParent;

		// Token: 0x0400004C RID: 76
		public NativeArray<ReadOnlyTransformHandle> sourceTransforms;

		// Token: 0x0400004D RID: 77
		public NativeArray<PropertyStreamHandle> sourceWeights;

		// Token: 0x0400004E RID: 78
		public NativeArray<AffineTransform> sourceOffsets;

		// Token: 0x0400004F RID: 79
		public NativeArray<float> weightBuffer;

		// Token: 0x04000050 RID: 80
		public Vector3 positionAxesMask;

		// Token: 0x04000051 RID: 81
		public Vector3 rotationAxesMask;
	}
}
