using System;

namespace UnityEngine.VFX
{
	// Token: 0x02000006 RID: 6
	internal enum VFXTaskType
	{
		// Token: 0x040000B6 RID: 182
		None,
		// Token: 0x040000B7 RID: 183
		Spawner = 268435456,
		// Token: 0x040000B8 RID: 184
		Initialize = 536870912,
		// Token: 0x040000B9 RID: 185
		Update = 805306368,
		// Token: 0x040000BA RID: 186
		Output = 1073741824,
		// Token: 0x040000BB RID: 187
		CameraSort = 805306369,
		// Token: 0x040000BC RID: 188
		PerCameraUpdate,
		// Token: 0x040000BD RID: 189
		PerCameraSort,
		// Token: 0x040000BE RID: 190
		PerOutputSort,
		// Token: 0x040000BF RID: 191
		GlobalSort,
		// Token: 0x040000C0 RID: 192
		ParticlePointOutput = 1073741824,
		// Token: 0x040000C1 RID: 193
		ParticleLineOutput,
		// Token: 0x040000C2 RID: 194
		ParticleQuadOutput,
		// Token: 0x040000C3 RID: 195
		ParticleHexahedronOutput,
		// Token: 0x040000C4 RID: 196
		ParticleMeshOutput,
		// Token: 0x040000C5 RID: 197
		ParticleTriangleOutput,
		// Token: 0x040000C6 RID: 198
		ParticleOctagonOutput,
		// Token: 0x040000C7 RID: 199
		ConstantRateSpawner = 268435456,
		// Token: 0x040000C8 RID: 200
		BurstSpawner,
		// Token: 0x040000C9 RID: 201
		PeriodicBurstSpawner,
		// Token: 0x040000CA RID: 202
		VariableRateSpawner,
		// Token: 0x040000CB RID: 203
		CustomCallbackSpawner,
		// Token: 0x040000CC RID: 204
		SetAttributeSpawner,
		// Token: 0x040000CD RID: 205
		EvaluateExpressionsSpawner
	}
}
