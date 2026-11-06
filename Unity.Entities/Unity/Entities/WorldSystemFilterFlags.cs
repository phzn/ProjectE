using System;

namespace Unity.Entities
{
	// Token: 0x02000354 RID: 852
	[Flags]
	public enum WorldSystemFilterFlags : uint
	{
		// Token: 0x04000893 RID: 2195
		Default = 1U,
		// Token: 0x04000894 RID: 2196
		Disabled = 2U,
		// Token: 0x04000895 RID: 2197
		EntitySceneOptimizations = 4U,
		// Token: 0x04000896 RID: 2198
		ProcessAfterLoad = 8U,
		// Token: 0x04000897 RID: 2199
		Editor = 64U,
		// Token: 0x04000898 RID: 2200
		BakingSystem = 128U,
		// Token: 0x04000899 RID: 2201
		LocalSimulation = 256U,
		// Token: 0x0400089A RID: 2202
		ServerSimulation = 512U,
		// Token: 0x0400089B RID: 2203
		ClientSimulation = 1024U,
		// Token: 0x0400089C RID: 2204
		ThinClientSimulation = 2048U,
		// Token: 0x0400089D RID: 2205
		Presentation = 4096U,
		// Token: 0x0400089E RID: 2206
		Streaming = 8192U,
		// Token: 0x0400089F RID: 2207
		EntityProxy = 16384U,
		// Token: 0x040008A0 RID: 2208
		EntityProxyPreview = 32768U,
		// Token: 0x040008A1 RID: 2209
		All = 4294967295U
	}
}
