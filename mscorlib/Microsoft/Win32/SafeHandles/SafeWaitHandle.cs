using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Represents a wrapper class for a wait handle.</summary>
	// Token: 0x020000B7 RID: 183
	[SecurityCritical]
	public sealed class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x0001747F File Offset: 0x0001567F
		private SafeWaitHandle()
			: base(true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeWaitHandle" /> class.</summary>
		/// <param name="existingHandle">An <see cref="T:System.IntPtr" /> object that represents the pre-existing handle to use.</param>
		/// <param name="ownsHandle">
		///   <see langword="true" /> to reliably release the handle during the finalization phase; <see langword="false" /> to prevent reliable release (not recommended).</param>
		// Token: 0x06000493 RID: 1171 RVA: 0x000174AE File Offset: 0x000156AE
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public SafeWaitHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00017634 File Offset: 0x00015834
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			NativeEventCalls.CloseEvent_internal(this.handle);
			return true;
		}
	}
}
