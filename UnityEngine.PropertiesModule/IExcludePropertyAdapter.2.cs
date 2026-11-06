using System;

namespace Unity.Properties
{
	// Token: 0x02000059 RID: 89
	public interface IExcludePropertyAdapter<TValue> : IPropertyVisitorAdapter
	{
		// Token: 0x060001B3 RID: 435
		bool IsExcluded<TContainer>(in ExcludeContext<TContainer, TValue> context, ref TContainer container, ref TValue value);
	}
}
