using System;

namespace Unity.Burst.CompilerServices
{
	// Token: 0x02000057 RID: 87
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class AssumeRangeAttribute : Attribute
	{
		// Token: 0x06000E53 RID: 3667 RVA: 0x00002050 File Offset: 0x00000250
		public AssumeRangeAttribute(long min, long max)
		{
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00002050 File Offset: 0x00000250
		public AssumeRangeAttribute(ulong min, ulong max)
		{
		}
	}
}
