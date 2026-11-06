using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000073 RID: 115
	public interface ISetPropertyBagVisitor
	{
		// Token: 0x060001D2 RID: 466
		void Visit<TSet, TValue>(ISetPropertyBag<TSet, TValue> properties, ref TSet container) where TSet : ISet<TValue>;
	}
}
