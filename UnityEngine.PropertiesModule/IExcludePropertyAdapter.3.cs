using System;

namespace Unity.Properties
{
	// Token: 0x0200005A RID: 90
	public interface IExcludePropertyAdapter : IPropertyVisitorAdapter
	{
		// Token: 0x060001B4 RID: 436
		bool IsExcluded<TContainer, TValue>(in ExcludeContext<TContainer, TValue> context, ref TContainer container, ref TValue value);
	}
}
