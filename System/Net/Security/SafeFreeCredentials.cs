using System;
using System.Runtime.InteropServices;

namespace System.Net.Security
{
	// Token: 0x02000846 RID: 2118
	internal abstract class SafeFreeCredentials : SafeHandle
	{
		// Token: 0x06004377 RID: 17271 RVA: 0x000EB763 File Offset: 0x000E9963
		protected SafeFreeCredentials()
			: base(IntPtr.Zero, true)
		{
			this._handle = default(global::Interop.SspiCli.CredHandle);
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06004378 RID: 17272 RVA: 0x000EB77D File Offset: 0x000E997D
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this._handle.IsZero;
			}
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x000EB794 File Offset: 0x000E9994
		public static int AcquireCredentialsHandle(string package, global::Interop.SspiCli.CredentialUse intent, ref global::Interop.SspiCli.SEC_WINNT_AUTH_IDENTITY_W authdata, out SafeFreeCredentials outCredential)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Enter(null, package, intent, authdata, "AcquireCredentialsHandle");
			}
			outCredential = new SafeFreeCredential_SECURITY();
			long num2;
			int num = global::Interop.SspiCli.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x000EB7F0 File Offset: 0x000E99F0
		public static int AcquireDefaultCredential(string package, global::Interop.SspiCli.CredentialUse intent, out SafeFreeCredentials outCredential)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Enter(null, package, intent, "AcquireDefaultCredential");
			}
			outCredential = new SafeFreeCredential_SECURITY();
			long num2;
			int num = global::Interop.SspiCli.AcquireCredentialsHandleW(null, package, (int)intent, null, IntPtr.Zero, null, null, ref outCredential._handle, out num2);
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x000EB844 File Offset: 0x000E9A44
		public static int AcquireCredentialsHandle(string package, global::Interop.SspiCli.CredentialUse intent, ref SafeSspiAuthDataHandle authdata, out SafeFreeCredentials outCredential)
		{
			outCredential = new SafeFreeCredential_SECURITY();
			long num2;
			int num = global::Interop.SspiCli.AcquireCredentialsHandleW(null, package, (int)intent, null, authdata, null, null, ref outCredential._handle, out num2);
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x000EB87C File Offset: 0x000E9A7C
		public unsafe static int AcquireCredentialsHandle(string package, global::Interop.SspiCli.CredentialUse intent, ref global::Interop.SspiCli.SCHANNEL_CRED authdata, out SafeFreeCredentials outCredential)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Enter(null, package, intent, authdata, "AcquireCredentialsHandle");
			}
			int num = -1;
			IntPtr paCred = authdata.paCred;
			try
			{
				IntPtr intPtr = new IntPtr((void*)(&paCred));
				if (paCred != IntPtr.Zero)
				{
					authdata.paCred = intPtr;
				}
				outCredential = new SafeFreeCredential_SECURITY();
				long num2;
				num = global::Interop.SspiCli.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
			}
			finally
			{
				authdata.paCred = paCred;
			}
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x040028CD RID: 10445
		internal global::Interop.SspiCli.CredHandle _handle;
	}
}
