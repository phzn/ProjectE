using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	// Token: 0x02000014 RID: 20
	[NativeHeader("Modules/UnityWebRequest/Public/UploadHandler/UploadHandlerFile.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UploadHandlerFile : UploadHandler
	{
		// Token: 0x06000121 RID: 289
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(UploadHandlerFile self, string filePath);

		// Token: 0x06000122 RID: 290 RVA: 0x00005374 File Offset: 0x00003574
		public UploadHandlerFile(string filePath)
		{
			this.m_Ptr = UploadHandlerFile.Create(this, filePath);
		}
	}
}
