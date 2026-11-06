using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017D RID: 381
	public class ISteamMatchmakingServerListResponse
	{
		// Token: 0x060008D4 RID: 2260 RVA: 0x0000D2D0 File Offset: 0x0000B4D0
		public ISteamMatchmakingServerListResponse(ISteamMatchmakingServerListResponse.ServerResponded onServerResponded, ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond, ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
		{
			if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_RefreshComplete = onRefreshComplete;
			this.m_VTable = new ISteamMatchmakingServerListResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingServerListResponse.InternalServerResponded(ISteamMatchmakingServerListResponse.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(ISteamMatchmakingServerListResponse.InternalOnServerFailedToRespond),
				m_VTRefreshComplete = new ISteamMatchmakingServerListResponse.InternalRefreshComplete(ISteamMatchmakingServerListResponse.InternalOnRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf<ISteamMatchmakingServerListResponse.VTable>());
			Marshal.StructureToPtr<ISteamMatchmakingServerListResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
			this.m_pInstance = this.m_pGCHandle.AddrOfPinnedObject();
			Dictionary<IntPtr, ISteamMatchmakingServerListResponse> instances = ISteamMatchmakingServerListResponse.m_Instances;
			lock (instances)
			{
				ISteamMatchmakingServerListResponse.m_Instances[this.m_pInstance] = this;
			}
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		protected override void Finalize()
		{
			try
			{
				Dictionary<IntPtr, ISteamMatchmakingServerListResponse> instances = ISteamMatchmakingServerListResponse.m_Instances;
				lock (instances)
				{
					ISteamMatchmakingServerListResponse.m_Instances.Remove(this.m_pVTable);
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

		// Token: 0x060008D6 RID: 2262 RVA: 0x0000D464 File Offset: 0x0000B664
		[MonoPInvokeCallback(typeof(ISteamMatchmakingServerListResponse.InternalServerResponded))]
		private static void InternalOnServerResponded(IntPtr thisptr, HServerListRequest hRequest, int iServer)
		{
			try
			{
				ISteamMatchmakingServerListResponse steamMatchmakingServerListResponse;
				if (ISteamMatchmakingServerListResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingServerListResponse))
				{
					steamMatchmakingServerListResponse.m_ServerResponded(hRequest, iServer);
				}
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		[MonoPInvokeCallback(typeof(ISteamMatchmakingServerListResponse.InternalServerFailedToRespond))]
		private static void InternalOnServerFailedToRespond(IntPtr thisptr, HServerListRequest hRequest, int iServer)
		{
			try
			{
				ISteamMatchmakingServerListResponse steamMatchmakingServerListResponse;
				if (ISteamMatchmakingServerListResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingServerListResponse))
				{
					steamMatchmakingServerListResponse.m_ServerFailedToRespond(hRequest, iServer);
				}
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0000D4FC File Offset: 0x0000B6FC
		[MonoPInvokeCallback(typeof(ISteamMatchmakingServerListResponse.InternalRefreshComplete))]
		private static void InternalOnRefreshComplete(IntPtr thisptr, HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			try
			{
				ISteamMatchmakingServerListResponse steamMatchmakingServerListResponse;
				if (ISteamMatchmakingServerListResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingServerListResponse))
				{
					steamMatchmakingServerListResponse.m_RefreshComplete(hRequest, response);
				}
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0000D548 File Offset: 0x0000B748
		public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000B12 RID: 2834
		private ISteamMatchmakingServerListResponse.VTable m_VTable;

		// Token: 0x04000B13 RID: 2835
		private IntPtr m_pVTable;

		// Token: 0x04000B14 RID: 2836
		private GCHandle m_pGCHandle;

		// Token: 0x04000B15 RID: 2837
		private IntPtr m_pInstance;

		// Token: 0x04000B16 RID: 2838
		private ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000B17 RID: 2839
		private ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x04000B18 RID: 2840
		private ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

		// Token: 0x04000B19 RID: 2841
		private static readonly Dictionary<IntPtr, ISteamMatchmakingServerListResponse> m_Instances = new Dictionary<IntPtr, ISteamMatchmakingServerListResponse>();

		// Token: 0x020001CD RID: 461
		// (Invoke) Token: 0x06000BA3 RID: 2979
		public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

		// Token: 0x020001CE RID: 462
		// (Invoke) Token: 0x06000BA7 RID: 2983
		public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

		// Token: 0x020001CF RID: 463
		// (Invoke) Token: 0x06000BAB RID: 2987
		public delegate void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x020001D0 RID: 464
		// (Invoke) Token: 0x06000BAF RID: 2991
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, HServerListRequest hRequest, int iServer);

		// Token: 0x020001D1 RID: 465
		// (Invoke) Token: 0x06000BB3 RID: 2995
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr, HServerListRequest hRequest, int iServer);

		// Token: 0x020001D2 RID: 466
		// (Invoke) Token: 0x06000BB7 RID: 2999
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalRefreshComplete(IntPtr thisptr, HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x020001D3 RID: 467
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000C15 RID: 3093
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000C16 RID: 3094
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;

			// Token: 0x04000C17 RID: 3095
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
		}
	}
}
