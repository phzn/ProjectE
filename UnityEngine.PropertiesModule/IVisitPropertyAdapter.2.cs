using System;

namespace Unity.Properties
{
	// Token: 0x0200005F RID: 95
	public interface IVisitPropertyAdapter<TValue> : IPropertyVisitorAdapter
	{
		// Token: 0x060001B8 RID: 440
		void Visit<TContainer>(in VisitContext<TContainer, TValue> context, ref TContainer container, ref TValue value);
	}
}
