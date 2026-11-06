using System;

namespace Unity.Properties
{
	// Token: 0x02000021 RID: 33
	public interface ISetElementProperty<out TKey> : ISetElementProperty, ICollectionElementProperty
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000071 RID: 113
		TKey Key { get; }
	}
}
