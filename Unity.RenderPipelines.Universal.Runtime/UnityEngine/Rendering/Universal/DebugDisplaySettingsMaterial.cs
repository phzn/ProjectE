using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000058 RID: 88
	public class DebugDisplaySettingsMaterial : IDebugDisplaySettingsData, IDebugDisplaySettingsQuery
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00014119 File Offset: 0x00012319
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00014124 File Offset: 0x00012324
		public DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset albedoValidationPreset
		{
			get
			{
				return this.m_AlbedoValidationPreset;
			}
			set
			{
				this.m_AlbedoValidationPreset = value;
				DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData albedoDebugValidationPresetData = this.m_AlbedoDebugValidationPresetData[(int)value];
				this.albedoMinLuminance = albedoDebugValidationPresetData.minLuminance;
				this.albedoMaxLuminance = albedoDebugValidationPresetData.maxLuminance;
				this.albedoCompareColor = albedoDebugValidationPresetData.color;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00014169 File Offset: 0x00012369
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00014171 File Offset: 0x00012371
		public float albedoMinLuminance { get; set; } = 0.01f;

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0001417A File Offset: 0x0001237A
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00014182 File Offset: 0x00012382
		public float albedoMaxLuminance { get; set; } = 0.9f;

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0001418B File Offset: 0x0001238B
		// (set) Token: 0x0600031E RID: 798 RVA: 0x000141A1 File Offset: 0x000123A1
		public float albedoHueTolerance
		{
			get
			{
				if (this.m_AlbedoValidationPreset != DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset.DefaultLuminance)
				{
					return this.m_AlbedoHueTolerance;
				}
				return 1f;
			}
			set
			{
				this.m_AlbedoHueTolerance = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000141AA File Offset: 0x000123AA
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000141C0 File Offset: 0x000123C0
		public float albedoSaturationTolerance
		{
			get
			{
				if (this.m_AlbedoValidationPreset != DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset.DefaultLuminance)
				{
					return this.m_AlbedoSaturationTolerance;
				}
				return 1f;
			}
			set
			{
				this.m_AlbedoSaturationTolerance = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000321 RID: 801 RVA: 0x000141C9 File Offset: 0x000123C9
		// (set) Token: 0x06000322 RID: 802 RVA: 0x000141D1 File Offset: 0x000123D1
		public Color albedoCompareColor { get; set; } = new Color(0.49803922f, 0.49803922f, 0.49803922f, 1f);

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000323 RID: 803 RVA: 0x000141DA File Offset: 0x000123DA
		// (set) Token: 0x06000324 RID: 804 RVA: 0x000141E2 File Offset: 0x000123E2
		public float metallicMinValue { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000141EB File Offset: 0x000123EB
		// (set) Token: 0x06000326 RID: 806 RVA: 0x000141F3 File Offset: 0x000123F3
		public float metallicMaxValue { get; set; } = 0.9f;

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000327 RID: 807 RVA: 0x000141FC File Offset: 0x000123FC
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00014204 File Offset: 0x00012404
		public DebugMaterialValidationMode materialValidationMode { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0001420D File Offset: 0x0001240D
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00014215 File Offset: 0x00012415
		public DebugMaterialMode materialDebugMode { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0001421E File Offset: 0x0001241E
		// (set) Token: 0x0600032C RID: 812 RVA: 0x00014226 File Offset: 0x00012426
		public DebugVertexAttributeMode vertexAttributeDebugMode { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0001422F File Offset: 0x0001242F
		public bool AreAnySettingsActive
		{
			get
			{
				return this.materialDebugMode != DebugMaterialMode.None || this.vertexAttributeDebugMode != DebugVertexAttributeMode.None || this.materialValidationMode > DebugMaterialValidationMode.None;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001424C File Offset: 0x0001244C
		public bool IsPostProcessingAllowed
		{
			get
			{
				return !this.AreAnySettingsActive;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00014257 File Offset: 0x00012457
		public bool IsLightingActive
		{
			get
			{
				return !this.AreAnySettingsActive;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00014262 File Offset: 0x00012462
		public bool TryGetScreenClearColor(ref Color color)
		{
			return false;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00014265 File Offset: 0x00012465
		IDebugDisplaySettingsPanelDisposable IDebugDisplaySettingsData.CreatePanel()
		{
			return new DebugDisplaySettingsMaterial.SettingsPanel(this);
		}

		// Token: 0x0400025A RID: 602
		private DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData[] m_AlbedoDebugValidationPresetData = new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData[]
		{
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Default Luminance",
				color = new Color(0.49803922f, 0.49803922f, 0.49803922f),
				minLuminance = 0.01f,
				maxLuminance = 0.9f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Black Acrylic Paint",
				color = new Color(0.21960784f, 0.21960784f, 0.21960784f),
				minLuminance = 0.03f,
				maxLuminance = 0.07f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Dark Soil",
				color = new Color(0.33333334f, 0.23921569f, 0.19215687f),
				minLuminance = 0.05f,
				maxLuminance = 0.14f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Worn Asphalt",
				color = new Color(0.35686275f, 0.35686275f, 0.35686275f),
				minLuminance = 0.1f,
				maxLuminance = 0.15f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Dry Clay Soil",
				color = new Color(0.5372549f, 0.47058824f, 0.4f),
				minLuminance = 0.15f,
				maxLuminance = 0.35f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Green Grass",
				color = new Color(0.48235294f, 0.5137255f, 0.2901961f),
				minLuminance = 0.16f,
				maxLuminance = 0.26f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Old Concrete",
				color = new Color(0.5294118f, 0.53333336f, 0.5137255f),
				minLuminance = 0.17f,
				maxLuminance = 0.3f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Red Clay Tile",
				color = new Color(0.77254903f, 0.49019608f, 0.39215687f),
				minLuminance = 0.23f,
				maxLuminance = 0.33f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Dry Sand",
				color = new Color(0.69411767f, 0.654902f, 0.5176471f),
				minLuminance = 0.2f,
				maxLuminance = 0.45f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "New Concrete",
				color = new Color(0.7254902f, 0.7137255f, 0.6862745f),
				minLuminance = 0.32f,
				maxLuminance = 0.55f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "White Acrylic Paint",
				color = new Color(0.8901961f, 0.8901961f, 0.8901961f),
				minLuminance = 0.75f,
				maxLuminance = 0.85f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Fresh Snow",
				color = new Color(0.9529412f, 0.9529412f, 0.9529412f),
				minLuminance = 0.85f,
				maxLuminance = 0.95f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Blue Sky",
				color = new Color(0.3647059f, 0.48235294f, 0.6156863f),
				minLuminance = new Color(0.3647059f, 0.48235294f, 0.6156863f).linear.maxColorComponent - 0.05f,
				maxLuminance = new Color(0.3647059f, 0.48235294f, 0.6156863f).linear.maxColorComponent + 0.05f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Foliage",
				color = new Color(0.35686275f, 0.42352942f, 0.25490198f),
				minLuminance = new Color(0.35686275f, 0.42352942f, 0.25490198f).linear.maxColorComponent - 0.05f,
				maxLuminance = new Color(0.35686275f, 0.42352942f, 0.25490198f).linear.maxColorComponent + 0.05f
			},
			new DebugDisplaySettingsMaterial.AlbedoDebugValidationPresetData
			{
				name = "Custom",
				color = new Color(0.49803922f, 0.49803922f, 0.49803922f),
				minLuminance = 0.01f,
				maxLuminance = 0.9f
			}
		};

		// Token: 0x0400025B RID: 603
		private DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset m_AlbedoValidationPreset;

		// Token: 0x0400025E RID: 606
		private float m_AlbedoHueTolerance = 0.104f;

		// Token: 0x0400025F RID: 607
		private float m_AlbedoSaturationTolerance = 0.214f;

		// Token: 0x02000170 RID: 368
		public enum AlbedoDebugValidationPreset
		{
			// Token: 0x040009D7 RID: 2519
			DefaultLuminance,
			// Token: 0x040009D8 RID: 2520
			BlackAcrylicPaint,
			// Token: 0x040009D9 RID: 2521
			DarkSoil,
			// Token: 0x040009DA RID: 2522
			WornAsphalt,
			// Token: 0x040009DB RID: 2523
			DryClaySoil,
			// Token: 0x040009DC RID: 2524
			GreenGrass,
			// Token: 0x040009DD RID: 2525
			OldConcrete,
			// Token: 0x040009DE RID: 2526
			RedClayTile,
			// Token: 0x040009DF RID: 2527
			DrySand,
			// Token: 0x040009E0 RID: 2528
			NewConcrete,
			// Token: 0x040009E1 RID: 2529
			WhiteAcrylicPaint,
			// Token: 0x040009E2 RID: 2530
			FreshSnow,
			// Token: 0x040009E3 RID: 2531
			BlueSky,
			// Token: 0x040009E4 RID: 2532
			Foliage,
			// Token: 0x040009E5 RID: 2533
			Custom
		}

		// Token: 0x02000171 RID: 369
		private struct AlbedoDebugValidationPresetData
		{
			// Token: 0x040009E6 RID: 2534
			public string name;

			// Token: 0x040009E7 RID: 2535
			public Color color;

			// Token: 0x040009E8 RID: 2536
			public float minLuminance;

			// Token: 0x040009E9 RID: 2537
			public float maxLuminance;
		}

		// Token: 0x02000172 RID: 370
		private static class Strings
		{
			// Token: 0x040009EA RID: 2538
			public const string AlbedoSettingsContainerName = "Albedo Settings";

			// Token: 0x040009EB RID: 2539
			public const string MetallicSettingsContainerName = "Metallic Settings";

			// Token: 0x040009EC RID: 2540
			public static readonly DebugUI.Widget.NameAndTooltip MaterialOverride = new DebugUI.Widget.NameAndTooltip
			{
				name = "Material Override",
				tooltip = "Use the drop-down to select a Material property to visualize on every GameObject on screen."
			};

			// Token: 0x040009ED RID: 2541
			public static readonly DebugUI.Widget.NameAndTooltip VertexAttribute = new DebugUI.Widget.NameAndTooltip
			{
				name = "Vertex Attribute",
				tooltip = "Use the drop-down to select a 3D GameObject attribute, like Texture Coordinates or Vertex Color, to visualize on screen."
			};

			// Token: 0x040009EE RID: 2542
			public static readonly DebugUI.Widget.NameAndTooltip MaterialValidationMode = new DebugUI.Widget.NameAndTooltip
			{
				name = "Material Validation Mode",
				tooltip = "Debug and validate material properties."
			};

			// Token: 0x040009EF RID: 2543
			public static readonly DebugUI.Widget.NameAndTooltip ValidationPreset = new DebugUI.Widget.NameAndTooltip
			{
				name = "Validation Preset",
				tooltip = "Validate using a list of preset surfaces and inputs based on real-world surfaces."
			};

			// Token: 0x040009F0 RID: 2544
			public static readonly DebugUI.Widget.NameAndTooltip AlbedoCustomColor = new DebugUI.Widget.NameAndTooltip
			{
				name = "Target Color",
				tooltip = "Custom target color for albedo validation."
			};

			// Token: 0x040009F1 RID: 2545
			public static readonly DebugUI.Widget.NameAndTooltip AlbedoMinLuminance = new DebugUI.Widget.NameAndTooltip
			{
				name = "Min Luminance",
				tooltip = "Any values set below this field are invalid and appear red on screen."
			};

			// Token: 0x040009F2 RID: 2546
			public static readonly DebugUI.Widget.NameAndTooltip AlbedoMaxLuminance = new DebugUI.Widget.NameAndTooltip
			{
				name = "Max Luminance",
				tooltip = "Any values set above this field are invalid and appear blue on screen."
			};

			// Token: 0x040009F3 RID: 2547
			public static readonly DebugUI.Widget.NameAndTooltip AlbedoHueTolerance = new DebugUI.Widget.NameAndTooltip
			{
				name = "Hue Tolerance",
				tooltip = "Validate a material based on a specific hue."
			};

			// Token: 0x040009F4 RID: 2548
			public static readonly DebugUI.Widget.NameAndTooltip AlbedoSaturationTolerance = new DebugUI.Widget.NameAndTooltip
			{
				name = "Saturation Tolerance",
				tooltip = "Validate a material based on a specific Saturation."
			};

			// Token: 0x040009F5 RID: 2549
			public static readonly DebugUI.Widget.NameAndTooltip MetallicMinValue = new DebugUI.Widget.NameAndTooltip
			{
				name = "Min Value",
				tooltip = "Any values set below this field are invalid and appear red on screen."
			};

			// Token: 0x040009F6 RID: 2550
			public static readonly DebugUI.Widget.NameAndTooltip MetallicMaxValue = new DebugUI.Widget.NameAndTooltip
			{
				name = "Max Value",
				tooltip = "Any values set above this field are invalid and appear blue on screen."
			};
		}

		// Token: 0x02000173 RID: 371
		internal static class WidgetFactory
		{
			// Token: 0x06000A75 RID: 2677 RVA: 0x00047858 File Offset: 0x00045A58
			internal static DebugUI.Widget CreateMaterialOverride(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.MaterialOverride,
					autoEnum = typeof(DebugMaterialMode),
					getter = () => (int)panel.data.materialDebugMode,
					setter = delegate(int value)
					{
						panel.data.materialDebugMode = (DebugMaterialMode)value;
					},
					getIndex = () => (int)panel.data.materialDebugMode,
					setIndex = delegate(int value)
					{
						panel.data.materialDebugMode = (DebugMaterialMode)value;
					}
				};
			}

			// Token: 0x06000A76 RID: 2678 RVA: 0x000478DC File Offset: 0x00045ADC
			internal static DebugUI.Widget CreateVertexAttribute(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.EnumField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.VertexAttribute,
					autoEnum = typeof(DebugVertexAttributeMode),
					getter = () => (int)panel.data.vertexAttributeDebugMode,
					setter = delegate(int value)
					{
						panel.data.vertexAttributeDebugMode = (DebugVertexAttributeMode)value;
					},
					getIndex = () => (int)panel.data.vertexAttributeDebugMode,
					setIndex = delegate(int value)
					{
						panel.data.vertexAttributeDebugMode = (DebugVertexAttributeMode)value;
					}
				};
			}

			// Token: 0x06000A77 RID: 2679 RVA: 0x00047960 File Offset: 0x00045B60
			internal static DebugUI.Widget CreateMaterialValidationMode(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				DebugUI.EnumField enumField = new DebugUI.EnumField();
				enumField.nameAndTooltip = DebugDisplaySettingsMaterial.Strings.MaterialValidationMode;
				enumField.autoEnum = typeof(DebugMaterialValidationMode);
				enumField.getter = () => (int)panel.data.materialValidationMode;
				enumField.setter = delegate(int value)
				{
					panel.data.materialValidationMode = (DebugMaterialValidationMode)value;
				};
				enumField.getIndex = () => (int)panel.data.materialValidationMode;
				enumField.setIndex = delegate(int value)
				{
					panel.data.materialValidationMode = (DebugMaterialValidationMode)value;
				};
				enumField.onValueChanged = delegate(DebugUI.Field<int> _, int _)
				{
					DebugManager.instance.ReDrawOnScreenDebug();
				};
				return enumField;
			}

			// Token: 0x06000A78 RID: 2680 RVA: 0x00047A08 File Offset: 0x00045C08
			internal static DebugUI.Widget CreateAlbedoPreset(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				DebugUI.EnumField enumField = new DebugUI.EnumField();
				enumField.nameAndTooltip = DebugDisplaySettingsMaterial.Strings.ValidationPreset;
				enumField.autoEnum = typeof(DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset);
				enumField.getter = () => (int)panel.data.albedoValidationPreset;
				enumField.setter = delegate(int value)
				{
					panel.data.albedoValidationPreset = (DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset)value;
				};
				enumField.getIndex = () => (int)panel.data.albedoValidationPreset;
				enumField.setIndex = delegate(int value)
				{
					panel.data.albedoValidationPreset = (DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset)value;
				};
				enumField.onValueChanged = delegate(DebugUI.Field<int> _, int _)
				{
					DebugManager.instance.ReDrawOnScreenDebug();
				};
				return enumField;
			}

			// Token: 0x06000A79 RID: 2681 RVA: 0x00047AB0 File Offset: 0x00045CB0
			internal static DebugUI.Widget CreateAlbedoCustomColor(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.ColorField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.AlbedoCustomColor,
					getter = () => panel.data.albedoCompareColor,
					setter = delegate(Color value)
					{
						panel.data.albedoCompareColor = value;
					},
					isHiddenCallback = () => panel.data.albedoValidationPreset != DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset.Custom
				};
			}

			// Token: 0x06000A7A RID: 2682 RVA: 0x00047B10 File Offset: 0x00045D10
			internal static DebugUI.Widget CreateAlbedoMinLuminance(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.AlbedoMinLuminance,
					getter = () => panel.data.albedoMinLuminance,
					setter = delegate(float value)
					{
						panel.data.albedoMinLuminance = value;
					},
					incStep = 0.01f
				};
			}

			// Token: 0x06000A7B RID: 2683 RVA: 0x00047B6C File Offset: 0x00045D6C
			internal static DebugUI.Widget CreateAlbedoMaxLuminance(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.AlbedoMaxLuminance,
					getter = () => panel.data.albedoMaxLuminance,
					setter = delegate(float value)
					{
						panel.data.albedoMaxLuminance = value;
					},
					incStep = 0.01f
				};
			}

			// Token: 0x06000A7C RID: 2684 RVA: 0x00047BC8 File Offset: 0x00045DC8
			internal static DebugUI.Widget CreateAlbedoHueTolerance(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.AlbedoHueTolerance,
					getter = () => panel.data.albedoHueTolerance,
					setter = delegate(float value)
					{
						panel.data.albedoHueTolerance = value;
					},
					incStep = 0.01f,
					isHiddenCallback = () => panel.data.albedoValidationPreset == DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset.DefaultLuminance
				};
			}

			// Token: 0x06000A7D RID: 2685 RVA: 0x00047C34 File Offset: 0x00045E34
			internal static DebugUI.Widget CreateAlbedoSaturationTolerance(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.AlbedoSaturationTolerance,
					getter = () => panel.data.albedoSaturationTolerance,
					setter = delegate(float value)
					{
						panel.data.albedoSaturationTolerance = value;
					},
					incStep = 0.01f,
					isHiddenCallback = () => panel.data.albedoValidationPreset == DebugDisplaySettingsMaterial.AlbedoDebugValidationPreset.DefaultLuminance
				};
			}

			// Token: 0x06000A7E RID: 2686 RVA: 0x00047CA0 File Offset: 0x00045EA0
			internal static DebugUI.Widget CreateMetallicMinValue(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.MetallicMinValue,
					getter = () => panel.data.metallicMinValue,
					setter = delegate(float value)
					{
						panel.data.metallicMinValue = value;
					},
					incStep = 0.01f
				};
			}

			// Token: 0x06000A7F RID: 2687 RVA: 0x00047CFC File Offset: 0x00045EFC
			internal static DebugUI.Widget CreateMetallicMaxValue(DebugDisplaySettingsMaterial.SettingsPanel panel)
			{
				return new DebugUI.FloatField
				{
					nameAndTooltip = DebugDisplaySettingsMaterial.Strings.MetallicMaxValue,
					getter = () => panel.data.metallicMaxValue,
					setter = delegate(float value)
					{
						panel.data.metallicMaxValue = value;
					},
					incStep = 0.01f
				};
			}
		}

		// Token: 0x02000174 RID: 372
		[DisplayInfo(name = "Material", order = 2)]
		internal class SettingsPanel : DebugDisplaySettingsPanel<DebugDisplaySettingsMaterial>
		{
			// Token: 0x06000A80 RID: 2688 RVA: 0x00047D58 File Offset: 0x00045F58
			public SettingsPanel(DebugDisplaySettingsMaterial data)
				: base(data)
			{
				base.AddWidget(DebugDisplaySettingsCommon.WidgetFactory.CreateMissingDebugShadersWarning());
				base.AddWidget(new DebugUI.Foldout
				{
					displayName = "Material Filters",
					flags = DebugUI.Flags.FrequentlyUsed,
					isHeader = true,
					opened = true,
					children = 
					{
						DebugDisplaySettingsMaterial.WidgetFactory.CreateMaterialOverride(this),
						DebugDisplaySettingsMaterial.WidgetFactory.CreateVertexAttribute(this)
					}
				});
				base.AddWidget(new DebugUI.Foldout
				{
					displayName = "Material Validation",
					isHeader = true,
					opened = true,
					children = 
					{
						DebugDisplaySettingsMaterial.WidgetFactory.CreateMaterialValidationMode(this),
						new DebugUI.Container
						{
							displayName = "Albedo Settings",
							isHiddenCallback = () => data.materialValidationMode != DebugMaterialValidationMode.Albedo,
							children = 
							{
								DebugDisplaySettingsMaterial.WidgetFactory.CreateAlbedoPreset(this),
								DebugDisplaySettingsMaterial.WidgetFactory.CreateAlbedoCustomColor(this),
								DebugDisplaySettingsMaterial.WidgetFactory.CreateAlbedoMinLuminance(this),
								DebugDisplaySettingsMaterial.WidgetFactory.CreateAlbedoMaxLuminance(this),
								DebugDisplaySettingsMaterial.WidgetFactory.CreateAlbedoHueTolerance(this),
								DebugDisplaySettingsMaterial.WidgetFactory.CreateAlbedoSaturationTolerance(this)
							}
						},
						new DebugUI.Container
						{
							displayName = "Metallic Settings",
							isHiddenCallback = () => data.materialValidationMode != DebugMaterialValidationMode.Metallic,
							children = 
							{
								DebugDisplaySettingsMaterial.WidgetFactory.CreateMetallicMinValue(this),
								DebugDisplaySettingsMaterial.WidgetFactory.CreateMetallicMaxValue(this)
							}
						}
					}
				});
			}
		}
	}
}
