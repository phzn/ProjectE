using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	// Token: 0x02000002 RID: 2
	[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerAudioClip.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerAudioClip : DownloadHandler
	{
		// Token: 0x06000001 RID: 1
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerAudioClip obj, string url, AudioType audioType);

		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		private void InternalCreateAudioClip(string url, AudioType audioType)
		{
			this.m_Ptr = DownloadHandlerAudioClip.Create(this, url, audioType);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		public DownloadHandlerAudioClip(string url, AudioType audioType)
		{
			this.InternalCreateAudioClip(url, audioType);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002074 File Offset: 0x00000274
		public DownloadHandlerAudioClip(Uri uri, AudioType audioType)
		{
			this.InternalCreateAudioClip(uri.AbsoluteUri, audioType);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000208C File Offset: 0x0000028C
		protected override NativeArray<byte> GetNativeData()
		{
			return DownloadHandler.InternalGetNativeArray(this, ref this.m_NativeData);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020AA File Offset: 0x000002AA
		public override void Dispose()
		{
			DownloadHandler.DisposeNativeArray(ref this.m_NativeData);
			base.Dispose();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020C0 File Offset: 0x000002C0
		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for audio clips");
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8
		[NativeThrows]
		public extern AudioClip audioClip
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9
		// (set) Token: 0x0600000A RID: 10
		public extern bool streamAudio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11
		// (set) Token: 0x0600000C RID: 12
		public extern bool compressed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020D0 File Offset: 0x000002D0
		public static AudioClip GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerAudioClip>(www).audioClip;
		}

		// Token: 0x04000001 RID: 1
		private NativeArray<byte> m_NativeData;
	}
}
