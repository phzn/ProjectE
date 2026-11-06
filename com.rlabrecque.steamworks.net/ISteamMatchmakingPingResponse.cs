using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017E RID: 382
	public class ISteamMatchmakingPingResponse
	{
		// Token: 0x060008DB RID: 2267 RVA: 0x0000D564 File Offset: 0x0000B764
		public ISteamMatchmakingPingResponse(ISteamMatchmakingPingResponse.ServerResponded onServerResponded, ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
		{
			if (onServerResponded == null || onServerFailedToRespond == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_VTable = new ISteamMatchmakingPingResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(ISteamMatchmakingPingResponse.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(ISteamMatchmakingPingResponse.InternalOnServerFailedToRespond)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf<ISteamMatchmakingPingResponse.VTable>());
			Marshal.StructureToPtr<ISteamMatchmakingPingResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
			this.m_pInstance = this.m_pGCHandle.AddrOfPinnedObject();
			Dictionary<IntPtr, ISteamMatchmakingPingResponse> instances = ISteamMatchmakingPingResponse.m_Instances;
			lock (instances)
			{
				ISteamMatchmakingPingResponse.m_Instances[this.m_pInstance] = this;
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0000D648 File Offset: 0x0000B848
		protected override void Finalize()
		{
			try
			{
				Dictionary<IntPtr, ISteamMatchmakingPingResponse> instances = ISteamMatchmakingPingResponse.m_Instances;
				lock (instances)
				{
					ISteamMatchmakingPingResponse.m_Instances.Remove(this.m_pVTable);
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

		// Token: 0x060008DD RID: 2269 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		[MonoPInvokeCallback(typeof(ISteamMatchmakingPingResponse.InternalServerResponded))]
		private static void InternalOnServerResponded(IntPtr thisptr, gameserveritem_t server)
		{
			ISteamMatchmakingPingResponse steamMatchmakingPingResponse;
			if (ISteamMatchmakingPingResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingPingResponse))
			{
				steamMatchmakingPingResponse.m_ServerResponded(server);
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0000D704 File Offset: 0x0000B904
		[MonoPInvokeCallback(typeof(ISteamMatchmakingPingResponse.InternalServerFailedToRespond))]
		private static void InternalOnServerFailedToRespond(IntPtr thisptr)
		{
			ISteamMatchmakingPingResponse steamMatchmakingPingResponse;
			if (ISteamMatchmakingPingResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingPingResponse))
			{
				steamMatchmakingPingResponse.m_ServerFailedToRespond();
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0000D72B File Offset: 0x0000B92B
		public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000B1A RID: 2842
		private ISteamMatchmakingPingResponse.VTable m_VTable;

		// Token: 0x04000B1B RID: 2843
		private IntPtr m_pVTable;

		// Token: 0x04000B1C RID: 2844
		private GCHandle m_pGCHandle;

		// Token: 0x04000B1D RID: 2845
		private IntPtr m_pInstance;

		// Token: 0x04000B1E RID: 2846
		private ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000B1F RID: 2847
		private ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x04000B20 RID: 2848
		private static readonly Dictionary<IntPtr, ISteamMatchmakingPingResponse> m_Instances = new Dictionary<IntPtr, ISteamMatchmakingPingResponse>();

		// Token: 0x020001D4 RID: 468
		// (Invoke) Token: 0x06000BBC RID: 3004
		public delegate void ServerResponded(gameserveritem_t server);

		// Token: 0x020001D5 RID: 469
		// (Invoke) Token: 0x06000BC0 RID: 3008
		public delegate void ServerFailedToRespond();

		// Token: 0x020001D6 RID: 470
		// (Invoke) Token: 0x06000BC4 RID: 3012
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, gameserveritem_t server);

		// Token: 0x020001D7 RID: 471
		// (Invoke) Token: 0x06000BC8 RID: 3016
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr);

		// Token: 0x020001D8 RID: 472
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000C18 RID: 3096
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000C19 RID: 3097
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
		}
	}
}
