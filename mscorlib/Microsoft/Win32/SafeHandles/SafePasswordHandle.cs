using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020000B2 RID: 178
	internal sealed class SafePasswordHandle : SafeHandle
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x000174BE File Offset: 0x000156BE
		private IntPtr CreateHandle(string password)
		{
			return Marshal.StringToHGlobalUni(password);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000174C6 File Offset: 0x000156C6
		private IntPtr CreateHandle(SecureString password)
		{
			return Marshal.SecureStringToGlobalAllocUnicode(password);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000174CE File Offset: 0x000156CE
		private void FreeHandle()
		{
			Marshal.ZeroFreeGlobalAllocUnicode(this.handle);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000174DB File Offset: 0x000156DB
		public SafePasswordHandle(string password)
			: base(IntPtr.Zero, true)
		{
			if (password != null)
			{
				base.SetHandle(this.CreateHandle(password));
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000174F9 File Offset: 0x000156F9
		public SafePasswordHandle(SecureString password)
			: base(IntPtr.Zero, true)
		{
			if (password != null)
			{
				base.SetHandle(this.CreateHandle(password));
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00017517 File Offset: 0x00015717
		protected override bool ReleaseHandle()
		{
			if (this.handle != IntPtr.Zero)
			{
				this.FreeHandle();
			}
			base.SetHandle((IntPtr)(-1));
			return true;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001753E File Offset: 0x0001573E
		protected override void Dispose(bool disposing)
		{
			if (disposing && SafeHandleCache<SafePasswordHandle>.IsCachedInvalidHandle(this))
			{
				return;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00017553 File Offset: 0x00015753
		public override bool IsInvalid
		{
			get
			{
				return this.handle == (IntPtr)(-1);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00017566 File Offset: 0x00015766
		public static SafePasswordHandle InvalidHandle
		{
			get
			{
				return SafeHandleCache<SafePasswordHandle>.GetInvalidHandle(() => new SafePasswordHandle(null)
				{
					handle = (IntPtr)(-1)
				});
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001758C File Offset: 0x0001578C
		internal string Mono_DangerousGetString()
		{
			return Marshal.PtrToStringUni(base.DangerousGetHandle());
		}
	}
}
