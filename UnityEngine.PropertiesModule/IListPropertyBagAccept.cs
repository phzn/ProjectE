using System;

namespace Unity.Properties
{
	// Token: 0x02000067 RID: 103
	public interface IListPropertyBagAccept<TContainer>
	{
		// Token: 0x060001C6 RID: 454
		void Accept(IListPropertyBagVisitor visitor, ref TContainer container);
	}
}
