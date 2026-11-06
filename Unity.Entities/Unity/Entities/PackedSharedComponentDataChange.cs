using System;

namespace Unity.Entities
{
	// Token: 0x0200007B RID: 123
	public struct PackedSharedComponentDataChange
	{
		// Token: 0x040000CE RID: 206
		public PackedComponent Component;

		// Token: 0x040000CF RID: 207
		public int UnmanagedSharedValueDataOffsetWithManagedFlag;

		// Token: 0x040000D0 RID: 208
		public const int kManagedFlag = -2147483648;

		// Token: 0x040000D1 RID: 209
		public object BoxedSharedValue;
	}
}
