using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200025E RID: 606
	[Obsolete("IDataWatchService is no longer supported and will be removed soon", true)]
	internal interface IDataWatchService
	{
		// Token: 0x06001159 RID: 4441
		IDataWatchHandle AddWatch(Object watched, Action<Object> onDataChanged);

		// Token: 0x0600115A RID: 4442
		void RemoveWatch(IDataWatchHandle handle);

		// Token: 0x0600115B RID: 4443
		void ForceDirtyNextPoll(Object obj);
	}
}
