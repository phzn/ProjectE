using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200005A RID: 90
	public class DebugDisplaySettingsRendering : IDebugDisplaySettingsData, IDebugDisplaySettingsQuery
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00014821 File Offset: 0x00012A21
		// (set) Token: 0x06000334 RID: 820 RVA: 0x00014829 File Offset: 0x00012A29
		public DebugWireframeMode wireframeMode
		{
			get
			{
				return this.m_WireframeMode;
			}
			set
			{
				this.m_WireframeMode = value;
				this.UpdateDebugSceneOverrideMode();
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00014838 File Offset: 0x00012A38
		// (set) Token: 0x06000336 RID: 822 RVA: 0x00014840 File Offset: 0x00012A40
		[Obsolete("overdraw has been deprecated. Use overdrawMode instead.", false)]
		public bool overdraw
		{
			get
			{
				return this.m_Overdraw;
			}
			set
			{
				this.m_Overdraw = value;
				this.UpdateDebugSceneOverrideMode();
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0001484F File Offset: 0x00012A4F
		// (set) Token: 0x06000338 RID: 824 RVA: 0x00014857 File Offset: 0x00012A57
		public DebugOverdrawMode overdrawMode
		{
			get
			{
				return this.m_OverdrawMode;
			}
			set
			{
				this.m_OverdrawMode = value;
				this.UpdateDebugSceneOverrideMode();
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00014866 File Offset: 0x00012A66
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0001486E File Offset: 0x00012A6E
		public int maxOverdrawCount { get; set; } = 10;

		// Token: 0x0600033B RID: 827 RVA: 0x00014878 File Offset: 0x00012A78
		private void UpdateDebugSceneOverrideMode()
		{
			switch (this.wireframeMode)
			{
			case DebugWireframeMode.Wireframe:
				this.sceneOverrideMode = DebugSceneOverrideMode.Wireframe;
				return;
			case DebugWireframeMode.SolidWireframe:
				this.sceneOverrideMode = DebugSceneOverrideMode.SolidWireframe;
				return;
			case DebugWireframeMode.ShadedWireframe:
				this.sceneOverrideMode = DebugSceneOverrideMode.ShadedWireframe;
				return;
			default:
				this.sceneOverrideMode = ((this.overdrawMode != DebugOverdrawMode.None) ? DebugSceneOverrideMode.Overdraw : DebugSceneOverrideMode.None);
				return;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000148CC File Offset: 0x00012ACC
		// (set) Token: 0x0600033D RID: 829 RVA: 0x000148D4 File Offset: 0x00012AD4
		public DebugFullScreenMode fullScreenDebugMode { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600033E RID: 830 RVA: 0x000148DD File Offset: 0x00012ADD
		// (set) Token: 0x0600033F RID: 831 RVA: 0x000148E5 File Offset: 0x00012AE5
		public int fullScreenDebugModeOutputSizeScreenPercent { get; set; } = 50;

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000340 RID: 832 RVA: 0x000148EE File Offset: 0x00012AEE
		// (set) Token: 0x06000341 RID: 833 RVA: 0x000148F6 File Offset: 0x00012AF6
		internal DebugSceneOverrideMode sceneOverrideMode { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000148FF File Offset: 0x00012AFF
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00014907 File Offset: 0x00012B07
		internal DebugMipInfoMode mipInfoMode { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00014910 File Offset: 0x00012B10
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00014918 File Offset: 0x00012B18
		public DebugPostProcessingMode postProcessingDebugMode { get; set; } = DebugPostProcessingMode.Auto;

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00014921 File Offset: 0x00012B21
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00014929 File Offset: 0x00012B29
		public bool enableMsaa { get; set; } = true;

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00014932 File Offset: 0x00012B32
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0001493A File Offset: 0x00012B3A
		public bool enableHDR { get; set; } = true;

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00014943 File Offset: 0x00012B43
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0001494B File Offset: 0x00012B4B
		public DebugDisplaySettingsRendering.TaaDebugMode taaDebugMode { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00014954 File Offset: 0x00012B54
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0001495C File Offset: 0x00012B5C
		public DebugValidationMode validationMode { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00014965 File Offset: 0x00012B65
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0001496D File Offset: 0x00012B6D
		public PixelValidationChannels validationChannels { get; set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00014976 File Offset: 0x00012B76
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0001497E File Offset: 0x00012B7E
		public float validationRangeMin { get; set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00014987 File Offset: 0x00012B87
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0001498F File Offset: 0x00012B8F
		public float validationRangeMax { get; set; } = 1f;

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00014998 File Offset: 0x00012B98
		public bool AreAnySettingsActive
		{
			get
			{
				return this.postProcessingDebugMode != DebugPostProcessingMode.Auto || this.fullScreenDebugMode != DebugFullScreenMode.None || this.sceneOverrideMode != DebugSceneOverrideMode.None || this.mipInfoMode != DebugMipInfoMode.None || this.validationMode != DebugValidationMode.None || !this.enableMsaa || !this.enableHDR || this.taaDebugMode > DebugDisplaySettingsRendering.TaaDebugMode.None;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000355 RID: 853 RVA: 0x000149E9 File Offset: 0x00012BE9
		public bool IsPostProcessingAllowed
		{
			get
			{
				return this.postProcessingDebugMode != DebugPostProcessingMode.Disabled && this.sceneOverrideMode == DebugSceneOverrideMode.None && this.mipInfoMode == DebugMipInfoMode.None;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00014A06 File Offset: 0x00012C06
		public bool IsLightingActive
		{
			get
			{
				return this.sceneOverrideMode == DebugSceneOverrideMode.None && this.mipInfoMode == DebugMipInfoMode.None;
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00014A1C File Offset: 0x00012C1C
		public bool TryGetScreenClearColor(ref Color color)
		{
			switch (this.sceneOverrideMode)
			{
			case DebugSceneOverrideMode.None:
			case DebugSceneOverrideMode.ShadedWireframe:
				return false;
			case DebugSceneOverrideMode.Overdraw:
				color = Color.black;
				return true;
			case DebugSceneOverrideMode.Wireframe:
			case DebugSceneOverrideMode.SolidWireframe:
				color = new Color(0.1f, 0.1f, 0.1f, 1f);
				return true;
			default:
				throw new ArgumentOutOfRangeException("color");
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00014A86 File Offset: 0x00012C86
		IDebugDisplaySettingsPanelDisposable IDebugDisplaySettingsData.CreatePanel()
		{
			return new DebugDisplaySettingsRendering.SettingsPanel(this);
		}

		// Token: 0x0400026B RID: 619
		private DebugWireframeMode m_WireframeMode;

		// Token: 0x0400026C RID: 620
		private bool m_Overdraw;

		// Token: 0x0400026D RID: 621
		private DebugOverdrawMode m_OverdrawMode;

		// Token: 0x02000175 RID: 373
		public enum TaaDebugMode
		{
			// Token: 0x040009F8 RID: 2552
			None,
			// Token: 0x040009F9 RID: 2553
			ShowRawFrame,
			// Token: 0x040009FA RID: 2554
			ShowRawFrameNoJitter,
			// Token: 0x040009FB RID: 2555
			ShowClampedHistory
		}

		// Token: 0x02000176 RID: 374
		private static class Strings
		{
			// Token: 0x040009FC RID: 2556
			public const string RangeValidationSettingsContainerName = "Pixel Range Settings";

			// Token: 0x040009FD RID: 2557
			public static readonly DebugUI.Widget.NameAndTooltip MapOverlays = new DebugUI.Widget.NameAndTooltip
			{
				name = "Map Overlays",
				tooltip = "Overlays render pipeline textures to validate the scene."
			};

			// Token: 0x040009FE RID: 2558
			public static readonly DebugUI.Widget.NameAndTooltip MapSize = new DebugUI.Widget.NameAndTooltip
			{
				name = "Map Size",
				tooltip = "Set the size of the render pipeline texture in the scene."
			};

			// Token: 0x040009FF RID: 2559
			public static readonly DebugUI.Widget.NameAndTooltip AdditionalWireframeModes = new DebugUI.Widget.NameAndTooltip
			{
				name = "Additional Wireframe Modes",
				tooltip = "Debug the scene with additional wireframe shader views that are different from those in the scene view."
			};

			// Token: 0x04000A00 RID: 2560
			public static readonly DebugUI.Widget.NameAndTooltip WireframeNotSupportedWarning = new DebugUI.Widget.NameAndTooltip
			{
				name = "Warning: This platform might not support wireframe rendering.",
				tooltip = "Some platforms, for example, mobile platforms using OpenGL ES and Vulkan, might not support wireframe rendering."
			};

			// Token: 0x04000A01 RID: 2561
			public static readonly DebugUI.Widget.NameAndTooltip OverdrawMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "Overdraw Mode",
				tooltip = "Debug anywhere materials that overdrawn pixels top of each other."
			};

			// Token: 0x04000A02 RID: 2562
			public static readonly DebugUI.Widget.NameAndTooltip MaxOverdrawCount = new DebugUI.Widget.NameAndTooltip
			{
				name = "Max Overdraw Count",
				tooltip = "Maximum overdraw count allowed for a single pixel."
			};

			// Token: 0x04000A03 RID: 2563
			public static readonly DebugUI.Widget.NameAndTooltip PostProcessing = new DebugUI.Widget.NameAndTooltip
			{
				name = "Post-processing",
				tooltip = "Override the controls for Post Processing in the scene."
			};

			// Token: 0x04000A04 RID: 2564
			public static readonly DebugUI.Widget.NameAndTooltip MSAA = new DebugUI.Widget.NameAndTooltip
			{
				name = "MSAA",
				tooltip = "Use the checkbox to disable MSAA in the scene."
			};

			// Token: 0x04000A05 RID: 2565
			public static readonly DebugUI.Widget.NameAndTooltip HDR = new DebugUI.Widget.NameAndTooltip
			{
				name = "HDR",
				tooltip = "Use the checkbox to disable High Dynamic Range in the scene."
			};

			// Token: 0x04000A06 RID: 2566
			public static readonly DebugUI.Widget.NameAndTooltip TaaDebugMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "TAA Debug Mode",
				tooltip = "Choose whether to force TAA to output the raw jittered frame or clamped reprojected history."
			};

			// Token: 0x04000A07 RID: 2567
			public static readonly DebugUI.Widget.NameAndTooltip PixelValidationMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "Pixel Validation Mode",
				tooltip = "Choose between modes that validate pixel on screen."
			};

			// Token: 0x04000A08 RID: 2568
			public static readonly DebugUI.Widget.NameAndTooltip Channels = new DebugUI.Widget.NameAndTooltip
			{
				name = "Channels",
				tooltip = "Choose the texture channel used to validate the scene."
			};

			// Token: 0x04000A09 RID: 2569
			public static readonly DebugUI.Widget.NameAndTooltip ValueRangeMin = new DebugUI.Widget.NameAndTooltip
			{
				name = "Value Range Min",
				tooltip = "Any values set below this field will be considered invalid and will appear red on screen."
			};

			// Token: 0x04000A0A RID: 2570
			public static readonly DebugUI.Widget.NameAndTooltip ValueRangeMax = new DebugUI.Widget.NameAndTooltip
			{
				name = "Value Range Max",
				tooltip = "Any values set above this field will be considered invalid and will appear blue on screen."
			};
		}

		// Token: 0x02000177 RID: 375
		internal static class WidgetFactory
		{
			// Token: 0x06000A82 RID: 2690 RVA: 0x00048114 File Offset: 0x00046314
			internal static DebugUI.Widget CreateMapOverlays(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.MapOverlays,
					autoEnum = typeof(DebugFullScreenMode),
					getter = () => (int)panel.data.fullScreenDebugMode,
					setter = delegate(int value)
					{
						panel.data.fullScreenDebugMode = (DebugFullScreenMode)value;
					},
					getIndex = () => (int)panel.data.fullScreenDebugMode,
					setIndex = delegate(int value)
					{
						panel.data.fullScreenDebugMode = (DebugFullScreenMode)value;
					}
				};
			}

			// Token: 0x06000A83 RID: 2691 RVA: 0x00048198 File Offset: 0x00046398
			internal static DebugUI.Widget CreateMapOverlaySize(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				DebugUI.Container container = new DebugUI.Container();
				ObservableList<DebugUI.Widget> children = container.children;
				DebugUI.IntField intField = new DebugUI.IntField();
				intField.nameAndTooltip = DebugDisplaySettingsRendering.Strings.MapSize;
				intField.getter = () => panel.data.fullScreenDebugModeOutputSizeScreenPercent;
				intField.setter = delegate(int value)
				{
					panel.data.fullScreenDebugModeOutputSizeScreenPercent = value;
				};
				intField.incStep = 10;
				intField.min = () => 0;
				intField.max = () => 100;
				children.Add(intField);
				return container;
			}

			// Token: 0x06000A84 RID: 2692 RVA: 0x00048248 File Offset: 0x00046448
			internal static DebugUI.Widget CreateAdditionalWireframeShaderViews(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				DebugUI.EnumField enumField = new DebugUI.EnumField();
				enumField.nameAndTooltip = DebugDisplaySettingsRendering.Strings.AdditionalWireframeModes;
				enumField.autoEnum = typeof(DebugWireframeMode);
				enumField.getter = () => (int)panel.data.wireframeMode;
				enumField.setter = delegate(int value)
				{
					panel.data.wireframeMode = (DebugWireframeMode)value;
				};
				enumField.getIndex = () => (int)panel.data.wireframeMode;
				enumField.setIndex = delegate(int value)
				{
					panel.data.wireframeMode = (DebugWireframeMode)value;
				};
				enumField.onValueChanged = delegate(DebugUI.Field<int> _, int _)
				{
					DebugManager.instance.ReDrawOnScreenDebug();
				};
				return enumField;
			}

			// Token: 0x06000A85 RID: 2693 RVA: 0x000482F0 File Offset: 0x000464F0
			internal static DebugUI.Widget CreateWireframeNotSupportedWarning(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.MessageBox
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.WireframeNotSupportedWarning,
					style = DebugUI.MessageBox.Style.Warning,
					isHiddenCallback = delegate
					{
						GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
						return (graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && graphicsDeviceType != GraphicsDeviceType.Vulkan) || panel.data.wireframeMode == DebugWireframeMode.None;
					}
				};
			}

			// Token: 0x06000A86 RID: 2694 RVA: 0x00048334 File Offset: 0x00046534
			internal static DebugUI.Widget CreateOverdrawMode(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.OverdrawMode,
					autoEnum = typeof(DebugOverdrawMode),
					getter = () => (int)panel.data.overdrawMode,
					setter = delegate(int value)
					{
						panel.data.overdrawMode = (DebugOverdrawMode)value;
					},
					getIndex = () => (int)panel.data.overdrawMode,
					setIndex = delegate(int value)
					{
						panel.data.overdrawMode = (DebugOverdrawMode)value;
					}
				};
			}

			// Token: 0x06000A87 RID: 2695 RVA: 0x000483B8 File Offset: 0x000465B8
			internal static DebugUI.Widget CreateMaxOverdrawCount(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				DebugUI.Container container = new DebugUI.Container();
				container.isHiddenCallback = () => panel.data.overdrawMode == DebugOverdrawMode.None;
				ObservableList<DebugUI.Widget> children = container.children;
				DebugUI.IntField intField = new DebugUI.IntField();
				intField.nameAndTooltip = DebugDisplaySettingsRendering.Strings.MaxOverdrawCount;
				intField.getter = () => panel.data.maxOverdrawCount;
				intField.setter = delegate(int value)
				{
					panel.data.maxOverdrawCount = value;
				};
				intField.incStep = 10;
				intField.min = () => 1;
				intField.max = () => 500;
				children.Add(intField);
				return container;
			}

			// Token: 0x06000A88 RID: 2696 RVA: 0x0004847C File Offset: 0x0004667C
			internal static DebugUI.Widget CreatePostProcessing(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.PostProcessing,
					autoEnum = typeof(DebugPostProcessingMode),
					getter = () => (int)panel.data.postProcessingDebugMode,
					setter = delegate(int value)
					{
						panel.data.postProcessingDebugMode = (DebugPostProcessingMode)value;
					},
					getIndex = () => (int)panel.data.postProcessingDebugMode,
					setIndex = delegate(int value)
					{
						panel.data.postProcessingDebugMode = (DebugPostProcessingMode)value;
					}
				};
			}

			// Token: 0x06000A89 RID: 2697 RVA: 0x00048500 File Offset: 0x00046700
			internal static DebugUI.Widget CreateMSAA(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.BoolField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.MSAA,
					getter = () => panel.data.enableMsaa,
					setter = delegate(bool value)
					{
						panel.data.enableMsaa = value;
					}
				};
			}

			// Token: 0x06000A8A RID: 2698 RVA: 0x00048550 File Offset: 0x00046750
			internal static DebugUI.Widget CreateHDR(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.BoolField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.HDR,
					getter = () => panel.data.enableHDR,
					setter = delegate(bool value)
					{
						panel.data.enableHDR = value;
					}
				};
			}

			// Token: 0x06000A8B RID: 2699 RVA: 0x000485A0 File Offset: 0x000467A0
			internal static DebugUI.Widget CreateTaaDebugMode(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				DebugUI.EnumField enumField = new DebugUI.EnumField();
				enumField.nameAndTooltip = DebugDisplaySettingsRendering.Strings.TaaDebugMode;
				enumField.autoEnum = typeof(DebugDisplaySettingsRendering.TaaDebugMode);
				enumField.getter = () => (int)panel.data.taaDebugMode;
				enumField.setter = delegate(int value)
				{
					panel.data.taaDebugMode = (DebugDisplaySettingsRendering.TaaDebugMode)value;
				};
				enumField.getIndex = () => (int)panel.data.taaDebugMode;
				enumField.setIndex = delegate(int value)
				{
					panel.data.taaDebugMode = (DebugDisplaySettingsRendering.TaaDebugMode)value;
				};
				enumField.onValueChanged = delegate(DebugUI.Field<int> _, int _)
				{
					DebugManager.instance.ReDrawOnScreenDebug();
				};
				return enumField;
			}

			// Token: 0x06000A8C RID: 2700 RVA: 0x00048648 File Offset: 0x00046848
			internal static DebugUI.Widget CreatePixelValidationMode(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				DebugUI.EnumField enumField = new DebugUI.EnumField();
				enumField.nameAndTooltip = DebugDisplaySettingsRendering.Strings.PixelValidationMode;
				enumField.autoEnum = typeof(DebugValidationMode);
				enumField.getter = () => (int)panel.data.validationMode;
				enumField.setter = delegate(int value)
				{
					panel.data.validationMode = (DebugValidationMode)value;
				};
				enumField.getIndex = () => (int)panel.data.validationMode;
				enumField.setIndex = delegate(int value)
				{
					panel.data.validationMode = (DebugValidationMode)value;
				};
				enumField.onValueChanged = delegate(DebugUI.Field<int> _, int _)
				{
					DebugManager.instance.ReDrawOnScreenDebug();
				};
				return enumField;
			}

			// Token: 0x06000A8D RID: 2701 RVA: 0x000486F0 File Offset: 0x000468F0
			internal static DebugUI.Widget CreatePixelValidationChannels(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.Channels,
					autoEnum = typeof(PixelValidationChannels),
					getter = () => (int)panel.data.validationChannels,
					setter = delegate(int value)
					{
						panel.data.validationChannels = (PixelValidationChannels)value;
					},
					getIndex = () => (int)panel.data.validationChannels,
					setIndex = delegate(int value)
					{
						panel.data.validationChannels = (PixelValidationChannels)value;
					}
				};
			}

			// Token: 0x06000A8E RID: 2702 RVA: 0x00048774 File Offset: 0x00046974
			internal static DebugUI.Widget CreatePixelValueRangeMin(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.ValueRangeMin,
					getter = () => panel.data.validationRangeMin,
					setter = delegate(float value)
					{
						panel.data.validationRangeMin = value;
					},
					incStep = 0.01f
				};
			}

			// Token: 0x06000A8F RID: 2703 RVA: 0x000487D0 File Offset: 0x000469D0
			internal static DebugUI.Widget CreatePixelValueRangeMax(DebugDisplaySettingsRendering.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsRendering.Strings.ValueRangeMax,
					getter = () => panel.data.validationRangeMax,
					setter = delegate(float value)
					{
						panel.data.validationRangeMax = value;
					},
					incStep = 0.01f
				};
			}
		}

		// Token: 0x02000178 RID: 376
		[DisplayInfo(name = "Rendering", order = 1)]
		internal class SettingsPanel : DebugDisplaySettingsPanel<DebugDisplaySettingsRendering>
		{
			// Token: 0x06000A90 RID: 2704 RVA: 0x0004882C File Offset: 0x00046A2C
			public SettingsPanel(DebugDisplaySettingsRendering data)
				: base(data)
			{
				base.AddWidget(DebugDisplaySettingsCommon.WidgetFactory.CreateMissingDebugShadersWarning());
				base.AddWidget(new DebugUI.Foldout
				{
					displayName = "Rendering Debug",
					flags = DebugUI.Flags.FrequentlyUsed,
					isHeader = true,
					opened = true,
					children = 
					{
						DebugDisplaySettingsRendering.WidgetFactory.CreateMapOverlays(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateMapOverlaySize(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateHDR(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateMSAA(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateTaaDebugMode(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreatePostProcessing(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateAdditionalWireframeShaderViews(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateWireframeNotSupportedWarning(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateOverdrawMode(this),
						DebugDisplaySettingsRendering.WidgetFactory.CreateMaxOverdrawCount(this)
					}
				});
				base.AddWidget(new DebugUI.Foldout
				{
					displayName = "Pixel Validation",
					isHeader = true,
					opened = true,
					children = 
					{
						DebugDisplaySettingsRendering.WidgetFactory.CreatePixelValidationMode(this),
						new DebugUI.Container
						{
							displayName = "Pixel Range Settings",
							isHiddenCallback = () => data.validationMode != DebugValidationMode.HighlightOutsideOfRange,
							children = 
							{
								DebugDisplaySettingsRendering.WidgetFactory.CreatePixelValidationChannels(this),
								DebugDisplaySettingsRendering.WidgetFactory.CreatePixelValueRangeMin(this),
								DebugDisplaySettingsRendering.WidgetFactory.CreatePixelValueRangeMax(this)
							}
						}
					}
				});
			}
		}
	}
}
