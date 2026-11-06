using System;
using MagicaCloth2;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A8D RID: 2701
	public static class ClothQualityService
	{
		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004CDC RID: 19676 RVA: 0x001DDE17 File Offset: 0x001DC017
		public static bool Enabled
		{
			get
			{
				return ClothQualityService._enabled;
			}
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x001DDE1E File Offset: 0x001DC01E
		public static void Apply(bool enabled)
		{
			ClothQualityService._enabled = enabled;
			MagicaManager.SetSimulationFrequency(90);
			MagicaManager.SetMaxSimulationCountPerFrame(3);
			MagicaManager.SetGlobalTimeScale(enabled ? 1f : 0f);
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x001DDE47 File Offset: 0x001DC047
		public static void Register(MagicaCloth cloth)
		{
			if (cloth == null)
			{
				return;
			}
			MagicaManager.SetGlobalTimeScale(ClothQualityService._enabled ? 1f : 0f);
		}

		// Token: 0x04004E63 RID: 20067
		private const int StableSimulationFrequency = 90;

		// Token: 0x04004E64 RID: 20068
		private const int StableMaxSimulationCountPerFrame = 3;

		// Token: 0x04004E65 RID: 20069
		private static bool _enabled = true;
	}
}
