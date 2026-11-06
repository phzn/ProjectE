using System;
using System.Collections.Generic;

namespace Unity.Serialization.Binary
{
	// Token: 0x020000A4 RID: 164
	public struct BinarySerializationParameters
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00010993 File Offset: 0x0000EB93
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0001099B File Offset: 0x0000EB9B
		public Type SerializedType { readonly get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x000109A4 File Offset: 0x0000EBA4
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x000109AC File Offset: 0x0000EBAC
		public bool DisableRootAdapters { readonly get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000109B5 File Offset: 0x0000EBB5
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x000109BD File Offset: 0x0000EBBD
		public List<IBinaryAdapter> UserDefinedAdapters { readonly get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000109C6 File Offset: 0x0000EBC6
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x000109CE File Offset: 0x0000EBCE
		public bool RequiresThreadSafety { readonly get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x000109D7 File Offset: 0x0000EBD7
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x000109DF File Offset: 0x0000EBDF
		public bool DisableSerializedReferences { readonly get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x000109E8 File Offset: 0x0000EBE8
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x000109F0 File Offset: 0x0000EBF0
		public BinarySerializationState State { readonly get; set; }
	}
}
