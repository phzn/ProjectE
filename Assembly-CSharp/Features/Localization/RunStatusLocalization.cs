using System;

namespace Features.Localization
{
	// Token: 0x020010D2 RID: 4306
	public static class RunStatusLocalization
	{
		// Token: 0x06008284 RID: 33412 RVA: 0x00354DB8 File Offset: 0x00352FB8
		public static string GetSource(string english)
		{
			string[] scopes = RunStatusLocalization.Scopes;
			for (int i = 0; i < scopes.Length; i++)
			{
				string text;
				if (GameLocalization.TryGetSourceKey(scopes[i], english, out text))
				{
					return GameLocalization.Get(text, english);
				}
			}
			return english;
		}

		// Token: 0x06008285 RID: 33413 RVA: 0x00354DEF File Offset: 0x00352FEF
		public static Func<string> CreateExclusionMessage(string rawReason)
		{
			return () => GameLocalization.FormatSource("runtime_debug_ui", "This run will not appear on the Steam leaderboard.\n{0}", new object[] { RunStatusLocalization.GetSource(rawReason) });
		}

		// Token: 0x040085B1 RID: 34225
		public const string Scope = "runtime_debug_ui";

		// Token: 0x040085B2 RID: 34226
		private static readonly string[] Scopes = new string[] { "runtime_debug_ui", "rank_ui", "leaderboard_ui" };
	}
}
