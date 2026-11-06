using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering
{
	// Token: 0x0200005F RID: 95
	public class DebugDisplaySettingsVolume : IDebugDisplaySettingsData, IDebugDisplaySettingsQuery
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000CFE2 File Offset: 0x0000B1E2
		public IVolumeDebugSettings2 volumeDebugSettings { get; }

		// Token: 0x060002FF RID: 767 RVA: 0x0000CFEA File Offset: 0x0000B1EA
		public DebugDisplaySettingsVolume(IVolumeDebugSettings2 volumeDebugSettings)
		{
			this.volumeDebugSettings = volumeDebugSettings;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000CFF9 File Offset: 0x0000B1F9
		public bool AreAnySettingsActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		public bool IsPostProcessingAllowed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000CFFF File Offset: 0x0000B1FF
		public bool IsLightingActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000D002 File Offset: 0x0000B202
		public bool TryGetScreenClearColor(ref Color color)
		{
			return false;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000D005 File Offset: 0x0000B205
		public IDebugDisplaySettingsPanelDisposable CreatePanel()
		{
			return new DebugDisplaySettingsVolume.SettingsPanel(this);
		}

		// Token: 0x040001AB RID: 427
		internal int volumeComponentEnumIndex;

		// Token: 0x02000164 RID: 356
		private static class Styles
		{
			// Token: 0x040005F5 RID: 1525
			public static readonly GUIContent none = new GUIContent("None");

			// Token: 0x040005F6 RID: 1526
			public static readonly GUIContent editorCamera = new GUIContent("Editor Camera");
		}

		// Token: 0x02000165 RID: 357
		private static class Strings
		{
			// Token: 0x040005F7 RID: 1527
			public static readonly string none = "None";

			// Token: 0x040005F8 RID: 1528
			public static readonly string camera = "Camera";

			// Token: 0x040005F9 RID: 1529
			public static readonly string parameter = "Parameter";

			// Token: 0x040005FA RID: 1530
			public static readonly string component = "Component";

			// Token: 0x040005FB RID: 1531
			public static readonly string debugViewNotSupported = "N/A";

			// Token: 0x040005FC RID: 1532
			public static readonly string parameterNotOverrided = "-";

			// Token: 0x040005FD RID: 1533
			public static readonly string volumeInfo = "Volume Info";

			// Token: 0x040005FE RID: 1534
			public static readonly string gameObject = "GameObject";

			// Token: 0x040005FF RID: 1535
			public static readonly string resultValue = "Result";

			// Token: 0x04000600 RID: 1536
			public static readonly string resultValueTooltip = "The interpolated result value of the parameter. This value is used to render the camera.";

			// Token: 0x04000601 RID: 1537
			public static readonly string globalDefaultValue = "Graphics Settings";

			// Token: 0x04000602 RID: 1538
			public static readonly string globalDefaultValueTooltip = "Default value for this parameter, defined by the Default Volume Profile in Global Settings.";

			// Token: 0x04000603 RID: 1539
			public static readonly string qualityLevelValue = "Quality Settings";

			// Token: 0x04000604 RID: 1540
			public static readonly string qualityLevelValueTooltip = "Override value for this parameter, defined by the Volume Profile in the current SRP Asset.";

			// Token: 0x04000605 RID: 1541
			public static readonly string global = "Global";

			// Token: 0x04000606 RID: 1542
			public static readonly string local = "Local";

			// Token: 0x04000607 RID: 1543
			public static readonly string volumeProfile = "Volume Profile";
		}

		// Token: 0x02000166 RID: 358
		internal static class WidgetFactory
		{
			// Token: 0x060009F3 RID: 2547 RVA: 0x0002C63C File Offset: 0x0002A83C
			public static DebugUI.EnumField CreateComponentSelector(DebugDisplaySettingsVolume.SettingsPanel panel, Action<DebugUI.Field<int>, int> refresh)
			{
				int num = 0;
				List<GUIContent> list = new List<GUIContent> { DebugDisplaySettingsVolume.Styles.none };
				List<int> list2 = new List<int> { num++ };
				foreach (ValueTuple<string, Type> valueTuple in panel.data.volumeDebugSettings.volumeComponentsPathAndType)
				{
					list.Add(new GUIContent
					{
						text = valueTuple.Item1
					});
					list2.Add(num++);
				}
				return new DebugUI.EnumField
				{
					displayName = DebugDisplaySettingsVolume.Strings.component,
					getter = () => panel.data.volumeDebugSettings.selectedComponent,
					setter = delegate(int value)
					{
						panel.data.volumeDebugSettings.selectedComponent = value;
					},
					enumNames = list.ToArray(),
					enumValues = list2.ToArray(),
					getIndex = () => panel.data.volumeComponentEnumIndex,
					setIndex = delegate(int value)
					{
						panel.data.volumeComponentEnumIndex = value;
					},
					onValueChanged = refresh
				};
			}

			// Token: 0x060009F4 RID: 2548 RVA: 0x0002C764 File Offset: 0x0002A964
			public static DebugUI.ObjectPopupField CreateCameraSelector(DebugDisplaySettingsVolume.SettingsPanel panel, Action<DebugUI.Field<Object>, Object> refresh)
			{
				return new DebugUI.ObjectPopupField
				{
					displayName = DebugDisplaySettingsVolume.Strings.camera,
					getter = () => panel.data.volumeDebugSettings.selectedCamera,
					setter = delegate(Object value)
					{
						Camera[] array = panel.data.volumeDebugSettings.cameras.ToArray<Camera>();
						panel.data.volumeDebugSettings.selectedCameraIndex = Array.IndexOf<Camera>(array, value as Camera);
					},
					getObjects = () => panel.data.volumeDebugSettings.cameras,
					onValueChanged = refresh
				};
			}

			// Token: 0x060009F5 RID: 2549 RVA: 0x0002C7CB File Offset: 0x0002A9CB
			private static DebugUI.Widget CreateVolumeParameterWidget(string name, bool isResultParameter, VolumeParameter param, Func<bool> isHiddenCallback = null)
			{
				return new DebugUI.Value();
			}

			// Token: 0x060009F6 RID: 2550 RVA: 0x0002C7D4 File Offset: 0x0002A9D4
			private static VolumeComponent GetSelectedVolumeComponent(VolumeProfile profile, Type selectedType)
			{
				if (profile != null)
				{
					foreach (VolumeComponent volumeComponent in profile.components)
					{
						if (volumeComponent.GetType() == selectedType)
						{
							return volumeComponent;
						}
					}
				}
				return null;
			}

			// Token: 0x060009F7 RID: 2551 RVA: 0x0002C840 File Offset: 0x0002AA40
			private static List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain> GetResolutionChain(DebugDisplaySettingsVolume data)
			{
				List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain> list = new List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain>();
				Type selectedComponentType = data.volumeDebugSettings.selectedComponentType;
				if (selectedComponentType == null)
				{
					return list;
				}
				VolumeManager instance = VolumeManager.instance;
				VolumeComponent component = (data.volumeDebugSettings.selectedCameraVolumeStack ?? instance.stack).GetComponent(selectedComponentType);
				if (component == null)
				{
					return list;
				}
				DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain volumeParameterChain = new DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain
				{
					nameAndTooltip = new DebugUI.Widget.NameAndTooltip
					{
						name = DebugDisplaySettingsVolume.Strings.resultValue,
						tooltip = DebugDisplaySettingsVolume.Strings.resultValueTooltip
					},
					volumeComponent = component
				};
				list.Add(volumeParameterChain);
				foreach (Volume volume in data.volumeDebugSettings.GetVolumes())
				{
					VolumeProfile volumeProfile = (volume.HasInstantiatedProfile() ? volume.profile : volume.sharedProfile);
					VolumeComponent selectedVolumeComponent = DebugDisplaySettingsVolume.WidgetFactory.GetSelectedVolumeComponent(volumeProfile, selectedComponentType);
					if (selectedVolumeComponent != null)
					{
						DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain volumeParameterChain2 = new DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain
						{
							nameAndTooltip = new DebugUI.Widget.NameAndTooltip
							{
								name = volumeProfile.name,
								tooltip = volumeProfile.name
							},
							volumeProfile = volumeProfile,
							volumeComponent = selectedVolumeComponent,
							volume = volume
						};
						list.Add(volumeParameterChain2);
					}
				}
				return list;
			}

			// Token: 0x060009F8 RID: 2552 RVA: 0x0002C998 File Offset: 0x0002AB98
			public static DebugUI.Table CreateVolumeTable(DebugDisplaySettingsVolume data)
			{
				DebugUI.Table table = new DebugUI.Table
				{
					displayName = DebugDisplaySettingsVolume.Strings.parameter,
					isReadOnly = true
				};
				List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain> resolutionChain = DebugDisplaySettingsVolume.WidgetFactory.GetResolutionChain(data);
				if (resolutionChain.Count == 0)
				{
					return table;
				}
				DebugDisplaySettingsVolume.WidgetFactory.GenerateTableRows(table, resolutionChain);
				DebugDisplaySettingsVolume.WidgetFactory.GenerateTableColumns(table, data, resolutionChain);
				float timer = 0f;
				float refreshRate = 0.2f;
				Volume[] volumes = data.volumeDebugSettings.GetVolumes();
				table.isHiddenCallback = delegate
				{
					timer += Time.deltaTime;
					if (timer >= refreshRate)
					{
						if (data.volumeDebugSettings.selectedCamera != null)
						{
							DebugDisplaySettingsVolume.WidgetFactory.SetTableColumnVisibility(data, table);
							Volume[] volumes2 = data.volumeDebugSettings.GetVolumes();
							if (!volumes.SequenceEqual<Volume>(volumes2))
							{
								volumes = volumes2;
								DebugManager.instance.ReDrawOnScreenDebug();
							}
						}
						timer = 0f;
					}
					return false;
				};
				return table;
			}

			// Token: 0x060009F9 RID: 2553 RVA: 0x0002CA54 File Offset: 0x0002AC54
			private static void SetTableColumnVisibility(DebugDisplaySettingsVolume data, DebugUI.Table table)
			{
				List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain> resolutionChain = DebugDisplaySettingsVolume.WidgetFactory.GetResolutionChain(data);
				for (int i = 1; i < resolutionChain.Count; i++)
				{
					bool flag = true;
					if (resolutionChain[i].volume != null)
					{
						flag = data.volumeDebugSettings.VolumeHasInfluence(resolutionChain[i].volume);
					}
					else
					{
						flag = resolutionChain[i].volumeComponent.active;
						if (flag)
						{
							bool flag2 = false;
							using (List<VolumeParameter>.Enumerator enumerator = resolutionChain[i].volumeComponent.parameterList.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									if (enumerator.Current.overrideState)
									{
										flag2 = true;
										break;
									}
								}
							}
							flag = flag && flag2;
						}
					}
					table.SetColumnVisibility(i, flag);
				}
			}

			// Token: 0x060009FA RID: 2554 RVA: 0x0002CB28 File Offset: 0x0002AD28
			private static void GenerateTableColumns(DebugUI.Table table, DebugDisplaySettingsVolume data, List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain> resolutionChain)
			{
				for (int i = 0; i < resolutionChain.Count; i++)
				{
					DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain chain = resolutionChain[i];
					int num = -1;
					if (chain.volume != null)
					{
						((DebugUI.Table.Row)table.children[++num]).children.Add(new DebugUI.Value
						{
							nameAndTooltip = chain.nameAndTooltip,
							getter = delegate
							{
								string text = (chain.volume.isGlobal ? DebugDisplaySettingsVolume.Strings.global : DebugDisplaySettingsVolume.Strings.local);
								float volumeWeight = data.volumeDebugSettings.GetVolumeWeight(chain.volume);
								return text + " (" + (volumeWeight * 100f).ToString() + "%)";
							},
							refreshRate = 0.2f
						});
						((DebugUI.Table.Row)table.children[++num]).children.Add(new DebugUI.ObjectField
						{
							displayName = string.Empty,
							getter = () => chain.volume
						});
					}
					else
					{
						ObservableList<DebugUI.Widget> children = ((DebugUI.Table.Row)table.children[++num]).children;
						DebugUI.Value value = new DebugUI.Value();
						value.nameAndTooltip = chain.nameAndTooltip;
						value.getter = () => string.Empty;
						children.Add(value);
						((DebugUI.Table.Row)table.children[++num]).children.Add(DebugDisplaySettingsVolume.WidgetFactory.s_EmptyDebugUIValue);
					}
					ObservableList<DebugUI.Widget> children2 = ((DebugUI.Table.Row)table.children[++num]).children;
					DebugUI.Widget widget;
					if (!(chain.volumeProfile != null))
					{
						widget = DebugDisplaySettingsVolume.WidgetFactory.s_EmptyDebugUIValue;
					}
					else
					{
						DebugUI.ObjectField objectField = new DebugUI.ObjectField();
						objectField.displayName = string.Empty;
						widget = objectField;
						objectField.getter = () => chain.volumeProfile;
					}
					children2.Add(widget);
					((DebugUI.Table.Row)table.children[++num]).children.Add(DebugDisplaySettingsVolume.WidgetFactory.s_EmptyDebugUIValue);
					bool flag = i == 0;
					for (int j = 0; j < chain.volumeComponent.parameterList.Count; j++)
					{
						VolumeParameter volumeParameter = chain.volumeComponent.parameterList[j];
						((DebugUI.Table.Row)table.children[++num]).children.Add(DebugDisplaySettingsVolume.WidgetFactory.CreateVolumeParameterWidget(chain.nameAndTooltip.name, flag, volumeParameter, null));
					}
				}
			}

			// Token: 0x060009FB RID: 2555 RVA: 0x0002CD9C File Offset: 0x0002AF9C
			private static void GenerateTableRows(DebugUI.Table table, List<DebugDisplaySettingsVolume.WidgetFactory.VolumeParameterChain> resolutionChain)
			{
				DebugUI.Table.Row row = new DebugUI.Table.Row
				{
					displayName = DebugDisplaySettingsVolume.Strings.volumeInfo,
					opened = true
				};
				table.children.Add(row);
				DebugUI.Table.Row row2 = new DebugUI.Table.Row
				{
					displayName = DebugDisplaySettingsVolume.Strings.gameObject
				};
				table.children.Add(row2);
				DebugUI.Table.Row row3 = new DebugUI.Table.Row
				{
					displayName = DebugDisplaySettingsVolume.Strings.volumeProfile
				};
				table.children.Add(row3);
				DebugUI.Table.Row row4 = new DebugUI.Table.Row
				{
					displayName = string.Empty
				};
				table.children.Add(row4);
				VolumeComponent volumeComponent = resolutionChain[0].volumeComponent;
				for (int i = 0; i < volumeComponent.parameterList.Count; i++)
				{
					VolumeParameter volumeParameter = volumeComponent.parameterList[i];
					string text = i.ToString();
					table.children.Add(new DebugUI.Table.Row
					{
						displayName = text
					});
				}
			}

			// Token: 0x04000608 RID: 1544
			private static DebugUI.Value s_EmptyDebugUIValue = new DebugUI.Value
			{
				getter = () => string.Empty
			};

			// Token: 0x020001EC RID: 492
			private struct VolumeParameterChain
			{
				// Token: 0x040007C4 RID: 1988
				public DebugUI.Widget.NameAndTooltip nameAndTooltip;

				// Token: 0x040007C5 RID: 1989
				public VolumeProfile volumeProfile;

				// Token: 0x040007C6 RID: 1990
				public VolumeComponent volumeComponent;

				// Token: 0x040007C7 RID: 1991
				public Volume volume;
			}
		}

		// Token: 0x02000167 RID: 359
		[DisplayInfo(name = "Volume", order = 2147483647)]
		internal class SettingsPanel : DebugDisplaySettingsPanel<DebugDisplaySettingsVolume>
		{
			// Token: 0x060009FD RID: 2557 RVA: 0x0002CE9F File Offset: 0x0002B09F
			public SettingsPanel(DebugDisplaySettingsVolume data)
				: base(data)
			{
				base.AddWidget(DebugDisplaySettingsVolume.WidgetFactory.CreateComponentSelector(this, delegate(DebugUI.Field<int> _, int __)
				{
					this.Refresh();
				}));
				base.AddWidget(DebugDisplaySettingsVolume.WidgetFactory.CreateCameraSelector(this, delegate(DebugUI.Field<Object> _, Object __)
				{
					this.Refresh();
				}));
			}

			// Token: 0x060009FE RID: 2558 RVA: 0x0002CED8 File Offset: 0x0002B0D8
			private void Refresh()
			{
				DebugUI.Panel panel = DebugManager.instance.GetPanel(this.PanelName, false, 0, false);
				if (panel == null)
				{
					return;
				}
				bool flag = false;
				if (this.m_VolumeTable != null)
				{
					flag = true;
					panel.children.Remove(this.m_VolumeTable);
				}
				if (this.m_Data.volumeDebugSettings.selectedComponent > 0 && this.m_Data.volumeDebugSettings.selectedCamera != null)
				{
					flag = true;
					this.m_VolumeTable = DebugDisplaySettingsVolume.WidgetFactory.CreateVolumeTable(this.m_Data);
					base.AddWidget(this.m_VolumeTable);
					panel.children.Add(this.m_VolumeTable);
				}
				if (flag)
				{
					DebugManager.instance.ReDrawOnScreenDebug();
				}
			}

			// Token: 0x04000609 RID: 1545
			private DebugUI.Table m_VolumeTable;
		}
	}
}
