using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200001E RID: 30
	[BurstCompile]
	public struct MultiAimConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003851 File Offset: 0x00001A51
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003859 File Offset: 0x00001A59
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x06000077 RID: 119 RVA: 0x00003862 File Offset: 0x00001A62
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003864 File Offset: 0x00001A64
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
			Quaternion quaternion = Quaternion.Inverse(this.drivenParent.GetRotation(stream));
			Matrix4x4 matrix4x = Matrix4x4.Inverse(this.drivenParent.GetLocalToWorldMatrix(stream));
			float num4 = 0f;
			Quaternion quaternion2 = QuaternionExt.zero;
			Vector3 position = this.driven.GetPosition(stream);
			Quaternion localRotation = this.driven.GetLocalRotation(stream);
			Vector3 vector = this.ComputeWorldUpVector(stream);
			Vector3 vector2 = AnimationRuntimeUtils.Select(Vector3.zero, this.upAxis, this.axesMask);
			bool flag = Vector3.Dot(this.axesMask, this.axesMask) < 3f;
			bool flag2 = this.worldUpType != MultiAimConstraintJob.WorldUpType.None && Vector3.Dot(vector2, vector2) > 1E-05f;
			Vector2 vector3 = new Vector2(this.minLimit.Get(stream), this.maxLimit.Get(stream));
			for (int i = 0; i < this.sourceTransforms.Length; i++)
			{
				float num5 = this.weightBuffer[i] * num3;
				if (num5 >= 1E-05f)
				{
					ReadOnlyTransformHandle readOnlyTransformHandle = this.sourceTransforms[i];
					Vector3 vector4 = localRotation * this.aimAxis;
					Vector3 vector5 = matrix4x.MultiplyVector(readOnlyTransformHandle.GetPosition(stream) - position);
					if (vector5.sqrMagnitude >= 1E-05f)
					{
						Vector3 vector6 = Vector3.Cross(vector4, vector5).normalized;
						if (flag)
						{
							vector6 = AnimationRuntimeUtils.Select(Vector3.zero, vector6, this.axesMask).normalized;
							if (Vector3.Dot(vector6, vector6) > 1E-05f)
							{
								vector4 = AnimationRuntimeUtils.ProjectOnPlane(vector4, vector6);
								vector5 = AnimationRuntimeUtils.ProjectOnPlane(vector5, vector6);
							}
							else
							{
								vector5 = vector4;
							}
						}
						Quaternion quaternion3 = Quaternion.AngleAxis(Mathf.Clamp(Vector3.Angle(vector4, vector5), vector3.x, vector3.y), vector6);
						if (flag2)
						{
							Vector3 normalized = Vector3.Cross(Vector3.Cross(quaternion * vector, vector5).normalized, vector5).normalized;
							quaternion3 = QuaternionExt.FromToRotation(Vector3.Cross(Vector3.Cross(quaternion3 * localRotation * vector2, vector5).normalized, vector5).normalized, normalized) * quaternion3;
						}
						quaternion2 = QuaternionExt.Add(quaternion2, QuaternionExt.Scale(this.sourceOffsets[i] * quaternion3, num5));
						this.sourceTransforms[i] = readOnlyTransformHandle;
						num4 += num5;
					}
				}
			}
			quaternion2 = QuaternionExt.NormalizeSafe(quaternion2);
			if (num4 < 1f)
			{
				quaternion2 = Quaternion.Lerp(Quaternion.identity, quaternion2, num4);
			}
			Quaternion quaternion4 = quaternion2 * localRotation;
			if (flag)
			{
				quaternion4 = Quaternion.Euler(AnimationRuntimeUtils.Select(localRotation.eulerAngles, quaternion4.eulerAngles, this.axesMask));
			}
			Vector3 vector7 = this.drivenOffset.Get(stream);
			if (Vector3.Dot(vector7, vector7) > 0f)
			{
				quaternion4 *= Quaternion.Euler(vector7);
			}
			this.driven.SetLocalRotation(stream, Quaternion.Lerp(localRotation, quaternion4, num));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003BF4 File Offset: 0x00001DF4
		private Vector3 ComputeWorldUpVector(AnimationStream stream)
		{
			Vector3 vector = Vector3.up;
			switch (this.worldUpType)
			{
			case MultiAimConstraintJob.WorldUpType.None:
				vector = Vector3.zero;
				break;
			case MultiAimConstraintJob.WorldUpType.ObjectUp:
			{
				Vector3 vector2 = Vector3.zero;
				if (this.worldUpObject.IsValid(stream))
				{
					vector2 = this.worldUpObject.GetPosition(stream);
				}
				Vector3 position = this.driven.GetPosition(stream);
				vector = (vector2 - position).normalized;
				break;
			}
			case MultiAimConstraintJob.WorldUpType.ObjectRotationUp:
			{
				Quaternion quaternion = Quaternion.identity;
				if (this.worldUpObject.IsValid(stream))
				{
					quaternion = this.worldUpObject.GetRotation(stream);
				}
				vector = quaternion * this.worldUpAxis;
				break;
			}
			case MultiAimConstraintJob.WorldUpType.Vector:
				vector = this.worldUpAxis;
				break;
			}
			return vector;
		}

		// Token: 0x04000038 RID: 56
		private const float k_Epsilon = 1E-05f;

		// Token: 0x04000039 RID: 57
		public ReadWriteTransformHandle driven;

		// Token: 0x0400003A RID: 58
		public ReadOnlyTransformHandle drivenParent;

		// Token: 0x0400003B RID: 59
		public Vector3Property drivenOffset;

		// Token: 0x0400003C RID: 60
		public NativeArray<ReadOnlyTransformHandle> sourceTransforms;

		// Token: 0x0400003D RID: 61
		public NativeArray<PropertyStreamHandle> sourceWeights;

		// Token: 0x0400003E RID: 62
		public NativeArray<Quaternion> sourceOffsets;

		// Token: 0x0400003F RID: 63
		public NativeArray<float> weightBuffer;

		// Token: 0x04000040 RID: 64
		public Vector3 aimAxis;

		// Token: 0x04000041 RID: 65
		public Vector3 upAxis;

		// Token: 0x04000042 RID: 66
		public MultiAimConstraintJob.WorldUpType worldUpType;

		// Token: 0x04000043 RID: 67
		public Vector3 worldUpAxis;

		// Token: 0x04000044 RID: 68
		public ReadOnlyTransformHandle worldUpObject;

		// Token: 0x04000045 RID: 69
		public Vector3 axesMask;

		// Token: 0x04000046 RID: 70
		public FloatProperty minLimit;

		// Token: 0x04000047 RID: 71
		public FloatProperty maxLimit;

		// Token: 0x0200007B RID: 123
		public enum WorldUpType
		{
			// Token: 0x0400014C RID: 332
			None,
			// Token: 0x0400014D RID: 333
			SceneUp,
			// Token: 0x0400014E RID: 334
			ObjectUp,
			// Token: 0x0400014F RID: 335
			ObjectRotationUp,
			// Token: 0x04000150 RID: 336
			Vector
		}
	}
}
