using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200026C RID: 620
	internal class ImmediateModeException : Exception
	{
		// Token: 0x060011B0 RID: 4528 RVA: 0x000408CC File Offset: 0x0003EACC
		public ImmediateModeException(Exception inner)
			: base("", inner)
		{
		}
	}
}
