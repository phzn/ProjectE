using System;
using Unity.Burst;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200002D RID: 45
	[BurstCompile]
	public struct OverrideTransformJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004E34 File Offset: 0x00003034
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00004E3C File Offset: 0x0000303C
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x060000C9 RID: 201 RVA: 0x00004E45 File Offset: 0x00003045
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004E48 File Offset: 0x00003048
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num <= 0f)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.driven);
				return;
			}
			AffineTransform affineTransform2;
			if (this.source.IsValid(stream))
			{
				Vector3 vector;
				Quaternion quaternion;
				Vector3 vector2;
				this.source.GetLocalTRS(stream, out vector, out quaternion, out vector2);
				AffineTransform affineTransform = new AffineTransform(vector, quaternion);
				Quaternion quaternion2 = this.cache.Get<Quaternion>(this.sourceToCurrSpaceRotIdx, 0);
				affineTransform2 = Quaternion.Inverse(quaternion2) * (this.sourceInvLocalBindTx * affineTransform) * quaternion2;
			}
			else
			{
				affineTransform2 = new AffineTransform(this.position.Get(stream), Quaternion.Euler(this.rotation.Get(stream)));
			}
			OverrideTransformJob.Space space = (OverrideTransformJob.Space)this.cache.GetRaw(this.spaceIdx, 0);
			float num2 = this.positionWeight.Get(stream) * num;
			float num3 = this.rotationWeight.Get(stream) * num;
			switch (space)
			{
			case OverrideTransformJob.Space.World:
			{
				Vector3 vector3;
				Quaternion quaternion3;
				this.driven.GetGlobalTR(stream, out vector3, out quaternion3);
				this.driven.SetGlobalTR(stream, Vector3.Lerp(vector3, affineTransform2.translation, num2), Quaternion.Lerp(quaternion3, affineTransform2.rotation, num3), false);
				return;
			}
			case OverrideTransformJob.Space.Local:
			{
				Vector3 vector4;
				Quaternion quaternion4;
				Vector3 vector5;
				this.driven.GetLocalTRS(stream, out vector4, out quaternion4, out vector5);
				this.driven.SetLocalTRS(stream, Vector3.Lerp(vector4, affineTransform2.translation, num2), Quaternion.Lerp(quaternion4, affineTransform2.rotation, num3), vector5, false);
				return;
			}
			case OverrideTransformJob.Space.Pivot:
			{
				Vector3 vector6;
				Quaternion quaternion5;
				Vector3 vector7;
				this.driven.GetLocalTRS(stream, out vector6, out quaternion5, out vector7);
				AffineTransform affineTransform3 = new AffineTransform(vector6, quaternion5);
				affineTransform2 = affineTransform3 * affineTransform2;
				this.driven.SetLocalTRS(stream, Vector3.Lerp(affineTransform3.translation, affineTransform2.translation, num2), Quaternion.Lerp(affineTransform3.rotation, affineTransform2.rotation, num3), vector7, false);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005024 File Offset: 0x00003224
		internal void UpdateSpace(int space)
		{
			if ((int)this.cache.GetRaw(this.spaceIdx, 0) == space)
			{
				return;
			}
			this.cache.SetRaw((float)space, this.spaceIdx, 0);
			if (space == 2)
			{
				this.cache.Set<Quaternion>(this.sourceToPivotRot, this.sourceToCurrSpaceRotIdx, 0);
				return;
			}
			if (space == 1)
			{
				this.cache.Set<Quaternion>(this.sourceToLocalRot, this.sourceToCurrSpaceRotIdx, 0);
				return;
			}
			this.cache.Set<Quaternion>(this.sourceToWorldRot, this.sourceToCurrSpaceRotIdx, 0);
		}

		// Token: 0x0400006D RID: 109
		public ReadWriteTransformHandle driven;

		// Token: 0x0400006E RID: 110
		public ReadOnlyTransformHandle source;

		// Token: 0x0400006F RID: 111
		public AffineTransform sourceInvLocalBindTx;

		// Token: 0x04000070 RID: 112
		public Quaternion sourceToWorldRot;

		// Token: 0x04000071 RID: 113
		public Quaternion sourceToLocalRot;

		// Token: 0x04000072 RID: 114
		public Quaternion sourceToPivotRot;

		// Token: 0x04000073 RID: 115
		public CacheIndex spaceIdx;

		// Token: 0x04000074 RID: 116
		public CacheIndex sourceToCurrSpaceRotIdx;

		// Token: 0x04000075 RID: 117
		public Vector3Property position;

		// Token: 0x04000076 RID: 118
		public Vector3Property rotation;

		// Token: 0x04000077 RID: 119
		public FloatProperty positionWeight;

		// Token: 0x04000078 RID: 120
		public FloatProperty rotationWeight;

		// Token: 0x04000079 RID: 121
		public AnimationJobCache cache;

		// Token: 0x0200007C RID: 124
		public enum Space
		{
			// Token: 0x04000152 RID: 338
			World,
			// Token: 0x04000153 RID: 339
			Local,
			// Token: 0x04000154 RID: 340
			Pivot
		}
	}
}
