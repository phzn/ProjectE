using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000039 RID: 57
	[BurstCompile]
	public struct TwistCorrectionJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005EED File Offset: 0x000040ED
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005EF5 File Offset: 0x000040F5
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x0600010E RID: 270 RVA: 0x00005EFE File Offset: 0x000040FE
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005F00 File Offset: 0x00004100
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num <= 0f)
			{
				for (int i = 0; i < this.twistTransforms.Length; i++)
				{
					AnimationRuntimeUtils.PassThrough(stream, this.twistTransforms[i]);
				}
				return;
			}
			if (this.twistTransforms.Length == 0)
			{
				return;
			}
			AnimationStreamHandleUtility.ReadFloats(stream, this.twistWeights, this.weightBuffer);
			Quaternion quaternion = TwistCorrectionJob.TwistRotation(this.axisMask, this.sourceInverseBindRotation * this.source.GetLocalRotation(stream));
			Quaternion quaternion2 = Quaternion.Inverse(quaternion);
			for (int j = 0; j < this.twistTransforms.Length; j++)
			{
				ReadWriteTransformHandle readWriteTransformHandle = this.twistTransforms[j];
				float num2 = Mathf.Clamp(this.weightBuffer[j], -1f, 1f);
				Quaternion quaternion3 = Quaternion.Lerp(Quaternion.identity, (Mathf.Sign(num2) < 0f) ? quaternion2 : quaternion, Mathf.Abs(num2));
				readWriteTransformHandle.SetLocalRotation(stream, Quaternion.Lerp(this.twistBindRotations[j], quaternion3, num));
				this.twistTransforms[j] = readWriteTransformHandle;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000603E File Offset: 0x0000423E
		private static Quaternion TwistRotation(Vector3 axis, Quaternion rot)
		{
			return new Quaternion(axis.x * rot.x, axis.y * rot.y, axis.z * rot.z, rot.w);
		}

		// Token: 0x04000090 RID: 144
		public ReadOnlyTransformHandle source;

		// Token: 0x04000091 RID: 145
		public Quaternion sourceInverseBindRotation;

		// Token: 0x04000092 RID: 146
		public Vector3 axisMask;

		// Token: 0x04000093 RID: 147
		public NativeArray<ReadWriteTransformHandle> twistTransforms;

		// Token: 0x04000094 RID: 148
		public NativeArray<PropertyStreamHandle> twistWeights;

		// Token: 0x04000095 RID: 149
		public NativeArray<Quaternion> twistBindRotations;

		// Token: 0x04000096 RID: 150
		public NativeArray<float> weightBuffer;
	}
}
