using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	// Token: 0x02000005 RID: 5
	[NativeHeader("Modules/UnityWebRequest/Public/CertificateHandler/CertificateHandlerScript.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class CertificateHandler : IDisposable
	{
		// Token: 0x0600002E RID: 46
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(CertificateHandler obj);

		// Token: 0x0600002F RID: 47
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();

		// Token: 0x06000030 RID: 48 RVA: 0x000032E3 File Offset: 0x000014E3
		protected CertificateHandler()
		{
			this.m_Ptr = CertificateHandler.Create(this);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000032FC File Offset: 0x000014FC
		~CertificateHandler()
		{
			this.Dispose();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000332C File Offset: 0x0000152C
		protected virtual bool ValidateCertificate(byte[] certificateData)
		{
			return false;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003340 File Offset: 0x00001540
		[RequiredByNativeCode]
		internal bool ValidateCertificateNative(byte[] certificateData)
		{
			return this.ValidateCertificate(certificateData);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000335C File Offset: 0x0000155C
		public void Dispose()
		{
			bool flag = this.m_Ptr != IntPtr.Zero;
			if (flag)
			{
				this.Release();
				this.m_Ptr = IntPtr.Zero;
			}
		}

		// Token: 0x04000019 RID: 25
		[NonSerialized]
		internal IntPtr m_Ptr;
	}
}
