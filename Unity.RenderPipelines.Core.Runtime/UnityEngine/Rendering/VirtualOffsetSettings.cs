using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000093 RID: 147
	[Serializable]
	internal struct VirtualOffsetSettings
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x00017EAC File Offset: 0x000160AC
		internal void SetDefaults()
		{
			this.useVirtualOffset = true;
			this.outOfGeoOffset = 0.01f;
			this.searchMultiplier = 0.2f;
			this.UpgradeFromTo(ProbeVolumeBakingProcessSettings.SettingsVersion.Initial, ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00017ED3 File Offset: 0x000160D3
		internal void UpgradeFromTo(ProbeVolumeBakingProcessSettings.SettingsVersion from, ProbeVolumeBakingProcessSettings.SettingsVersion to)
		{
			if (from < ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset && to >= ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset)
			{
				this.rayOriginBias = -0.001f;
				this.maxHitsPerRay = 10;
				this.collisionMask = -5;
			}
		}

		// Token: 0x0400032D RID: 813
		public bool useVirtualOffset;

		// Token: 0x0400032E RID: 814
		[Range(0f, 1f)]
		public float outOfGeoOffset;

		// Token: 0x0400032F RID: 815
		[Range(0f, 2f)]
		public float searchMultiplier;

		// Token: 0x04000330 RID: 816
		[Range(-0.05f, 0f)]
		public float rayOriginBias;

		// Token: 0x04000331 RID: 817
		[Range(4f, 24f)]
		public int maxHitsPerRay;

		// Token: 0x04000332 RID: 818
		public LayerMask collisionMask;
	}
}
