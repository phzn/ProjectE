using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Bindings;

namespace UnityEngine.Networking
{
	// Token: 0x02000002 RID: 2
	[NativeHeader("Modules/UnityWebRequestTexture/Public/DownloadHandlerTexture.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerTexture : DownloadHandler
	{
		// Token: 0x06000001 RID: 1
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerTexture obj, bool readable);

		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		private void InternalCreateTexture(bool readable)
		{
			this.m_Ptr = DownloadHandlerTexture.Create(this, readable);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public DownloadHandlerTexture()
		{
			this.InternalCreateTexture(true);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002072 File Offset: 0x00000272
		public DownloadHandlerTexture(bool readable)
		{
			this.InternalCreateTexture(readable);
			this.mNonReadable = !readable;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002090 File Offset: 0x00000290
		protected override NativeArray<byte> GetNativeData()
		{
			return DownloadHandler.InternalGetNativeArray(this, ref this.m_NativeData);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020AE File Offset: 0x000002AE
		public override void Dispose()
		{
			DownloadHandler.DisposeNativeArray(ref this.m_NativeData);
			base.Dispose();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020C4 File Offset: 0x000002C4
		public Texture2D texture
		{
			get
			{
				return this.InternalGetTextureNative();
			}
		}

		// Token: 0x06000008 RID: 8
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D InternalGetTextureNative();

		// Token: 0x06000009 RID: 9 RVA: 0x000020DC File Offset: 0x000002DC
		public static Texture2D GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerTexture>(www).texture;
		}

		// Token: 0x04000001 RID: 1
		private NativeArray<byte> m_NativeData;

		// Token: 0x04000002 RID: 2
		private bool mNonReadable;
	}
}
