using System;
using Unity.Burst;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000027 RID: 39
	[BurstCompile]
	public struct MultiReferentialConstraintJob : IWeightedAnimationJob, IAnimationJob
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000047BF File Offset: 0x000029BF
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000047C7 File Offset: 0x000029C7
		public FloatProperty jobWeight { readonly get; set; }

		// Token: 0x060000AF RID: 175 RVA: 0x000047D0 File Offset: 0x000029D0
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000047D4 File Offset: 0x000029D4
		public void ProcessAnimation(AnimationStream stream)
		{
			float num = this.jobWeight.Get(stream);
			if (num > 0f)
			{
				int num2 = this.driver.Get(stream);
				if (num2 != this.m_PrevDriverIdx)
				{
					this.UpdateOffsets(num2);
				}
				Vector3 vector;
				Quaternion quaternion;
				this.sources[num2].GetGlobalTR(stream, out vector, out quaternion);
				AffineTransform affineTransform = new AffineTransform(vector, quaternion);
				int num3 = 0;
				for (int i = 0; i < this.sources.Length; i++)
				{
					if (i != num2)
					{
						AffineTransform affineTransform2 = affineTransform * this.offsetTx[num3];
						ReadWriteTransformHandle readWriteTransformHandle = this.sources[i];
						Vector3 vector2;
						Quaternion quaternion2;
						readWriteTransformHandle.GetGlobalTR(stream, out vector2, out quaternion2);
						readWriteTransformHandle.SetGlobalTR(stream, Vector3.Lerp(vector2, affineTransform2.translation, num), Quaternion.Lerp(quaternion2, affineTransform2.rotation, num), false);
						num3++;
						this.sources[i] = readWriteTransformHandle;
					}
				}
				AnimationRuntimeUtils.PassThrough(stream, this.sources[num2]);
				return;
			}
			for (int j = 0; j < this.sources.Length; j++)
			{
				AnimationRuntimeUtils.PassThrough(stream, this.sources[j]);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004910 File Offset: 0x00002B10
		internal void UpdateOffsets(int driver)
		{
			driver = Mathf.Clamp(driver, 0, this.sources.Length - 1);
			int num = 0;
			AffineTransform affineTransform = this.sourceBindTx[driver].Inverse();
			for (int i = 0; i < this.sourceBindTx.Length; i++)
			{
				if (i != driver)
				{
					this.offsetTx[num] = affineTransform * this.sourceBindTx[i];
					num++;
				}
			}
			this.m_PrevDriverIdx = driver;
		}

		// Token: 0x0400005D RID: 93
		public IntProperty driver;

		// Token: 0x0400005E RID: 94
		public NativeArray<ReadWriteTransformHandle> sources;

		// Token: 0x0400005F RID: 95
		public NativeArray<AffineTransform> sourceBindTx;

		// Token: 0x04000060 RID: 96
		public NativeArray<AffineTransform> offsetTx;

		// Token: 0x04000061 RID: 97
		private int m_PrevDriverIdx;
	}
}
