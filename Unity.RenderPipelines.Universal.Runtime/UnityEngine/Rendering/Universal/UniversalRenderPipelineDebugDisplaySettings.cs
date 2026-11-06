using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200005E RID: 94
	public class UniversalRenderPipelineDebugDisplaySettings : DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0001571A File Offset: 0x0001391A
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00015722 File Offset: 0x00013922
		private DebugDisplaySettingsCommon commonSettings { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0001572B File Offset: 0x0001392B
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00015733 File Offset: 0x00013933
		public DebugDisplaySettingsMaterial materialSettings { get; private set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0001573C File Offset: 0x0001393C
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00015744 File Offset: 0x00013944
		public DebugDisplaySettingsRendering renderingSettings { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0001574D File Offset: 0x0001394D
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00015755 File Offset: 0x00013955
		public DebugDisplaySettingsLighting lightingSettings { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0001575E File Offset: 0x0001395E
		// (set) Token: 0x06000393 RID: 915 RVA: 0x00015766 File Offset: 0x00013966
		public DebugDisplaySettingsVolume volumeSettings { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0001576F File Offset: 0x0001396F
		// (set) Token: 0x06000395 RID: 917 RVA: 0x00015777 File Offset: 0x00013977
		internal DebugDisplayStats displayStats { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00015780 File Offset: 0x00013980
		public override bool IsPostProcessingAllowed
		{
			get
			{
				DebugPostProcessingMode postProcessingDebugMode = this.renderingSettings.postProcessingDebugMode;
				switch (postProcessingDebugMode)
				{
				case DebugPostProcessingMode.Disabled:
					return false;
				case DebugPostProcessingMode.Auto:
				{
					bool flag = true;
					foreach (IDebugDisplaySettingsData debugDisplaySettingsData in this.m_Settings)
					{
						flag &= debugDisplaySettingsData.IsPostProcessingAllowed;
					}
					return flag;
				}
				case DebugPostProcessingMode.Enabled:
					return true;
				default:
					throw new ArgumentOutOfRangeException("debugPostProcessingMode", string.Format("Invalid post-processing state {0}", postProcessingDebugMode));
				}
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00015818 File Offset: 0x00013A18
		public UniversalRenderPipelineDebugDisplaySettings()
		{
			this.Reset();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00015828 File Offset: 0x00013A28
		public override void Reset()
		{
			base.Reset();
			this.displayStats = base.Add<DebugDisplayStats>(new DebugDisplayStats());
			this.materialSettings = base.Add<DebugDisplaySettingsMaterial>(new DebugDisplaySettingsMaterial());
			this.lightingSettings = base.Add<DebugDisplaySettingsLighting>(new DebugDisplaySettingsLighting());
			this.renderingSettings = base.Add<DebugDisplaySettingsRendering>(new DebugDisplaySettingsRendering());
			this.volumeSettings = base.Add<DebugDisplaySettingsVolume>(new DebugDisplaySettingsVolume(new UniversalRenderPipelineVolumeDebugSettings()));
			this.commonSettings = base.Add<DebugDisplaySettingsCommon>(new DebugDisplaySettingsCommon());
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000158A6 File Offset: 0x00013AA6
		internal void UpdateFrameTiming()
		{
			if (this.displayStats != null)
			{
				this.displayStats.UpdateFrameTiming();
			}
		}
	}
}
