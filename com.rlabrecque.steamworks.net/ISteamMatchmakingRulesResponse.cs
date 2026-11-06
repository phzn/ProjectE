using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000180 RID: 384
	public class ISteamMatchmakingRulesResponse
	{
		// Token: 0x060008E8 RID: 2280 RVA: 0x0000D970 File Offset: 0x0000BB70
		public ISteamMatchmakingRulesResponse(ISteamMatchmakingRulesResponse.RulesResponded onRulesResponded, ISteamMatchmakingRulesResponse.RulesFailedToRespond onRulesFailedToRespond, ISteamMatchmakingRulesResponse.RulesRefreshComplete onRulesRefreshComplete)
		{
			if (onRulesResponded == null || onRulesFailedToRespond == null || onRulesRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_RulesResponded = onRulesResponded;
			this.m_RulesFailedToRespond = onRulesFailedToRespond;
			this.m_RulesRefreshComplete = onRulesRefreshComplete;
			this.m_VTable = new ISteamMatchmakingRulesResponse.VTable
			{
				m_VTRulesResponded = new ISteamMatchmakingRulesResponse.InternalRulesResponded(ISteamMatchmakingRulesResponse.InternalOnRulesResponded),
				m_VTRulesFailedToRespond = new ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond(ISteamMatchmakingRulesResponse.InternalOnRulesFailedToRespond),
				m_VTRulesRefreshComplete = new ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete(ISteamMatchmakingRulesResponse.InternalOnRulesRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf<ISteamMatchmakingRulesResponse.VTable>());
			Marshal.StructureToPtr<ISteamMatchmakingRulesResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
			this.m_pInstance = this.m_pGCHandle.AddrOfPinnedObject();
			Dictionary<IntPtr, ISteamMatchmakingRulesResponse> instances = ISteamMatchmakingRulesResponse.m_Instances;
			lock (instances)
			{
				ISteamMatchmakingRulesResponse.m_Instances[this.m_pInstance] = this;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0000DA70 File Offset: 0x0000BC70
		protected override void Finalize()
		{
			try
			{
				Dictionary<IntPtr, ISteamMatchmakingRulesResponse> instances = ISteamMatchmakingRulesResponse.m_Instances;
				lock (instances)
				{
					ISteamMatchmakingRulesResponse.m_Instances.Remove(this.m_pVTable);
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

		// Token: 0x060008EA RID: 2282 RVA: 0x0000DB04 File Offset: 0x0000BD04
		[MonoPInvokeCallback(typeof(ISteamMatchmakingRulesResponse.InternalRulesResponded))]
		private static void InternalOnRulesResponded(IntPtr thisptr, IntPtr pchRule, IntPtr pchValue)
		{
			ISteamMatchmakingRulesResponse steamMatchmakingRulesResponse;
			if (ISteamMatchmakingRulesResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingRulesResponse))
			{
				steamMatchmakingRulesResponse.m_RulesResponded(InteropHelp.PtrToStringUTF8(pchRule), InteropHelp.PtrToStringUTF8(pchValue));
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0000DB38 File Offset: 0x0000BD38
		[MonoPInvokeCallback(typeof(ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond))]
		private static void InternalOnRulesFailedToRespond(IntPtr thisptr)
		{
			ISteamMatchmakingRulesResponse steamMatchmakingRulesResponse;
			if (ISteamMatchmakingRulesResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingRulesResponse))
			{
				steamMatchmakingRulesResponse.m_RulesFailedToRespond();
			}
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000DB60 File Offset: 0x0000BD60
		[MonoPInvokeCallback(typeof(ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete))]
		private static void InternalOnRulesRefreshComplete(IntPtr thisptr)
		{
			ISteamMatchmakingRulesResponse steamMatchmakingRulesResponse;
			if (ISteamMatchmakingRulesResponse.m_Instances.TryGetValue(thisptr, out steamMatchmakingRulesResponse))
			{
				steamMatchmakingRulesResponse.m_RulesRefreshComplete();
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000DB87 File Offset: 0x0000BD87
		public static explicit operator IntPtr(ISteamMatchmakingRulesResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000B29 RID: 2857
		private ISteamMatchmakingRulesResponse.VTable m_VTable;

		// Token: 0x04000B2A RID: 2858
		private IntPtr m_pVTable;

		// Token: 0x04000B2B RID: 2859
		private GCHandle m_pGCHandle;

		// Token: 0x04000B2C RID: 2860
		private IntPtr m_pInstance;

		// Token: 0x04000B2D RID: 2861
		private ISteamMatchmakingRulesResponse.RulesResponded m_RulesResponded;

		// Token: 0x04000B2E RID: 2862
		private ISteamMatchmakingRulesResponse.RulesFailedToRespond m_RulesFailedToRespond;

		// Token: 0x04000B2F RID: 2863
		private ISteamMatchmakingRulesResponse.RulesRefreshComplete m_RulesRefreshComplete;

		// Token: 0x04000B30 RID: 2864
		private static readonly Dictionary<IntPtr, ISteamMatchmakingRulesResponse> m_Instances = new Dictionary<IntPtr, ISteamMatchmakingRulesResponse>();

		// Token: 0x020001E0 RID: 480
		// (Invoke) Token: 0x06000BE6 RID: 3046
		public delegate void RulesResponded(string pchRule, string pchValue);

		// Token: 0x020001E1 RID: 481
		// (Invoke) Token: 0x06000BEA RID: 3050
		public delegate void RulesFailedToRespond();

		// Token: 0x020001E2 RID: 482
		// (Invoke) Token: 0x06000BEE RID: 3054
		public delegate void RulesRefreshComplete();

		// Token: 0x020001E3 RID: 483
		// (Invoke) Token: 0x06000BF2 RID: 3058
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalRulesResponded(IntPtr thisptr, IntPtr pchRule, IntPtr pchValue);

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000BF6 RID: 3062
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalRulesFailedToRespond(IntPtr thisptr);

		// Token: 0x020001E5 RID: 485
		// (Invoke) Token: 0x06000BFA RID: 3066
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalRulesRefreshComplete(IntPtr thisptr);

		// Token: 0x020001E6 RID: 486
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000C1D RID: 3101
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;

			// Token: 0x04000C1E RID: 3102
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;

			// Token: 0x04000C1F RID: 3103
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
		}
	}
}
