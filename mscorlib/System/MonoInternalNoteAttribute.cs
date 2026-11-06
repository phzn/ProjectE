using System;

namespace System
{
	// Token: 0x020001DD RID: 477
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoInternalNoteAttribute : MonoTODOAttribute
	{
		// Token: 0x060014A8 RID: 5288 RVA: 0x0005177C File Offset: 0x0004F97C
		public MonoInternalNoteAttribute(string comment)
			: base(comment)
		{
		}
	}
}
