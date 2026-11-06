using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.AddressableAssets
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class AssetReferenceT<TObject> : AssetReference where TObject : Object
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00005AD5 File Offset: 0x00003CD5
		public AssetReferenceT(string guid)
			: base(guid)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005ADE File Offset: 0x00003CDE
		[Obsolete]
		public AsyncOperationHandle<TObject> LoadAsset()
		{
			return this.LoadAssetAsync();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005AE6 File Offset: 0x00003CE6
		public virtual AsyncOperationHandle<TObject> LoadAssetAsync()
		{
			return this.LoadAssetAsync<TObject>();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005AF0 File Offset: 0x00003CF0
		public override bool ValidateAsset(Object obj)
		{
			Type type = obj.GetType();
			return typeof(TObject).IsAssignableFrom(type);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005B14 File Offset: 0x00003D14
		public override bool ValidateAsset(string mainAssetPath)
		{
			return false;
		}
	}
}
