using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Burst
{
	// Token: 0x0200002C RID: 44
	public readonly struct FunctionPointer<T> : IFunctionPointer
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00005960 File Offset: 0x00003B60
		public FunctionPointer(IntPtr ptr)
		{
			this._ptr = ptr;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005969 File Offset: 0x00003B69
		public IntPtr Value
		{
			get
			{
				return this._ptr;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005971 File Offset: 0x00003B71
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckIsCreated()
		{
			if (!this.IsCreated)
			{
				throw new NullReferenceException("Object reference not set to an instance of an object");
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005986 File Offset: 0x00003B86
		public T Invoke
		{
			get
			{
				return Marshal.GetDelegateForFunctionPointer<T>(this._ptr);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005993 File Offset: 0x00003B93
		public bool IsCreated
		{
			get
			{
				return this._ptr != IntPtr.Zero;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000059A5 File Offset: 0x00003BA5
		IFunctionPointer IFunctionPointer.FromIntPtr(IntPtr ptr)
		{
			return new FunctionPointer<T>(ptr);
		}

		// Token: 0x0400017D RID: 381
		[NativeDisableUnsafePtrRestriction]
		private readonly IntPtr _ptr;
	}
}
