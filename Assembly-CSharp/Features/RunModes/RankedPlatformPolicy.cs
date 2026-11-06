using System;
using Features.Localization;
using Features.Workshop;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D62 RID: 3426
	public static class RankedPlatformPolicy
	{
		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06006495 RID: 25749 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public static bool EditorTestUploadsEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06006496 RID: 25750 RVA: 0x00283B7E File Offset: 0x00281D7E
		public static string CurrentSubmissionIdentity
		{
			get
			{
				return RankedPlatformPolicy.SubmissionIdentityFor(RankedPlatformPolicy.CurrentBuildKind, RankedPlatformPolicy.EditorTestUploadsEnabled);
			}
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x00283B8F File Offset: 0x00281D8F
		public static string SubmissionIdentityFor(RankedBuildKind kind, bool editorTestUploads)
		{
			if (kind == RankedBuildKind.SteamLocalQa)
			{
				return "PE_STEAM_LOCAL_QA.v1";
			}
			if (kind != RankedBuildKind.EditorPreview || !editorTestUploads)
			{
				return "PE_STEAM_BUILD.release.v1";
			}
			return "PE_STEAM_EDITOR_TEST.v1";
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06006498 RID: 25752 RVA: 0x0002AE8C File Offset: 0x0002908C
		private static bool SteamIntegrationCompiled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06006499 RID: 25753 RVA: 0x0002AE8C File Offset: 0x0002908C
		private static bool ExplicitSteamDistribution
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x0600649A RID: 25754 RVA: 0x0000D91D File Offset: 0x0000BB1D
		private static bool ExplicitLocalQaDistribution
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x0600649B RID: 25755 RVA: 0x00283BAE File Offset: 0x00281DAE
		public static RankedBuildKind CurrentBuildKind
		{
			get
			{
				return RankedPlatformPolicy.Classify(RankedPlatformPolicy.SteamIntegrationCompiled, Application.isEditor, Debug.isDebugBuild, RankedPlatformPolicy.ExplicitSteamDistribution, RankedPlatformPolicy.ExplicitLocalQaDistribution);
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x0600649C RID: 25756 RVA: 0x00283BCE File Offset: 0x00281DCE
		public static bool CanInitializeSteam
		{
			get
			{
				return RankedPlatformPolicy.CurrentBuildKind > RankedBuildKind.RawStandalone;
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x0600649D RID: 25757 RVA: 0x00283BD8 File Offset: 0x00281DD8
		public static bool CanUploadScores
		{
			get
			{
				return !WorkshopModManager.HasModdedSession && RankedPlatformPolicy.CanUploadScoresFor(RankedPlatformPolicy.CurrentBuildKind, RankedPlatformPolicy.EditorTestUploadsEnabled);
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x0600649E RID: 25758 RVA: 0x00283BF2 File Offset: 0x00281DF2
		public static string SubmissionDisabledReason
		{
			get
			{
				if (WorkshopModManager.HasModdedSession)
				{
					return GameLocalization.Get("workshop.mod.ranked", "Steam score submissions are disabled while mods are active.");
				}
				if (!RankedPlatformPolicy.CanUploadScores)
				{
					return RankedPlatformPolicy.DisabledReasonFor(RankedPlatformPolicy.CurrentBuildKind);
				}
				return string.Empty;
			}
		}

		// Token: 0x0600649F RID: 25759 RVA: 0x00283C22 File Offset: 0x00281E22
		public static bool CanUploadScoresFor(RankedBuildKind kind, bool editorTestUploads)
		{
			return kind == RankedBuildKind.SteamRelease || kind == RankedBuildKind.SteamLocalQa || (kind == RankedBuildKind.EditorPreview && editorTestUploads);
		}

		// Token: 0x060064A0 RID: 25760 RVA: 0x00283C34 File Offset: 0x00281E34
		public static RankedBuildKind Classify(bool steamCompiled, bool editor, bool development, bool steamDistribution, bool localQaDistribution = false)
		{
			if (!steamCompiled)
			{
				return RankedBuildKind.RawStandalone;
			}
			if (editor)
			{
				return RankedBuildKind.EditorPreview;
			}
			if (!steamDistribution)
			{
				return RankedBuildKind.RawStandalone;
			}
			if (localQaDistribution)
			{
				if (!development)
				{
					return RankedBuildKind.RawStandalone;
				}
				return RankedBuildKind.SteamLocalQa;
			}
			else
			{
				if (!development)
				{
					return RankedBuildKind.SteamRelease;
				}
				return RankedBuildKind.SteamDevelopment;
			}
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x00283C56 File Offset: 0x00281E56
		public static string DisabledReasonFor(RankedBuildKind kind)
		{
			switch (kind)
			{
			case RankedBuildKind.EditorPreview:
				return "Scores cannot be submitted in this session.";
			case RankedBuildKind.SteamDevelopment:
				return "Development preview: scores are never submitted. Use a Steam release build to compete.";
			case RankedBuildKind.SteamRelease:
			case RankedBuildKind.SteamLocalQa:
				return string.Empty;
			default:
				return "Steam leaderboards are unavailable in this standalone build.";
			}
		}

		// Token: 0x0400676F RID: 26479
		public const uint ExpectedSteamAppId = 4688700U;

		// Token: 0x04006770 RID: 26480
		public const string SteamBuildDefine = "PE_STEAM_BUILD";

		// Token: 0x04006771 RID: 26481
		public const string LocalQaBuildDefine = "PE_STEAM_LOCAL_QA";

		// Token: 0x04006772 RID: 26482
		public const string ReleaseIdentity = "PE_STEAM_BUILD.release.v1";

		// Token: 0x04006773 RID: 26483
		public const string EditorTestIdentity = "PE_STEAM_EDITOR_TEST.v1";

		// Token: 0x04006774 RID: 26484
		public const string LocalQaIdentity = "PE_STEAM_LOCAL_QA.v1";
	}
}
