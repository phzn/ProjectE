using System;

namespace System
{
	// Token: 0x020001DF RID: 479
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoNotSupportedAttribute : MonoTODOAttribute
	{
		// Token: 0x060014AA RID: 5290 RVA: 0x0005177C File Offset: 0x0004F97C
		public MonoNotSupportedAttribute(string comment)
			: base(comment)
		{
		}
	}
}
