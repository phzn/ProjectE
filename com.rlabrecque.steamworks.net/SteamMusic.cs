using System;

namespace Steamworks
{
	// Token: 0x02000018 RID: 24
	public static class SteamMusic
	{
		// Token: 0x060002EB RID: 747 RVA: 0x000084E1 File Offset: 0x000066E1
		public static bool BIsEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsEnabled(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000084F2 File Offset: 0x000066F2
		public static bool BIsPlaying()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_BIsPlaying(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00008503 File Offset: 0x00006703
		public static AudioPlayback_Status GetPlaybackStatus()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetPlaybackStatus(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00008514 File Offset: 0x00006714
		public static void Play()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Play(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00008525 File Offset: 0x00006725
		public static void Pause()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_Pause(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00008536 File Offset: 0x00006736
		public static void PlayPrevious()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayPrevious(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00008547 File Offset: 0x00006747
		public static void PlayNext()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_PlayNext(CSteamAPIContext.GetSteamMusic());
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00008558 File Offset: 0x00006758
		public static void SetVolume(float flVolume)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMusic_SetVolume(CSteamAPIContext.GetSteamMusic(), flVolume);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000856A File Offset: 0x0000676A
		public static float GetVolume()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusic_GetVolume(CSteamAPIContext.GetSteamMusic());
		}
	}
}
