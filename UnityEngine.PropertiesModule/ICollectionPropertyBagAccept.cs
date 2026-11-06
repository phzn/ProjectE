using System;

namespace Unity.Properties
{
	// Token: 0x02000066 RID: 102
	public interface ICollectionPropertyBagAccept<TContainer>
	{
		// Token: 0x060001C5 RID: 453
		void Accept(ICollectionPropertyBagVisitor visitor, ref TContainer container);
	}
}
