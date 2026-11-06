using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	// Token: 0x02000008 RID: 8
	[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerScript.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class DownloadHandlerScript : DownloadHandler
	{
		// Token: 0x06000056 RID: 86
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerScript obj);

		// Token: 0x06000057 RID: 87
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreatePreallocated(DownloadHandlerScript obj, [Unmarshalled] byte[] preallocatedBuffer);

		// Token: 0x06000058 RID: 88 RVA: 0x000037FD File Offset: 0x000019FD
		private void InternalCreateScript()
		{
			this.m_Ptr = DownloadHandlerScript.Create(this);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000380C File Offset: 0x00001A0C
		private void InternalCreateScript(byte[] preallocatedBuffer)
		{
			this.m_Ptr = DownloadHandlerScript.CreatePreallocated(this, preallocatedBuffer);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000381C File Offset: 0x00001A1C
		public DownloadHandlerScript()
		{
			this.InternalCreateScript();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003830 File Offset: 0x00001A30
		public DownloadHandlerScript(byte[] preallocatedBuffer)
		{
			bool flag = preallocatedBuffer == null || preallocatedBuffer.Length < 1;
			if (flag)
			{
				throw new ArgumentException("Cannot create a preallocated-buffer DownloadHandlerScript backed by a null or zero-length array");
			}
			this.InternalCreateScript(preallocatedBuffer);
		}
	}
}
