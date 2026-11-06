using System;

namespace Steamworks
{
	// Token: 0x0200015A RID: 346
	public enum ESteamNetworkingConfigValue
	{
		// Token: 0x04000A1F RID: 2591
		k_ESteamNetworkingConfig_Invalid,
		// Token: 0x04000A20 RID: 2592
		k_ESteamNetworkingConfig_TimeoutInitial = 24,
		// Token: 0x04000A21 RID: 2593
		k_ESteamNetworkingConfig_TimeoutConnected,
		// Token: 0x04000A22 RID: 2594
		k_ESteamNetworkingConfig_SendBufferSize = 9,
		// Token: 0x04000A23 RID: 2595
		k_ESteamNetworkingConfig_RecvBufferSize = 47,
		// Token: 0x04000A24 RID: 2596
		k_ESteamNetworkingConfig_RecvBufferMessages,
		// Token: 0x04000A25 RID: 2597
		k_ESteamNetworkingConfig_RecvMaxMessageSize,
		// Token: 0x04000A26 RID: 2598
		k_ESteamNetworkingConfig_RecvMaxSegmentsPerPacket,
		// Token: 0x04000A27 RID: 2599
		k_ESteamNetworkingConfig_ConnectionUserData = 40,
		// Token: 0x04000A28 RID: 2600
		k_ESteamNetworkingConfig_SendRateMin = 10,
		// Token: 0x04000A29 RID: 2601
		k_ESteamNetworkingConfig_SendRateMax,
		// Token: 0x04000A2A RID: 2602
		k_ESteamNetworkingConfig_NagleTime,
		// Token: 0x04000A2B RID: 2603
		k_ESteamNetworkingConfig_IP_AllowWithoutAuth = 23,
		// Token: 0x04000A2C RID: 2604
		k_ESteamNetworkingConfig_IPLocalHost_AllowWithoutAuth = 52,
		// Token: 0x04000A2D RID: 2605
		k_ESteamNetworkingConfig_MTU_PacketSize = 32,
		// Token: 0x04000A2E RID: 2606
		k_ESteamNetworkingConfig_MTU_DataSize,
		// Token: 0x04000A2F RID: 2607
		k_ESteamNetworkingConfig_Unencrypted,
		// Token: 0x04000A30 RID: 2608
		k_ESteamNetworkingConfig_SymmetricConnect = 37,
		// Token: 0x04000A31 RID: 2609
		k_ESteamNetworkingConfig_LocalVirtualPort,
		// Token: 0x04000A32 RID: 2610
		k_ESteamNetworkingConfig_DualWifi_Enable,
		// Token: 0x04000A33 RID: 2611
		k_ESteamNetworkingConfig_EnableDiagnosticsUI = 46,
		// Token: 0x04000A34 RID: 2612
		k_ESteamNetworkingConfig_SendTimeSincePreviousPacket = 59,
		// Token: 0x04000A35 RID: 2613
		k_ESteamNetworkingConfig_FakePacketLoss_Send = 2,
		// Token: 0x04000A36 RID: 2614
		k_ESteamNetworkingConfig_FakePacketLoss_Recv,
		// Token: 0x04000A37 RID: 2615
		k_ESteamNetworkingConfig_FakePacketLag_Send,
		// Token: 0x04000A38 RID: 2616
		k_ESteamNetworkingConfig_FakePacketLag_Recv,
		// Token: 0x04000A39 RID: 2617
		k_ESteamNetworkingConfig_FakePacketJitter_Send_Avg = 53,
		// Token: 0x04000A3A RID: 2618
		k_ESteamNetworkingConfig_FakePacketJitter_Send_Max,
		// Token: 0x04000A3B RID: 2619
		k_ESteamNetworkingConfig_FakePacketJitter_Send_Pct,
		// Token: 0x04000A3C RID: 2620
		k_ESteamNetworkingConfig_FakePacketJitter_Recv_Avg,
		// Token: 0x04000A3D RID: 2621
		k_ESteamNetworkingConfig_FakePacketJitter_Recv_Max,
		// Token: 0x04000A3E RID: 2622
		k_ESteamNetworkingConfig_FakePacketJitter_Recv_Pct,
		// Token: 0x04000A3F RID: 2623
		k_ESteamNetworkingConfig_FakePacketReorder_Send = 6,
		// Token: 0x04000A40 RID: 2624
		k_ESteamNetworkingConfig_FakePacketReorder_Recv,
		// Token: 0x04000A41 RID: 2625
		k_ESteamNetworkingConfig_FakePacketReorder_Time,
		// Token: 0x04000A42 RID: 2626
		k_ESteamNetworkingConfig_FakePacketDup_Send = 26,
		// Token: 0x04000A43 RID: 2627
		k_ESteamNetworkingConfig_FakePacketDup_Recv,
		// Token: 0x04000A44 RID: 2628
		k_ESteamNetworkingConfig_FakePacketDup_TimeMax,
		// Token: 0x04000A45 RID: 2629
		k_ESteamNetworkingConfig_PacketTraceMaxBytes = 41,
		// Token: 0x04000A46 RID: 2630
		k_ESteamNetworkingConfig_FakeRateLimit_Send_Rate,
		// Token: 0x04000A47 RID: 2631
		k_ESteamNetworkingConfig_FakeRateLimit_Send_Burst,
		// Token: 0x04000A48 RID: 2632
		k_ESteamNetworkingConfig_FakeRateLimit_Recv_Rate,
		// Token: 0x04000A49 RID: 2633
		k_ESteamNetworkingConfig_FakeRateLimit_Recv_Burst,
		// Token: 0x04000A4A RID: 2634
		k_ESteamNetworkingConfig_OutOfOrderCorrectionWindowMicroseconds = 51,
		// Token: 0x04000A4B RID: 2635
		k_ESteamNetworkingConfig_Callback_ConnectionStatusChanged = 201,
		// Token: 0x04000A4C RID: 2636
		k_ESteamNetworkingConfig_Callback_AuthStatusChanged,
		// Token: 0x04000A4D RID: 2637
		k_ESteamNetworkingConfig_Callback_RelayNetworkStatusChanged,
		// Token: 0x04000A4E RID: 2638
		k_ESteamNetworkingConfig_Callback_MessagesSessionRequest,
		// Token: 0x04000A4F RID: 2639
		k_ESteamNetworkingConfig_Callback_MessagesSessionFailed,
		// Token: 0x04000A50 RID: 2640
		k_ESteamNetworkingConfig_Callback_CreateConnectionSignaling,
		// Token: 0x04000A51 RID: 2641
		k_ESteamNetworkingConfig_Callback_FakeIPResult,
		// Token: 0x04000A52 RID: 2642
		k_ESteamNetworkingConfig_P2P_STUN_ServerList = 103,
		// Token: 0x04000A53 RID: 2643
		k_ESteamNetworkingConfig_P2P_Transport_ICE_Enable,
		// Token: 0x04000A54 RID: 2644
		k_ESteamNetworkingConfig_P2P_Transport_ICE_Penalty,
		// Token: 0x04000A55 RID: 2645
		k_ESteamNetworkingConfig_P2P_Transport_SDR_Penalty,
		// Token: 0x04000A56 RID: 2646
		k_ESteamNetworkingConfig_P2P_TURN_ServerList,
		// Token: 0x04000A57 RID: 2647
		k_ESteamNetworkingConfig_P2P_TURN_UserList,
		// Token: 0x04000A58 RID: 2648
		k_ESteamNetworkingConfig_P2P_TURN_PassList,
		// Token: 0x04000A59 RID: 2649
		k_ESteamNetworkingConfig_P2P_Transport_ICE_Implementation,
		// Token: 0x04000A5A RID: 2650
		k_ESteamNetworkingConfig_SDRClient_ConsecutitivePingTimeoutsFailInitial = 19,
		// Token: 0x04000A5B RID: 2651
		k_ESteamNetworkingConfig_SDRClient_ConsecutitivePingTimeoutsFail,
		// Token: 0x04000A5C RID: 2652
		k_ESteamNetworkingConfig_SDRClient_MinPingsBeforePingAccurate,
		// Token: 0x04000A5D RID: 2653
		k_ESteamNetworkingConfig_SDRClient_SingleSocket,
		// Token: 0x04000A5E RID: 2654
		k_ESteamNetworkingConfig_SDRClient_ForceRelayCluster = 29,
		// Token: 0x04000A5F RID: 2655
		k_ESteamNetworkingConfig_SDRClient_DevTicket,
		// Token: 0x04000A60 RID: 2656
		k_ESteamNetworkingConfig_SDRClient_ForceProxyAddr,
		// Token: 0x04000A61 RID: 2657
		k_ESteamNetworkingConfig_SDRClient_FakeClusterPing = 36,
		// Token: 0x04000A62 RID: 2658
		k_ESteamNetworkingConfig_SDRClient_LimitPingProbesToNearestN = 60,
		// Token: 0x04000A63 RID: 2659
		k_ESteamNetworkingConfig_LogLevel_AckRTT = 13,
		// Token: 0x04000A64 RID: 2660
		k_ESteamNetworkingConfig_LogLevel_PacketDecode,
		// Token: 0x04000A65 RID: 2661
		k_ESteamNetworkingConfig_LogLevel_Message,
		// Token: 0x04000A66 RID: 2662
		k_ESteamNetworkingConfig_LogLevel_PacketGaps,
		// Token: 0x04000A67 RID: 2663
		k_ESteamNetworkingConfig_LogLevel_P2PRendezvous,
		// Token: 0x04000A68 RID: 2664
		k_ESteamNetworkingConfig_LogLevel_SDRRelayPings,
		// Token: 0x04000A69 RID: 2665
		k_ESteamNetworkingConfig_ECN = 999,
		// Token: 0x04000A6A RID: 2666
		k_ESteamNetworkingConfig_SDRClient_EnableTOSProbes = 998,
		// Token: 0x04000A6B RID: 2667
		k_ESteamNetworkingConfig_DELETED_EnumerateDevVars = 35,
		// Token: 0x04000A6C RID: 2668
		k_ESteamNetworkingConfigValue__Force32Bit = 2147483647
	}
}
