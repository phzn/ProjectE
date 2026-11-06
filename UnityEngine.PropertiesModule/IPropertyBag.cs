using System;

namespace Unity.Properties
{
	// Token: 0x02000042 RID: 66
	public interface IPropertyBag
	{
		// Token: 0x0600012D RID: 301
		void Accept(ITypeVisitor visitor);

		// Token: 0x0600012E RID: 302
		void Accept(IPropertyBagVisitor visitor, ref object container);
	}
}
