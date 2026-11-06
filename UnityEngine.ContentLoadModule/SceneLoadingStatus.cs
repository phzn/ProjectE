using System;

namespace Unity.Loading
{
	// Token: 0x02000006 RID: 6
	public enum SceneLoadingStatus
	{
		// Token: 0x0400000B RID: 11
		InProgress,
		// Token: 0x0400000C RID: 12
		WaitingForIntegrate,
		// Token: 0x0400000D RID: 13
		WillIntegrateNextFrame,
		// Token: 0x0400000E RID: 14
		Complete,
		// Token: 0x0400000F RID: 15
		Failed
	}
}
