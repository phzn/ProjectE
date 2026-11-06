using System;

namespace Unity.Properties
{
	// Token: 0x02000063 RID: 99
	public abstract class ConcreteTypeVisitor : IPropertyBagVisitor
	{
		// Token: 0x060001BC RID: 444
		protected abstract void VisitContainer<TContainer>(ref TContainer container);

		// Token: 0x060001BD RID: 445 RVA: 0x0000660B File Offset: 0x0000480B
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			this.VisitContainer<TContainer>(ref container);
		}
	}
}
