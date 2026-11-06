using System;
using Unity.Collections;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x02000067 RID: 103
	public struct ParticleSystemNativeArray3
	{
		// Token: 0x170001EE RID: 494
		public Vector3 this[int index]
		{
			get
			{
				return new Vector3(this.x[index], this.y[index], this.z[index]);
			}
			set
			{
				this.x[index] = value.x;
				this.y[index] = value.y;
				this.z[index] = value.z;
			}
		}

		// Token: 0x04000194 RID: 404
		public NativeArray<float> x;

		// Token: 0x04000195 RID: 405
		public NativeArray<float> y;

		// Token: 0x04000196 RID: 406
		public NativeArray<float> z;
	}
}
