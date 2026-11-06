using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a safe handle that represents a key (NCRYPT_KEY_HANDLE).</summary>
	// Token: 0x02000017 RID: 23
	public sealed class SafeNCryptKeyHandle : SafeNCryptHandle
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle" /> class.</summary>
		// Token: 0x06000048 RID: 72 RVA: 0x000023D4 File Offset: 0x000005D4
		public SafeNCryptKeyHandle()
		{
		}

		/// <summary>Instantiates a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle" /> class. </summary>
		/// <param name="handle">The pre-existing handle to use. Using <see cref="F:System.IntPtr.Zero" /> returns an invalid handle. </param>
		/// <param name="parentHandle">The parent handle of this <see cref="T:Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="parentHandle" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="parentHandle" /> is closed. -or-<paramref name="parentHandle" /> is invalid. </exception>
		// Token: 0x06000049 RID: 73 RVA: 0x000023DC File Offset: 0x000005DC
		public SafeNCryptKeyHandle(IntPtr handle, SafeHandle parentHandle)
			: base(handle, parentHandle)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000023D1 File Offset: 0x000005D1
		protected override bool ReleaseNativeHandle()
		{
			return false;
		}
	}
}
