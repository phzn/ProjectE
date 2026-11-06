using System;
using Unity.Burst;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200000E RID: 14
	[BurstCompile]
	public struct DampedTransformJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002C54 File Offset: 0x00000E54
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002C5C File Offset: 0x00000E5C
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x06000039 RID: 57 RVA: 0x00002C65 File Offset: 0x00000E65
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C68 File Offset: 0x00000E68
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			float num2 = Mathf.Abs(stream.deltaTime);
			Vector3 vector;
			Quaternion quaternion;
			this.driven.GetGlobalTR(stream, out vector, out quaternion);
			if (num > 0f && num2 > 0f)
			{
				Vector3 vector2;
				Quaternion quaternion2;
				this.source.GetGlobalTR(stream, out vector2, out quaternion2);
				AffineTransform affineTransform = new AffineTransform(vector2, quaternion2);
				AffineTransform affineTransform2 = affineTransform * this.localBindTx;
				affineTransform2.translation = Vector3.Lerp(vector, affineTransform2.translation, num);
				affineTransform2.rotation = Quaternion.Lerp(quaternion, affineTransform2.rotation, num);
				float num3 = AnimationRuntimeUtils.Square(1f - this.dampPosition.Get(stream));
				float num4 = AnimationRuntimeUtils.Square(1f - this.dampRotation.Get(stream));
				bool flag = Vector3.Dot(this.aimBindAxis, this.aimBindAxis) > 0f;
				while (num2 > 0f)
				{
					float num5 = 40f * Mathf.Min(0.01667f, num2);
					this.prevDrivenTx.translation = this.prevDrivenTx.translation + (affineTransform2.translation - this.prevDrivenTx.translation) * num3 * num5;
					this.prevDrivenTx.rotation = this.prevDrivenTx.rotation * Quaternion.Lerp(Quaternion.identity, Quaternion.Inverse(this.prevDrivenTx.rotation) * affineTransform2.rotation, num4 * num5);
					if (flag)
					{
						Vector3 vector3 = this.prevDrivenTx.rotation * this.aimBindAxis;
						Vector3 vector4 = affineTransform.translation - this.prevDrivenTx.translation;
						this.prevDrivenTx.rotation = Quaternion.AngleAxis(Vector3.Angle(vector3, vector4), Vector3.Cross(vector3, vector4).normalized) * this.prevDrivenTx.rotation;
					}
					num2 -= 0.01667f;
				}
				this.driven.SetGlobalTR(stream, this.prevDrivenTx.translation, this.prevDrivenTx.rotation, false);
				return;
			}
			this.prevDrivenTx.Set(vector, quaternion);
			AnimationRuntimeUtils.PassThrough(stream, this.driven);
		}

		// Token: 0x0400001C RID: 28
		private const float k_FixedDt = 0.01667f;

		// Token: 0x0400001D RID: 29
		private const float k_DampFactor = 40f;

		// Token: 0x0400001E RID: 30
		public ReadWriteTransformHandle driven;

		// Token: 0x0400001F RID: 31
		public ReadOnlyTransformHandle source;

		// Token: 0x04000020 RID: 32
		public AffineTransform localBindTx;

		// Token: 0x04000021 RID: 33
		public Vector3 aimBindAxis;

		// Token: 0x04000022 RID: 34
		public AffineTransform prevDrivenTx;

		// Token: 0x04000023 RID: 35
		public FloatProperty dampPosition;

		// Token: 0x04000024 RID: 36
		public FloatProperty dampRotation;
	}
}
