using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000024 RID: 36
	public interface IProperty
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000074 RID: 116
		string Name { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000075 RID: 117
		bool IsReadOnly { get; }

		// Token: 0x06000076 RID: 118
		Type DeclaredValueType();

		// Token: 0x06000077 RID: 119
		bool HasAttribute<TAttribute>() where TAttribute : Attribute;

		// Token: 0x06000078 RID: 120
		TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute;

		// Token: 0x06000079 RID: 121
		IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute;

		// Token: 0x0600007A RID: 122
		IEnumerable<Attribute> GetAttributes();
	}
}
