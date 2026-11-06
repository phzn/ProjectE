using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Video
{
	// Token: 0x02000017 RID: 23
	[UsedByNativeCode]
	[NativeHeader("Modules/Video/Public/Base/VideoMediaPlayback.h")]
	internal class VideoPlaybackMgr : IDisposable
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00002B03 File Offset: 0x00000D03
		public VideoPlaybackMgr()
		{
			this.m_Ptr = VideoPlaybackMgr.Internal_Create();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00002B18 File Offset: 0x00000D18
		public void Dispose()
		{
			bool flag = this.m_Ptr != IntPtr.Zero;
			if (flag)
			{
				VideoPlaybackMgr.Internal_Destroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000DA RID: 218
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create();

		// Token: 0x060000DB RID: 219
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		// Token: 0x060000DC RID: 220
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern VideoPlayback CreateVideoPlayback(string fileName, VideoPlaybackMgr.MessageCallback errorCallback, VideoPlaybackMgr.Callback readyCallback, VideoPlaybackMgr.Callback reachedEndCallback, bool splitAlpha = false);

		// Token: 0x060000DD RID: 221
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ReleaseVideoPlayback(VideoPlayback playback);

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DE RID: 222
		public extern ulong videoPlaybackCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060000DF RID: 223
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Update();

		// Token: 0x060000E0 RID: 224
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ProcessOSMainLoopMessagesForTesting();

		// Token: 0x04000042 RID: 66
		internal IntPtr m_Ptr;

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x060000E2 RID: 226
		public delegate void Callback();

		// Token: 0x02000019 RID: 25
		// (Invoke) Token: 0x060000E6 RID: 230
		public delegate void MessageCallback(string message);
	}
}
