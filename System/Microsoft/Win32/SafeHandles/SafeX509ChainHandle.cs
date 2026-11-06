using System;
using Unity;

namespace Microsoft.Win32.SafeHandles
{
	/// <summary>Provides a wrapper class that represents the handle of an X.509 chain object. For more information, see <see cref="T:System.Security.Cryptography.X509Certificates.X509Chain" />.</summary>
	// Token: 0x02000134 RID: 308
	public sealed class SafeX509ChainHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600072E RID: 1838 RVA: 0x00013BBC File Offset: 0x00011DBC
		internal SafeX509ChainHandle(IntPtr handle)
			: base(true)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000829A File Offset: 0x0000649A
		[MonoTODO]
		protected override bool ReleaseHandle()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00013BCA File Offset: 0x00011DCA
		internal SafeX509ChainHandle()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}
	}
}
