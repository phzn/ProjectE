using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x020000ED RID: 237
	public class Column
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0001EBD0 File Offset: 0x0001CDD0
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				bool flag = this.m_Name == value;
				if (!flag)
				{
					this.m_Name = value;
					this.NotifyChange(ColumnDataType.Name);
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0001EBFF File Offset: 0x0001CDFF
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0001EC08 File Offset: 0x0001CE08
		public string title
		{
			get
			{
				return this.m_Title;
			}
			set
			{
				bool flag = this.m_Title == value;
				if (!flag)
				{
					this.m_Title = value;
					this.NotifyChange(ColumnDataType.Title);
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0001EC37 File Offset: 0x0001CE37
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x0001EC40 File Offset: 0x0001CE40
		public Background icon
		{
			get
			{
				return this.m_Icon;
			}
			set
			{
				bool flag = this.m_Icon == value;
				if (!flag)
				{
					this.m_Icon = value;
					this.NotifyChange(ColumnDataType.Icon);
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x0001EC6F File Offset: 0x0001CE6F
		internal int index
		{
			get
			{
				Columns collection = this.collection;
				return (collection != null) ? collection.IndexOf(this) : (-1);
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0001EC84 File Offset: 0x0001CE84
		internal int displayIndex
		{
			get
			{
				Columns collection = this.collection;
				List<Column> list = ((collection != null) ? collection.displayList : null) as List<Column>;
				return (list != null) ? list.IndexOf(this) : (-1);
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0001ECAA File Offset: 0x0001CEAA
		internal int visibleIndex
		{
			get
			{
				Columns collection = this.collection;
				List<Column> list = ((collection != null) ? collection.visibleList : null) as List<Column>;
				return (list != null) ? list.IndexOf(this) : (-1);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001ECD0 File Offset: 0x0001CED0
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x0001ECD8 File Offset: 0x0001CED8
		public bool visible
		{
			get
			{
				return this.m_Visible;
			}
			set
			{
				bool flag = this.m_Visible == value;
				if (!flag)
				{
					this.m_Visible = value;
					this.NotifyChange(ColumnDataType.Visibility);
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000806 RID: 2054 RVA: 0x0001ED04 File Offset: 0x0001CF04
		// (set) Token: 0x06000807 RID: 2055 RVA: 0x0001ED0C File Offset: 0x0001CF0C
		public Length width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				bool flag = this.m_Width == value;
				if (!flag)
				{
					this.m_Width = value;
					this.desiredWidth = float.NaN;
					this.NotifyChange(ColumnDataType.Width);
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x0001ED47 File Offset: 0x0001CF47
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x0001ED50 File Offset: 0x0001CF50
		public Length minWidth
		{
			get
			{
				return this.m_MinWidth;
			}
			set
			{
				bool flag = this.m_MinWidth == value;
				if (!flag)
				{
					this.m_MinWidth = value;
					this.NotifyChange(ColumnDataType.MinWidth);
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0001ED7F File Offset: 0x0001CF7F
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x0001ED88 File Offset: 0x0001CF88
		public Length maxWidth
		{
			get
			{
				return this.m_MaxWidth;
			}
			set
			{
				bool flag = this.m_MaxWidth == value;
				if (!flag)
				{
					this.m_MaxWidth = value;
					this.NotifyChange(ColumnDataType.MaxWidth);
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x0001EDB7 File Offset: 0x0001CFB7
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
		internal float desiredWidth
		{
			get
			{
				return this.m_DesiredWidth;
			}
			set
			{
				bool flag = this.m_DesiredWidth == value;
				if (!flag)
				{
					this.m_DesiredWidth = value;
					Action<Column> action = this.resized;
					if (action != null)
					{
						action(this);
					}
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0001EDF7 File Offset: 0x0001CFF7
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x0001EE00 File Offset: 0x0001D000
		public bool sortable
		{
			get
			{
				return this.m_Sortable;
			}
			set
			{
				bool flag = this.m_Sortable == value;
				if (!flag)
				{
					this.m_Sortable = value;
					this.NotifyChange(ColumnDataType.Sortable);
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0001EE2C File Offset: 0x0001D02C
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0001EE34 File Offset: 0x0001D034
		public bool stretchable
		{
			get
			{
				return this.m_Stretchable;
			}
			set
			{
				bool flag = this.m_Stretchable == value;
				if (!flag)
				{
					this.m_Stretchable = value;
					this.NotifyChange(ColumnDataType.Stretchable);
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x0001EE60 File Offset: 0x0001D060
		// (set) Token: 0x06000813 RID: 2067 RVA: 0x0001EE68 File Offset: 0x0001D068
		public bool optional
		{
			get
			{
				return this.m_Optional;
			}
			set
			{
				bool flag = this.m_Optional == value;
				if (!flag)
				{
					this.m_Optional = value;
					this.NotifyChange(ColumnDataType.Optional);
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001EE95 File Offset: 0x0001D095
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
		public bool resizable
		{
			get
			{
				return this.m_Resizable;
			}
			set
			{
				bool flag = this.m_Resizable == value;
				if (!flag)
				{
					this.m_Resizable = value;
					this.NotifyChange(ColumnDataType.Resizable);
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x0001EECD File Offset: 0x0001D0CD
		// (set) Token: 0x06000817 RID: 2071 RVA: 0x0001EED8 File Offset: 0x0001D0D8
		public Func<VisualElement> makeHeader
		{
			get
			{
				return this.m_MakeHeader;
			}
			set
			{
				bool flag = this.m_MakeHeader == value;
				if (!flag)
				{
					this.m_MakeHeader = value;
					this.NotifyChange(ColumnDataType.HeaderTemplate);
				}
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x0001EF08 File Offset: 0x0001D108
		// (set) Token: 0x06000819 RID: 2073 RVA: 0x0001EF10 File Offset: 0x0001D110
		public Action<VisualElement> bindHeader
		{
			get
			{
				return this.m_BindHeader;
			}
			set
			{
				bool flag = this.m_BindHeader == value;
				if (!flag)
				{
					this.m_BindHeader = value;
					this.NotifyChange(ColumnDataType.HeaderTemplate);
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001EF40 File Offset: 0x0001D140
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001EF48 File Offset: 0x0001D148
		public Action<VisualElement> unbindHeader
		{
			get
			{
				return this.m_UnbindHeader;
			}
			set
			{
				bool flag = this.m_UnbindHeader == value;
				if (!flag)
				{
					this.m_UnbindHeader = value;
					this.NotifyChange(ColumnDataType.HeaderTemplate);
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001EF78 File Offset: 0x0001D178
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001EF80 File Offset: 0x0001D180
		public Action<VisualElement> destroyHeader
		{
			get
			{
				return this.m_DestroyHeader;
			}
			set
			{
				bool flag = this.m_DestroyHeader == value;
				if (!flag)
				{
					this.m_DestroyHeader = value;
					this.NotifyChange(ColumnDataType.HeaderTemplate);
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001EFB0 File Offset: 0x0001D1B0
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x0001EFB8 File Offset: 0x0001D1B8
		public Func<VisualElement> makeCell
		{
			get
			{
				return this.m_MakeCell;
			}
			set
			{
				bool flag = this.m_MakeCell == value;
				if (!flag)
				{
					this.m_MakeCell = value;
					this.NotifyChange(ColumnDataType.CellTemplate);
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x0001EFE8 File Offset: 0x0001D1E8
		// (set) Token: 0x06000821 RID: 2081 RVA: 0x0001EFF0 File Offset: 0x0001D1F0
		public Action<VisualElement, int> bindCell
		{
			get
			{
				return this.m_BindCell;
			}
			set
			{
				bool flag = this.m_BindCell == value;
				if (!flag)
				{
					this.m_BindCell = value;
					this.NotifyChange(ColumnDataType.CellTemplate);
				}
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0001F020 File Offset: 0x0001D220
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0001F028 File Offset: 0x0001D228
		public Action<VisualElement, int> unbindCell
		{
			get
			{
				return this.m_UnbindCellItem;
			}
			set
			{
				bool flag = this.m_UnbindCellItem == value;
				if (!flag)
				{
					this.m_UnbindCellItem = value;
					this.NotifyChange(ColumnDataType.CellTemplate);
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0001F058 File Offset: 0x0001D258
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x0001F060 File Offset: 0x0001D260
		public Action<VisualElement> destroyCell { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x0001F069 File Offset: 0x0001D269
		// (set) Token: 0x06000827 RID: 2087 RVA: 0x0001F071 File Offset: 0x0001D271
		public Columns collection { get; internal set; }

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000828 RID: 2088 RVA: 0x0001F07C File Offset: 0x0001D27C
		// (remove) Token: 0x06000829 RID: 2089 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column, ColumnDataType> changed;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x0600082A RID: 2090 RVA: 0x0001F0EC File Offset: 0x0001D2EC
		// (remove) Token: 0x0600082B RID: 2091 RVA: 0x0001F124 File Offset: 0x0001D324
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action<Column> resized;

		// Token: 0x0600082C RID: 2092 RVA: 0x0001F159 File Offset: 0x0001D359
		private void NotifyChange(ColumnDataType type)
		{
			Action<Column, ColumnDataType> action = this.changed;
			if (action != null)
			{
				action(this, type);
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001F170 File Offset: 0x0001D370
		internal float GetWidth(float layoutWidth)
		{
			return (this.width.unit == LengthUnit.Pixel) ? this.width.value : (this.width.value * layoutWidth / 100f);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001F1B8 File Offset: 0x0001D3B8
		internal float GetMaxWidth(float layoutWidth)
		{
			return (this.maxWidth.unit == LengthUnit.Pixel) ? this.maxWidth.value : (this.maxWidth.value * layoutWidth / 100f);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001F200 File Offset: 0x0001D400
		internal float GetMinWidth(float layoutWidth)
		{
			return (this.minWidth.unit == LengthUnit.Pixel) ? this.minWidth.value : (this.minWidth.value * layoutWidth / 100f);
		}

		// Token: 0x04000384 RID: 900
		internal const float kDefaultMinWidth = 35f;

		// Token: 0x04000385 RID: 901
		private static readonly string k_InvalidTemplateError = "Not Found";

		// Token: 0x04000386 RID: 902
		private string m_Name;

		// Token: 0x04000387 RID: 903
		private string m_Title;

		// Token: 0x04000388 RID: 904
		private Background m_Icon;

		// Token: 0x04000389 RID: 905
		private bool m_Visible = true;

		// Token: 0x0400038A RID: 906
		private Length m_Width = 0f;

		// Token: 0x0400038B RID: 907
		private Length m_MinWidth = 35f;

		// Token: 0x0400038C RID: 908
		private Length m_MaxWidth = 8388608f;

		// Token: 0x0400038D RID: 909
		private float m_DesiredWidth = float.NaN;

		// Token: 0x0400038E RID: 910
		private bool m_Stretchable;

		// Token: 0x0400038F RID: 911
		private bool m_Sortable = true;

		// Token: 0x04000390 RID: 912
		private bool m_Optional = true;

		// Token: 0x04000391 RID: 913
		private bool m_Resizable = true;

		// Token: 0x04000392 RID: 914
		private Func<VisualElement> m_MakeHeader;

		// Token: 0x04000393 RID: 915
		private Action<VisualElement> m_BindHeader;

		// Token: 0x04000394 RID: 916
		private Action<VisualElement> m_UnbindHeader;

		// Token: 0x04000395 RID: 917
		private Action<VisualElement> m_DestroyHeader;

		// Token: 0x04000396 RID: 918
		private Func<VisualElement> m_MakeCell;

		// Token: 0x04000397 RID: 919
		private Action<VisualElement, int> m_BindCell;

		// Token: 0x04000398 RID: 920
		private Action<VisualElement, int> m_UnbindCellItem;

		// Token: 0x020000EE RID: 238
		internal class UxmlObjectFactory<T> : UxmlObjectFactory<T, Column.UxmlObjectTraits<T>> where T : Column, new()
		{
		}

		// Token: 0x020000EF RID: 239
		internal class UxmlObjectTraits<T> : UnityEngine.UIElements.UxmlObjectTraits<T> where T : Column
		{
			// Token: 0x06000833 RID: 2099 RVA: 0x0001F2C8 File Offset: 0x0001D4C8
			private static Length ParseLength(string str, Length defaultValue)
			{
				float num = defaultValue.value;
				LengthUnit lengthUnit = defaultValue.unit;
				int num2 = 0;
				int num3 = -1;
				for (int i = 0; i < str.Length; i++)
				{
					char c = str[i];
					bool flag = char.IsLetter(c) || c == '%';
					if (flag)
					{
						num3 = i;
						break;
					}
					num2++;
				}
				string text = str.Substring(0, num2);
				string text2 = string.Empty;
				bool flag2 = num3 > 0;
				if (flag2)
				{
					text2 = str.Substring(num3, str.Length - num3).ToLowerInvariant();
				}
				float num4;
				bool flag3 = float.TryParse(text, out num4);
				if (flag3)
				{
					num = num4;
				}
				string text3 = text2;
				string text4 = text3;
				if (!(text4 == "px"))
				{
					if (text4 == "%")
					{
						lengthUnit = LengthUnit.Percent;
					}
				}
				else
				{
					lengthUnit = LengthUnit.Pixel;
				}
				return new Length(num, lengthUnit);
			}

			// Token: 0x06000834 RID: 2100 RVA: 0x0001F3B4 File Offset: 0x0001D5B4
			public override void Init(ref T obj, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ref obj, bag, cc);
				obj.name = this.m_Name.GetValueFromBag(bag, cc);
				obj.title = this.m_Text.GetValueFromBag(bag, cc);
				obj.visible = this.m_Visible.GetValueFromBag(bag, cc);
				obj.width = Column.UxmlObjectTraits<T>.ParseLength(this.m_Width.GetValueFromBag(bag, cc), default(Length));
				obj.maxWidth = Column.UxmlObjectTraits<T>.ParseLength(this.m_MaxWidth.GetValueFromBag(bag, cc), new Length(8388608f));
				obj.minWidth = Column.UxmlObjectTraits<T>.ParseLength(this.m_MinWidth.GetValueFromBag(bag, cc), new Length(35f));
				obj.sortable = this.m_Sortable.GetValueFromBag(bag, cc);
				obj.stretchable = this.m_Stretch.GetValueFromBag(bag, cc);
				obj.optional = this.m_Optional.GetValueFromBag(bag, cc);
				obj.resizable = this.m_Resizable.GetValueFromBag(bag, cc);
				string valueFromBag = this.m_HeaderTemplateId.GetValueFromBag(bag, cc);
				bool flag = !string.IsNullOrEmpty(valueFromBag);
				if (flag)
				{
					Column.UxmlObjectTraits<T>.<>c__DisplayClass15_0 CS$<>8__locals1 = new Column.UxmlObjectTraits<T>.<>c__DisplayClass15_0();
					Column.UxmlObjectTraits<T>.<>c__DisplayClass15_0 CS$<>8__locals2 = CS$<>8__locals1;
					VisualTreeAsset visualTreeAsset = cc.visualTreeAsset;
					CS$<>8__locals2.asset = ((visualTreeAsset != null) ? visualTreeAsset.ResolveTemplate(valueFromBag) : null);
					obj.makeHeader = delegate
					{
						bool flag3 = CS$<>8__locals1.asset != null;
						VisualElement visualElement;
						if (flag3)
						{
							visualElement = CS$<>8__locals1.asset.Instantiate();
						}
						else
						{
							visualElement = new Label(Column.k_InvalidTemplateError);
						}
						return visualElement;
					};
				}
				string valueFromBag2 = this.m_CellTemplateId.GetValueFromBag(bag, cc);
				bool flag2 = !string.IsNullOrEmpty(valueFromBag2);
				if (flag2)
				{
					Column.UxmlObjectTraits<T>.<>c__DisplayClass15_1 CS$<>8__locals3 = new Column.UxmlObjectTraits<T>.<>c__DisplayClass15_1();
					Column.UxmlObjectTraits<T>.<>c__DisplayClass15_1 CS$<>8__locals4 = CS$<>8__locals3;
					VisualTreeAsset visualTreeAsset2 = cc.visualTreeAsset;
					CS$<>8__locals4.asset = ((visualTreeAsset2 != null) ? visualTreeAsset2.ResolveTemplate(valueFromBag2) : null);
					obj.makeCell = delegate
					{
						bool flag4 = CS$<>8__locals3.asset != null;
						VisualElement visualElement2;
						if (flag4)
						{
							visualElement2 = CS$<>8__locals3.asset.Instantiate();
						}
						else
						{
							visualElement2 = new Label(Column.k_InvalidTemplateError);
						}
						return visualElement2;
					};
				}
			}

			// Token: 0x0400039D RID: 925
			internal const string k_HeaderTemplateAttributeName = "header-template";

			// Token: 0x0400039E RID: 926
			internal const string k_CellTemplateAttributeName = "cell-template";

			// Token: 0x0400039F RID: 927
			private UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription
			{
				name = "name"
			};

			// Token: 0x040003A0 RID: 928
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "title"
			};

			// Token: 0x040003A1 RID: 929
			private UxmlBoolAttributeDescription m_Visible = new UxmlBoolAttributeDescription
			{
				name = "visible",
				defaultValue = true
			};

			// Token: 0x040003A2 RID: 930
			private UxmlStringAttributeDescription m_Width = new UxmlStringAttributeDescription
			{
				name = "width"
			};

			// Token: 0x040003A3 RID: 931
			private UxmlStringAttributeDescription m_MinWidth = new UxmlStringAttributeDescription
			{
				name = "min-width"
			};

			// Token: 0x040003A4 RID: 932
			private UxmlStringAttributeDescription m_MaxWidth = new UxmlStringAttributeDescription
			{
				name = "max-width"
			};

			// Token: 0x040003A5 RID: 933
			private UxmlBoolAttributeDescription m_Stretch = new UxmlBoolAttributeDescription
			{
				name = "stretchable"
			};

			// Token: 0x040003A6 RID: 934
			private UxmlBoolAttributeDescription m_Sortable = new UxmlBoolAttributeDescription
			{
				name = "sortable",
				defaultValue = true
			};

			// Token: 0x040003A7 RID: 935
			private UxmlBoolAttributeDescription m_Optional = new UxmlBoolAttributeDescription
			{
				name = "optional",
				defaultValue = true
			};

			// Token: 0x040003A8 RID: 936
			private UxmlBoolAttributeDescription m_Resizable = new UxmlBoolAttributeDescription
			{
				name = "resizable",
				defaultValue = true
			};

			// Token: 0x040003A9 RID: 937
			private UxmlStringAttributeDescription m_HeaderTemplateId = new UxmlStringAttributeDescription
			{
				name = "header-template"
			};

			// Token: 0x040003AA RID: 938
			private UxmlStringAttributeDescription m_CellTemplateId = new UxmlStringAttributeDescription
			{
				name = "cell-template"
			};
		}
	}
}
