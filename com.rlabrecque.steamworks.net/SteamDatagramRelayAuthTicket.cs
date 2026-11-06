using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200018F RID: 399
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamDatagramRelayAuthTicket
	{
		// Token: 0x060009A5 RID: 2469 RVA: 0x0000F515 File Offset: 0x0000D715
		public void Clear()
		{
		}

		// Token: 0x04000B77 RID: 2935
		private SteamNetworkingIdentity m_identityGameserver;

		// Token: 0x04000B78 RID: 2936
		private SteamNetworkingIdentity m_identityAuthorizedClient;

		// Token: 0x04000B79 RID: 2937
		private uint m_unPublicIP;

		// Token: 0x04000B7A RID: 2938
		private RTime32 m_rtimeTicketExpiry;

		// Token: 0x04000B7B RID: 2939
		private SteamDatagramHostedAddress m_routing;

		// Token: 0x04000B7C RID: 2940
		private uint m_nAppID;

		// Token: 0x04000B7D RID: 2941
		private int m_nRestrictToVirtualPort;

		// Token: 0x04000B7E RID: 2942
		private const int k_nMaxExtraFields = 16;

		// Token: 0x04000B7F RID: 2943
		private int m_nExtraFields;

		// Token: 0x04000B80 RID: 2944
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		private SteamDatagramRelayAuthTicket.ExtraField[] m_vecExtraFields;

		// Token: 0x020001E9 RID: 489
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ExtraField
		{
			// Token: 0x04000C29 RID: 3113
			private SteamDatagramRelayAuthTicket.ExtraField.EType m_eType;

			// Token: 0x04000C2A RID: 3114
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
			private byte[] m_szName;

			// Token: 0x04000C2B RID: 3115
			private SteamDatagramRelayAuthTicket.ExtraField.OptionValue m_val;

			// Token: 0x020001F1 RID: 497
			private enum EType
			{
				// Token: 0x04000C3C RID: 3132
				k_EType_String,
				// Token: 0x04000C3D RID: 3133
				k_EType_Int,
				// Token: 0x04000C3E RID: 3134
				k_EType_Fixed64
			}

			// Token: 0x020001F2 RID: 498
			public struct FixedBytes128
			{
			}

			// Token: 0x020001F3 RID: 499
			[StructLayout(LayoutKind.Explicit)]
			private struct OptionValue
			{
				// Token: 0x04000C3F RID: 3135
				[FieldOffset(0)]
				public SteamDatagramRelayAuthTicket.ExtraField.FixedBytes128 m_szStringValue;

				// Token: 0x04000C40 RID: 3136
				[FieldOffset(0)]
				private long m_nIntValue;

				// Token: 0x04000C41 RID: 3137
				[FieldOffset(0)]
				private ulong m_nFixed64Value;
			}
		}
	}
}
