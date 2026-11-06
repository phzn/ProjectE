using System;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Represents a safe handle to the Windows registry.</summary>
	// Token: 0x020000B1 RID: 177
	public sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x0001749E File Offset: 0x0001569E
		protected override bool ReleaseHandle()
		{
			return Interop.Advapi32.RegCloseKey(this.handle) == 0;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001747F File Offset: 0x0001567F
		internal SafeRegistryHandle()
			: base(true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeRegistryHandle" /> class.</summary>
		/// <param name="preexistingHandle">An object that represents the pre-existing handle to use.</param>
		/// <param name="ownsHandle">
		///   <see langword="true" /> to reliably release the handle during the finalization phase; <see langword="false" /> to prevent reliable release.</param>
		// Token: 0x0600047A RID: 1146 RVA: 0x000174AE File Offset: 0x000156AE
		public SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}
	}
}
