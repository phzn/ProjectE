using System;

namespace Unity.Properties
{
	// Token: 0x0200003F RID: 63
	public interface IIndexedProperties<TContainer>
	{
		// Token: 0x0600012A RID: 298
		bool TryGetProperty(ref TContainer container, int index, out IProperty<TContainer> property);
	}
}
