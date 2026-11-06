using System;

namespace Unity.Properties
{
	// Token: 0x02000043 RID: 67
	public interface IPropertyBag<TContainer> : IPropertyBag
	{
		// Token: 0x0600012F RID: 303
		PropertyCollection<TContainer> GetProperties();

		// Token: 0x06000130 RID: 304
		PropertyCollection<TContainer> GetProperties(ref TContainer container);

		// Token: 0x06000131 RID: 305
		TContainer CreateInstance();

		// Token: 0x06000132 RID: 306
		bool TryCreateInstance(out TContainer instance);

		// Token: 0x06000133 RID: 307
		void Accept(IPropertyBagVisitor visitor, ref TContainer container);
	}
}
