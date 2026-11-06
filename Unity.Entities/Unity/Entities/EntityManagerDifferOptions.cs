using System;

namespace Unity.Entities
{
	// Token: 0x02000087 RID: 135
	[Flags]
	public enum EntityManagerDifferOptions
	{
		// Token: 0x04000103 RID: 259
		None = 0,
		// Token: 0x04000104 RID: 260
		IncludeForwardChangeSet = 2,
		// Token: 0x04000105 RID: 261
		IncludeReverseChangeSet = 4,
		// Token: 0x04000106 RID: 262
		FastForwardShadowWorld = 8,
		// Token: 0x04000107 RID: 263
		ClearMissingReferences = 16,
		// Token: 0x04000108 RID: 264
		ValidateUniqueEntityGuid = 32,
		// Token: 0x04000109 RID: 265
		UseReferentialEquality = 64,
		// Token: 0x0400010A RID: 266
		Default = 62
	}
}
