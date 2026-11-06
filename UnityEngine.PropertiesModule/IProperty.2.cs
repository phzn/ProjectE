using System;

namespace Unity.Properties
{
	// Token: 0x02000025 RID: 37
	public interface IProperty<TContainer> : IProperty, IPropertyAccept<TContainer>
	{
		// Token: 0x0600007B RID: 123
		object GetValue(ref TContainer container);

		// Token: 0x0600007C RID: 124
		void SetValue(ref TContainer container, object value);
	}
}
