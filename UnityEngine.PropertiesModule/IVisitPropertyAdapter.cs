using System;

namespace Unity.Properties
{
	// Token: 0x0200005E RID: 94
	public interface IVisitPropertyAdapter<TContainer, TValue> : IPropertyVisitorAdapter
	{
		// Token: 0x060001B7 RID: 439
		void Visit(in VisitContext<TContainer, TValue> context, ref TContainer container, ref TValue value);
	}
}
