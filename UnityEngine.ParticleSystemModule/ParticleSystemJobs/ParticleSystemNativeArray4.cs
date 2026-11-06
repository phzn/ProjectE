using System;
using Unity.Collections;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000068 RID: 104
	public struct ParticleSystemNativeArray4
	{
		// Token: 0x170001EF RID: 495
		public Vector4 this[int index]
		{
			get
			{
				return new Vector4(this.x[index], this.y[index], this.z[index], this.w[index]);
			}
			set
			{
				this.x[index] = value.x;
				this.y[index] = value.y;
				this.z[index] = value.z;
				this.w[index] = value.w;
			}
		}

		// Token: 0x04000197 RID: 407
		public NativeArray<float> x;

		// Token: 0x04000198 RID: 408
		public NativeArray<float> y;

		// Token: 0x04000199 RID: 409
		public NativeArray<float> z;

		// Token: 0x0400019A RID: 410
		public NativeArray<float> w;
	}
}
