using System;

namespace Unity.Properties
{
	// Token: 0x02000041 RID: 65
	public interface IKeyedProperties<TContainer, TKey>
	{
		// Token: 0x0600012C RID: 300
		bool TryGetProperty(ref TContainer container, TKey key, out IProperty<TContainer> property);
	}
}
