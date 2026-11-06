using System;
using Unity.Properties;

namespace Unity.Entities
{
	// Token: 0x02000251 RID: 593
	internal interface ITypedVisit<TValue>
	{
		// Token: 0x06000F48 RID: 3912
		void Visit<TContainer>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value);
	}
}
