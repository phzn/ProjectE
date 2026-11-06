using System;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200001F RID: 31
	public interface IObjectInitializationDataProvider
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000E4 RID: 228
		string Name { get; }

		// Token: 0x060000E5 RID: 229
		ObjectInitializationData CreateObjectInitializationData();
	}
}
