using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net.Security
{
	// Token: 0x02000843 RID: 2115
	internal abstract class SafeFreeContextBuffer : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600436C RID: 17260 RVA: 0x00013B6C File Offset: 0x00011D6C
		protected SafeFreeContextBuffer()
			: base(true)
		{
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00013B95 File Offset: 0x00011D95
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x000EB64C File Offset: 0x000E984C
		internal static int EnumeratePackages(out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			SafeFreeContextBuffer_SECURITY safeFreeContextBuffer_SECURITY = null;
			int num = global::Interop.SspiCli.EnumerateSecurityPackagesW(out pkgnum, out safeFreeContextBuffer_SECURITY);
			pkgArray = safeFreeContextBuffer_SECURITY;
			if (num != 0 && pkgArray != null)
			{
				pkgArray.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x000EB674 File Offset: 0x000E9874
		internal static SafeFreeContextBuffer CreateEmptyHandle()
		{
			return new SafeFreeContextBuffer_SECURITY();
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x000EB67C File Offset: 0x000E987C
		public unsafe static int QueryContextAttributes(SafeDeleteContext phContext, global::Interop.SspiCli.ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			int num = -2146893055;
			try
			{
				bool flag = false;
				phContext.DangerousAddRef(ref flag);
				num = global::Interop.SspiCli.QueryContextAttributesW(ref phContext._handle, contextAttribute, (void*)buffer);
			}
			finally
			{
				phContext.DangerousRelease();
			}
			if (num == 0 && refHandle != null)
			{
				if (refHandle is SafeFreeContextBuffer)
				{
					((SafeFreeContextBuffer)refHandle).Set(*(IntPtr*)buffer);
				}
				else
				{
					((SafeFreeCertContext)refHandle).Set(*(IntPtr*)buffer);
				}
			}
			if (num != 0 && refHandle != null)
			{
				refHandle.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x000EB6F8 File Offset: 0x000E98F8
		public static int SetContextAttributes(SafeDeleteContext phContext, global::Interop.SspiCli.ContextAttribute contextAttribute, byte[] buffer)
		{
			int num;
			try
			{
				bool flag = false;
				phContext.DangerousAddRef(ref flag);
				num = global::Interop.SspiCli.SetContextAttributesW(ref phContext._handle, contextAttribute, buffer, buffer.Length);
			}
			finally
			{
				phContext.DangerousRelease();
			}
			return num;
		}
	}
}
