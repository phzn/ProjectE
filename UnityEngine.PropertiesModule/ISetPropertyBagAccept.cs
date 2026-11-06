using System;

namespace Unity.Properties
{
	// Token: 0x02000068 RID: 104
	public interface ISetPropertyBagAccept<TContainer>
	{
		// Token: 0x060001C7 RID: 455
		void Accept(ISetPropertyBagVisitor visitor, ref TContainer container);
	}
}
