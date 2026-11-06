using System;

namespace Unity.Properties
{
	// Token: 0x02000058 RID: 88
	public interface IExcludePropertyAdapter<TContainer, TValue> : IPropertyVisitorAdapter
	{
		// Token: 0x060001B2 RID: 434
		bool IsExcluded(in ExcludeContext<TContainer, TValue> context, ref TContainer container, ref TValue value);
	}
}
