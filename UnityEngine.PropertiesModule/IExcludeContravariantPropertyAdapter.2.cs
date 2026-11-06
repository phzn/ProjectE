using System;

namespace Unity.Properties
{
	// Token: 0x0200005C RID: 92
	public interface IExcludeContravariantPropertyAdapter<in TValue> : IPropertyVisitorAdapter
	{
		// Token: 0x060001B6 RID: 438
		bool IsExcluded<TContainer>(in ExcludeContext<TContainer> context, ref TContainer container, TValue value);
	}
}
