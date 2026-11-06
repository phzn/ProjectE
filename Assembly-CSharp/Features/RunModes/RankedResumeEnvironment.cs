using System;

namespace Features.RunModes
{
	// Token: 0x02000D65 RID: 3429
	public static class RankedResumeEnvironment
	{
		// Token: 0x060064A5 RID: 25765 RVA: 0x00283C8C File Offset: 0x00281E8C
		public static string GetExclusionReason(RankedBuildKind buildKind, bool canSubmitScores, bool hasKnownBackend, string backendReason, bool editorTestUploads = false)
		{
			if (canSubmitScores)
			{
				return string.Empty;
			}
			if (buildKind == RankedBuildKind.EditorPreview && editorTestUploads && !hasKnownBackend)
			{
				return string.Empty;
			}
			switch (buildKind)
			{
			case RankedBuildKind.RawStandalone:
				return "This run is running outside the Steam edition. Start a new run in the Steam release to compete.";
			case RankedBuildKind.EditorPreview:
				return "This run is running in an unsupported session. Start a new run in the Steam release to compete.";
			case RankedBuildKind.SteamDevelopment:
				return "This run is running in a development build. Start a new run in the Steam release to compete.";
			default:
				if (!hasKnownBackend)
				{
					return string.Empty;
				}
				if (!string.IsNullOrWhiteSpace(backendReason))
				{
					return "This run is running in a Steam session that cannot submit scores. " + backendReason;
				}
				return "This run is running in a Steam session that cannot submit scores. Start a new run to compete.";
			}
		}
	}
}
