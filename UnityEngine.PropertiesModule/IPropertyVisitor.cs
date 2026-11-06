using System;

namespace Unity.Properties
{
	// Token: 0x02000075 RID: 117
	public interface IPropertyVisitor
	{
		// Token: 0x060001D4 RID: 468
		void Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container);
	}
}
