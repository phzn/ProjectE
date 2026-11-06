using System;

namespace Unity.Properties
{
	// Token: 0x02000062 RID: 98
	public interface IVisitContravariantPropertyAdapter<in TValue> : IPropertyVisitorAdapter
	{
		// Token: 0x060001BB RID: 443
		void Visit<TContainer>(in VisitContext<TContainer> context, ref TContainer container, TValue value);
	}
}
