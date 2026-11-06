using System;

namespace Steamworks
{
	// Token: 0x0200001D RID: 29
	public static class SteamParentalSettings
	{
		// Token: 0x0600035A RID: 858 RVA: 0x00008F74 File Offset: 0x00007174
		public static bool BIsParentalLockEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsParentalLockEnabled(CSteamAPIContext.GetSteamParentalSettings());
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00008F85 File Offset: 0x00007185
		public static bool BIsParentalLockLocked()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsParentalLockLocked(CSteamAPIContext.GetSteamParentalSettings());
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00008F96 File Offset: 0x00007196
		public static bool BIsAppBlocked(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsAppBlocked(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00008FA8 File Offset: 0x000071A8
		public static bool BIsAppInBlockList(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsAppInBlockList(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00008FBA File Offset: 0x000071BA
		public static bool BIsFeatureBlocked(EParentalFeature eFeature)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsFeatureBlocked(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00008FCC File Offset: 0x000071CC
		public static bool BIsFeatureInBlockList(EParentalFeature eFeature)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamParentalSettings_BIsFeatureInBlockList(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
		}
	}
}
