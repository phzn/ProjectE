using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017F RID: 383
	public class ISteamMatchmakingPlayersResponse
	{
		// Token: 0x060008E1 RID: 2273 RVA: 0x0000D744 File Offset: 0x0000B944
		public ISteamMatchmakingPlayersResponse(ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList, ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond, ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
		{
			if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_AddPlayerToList = onAddPlayerToList;
			this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
			this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
			this.m_VTable = new ISteamMatchmakingPlayersResponse.VTable
			{
				m_VTAddPlayerToList = new ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(ISteamMatchmakingPlayersResponse.InternalOnAddPlayerToList),
				m_VTPlayersFailedToRespond = new ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(ISteamMatchmakingPlayersResponse.InternalOnPlayersFailedToRespond),
				m_VTPlayersRefreshComplete = new ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(ISteamMatchmakingPlayersResponse.InternalOnPlayersRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf<ISteamMatchmakingPlayersResponse.VTable>());
			Marshal.StructureToPtr<ISteamMatchmakingPlayersResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
			this.m_pInstance = this.m_pGCHandle.AddrOfPinnedObject();
			Dictionary<IntPtr, ISteamMatchmakingPlayersResponse> instances = ISteamMatchmakingPlayersResponse.m_Instances;
			lock (instances)
			{
				ISteamMatchmakingPlayersResponse.m_Instances[this.m_pInstance] = this;
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0000D844 File Offset: 0x0000BA44
		protected override void Finalize()
		{
			try
			{
				Dictionary<IntPtr, ISteamMatchmakingPlayersResponse> instances = ISteamMatchmakingPlayersResponse.m_Instances;
				lock (instances)
				{
					ISteamMatchmakingPlayersResponse.m_Instances.Remove(this.m_pVTable);
				}
				if (this.m_pVTable != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.m_pVTable);
				}
				if (this.m_pGCHandle.IsAllocated)
				{
					this.m_pGCHandle.Free();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0000D8D8 File Offset: 0x0000BAD8
		[MonoPInvokeCallback(typeof(ISteamMatchmakingPlayersResponse.InternalAddPlayerToList))]
		private static void InternalOnAddPlayerToList(IntPtr thisptr, IntPtr pchName, int nScore, float flTimePlayed)
		{
			ISteamMatchmakingPlayersResponse steamMatchmakingPlayersResponse;
			if (ISteamMatchmakingPlayersResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingPlayersResponse))
			{
				steamMatchmakingPlayersResponse.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000D908 File Offset: 0x0000BB08
		[MonoPInvokeCallback(typeof(ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond))]
		private static void InternalOnPlayersFailedToRespond(IntPtr thisptr)
		{
			ISteamMatchmakingPlayersResponse steamMatchmakingPlayersResponse;
			if (ISteamMatchmakingPlayersResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingPlayersResponse))
			{
				steamMatchmakingPlayersResponse.m_PlayersFailedToRespond();
			}
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0000D930 File Offset: 0x0000BB30
		[MonoPInvokeCallback(typeof(ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete))]
		private static void InternalOnPlayersRefreshComplete(IntPtr thisptr)
		{
			ISteamMatchmakingPlayersResponse steamMatchmakingPlayersResponse;
			if (ISteamMatchmakingPlayersResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingPlayersResponse))
			{
				steamMatchmakingPlayersResponse.m_PlayersRefreshComplete();
			}
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0000D957 File Offset: 0x0000BB57
		public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000B21 RID: 2849
		private ISteamMatchmakingPlayersResponse.VTable m_VTable;

		// Token: 0x04000B22 RID: 2850
		private IntPtr m_pVTable;

		// Token: 0x04000B23 RID: 2851
		private GCHandle m_pGCHandle;

		// Token: 0x04000B24 RID: 2852
		private IntPtr m_pInstance;

		// Token: 0x04000B25 RID: 2853
		private ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;

		// Token: 0x04000B26 RID: 2854
		private ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;

		// Token: 0x04000B27 RID: 2855
		private ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

		// Token: 0x04000B28 RID: 2856
		private static readonly Dictionary<IntPtr, ISteamMatchmakingPlayersResponse> m_Instances = new Dictionary<IntPtr, ISteamMatchmakingPlayersResponse>();

		// Token: 0x020001D9 RID: 473
		// (Invoke) Token: 0x06000BCD RID: 3021
		public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

		// Token: 0x020001DA RID: 474
		// (Invoke) Token: 0x06000BD1 RID: 3025
		public delegate void PlayersFailedToRespond();

		// Token: 0x020001DB RID: 475
		// (Invoke) Token: 0x06000BD5 RID: 3029
		public delegate void PlayersRefreshComplete();

		// Token: 0x020001DC RID: 476
		// (Invoke) Token: 0x06000BD9 RID: 3033
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalAddPlayerToList(IntPtr thisptr, IntPtr pchName, int nScore, float flTimePlayed);

		// Token: 0x020001DD RID: 477
		// (Invoke) Token: 0x06000BDD RID: 3037
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalPlayersFailedToRespond(IntPtr thisptr);

		// Token: 0x020001DE RID: 478
		// (Invoke) Token: 0x06000BE1 RID: 3041
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalPlayersRefreshComplete(IntPtr thisptr);

		// Token: 0x020001DF RID: 479
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000C1A RID: 3098
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;

			// Token: 0x04000C1B RID: 3099
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

			// Token: 0x04000C1C RID: 3100
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
		}
	}
}
