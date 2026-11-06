using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000221 RID: 545
	[StructLayout(LayoutKind.Sequential)]
	internal class MonoTypeInfo
	{
		// Token: 0x040016B1 RID: 5809
		public string full_name;

		// Token: 0x040016B2 RID: 5810
		public RuntimeConstructorInfo default_ctor;
	}
}
