using System;

namespace Unity.Properties
{
	// Token: 0x02000023 RID: 35
	public interface IDictionaryElementProperty<out TKey> : IDictionaryElementProperty, ICollectionElementProperty
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000073 RID: 115
		TKey Key { get; }
	}
}
