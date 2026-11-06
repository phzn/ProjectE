using System;

namespace Unity.Burst
{
	// Token: 0x0200002B RID: 43
	public interface IFunctionPointer
	{
		// Token: 0x060000DE RID: 222
		[Obsolete("This method will be removed in a future version of Burst")]
		IFunctionPointer FromIntPtr(IntPtr ptr);
	}
}
