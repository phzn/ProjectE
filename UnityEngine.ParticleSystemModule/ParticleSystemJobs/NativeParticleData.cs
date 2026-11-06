using System;

namespace UnityEngine.ParticleSystemJobs
{
	// Token: 0x0200006A RID: 106
	internal struct NativeParticleData
	{
		// Token: 0x040001A9 RID: 425
		internal int count;

		// Token: 0x040001AA RID: 426
		internal NativeParticleData.Array3 positions;

		// Token: 0x040001AB RID: 427
		internal NativeParticleData.Array3 velocities;

		// Token: 0x040001AC RID: 428
		internal NativeParticleData.Array3 axisOfRotations;

		// Token: 0x040001AD RID: 429
		internal NativeParticleData.Array3 rotations;

		// Token: 0x040001AE RID: 430
		internal NativeParticleData.Array3 rotationalSpeeds;

		// Token: 0x040001AF RID: 431
		internal NativeParticleData.Array3 sizes;

		// Token: 0x040001B0 RID: 432
		internal unsafe void* startColors;

		// Token: 0x040001B1 RID: 433
		internal unsafe void* aliveTimePercent;

		// Token: 0x040001B2 RID: 434
		internal unsafe void* inverseStartLifetimes;

		// Token: 0x040001B3 RID: 435
		internal unsafe void* randomSeeds;

		// Token: 0x040001B4 RID: 436
		internal NativeParticleData.Array4 customData1;

		// Token: 0x040001B5 RID: 437
		internal NativeParticleData.Array4 customData2;

		// Token: 0x040001B6 RID: 438
		internal unsafe void* meshIndices;

		// Token: 0x0200006B RID: 107
		internal struct Array3
		{
			// Token: 0x040001B7 RID: 439
			internal unsafe float* x;

			// Token: 0x040001B8 RID: 440
			internal unsafe float* y;

			// Token: 0x040001B9 RID: 441
			internal unsafe float* z;
		}

		// Token: 0x0200006C RID: 108
		internal struct Array4
		{
			// Token: 0x040001BA RID: 442
			internal unsafe float* x;

			// Token: 0x040001BB RID: 443
			internal unsafe float* y;

			// Token: 0x040001BC RID: 444
			internal unsafe float* z;

			// Token: 0x040001BD RID: 445
			internal unsafe float* w;
		}
	}
}
