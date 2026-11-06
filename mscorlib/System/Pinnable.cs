using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020001C4 RID: 452
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class Pinnable<T>
	{
		// Token: 0x04001443 RID: 5187
		public T Data;
	}
}
