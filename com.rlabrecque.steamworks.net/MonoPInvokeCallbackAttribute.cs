using System;

namespace Steamworks
{
	// Token: 0x0200017C RID: 380
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	internal sealed class MonoPInvokeCallbackAttribute : Attribute
	{
		// Token: 0x060008D3 RID: 2259 RVA: 0x0000D2C7 File Offset: 0x0000B4C7
		public MonoPInvokeCallbackAttribute(Type placeholder)
		{
		}
	}
}
