using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	internal struct ProbeDilationSettings
	{
		// Token: 0x060004D3 RID: 1235 RVA: 0x00017E7D File Offset: 0x0001607D
		internal void SetDefaults()
		{
			this.enableDilation = true;
			this.dilationDistance = 1f;
			this.dilationValidityThreshold = 0.25f;
			this.dilationIterations = 1;
			this.squaredDistWeighting = true;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00017EAA File Offset: 0x000160AA
		internal void UpgradeFromTo(ProbeVolumeBakingProcessSettings.SettingsVersion from, ProbeVolumeBakingProcessSettings.SettingsVersion to)
		{
		}

		// Token: 0x04000328 RID: 808
		public bool enableDilation;

		// Token: 0x04000329 RID: 809
		public float dilationDistance;

		// Token: 0x0400032A RID: 810
		public float dilationValidityThreshold;

		// Token: 0x0400032B RID: 811
		public int dilationIterations;

		// Token: 0x0400032C RID: 812
		public bool squaredDistWeighting;
	}
}
