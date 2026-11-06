using System;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x0200023F RID: 575
	public interface IRefCounted
	{
		// Token: 0x06000EDB RID: 3803
		void Retain();

		// Token: 0x06000EDC RID: 3804
		void Release();

		// Token: 0x02000240 RID: 576
		// (Invoke) Token: 0x06000EDE RID: 3806
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void RefCountDelegate(IntPtr _this);
	}
}
