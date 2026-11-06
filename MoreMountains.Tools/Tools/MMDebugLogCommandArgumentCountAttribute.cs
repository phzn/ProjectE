using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200009F RID: 159
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class MMDebugLogCommandArgumentCountAttribute : Attribute
	{
		// Token: 0x06000362 RID: 866 RVA: 0x0001171C File Offset: 0x0000F91C
		public MMDebugLogCommandArgumentCountAttribute(int argumentCount)
		{
			this.ArgumentCount = argumentCount;
		}

		// Token: 0x040004D2 RID: 1234
		public readonly int ArgumentCount;
	}
}
