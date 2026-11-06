using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200028D RID: 653
	[Flags]
	internal enum RenderHints
	{
		// Token: 0x0400085D RID: 2141
		None = 0,
		// Token: 0x0400085E RID: 2142
		GroupTransform = 1,
		// Token: 0x0400085F RID: 2143
		BoneTransform = 2,
		// Token: 0x04000860 RID: 2144
		ClipWithScissors = 4,
		// Token: 0x04000861 RID: 2145
		MaskContainer = 8,
		// Token: 0x04000862 RID: 2146
		DynamicColor = 16,
		// Token: 0x04000863 RID: 2147
		DirtyOffset = 5,
		// Token: 0x04000864 RID: 2148
		DirtyGroupTransform = 32,
		// Token: 0x04000865 RID: 2149
		DirtyBoneTransform = 64,
		// Token: 0x04000866 RID: 2150
		DirtyClipWithScissors = 128,
		// Token: 0x04000867 RID: 2151
		DirtyMaskContainer = 256,
		// Token: 0x04000868 RID: 2152
		DirtyDynamicColor = 512,
		// Token: 0x04000869 RID: 2153
		DirtyAll = 992
	}
}
