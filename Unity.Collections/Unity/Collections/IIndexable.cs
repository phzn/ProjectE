using System;
using System.Runtime.CompilerServices;

namespace Unity.Collections
{
	// Token: 0x02000098 RID: 152
	public interface IIndexable<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000789 RID: 1929
		// (set) Token: 0x0600078A RID: 1930
		int Length { get; set; }

		// Token: 0x0600078B RID: 1931
		ref T ElementAt(int index);
	}
}
