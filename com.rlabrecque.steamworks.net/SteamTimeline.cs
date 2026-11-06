using System;

namespace Steamworks
{
	// Token: 0x02000021 RID: 33
	public static class SteamTimeline
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00009D5C File Offset: 0x00007F5C
		public static void SetTimelineTooltip(string pchDescription, float flTimeDelta)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				NativeMethods.ISteamTimeline_SetTimelineTooltip(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, flTimeDelta);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00009DA0 File Offset: 0x00007FA0
		public static void ClearTimelineTooltip(float flTimeDelta)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_ClearTimelineTooltip(CSteamAPIContext.GetSteamTimeline(), flTimeDelta);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00009DB2 File Offset: 0x00007FB2
		public static void SetTimelineGameMode(ETimelineGameMode eMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_SetTimelineGameMode(CSteamAPIContext.GetSteamTimeline(), eMode);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00009DC4 File Offset: 0x00007FC4
		public static TimelineEventHandle_t AddInstantaneousTimelineEvent(string pchTitle, string pchDescription, string pchIcon, uint unIconPriority, float flStartOffsetSeconds = 0f, ETimelineEventClipPriority ePossibleClip = ETimelineEventClipPriority.k_ETimelineEventClipPriority_None)
		{
			InteropHelp.TestIfAvailableClient();
			TimelineEventHandle_t timelineEventHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						timelineEventHandle_t = (TimelineEventHandle_t)NativeMethods.ISteamTimeline_AddInstantaneousTimelineEvent(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unIconPriority, flStartOffsetSeconds, ePossibleClip);
					}
				}
			}
			return timelineEventHandle_t;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00009E4C File Offset: 0x0000804C
		public static TimelineEventHandle_t AddRangeTimelineEvent(string pchTitle, string pchDescription, string pchIcon, uint unIconPriority, float flStartOffsetSeconds = 0f, float flDuration = 0f, ETimelineEventClipPriority ePossibleClip = ETimelineEventClipPriority.k_ETimelineEventClipPriority_None)
		{
			InteropHelp.TestIfAvailableClient();
			TimelineEventHandle_t timelineEventHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						timelineEventHandle_t = (TimelineEventHandle_t)NativeMethods.ISteamTimeline_AddRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unIconPriority, flStartOffsetSeconds, flDuration, ePossibleClip);
					}
				}
			}
			return timelineEventHandle_t;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00009ED8 File Offset: 0x000080D8
		public static TimelineEventHandle_t StartRangeTimelineEvent(string pchTitle, string pchDescription, string pchIcon, uint unPriority, float flStartOffsetSeconds, ETimelineEventClipPriority ePossibleClip)
		{
			InteropHelp.TestIfAvailableClient();
			TimelineEventHandle_t timelineEventHandle_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						timelineEventHandle_t = (TimelineEventHandle_t)NativeMethods.ISteamTimeline_StartRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unPriority, flStartOffsetSeconds, ePossibleClip);
					}
				}
			}
			return timelineEventHandle_t;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00009F60 File Offset: 0x00008160
		public static void UpdateRangeTimelineEvent(TimelineEventHandle_t ulEvent, string pchTitle, string pchDescription, string pchIcon, uint unPriority, ETimelineEventClipPriority ePossibleClip)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						NativeMethods.ISteamTimeline_UpdateRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent, utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unPriority, ePossibleClip);
					}
				}
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00009FE4 File Offset: 0x000081E4
		public static void EndRangeTimelineEvent(TimelineEventHandle_t ulEvent, float flEndOffsetSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_EndRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent, flEndOffsetSeconds);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00009FF7 File Offset: 0x000081F7
		public static void RemoveTimelineEvent(TimelineEventHandle_t ulEvent)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_RemoveTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000A009 File Offset: 0x00008209
		public static SteamAPICall_t DoesEventRecordingExist(TimelineEventHandle_t ulEvent)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamTimeline_DoesEventRecordingExist(CSteamAPIContext.GetSteamTimeline(), ulEvent);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000A020 File Offset: 0x00008220
		public static void StartGamePhase()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_StartGamePhase(CSteamAPIContext.GetSteamTimeline());
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A031 File Offset: 0x00008231
		public static void EndGamePhase()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_EndGamePhase(CSteamAPIContext.GetSteamTimeline());
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000A044 File Offset: 0x00008244
		public static void SetGamePhaseID(string pchPhaseID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPhaseID))
			{
				NativeMethods.ISteamTimeline_SetGamePhaseID(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000A084 File Offset: 0x00008284
		public static SteamAPICall_t DoesGamePhaseRecordingExist(string pchPhaseID)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t steamAPICall_t;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPhaseID))
			{
				steamAPICall_t = (SteamAPICall_t)NativeMethods.ISteamTimeline_DoesGamePhaseRecordingExist(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle);
			}
			return steamAPICall_t;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000A0CC File Offset: 0x000082CC
		public static void AddGamePhaseTag(string pchTagName, string pchTagIcon, string pchTagGroup, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTagName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchTagIcon))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchTagGroup))
					{
						NativeMethods.ISteamTimeline_AddGamePhaseTag(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unPriority);
					}
				}
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000A14C File Offset: 0x0000834C
		public static void SetGamePhaseAttribute(string pchAttributeGroup, string pchAttributeValue, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAttributeGroup))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchAttributeValue))
				{
					NativeMethods.ISteamTimeline_SetGamePhaseAttribute(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, unPriority);
				}
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000A1AC File Offset: 0x000083AC
		public static void OpenOverlayToGamePhase(string pchPhaseID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPhaseID))
			{
				NativeMethods.ISteamTimeline_OpenOverlayToGamePhase(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle);
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000A1EC File Offset: 0x000083EC
		public static void OpenOverlayToTimelineEvent(TimelineEventHandle_t ulEvent)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_OpenOverlayToTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent);
		}
	}
}
