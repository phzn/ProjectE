using System;

namespace System
{
	// Token: 0x020001DE RID: 478
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoLimitationAttribute : MonoTODOAttribute
	{
		// Token: 0x060014A9 RID: 5289 RVA: 0x0005177C File Offset: 0x0004F97C
		public MonoLimitationAttribute(string comment)
			: base(comment)
		{
		}
	}
}
