using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public abstract class LocalizedAssetBase : LocalizedReference
	{
		// Token: 0x0600005E RID: 94
		public abstract AsyncOperationHandle<Object> LoadAssetAsObjectAsync();

		// Token: 0x0600005F RID: 95
		public abstract AsyncOperationHandle<TObject> LoadAssetAsync<TObject>() where TObject : Object;
	}
}
