using System;

namespace Unity.Properties
{
	// Token: 0x0200006C RID: 108
	public interface IListPropertyAccept<TList>
	{
		// Token: 0x060001CB RID: 459
		void Accept<TContainer>(IListPropertyVisitor visitor, Property<TContainer, TList> property, ref TContainer container, ref TList list);
	}
}
