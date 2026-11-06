using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000B4 RID: 180
	internal class SafeThreadPoolIOHandle : SafeHandle
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x000175B9 File Offset: 0x000157B9
		static SafeThreadPoolIOHandle()
		{
			if (!Environment.IsRunningOnWindows)
			{
				throw new PlatformNotSupportedException();
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000175C8 File Offset: 0x000157C8
		private SafeThreadPoolIOHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x000175D6 File Offset: 0x000157D6
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000175E8 File Offset: 0x000157E8
		protected override bool ReleaseHandle()
		{
			Interop.mincore.CloseThreadpoolIo(this.handle);
			return true;
		}
	}
}
