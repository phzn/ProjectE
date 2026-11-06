using System;

namespace Unity.Entities
{
	// Token: 0x0200002F RID: 47
	public static class ChangeVersionUtility
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00003E81 File Offset: 0x00002081
		public static bool DidChange(uint changeVersion, uint requiredVersion)
		{
			return requiredVersion == 0U || changeVersion - requiredVersion > 0U;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003E8E File Offset: 0x0000208E
		internal static void IncrementGlobalSystemVersion(ref uint globalSystemVersion)
		{
			globalSystemVersion += 1U;
			if (globalSystemVersion == 0U)
			{
				globalSystemVersion += 1U;
			}
		}

		// Token: 0x04000044 RID: 68
		internal const uint InitialGlobalSystemVersion = 1U;
	}
}
