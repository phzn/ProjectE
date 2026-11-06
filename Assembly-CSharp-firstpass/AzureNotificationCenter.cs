using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000017 RID: 23
	public static class AzureNotificationCenter
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000075 RID: 117 RVA: 0x00003070 File Offset: 0x00001270
		// (remove) Token: 0x06000076 RID: 118 RVA: 0x000030A4 File Offset: 0x000012A4
		public static event AzureNotificationCenter.TimeSystemDelegate OnTimelineChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000077 RID: 119 RVA: 0x000030D8 File Offset: 0x000012D8
		// (remove) Token: 0x06000078 RID: 120 RVA: 0x0000310C File Offset: 0x0000130C
		public static event AzureNotificationCenter.TimeSystemDelegate OnMinuteChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000079 RID: 121 RVA: 0x00003140 File Offset: 0x00001340
		// (remove) Token: 0x0600007A RID: 122 RVA: 0x00003174 File Offset: 0x00001374
		public static event AzureNotificationCenter.TimeSystemDelegate OnHourChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600007B RID: 123 RVA: 0x000031A8 File Offset: 0x000013A8
		// (remove) Token: 0x0600007C RID: 124 RVA: 0x000031DC File Offset: 0x000013DC
		public static event AzureNotificationCenter.TimeSystemDelegate OnDayChanged;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600007D RID: 125 RVA: 0x00003210 File Offset: 0x00001410
		// (remove) Token: 0x0600007E RID: 126 RVA: 0x00003244 File Offset: 0x00001444
		public static event AzureNotificationCenter.TimeSystemDelegate OnMonthChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600007F RID: 127 RVA: 0x00003278 File Offset: 0x00001478
		// (remove) Token: 0x06000080 RID: 128 RVA: 0x000032AC File Offset: 0x000014AC
		public static event AzureNotificationCenter.TimeSystemDelegate OnYearChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000081 RID: 129 RVA: 0x000032E0 File Offset: 0x000014E0
		// (remove) Token: 0x06000082 RID: 130 RVA: 0x00003314 File Offset: 0x00001514
		public static event AzureNotificationCenter.WeatherSystemDelegate OnBeforeOverrideUpdate;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000083 RID: 131 RVA: 0x00003348 File Offset: 0x00001548
		// (remove) Token: 0x06000084 RID: 132 RVA: 0x0000337C File Offset: 0x0000157C
		public static event AzureNotificationCenter.WeatherSystemDelegate OnAfterOverrideUpdate;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000085 RID: 133 RVA: 0x000033B0 File Offset: 0x000015B0
		// (remove) Token: 0x06000086 RID: 134 RVA: 0x000033E4 File Offset: 0x000015E4
		public static event AzureNotificationCenter.WeatherSystemDelegate OnBeforeWeatherSystemUpdate;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000087 RID: 135 RVA: 0x00003418 File Offset: 0x00001618
		// (remove) Token: 0x06000088 RID: 136 RVA: 0x0000344C File Offset: 0x0000164C
		public static event AzureNotificationCenter.WeatherSystemDelegate OnAfterWeatherSystemUpdate;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000089 RID: 137 RVA: 0x00003480 File Offset: 0x00001680
		// (remove) Token: 0x0600008A RID: 138 RVA: 0x000034B4 File Offset: 0x000016B4
		public static event AzureNotificationCenter.WeatherSystemDelegate OnWeatherTransitionEnd;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600008B RID: 139 RVA: 0x000034E8 File Offset: 0x000016E8
		// (remove) Token: 0x0600008C RID: 140 RVA: 0x0000351C File Offset: 0x0000171C
		public static event AzureNotificationCenter.VolumetriLightPreRenderDelegate OnVolumetricLightPreRender;

		// Token: 0x0200002E RID: 46
		public static class Invoke
		{
			// Token: 0x0600028C RID: 652 RVA: 0x0000AC4C File Offset: 0x00008E4C
			public static void OnTimelineChangedCallback(AzureTimeSystem timeSystem)
			{
				AzureNotificationCenter.TimeSystemDelegate onTimelineChanged = AzureNotificationCenter.OnTimelineChanged;
				if (onTimelineChanged == null)
				{
					return;
				}
				onTimelineChanged(timeSystem);
			}

			// Token: 0x0600028D RID: 653 RVA: 0x0000AC5E File Offset: 0x00008E5E
			public static void OnMinuteChangedCallback(AzureTimeSystem timeSystem)
			{
				AzureNotificationCenter.TimeSystemDelegate onMinuteChanged = AzureNotificationCenter.OnMinuteChanged;
				if (onMinuteChanged == null)
				{
					return;
				}
				onMinuteChanged(timeSystem);
			}

			// Token: 0x0600028E RID: 654 RVA: 0x0000AC70 File Offset: 0x00008E70
			public static void OnHourChangedCallback(AzureTimeSystem timeSystem)
			{
				AzureNotificationCenter.TimeSystemDelegate onHourChanged = AzureNotificationCenter.OnHourChanged;
				if (onHourChanged == null)
				{
					return;
				}
				onHourChanged(timeSystem);
			}

			// Token: 0x0600028F RID: 655 RVA: 0x0000AC82 File Offset: 0x00008E82
			public static void OnDayChangedCallback(AzureTimeSystem timeSystem)
			{
				AzureNotificationCenter.TimeSystemDelegate onDayChanged = AzureNotificationCenter.OnDayChanged;
				if (onDayChanged == null)
				{
					return;
				}
				onDayChanged(timeSystem);
			}

			// Token: 0x06000290 RID: 656 RVA: 0x0000AC94 File Offset: 0x00008E94
			public static void OnMonthChangedCallback(AzureTimeSystem timeSystem)
			{
				AzureNotificationCenter.TimeSystemDelegate onMonthChanged = AzureNotificationCenter.OnMonthChanged;
				if (onMonthChanged == null)
				{
					return;
				}
				onMonthChanged(timeSystem);
			}

			// Token: 0x06000291 RID: 657 RVA: 0x0000ACA6 File Offset: 0x00008EA6
			public static void OnYearChangedCallback(AzureTimeSystem timeSystem)
			{
				AzureNotificationCenter.TimeSystemDelegate onYearChanged = AzureNotificationCenter.OnYearChanged;
				if (onYearChanged == null)
				{
					return;
				}
				onYearChanged(timeSystem);
			}

			// Token: 0x06000292 RID: 658 RVA: 0x0000ACB8 File Offset: 0x00008EB8
			public static void OnBeforeOverrideUpdateCallback(AzureWeatherSystem azureWeatherSystem)
			{
				AzureNotificationCenter.WeatherSystemDelegate onBeforeOverrideUpdate = AzureNotificationCenter.OnBeforeOverrideUpdate;
				if (onBeforeOverrideUpdate == null)
				{
					return;
				}
				onBeforeOverrideUpdate(azureWeatherSystem);
			}

			// Token: 0x06000293 RID: 659 RVA: 0x0000ACCA File Offset: 0x00008ECA
			public static void OnAfterOverrideUpdateCallback(AzureWeatherSystem azureWeatherSystem)
			{
				AzureNotificationCenter.WeatherSystemDelegate onAfterOverrideUpdate = AzureNotificationCenter.OnAfterOverrideUpdate;
				if (onAfterOverrideUpdate == null)
				{
					return;
				}
				onAfterOverrideUpdate(azureWeatherSystem);
			}

			// Token: 0x06000294 RID: 660 RVA: 0x0000ACDC File Offset: 0x00008EDC
			public static void OnBeforeWeatherSystemUpdateCallback(AzureWeatherSystem azureWeatherSystem)
			{
				AzureNotificationCenter.WeatherSystemDelegate onBeforeWeatherSystemUpdate = AzureNotificationCenter.OnBeforeWeatherSystemUpdate;
				if (onBeforeWeatherSystemUpdate == null)
				{
					return;
				}
				onBeforeWeatherSystemUpdate(azureWeatherSystem);
			}

			// Token: 0x06000295 RID: 661 RVA: 0x0000ACEE File Offset: 0x00008EEE
			public static void OnAfterWeatherSystemUpdateCallback(AzureWeatherSystem azureWeatherSystem)
			{
				AzureNotificationCenter.WeatherSystemDelegate onAfterWeatherSystemUpdate = AzureNotificationCenter.OnAfterWeatherSystemUpdate;
				if (onAfterWeatherSystemUpdate == null)
				{
					return;
				}
				onAfterWeatherSystemUpdate(azureWeatherSystem);
			}

			// Token: 0x06000296 RID: 662 RVA: 0x0000AD00 File Offset: 0x00008F00
			public static void OnWeatherTransitionEndCallback(AzureWeatherSystem azureWeatherSystem)
			{
				AzureNotificationCenter.WeatherSystemDelegate onWeatherTransitionEnd = AzureNotificationCenter.OnWeatherTransitionEnd;
				if (onWeatherTransitionEnd == null)
				{
					return;
				}
				onWeatherTransitionEnd(azureWeatherSystem);
			}

			// Token: 0x06000297 RID: 663 RVA: 0x0000AD12 File Offset: 0x00008F12
			public static void OnVolumetricLightPreRenderCallback(AzureVolumetricLightRenderer renderer)
			{
				AzureNotificationCenter.VolumetriLightPreRenderDelegate onVolumetricLightPreRender = AzureNotificationCenter.OnVolumetricLightPreRender;
				if (onVolumetricLightPreRender == null)
				{
					return;
				}
				onVolumetricLightPreRender(renderer);
			}
		}

		// Token: 0x0200002F RID: 47
		// (Invoke) Token: 0x06000299 RID: 665
		public delegate void TimeSystemDelegate(AzureTimeSystem timeSystem);

		// Token: 0x02000030 RID: 48
		// (Invoke) Token: 0x0600029D RID: 669
		public delegate void WeatherSystemDelegate(AzureWeatherSystem azureWeatherSystem);

		// Token: 0x02000031 RID: 49
		// (Invoke) Token: 0x060002A1 RID: 673
		public delegate void VolumetriLightPreRenderDelegate(AzureVolumetricLightRenderer renderer);
	}
}
