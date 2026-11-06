using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025D RID: 605
	[Obsolete("IDataWatchHandle is no longer supported and will be removed soon", true)]
	internal interface IDataWatchHandle : IDisposable
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001157 RID: 4439
		Object watched { get; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001158 RID: 4440
		bool disposed { get; }
	}
}
