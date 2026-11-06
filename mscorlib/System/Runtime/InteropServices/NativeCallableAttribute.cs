using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006CF RID: 1743
	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class NativeCallableAttribute : Attribute
	{
		// Token: 0x04002A13 RID: 10771
		public string EntryPoint;

		// Token: 0x04002A14 RID: 10772
		public CallingConvention CallingConvention;
	}
}
