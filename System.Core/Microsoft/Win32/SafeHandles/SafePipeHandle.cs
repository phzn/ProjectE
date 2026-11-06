using System;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Represents a wrapper class for a pipe handle. </summary>
	// Token: 0x02000013 RID: 19
	public sealed class SafePipeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000038 RID: 56 RVA: 0x000022F8 File Offset: 0x000004F8
		protected override bool ReleaseHandle()
		{
			return global::Interop.Kernel32.CloseHandle(this.handle);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002305 File Offset: 0x00000505
		internal SafePipeHandle()
			: this(new IntPtr(0), true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafePipeHandle" /> class.</summary>
		/// <param name="preexistingHandle">An <see cref="T:System.IntPtr" /> object that represents the pre-existing handle to use.</param>
		/// <param name="ownsHandle">
		///       <see langword="true" /> to reliably release the handle during the finalization phase; <see langword="false" /> to prevent reliable release (not recommended).</param>
		// Token: 0x0600003A RID: 58 RVA: 0x00002314 File Offset: 0x00000514
		public SafePipeHandle(IntPtr preexistingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002324 File Offset: 0x00000524
		internal void SetHandle(int descriptor)
		{
			base.SetHandle((IntPtr)descriptor);
		}

		// Token: 0x040002B7 RID: 695
		private const int DefaultInvalidHandle = 0;
	}
}
