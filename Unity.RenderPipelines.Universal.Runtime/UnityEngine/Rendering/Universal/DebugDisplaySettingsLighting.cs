using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000057 RID: 87
	public class DebugDisplaySettingsLighting : IDebugDisplaySettingsData, IDebugDisplaySettingsQuery
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0001409A File Offset: 0x0001229A
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000140A2 File Offset: 0x000122A2
		public DebugLightingMode lightingDebugMode { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000140AB File Offset: 0x000122AB
		// (set) Token: 0x0600030E RID: 782 RVA: 0x000140B3 File Offset: 0x000122B3
		public DebugLightingFeatureFlags lightingFeatureFlags { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000140BC File Offset: 0x000122BC
		// (set) Token: 0x06000310 RID: 784 RVA: 0x000140C4 File Offset: 0x000122C4
		public HDRDebugMode hdrDebugMode { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000140CD File Offset: 0x000122CD
		public bool AreAnySettingsActive
		{
			get
			{
				return this.lightingDebugMode != DebugLightingMode.None || this.lightingFeatureFlags != DebugLightingFeatureFlags.None || this.hdrDebugMode > HDRDebugMode.None;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000140EA File Offset: 0x000122EA
		public bool IsPostProcessingAllowed
		{
			get
			{
				return this.lightingDebugMode != DebugLightingMode.Reflections && this.lightingDebugMode != DebugLightingMode.ReflectionsWithSmoothness;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00014103 File Offset: 0x00012303
		public bool IsLightingActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00014106 File Offset: 0x00012306
		public bool TryGetScreenClearColor(ref Color color)
		{
			return false;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00014109 File Offset: 0x00012309
		IDebugDisplaySettingsPanelDisposable IDebugDisplaySettingsData.CreatePanel()
		{
			return new DebugDisplaySettingsLighting.SettingsPanel(this);
		}

		// Token: 0x0200016D RID: 365
		internal static class Strings
		{
			// Token: 0x040009D3 RID: 2515
			public static readonly DebugUI.Widget.NameAndTooltip LightingDebugMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "Lighting Debug Mode",
				tooltip = "Use the drop-down to select which lighting and shadow debug information to overlay on the screen."
			};

			// Token: 0x040009D4 RID: 2516
			public static readonly DebugUI.Widget.NameAndTooltip LightingFeatures = new DebugUI.Widget.NameAndTooltip
			{
				name = "Lighting Features",
				tooltip = "Filter and debug selected lighting features in the system."
			};

			// Token: 0x040009D5 RID: 2517
			public static readonly DebugUI.Widget.NameAndTooltip HDRDebugMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "HDR Debug Mode",
				tooltip = "Select which HDR brightness debug information to overlay on the screen."
			};
		}

		// Token: 0x0200016E RID: 366
		internal static class WidgetFactory
		{
			// Token: 0x06000A70 RID: 2672 RVA: 0x000474C0 File Offset: 0x000456C0
			internal static DebugUI.Widget CreateLightingDebugMode(DebugDisplaySettingsLighting.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsLighting.Strings.LightingDebugMode,
					autoEnum = typeof(DebugLightingMode),
					getter = () => (int)panel.data.lightingDebugMode,
					setter = delegate(int value)
					{
						panel.data.lightingDebugMode = (DebugLightingMode)value;
					},
					getIndex = () => (int)panel.data.lightingDebugMode,
					setIndex = delegate(int value)
					{
						panel.data.lightingDebugMode = (DebugLightingMode)value;
					}
				};
			}

			// Token: 0x06000A71 RID: 2673 RVA: 0x00047544 File Offset: 0x00045744
			internal static DebugUI.Widget CreateLightingFeatures(DebugDisplaySettingsLighting.SettingsPanel panel)
			{
				return new DebugUI.BitField
				{
					nameAndTooltip = DebugDisplaySettingsLighting.Strings.LightingFeatures,
					getter = () => panel.data.lightingFeatureFlags,
					setter = delegate(Enum value)
					{
						panel.data.lightingFeatureFlags = (DebugLightingFeatureFlags)value;
					},
					enumType = typeof(DebugLightingFeatureFlags)
				};
			}

			// Token: 0x06000A72 RID: 2674 RVA: 0x000475A4 File Offset: 0x000457A4
			internal static DebugUI.Widget CreateHDRDebugMode(DebugDisplaySettingsLighting.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsLighting.Strings.HDRDebugMode,
					autoEnum = typeof(HDRDebugMode),
					getter = () => (int)panel.data.hdrDebugMode,
					setter = delegate(int value)
					{
						panel.data.hdrDebugMode = (HDRDebugMode)value;
					},
					getIndex = () => (int)panel.data.hdrDebugMode,
					setIndex = delegate(int value)
					{
						panel.data.hdrDebugMode = (HDRDebugMode)value;
					}
				};
			}
		}

		// Token: 0x0200016F RID: 367
		[DisplayInfo(name = "Lighting", order = 3)]
		internal class SettingsPanel : DebugDisplaySettingsPanel<DebugDisplaySettingsLighting>
		{
			// Token: 0x06000A73 RID: 2675 RVA: 0x00047628 File Offset: 0x00045828
			public SettingsPanel(DebugDisplaySettingsLighting data)
				: base(data)
			{
				base.AddWidget(DebugDisplaySettingsCommon.WidgetFactory.CreateMissingDebugShadersWarning());
				base.AddWidget(new DebugUI.Foldout
				{
					displayName = "Lighting Debug Modes",
					flags = DebugUI.Flags.FrequentlyUsed,
					isHeader = true,
					opened = true,
					children = 
					{
						DebugDisplaySettingsLighting.WidgetFactory.CreateLightingDebugMode(this),
						DebugDisplaySettingsLighting.WidgetFactory.CreateHDRDebugMode(this),
						DebugDisplaySettingsLighting.WidgetFactory.CreateLightingFeatures(this)
					}
				});
			}
		}
	}
}
