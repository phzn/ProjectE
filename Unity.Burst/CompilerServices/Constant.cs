using System;
using System.Runtime.CompilerServices;

namespace Unity.Burst.CompilerServices
{
	// Token: 0x02000058 RID: 88
	public static class Constant
	{
		// Token: 0x06000E55 RID: 3669 RVA: 0x00002556 File Offset: 0x00000756
		public static bool IsConstantExpression<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T t) where T : struct, ValueType
		{
			return false;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00002556 File Offset: 0x00000756
		public unsafe static bool IsConstantExpression(void* t)
		{
			return false;
		}
	}
}
