using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x0200002D RID: 45
	internal interface IMemberInfo
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000D0 RID: 208
		string Name { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D1 RID: 209
		bool IsReadOnly { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000D2 RID: 210
		Type ValueType { get; }

		// Token: 0x060000D3 RID: 211
		object GetValue(object obj);

		// Token: 0x060000D4 RID: 212
		void SetValue(object obj, object value);

		// Token: 0x060000D5 RID: 213
		IEnumerable<Attribute> GetCustomAttributes();
	}
}
