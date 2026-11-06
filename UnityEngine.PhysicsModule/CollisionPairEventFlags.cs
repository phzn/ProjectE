using System;

namespace UnityEngine
{
	// Token: 0x02000040 RID: 64
	internal enum CollisionPairEventFlags : ushort
	{
		// Token: 0x040000F0 RID: 240
		SolveContacts = 1,
		// Token: 0x040000F1 RID: 241
		ModifyContacts,
		// Token: 0x040000F2 RID: 242
		NotifyTouchFound = 4,
		// Token: 0x040000F3 RID: 243
		NotifyTouchPersists = 8,
		// Token: 0x040000F4 RID: 244
		NotifyTouchLost = 16,
		// Token: 0x040000F5 RID: 245
		NotifyTouchCCD = 32,
		// Token: 0x040000F6 RID: 246
		NotifyThresholdForceFound = 64,
		// Token: 0x040000F7 RID: 247
		NotifyThresholdForcePersists = 128,
		// Token: 0x040000F8 RID: 248
		NotifyThresholdForceLost = 256,
		// Token: 0x040000F9 RID: 249
		NotifyContactPoint = 512,
		// Token: 0x040000FA RID: 250
		DetectDiscreteContact = 1024,
		// Token: 0x040000FB RID: 251
		DetectCCDContact = 2048,
		// Token: 0x040000FC RID: 252
		PreSolverVelocity = 4096,
		// Token: 0x040000FD RID: 253
		PostSolverVelocity = 8192,
		// Token: 0x040000FE RID: 254
		ContactEventPose = 16384,
		// Token: 0x040000FF RID: 255
		NextFree = 32768,
		// Token: 0x04000100 RID: 256
		ContactDefault = 1025,
		// Token: 0x04000101 RID: 257
		TriggerDefault = 1044
	}
}
