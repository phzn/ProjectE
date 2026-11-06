using System;

namespace Unity.Properties
{
	// Token: 0x0200006B RID: 107
	public interface ICollectionPropertyAccept<TCollection>
	{
		// Token: 0x060001CA RID: 458
		void Accept<TContainer>(ICollectionPropertyVisitor visitor, Property<TContainer, TCollection> property, ref TContainer container, ref TCollection collection);
	}
}
