using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000094 RID: 148
	[Serializable]
	internal struct ProbeVolumeBakingProcessSettings
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00017F00 File Offset: 0x00016100
		internal static ProbeVolumeBakingProcessSettings Default
		{
			get
			{
				ProbeVolumeBakingProcessSettings probeVolumeBakingProcessSettings = default(ProbeVolumeBakingProcessSettings);
				probeVolumeBakingProcessSettings.SetDefaults();
				return probeVolumeBakingProcessSettings;
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00017F1D File Offset: 0x0001611D
		internal ProbeVolumeBakingProcessSettings(ProbeDilationSettings dilationSettings, VirtualOffsetSettings virtualOffsetSettings)
		{
			this.m_Version = ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset;
			this.dilationSettings = dilationSettings;
			this.virtualOffsetSettings = virtualOffsetSettings;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00017F34 File Offset: 0x00016134
		internal void SetDefaults()
		{
			this.m_Version = ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset;
			this.dilationSettings.SetDefaults();
			this.virtualOffsetSettings.SetDefaults();
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00017F53 File Offset: 0x00016153
		internal void Upgrade()
		{
			if (this.m_Version != ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset)
			{
				this.dilationSettings.UpgradeFromTo(this.m_Version, ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset);
				this.virtualOffsetSettings.UpgradeFromTo(this.m_Version, ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset);
				this.m_Version = ProbeVolumeBakingProcessSettings.SettingsVersion.ThreadedVirtualOffset;
			}
		}

		// Token: 0x04000333 RID: 819
		[SerializeField]
		private ProbeVolumeBakingProcessSettings.SettingsVersion m_Version;

		// Token: 0x04000334 RID: 820
		public ProbeDilationSettings dilationSettings;

		// Token: 0x04000335 RID: 821
		public VirtualOffsetSettings virtualOffsetSettings;

		// Token: 0x020001AD RID: 429
		internal enum SettingsVersion
		{
			// Token: 0x0400071A RID: 1818
			Initial,
			// Token: 0x0400071B RID: 1819
			ThreadedVirtualOffset,
			// Token: 0x0400071C RID: 1820
			Max,
			// Token: 0x0400071D RID: 1821
			Current = 1
		}
	}
}
