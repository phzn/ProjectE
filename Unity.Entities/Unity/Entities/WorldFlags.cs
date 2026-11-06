using System;

namespace Unity.Entities
{
	// Token: 0x0200034D RID: 845
	[Flags]
	public enum WorldFlags
	{
		// Token: 0x04000876 RID: 2166
		None = 0,
		// Token: 0x04000877 RID: 2167
		Live = 1,
		// Token: 0x04000878 RID: 2168
		Editor = 3,
		// Token: 0x04000879 RID: 2169
		Game = 5,
		// Token: 0x0400087A RID: 2170
		Simulation = 9,
		// Token: 0x0400087B RID: 2171
		Conversion = 16,
		// Token: 0x0400087C RID: 2172
		Staging = 32,
		// Token: 0x0400087D RID: 2173
		Shadow = 64,
		// Token: 0x0400087E RID: 2174
		Streaming = 128,
		// Token: 0x0400087F RID: 2175
		GameServer = 257,
		// Token: 0x04000880 RID: 2176
		GameClient = 513,
		// Token: 0x04000881 RID: 2177
		GameThinClient = 1025
	}
}
