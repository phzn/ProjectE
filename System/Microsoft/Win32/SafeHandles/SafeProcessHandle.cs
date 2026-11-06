using System;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a managed wrapper for a process handle.</summary>
	// Token: 0x02000133 RID: 307
	[SuppressUnmanagedCodeSecurity]
	public sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x00013B6C File Offset: 0x00011D6C
		internal SafeProcessHandle()
			: base(true)
		{
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00013B75 File Offset: 0x00011D75
		internal SafeProcessHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeProcessHandle" /> class from the specified handle, indicating whether to release the handle during the finalization phase.</summary>
		/// <param name="existingHandle">The handle to be wrapped.</param>
		/// <param name="ownsHandle">
		///   <see langword="true" /> to reliably let <see cref="T:Microsoft.Win32.SafeHandles.SafeProcessHandle" /> release the handle during the finalization phase; otherwise, <see langword="false" />.</param>
		// Token: 0x0600072A RID: 1834 RVA: 0x00013B85 File Offset: 0x00011D85
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public SafeProcessHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00013B95 File Offset: 0x00011D95
		internal void InitialSetHandle(IntPtr h)
		{
			this.handle = h;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00013B9E File Offset: 0x00011D9E
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseProcess(this.handle);
		}

		// Token: 0x04000517 RID: 1303
		internal static SafeProcessHandle InvalidHandle = new SafeProcessHandle(IntPtr.Zero);
	}
}
