using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000265 RID: 613
	internal interface ILiveReloadAssetTracker<T> where T : ScriptableObject
	{
		// Token: 0x0600116B RID: 4459
		int StartTrackingAsset(T asset);

		// Token: 0x0600116C RID: 4460
		void StopTrackingAsset(T asset);

		// Token: 0x0600116D RID: 4461
		bool IsTrackingAsset(T asset);

		// Token: 0x0600116E RID: 4462
		bool IsTrackingAssets();

		// Token: 0x0600116F RID: 4463
		bool CheckTrackedAssetsDirty();

		// Token: 0x06001170 RID: 4464
		void UpdateAssetTrackerCounts(T asset, int newDirtyCount, int newElementCount, int newInlinePropertiesCount, int newAttributePropertiesDirtyCount);

		// Token: 0x06001171 RID: 4465
		bool OnAssetsImported(HashSet<T> changedAssets, HashSet<string> deletedAssets);

		// Token: 0x06001172 RID: 4466
		void OnTrackedAssetChanged();
	}
}
