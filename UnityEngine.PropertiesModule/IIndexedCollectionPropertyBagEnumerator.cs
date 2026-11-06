using System;

namespace Unity.Properties
{
	// Token: 0x0200003B RID: 59
	internal interface IIndexedCollectionPropertyBagEnumerator<TContainer>
	{
		// Token: 0x06000113 RID: 275
		int GetCount(ref TContainer container);

		// Token: 0x06000114 RID: 276
		IProperty<TContainer> GetSharedProperty();

		// Token: 0x06000115 RID: 277
		IndexedCollectionSharedPropertyState GetSharedPropertyState();

		// Token: 0x06000116 RID: 278
		void SetSharedPropertyState(IndexedCollectionSharedPropertyState state);
	}
}
