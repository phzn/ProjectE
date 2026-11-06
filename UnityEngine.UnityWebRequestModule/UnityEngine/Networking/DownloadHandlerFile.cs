using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	// Token: 0x02000009 RID: 9
	[NativeHeader("Modules/UnityWebRequest/Public/DownloadHandler/DownloadHandlerVFS.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerFile : DownloadHandler
	{
		// Token: 0x0600005C RID: 92
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerFile obj, string path, bool append);

		// Token: 0x0600005D RID: 93 RVA: 0x0000386C File Offset: 0x00001A6C
		private void InternalCreateVFS(string path, bool append)
		{
			string directoryName = Path.GetDirectoryName(path);
			bool flag = !Directory.Exists(directoryName);
			if (flag)
			{
				Directory.CreateDirectory(directoryName);
			}
			this.m_Ptr = DownloadHandlerFile.Create(this, path, append);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000038A3 File Offset: 0x00001AA3
		public DownloadHandlerFile(string path)
		{
			this.InternalCreateVFS(path, false);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000038B6 File Offset: 0x00001AB6
		public DownloadHandlerFile(string path, bool append)
		{
			this.InternalCreateVFS(path, append);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000038C9 File Offset: 0x00001AC9
		protected override NativeArray<byte> GetNativeData()
		{
			throw new NotSupportedException("Raw data access is not supported");
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000038C9 File Offset: 0x00001AC9
		protected override byte[] GetData()
		{
			throw new NotSupportedException("Raw data access is not supported");
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000038D6 File Offset: 0x00001AD6
		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported");
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000063 RID: 99
		// (set) Token: 0x06000064 RID: 100
		public extern bool removeFileOnAbort
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
