using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000008 RID: 8
	public interface ILocalizedStringList
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000051 RID: 81
		// (remove) Token: 0x06000052 RID: 82
		event LocalizedListChangeHandler ListChanged;

		// Token: 0x06000053 RID: 83
		List<string> GetLocalizedList(List<string> target = null);

		// Token: 0x06000054 RID: 84
		AsyncOperationHandle<List<string>> GetLocalizedListAsync(List<string> target = null);
	}
}
