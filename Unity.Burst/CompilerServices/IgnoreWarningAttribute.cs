using System;

namespace Unity.Burst.CompilerServices
{
	// Token: 0x0200005A RID: 90
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class IgnoreWarningAttribute : Attribute
	{
		// Token: 0x06000E5A RID: 3674 RVA: 0x00002050 File Offset: 0x00000250
		public IgnoreWarningAttribute(int warning)
		{
		}
	}
}
