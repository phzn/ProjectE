using System;

namespace UnityEngine.UIElements.Internal
{
	// Token: 0x020004D2 RID: 1234
	internal class MultiColumnHeaderColumn : VisualElement
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000A2768 File Offset: 0x000A0968
		// (set) Token: 0x060026A9 RID: 9897 RVA: 0x000A2770 File Offset: 0x000A0970
		public Clickable clickable { get; private set; }

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x000A2779 File Offset: 0x000A0979
		// (set) Token: 0x060026AB RID: 9899 RVA: 0x000A2781 File Offset: 0x000A0981
		public ColumnMover mover { get; private set; }

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x000A278A File Offset: 0x000A098A
		// (set) Token: 0x060026AD RID: 9901 RVA: 0x000A2797 File Offset: 0x000A0997
		public string sortOrderLabel
		{
			get
			{
				return this.m_SortIndicatorContainer.sortOrderLabel;
			}
			set
			{
				this.m_SortIndicatorContainer.sortOrderLabel = value;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x000A27A6 File Offset: 0x000A09A6
		// (set) Token: 0x060026AF RID: 9903 RVA: 0x000A27AE File Offset: 0x000A09AE
		public Column column { get; private set; }

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000A27B7 File Offset: 0x000A09B7
		internal Label title
		{
			get
			{
				VisualElement content = this.content;
				return (content != null) ? content.Q<Label>(MultiColumnHeaderColumn.titleElementName, null) : null;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000A27D1 File Offset: 0x000A09D1
		// (set) Token: 0x060026B2 RID: 9906 RVA: 0x000A27DC File Offset: 0x000A09DC
		public VisualElement content
		{
			get
			{
				return this.m_Content;
			}
			set
			{
				bool flag = this.m_Content != null;
				if (flag)
				{
					bool flag2 = this.m_Content.parent == this.m_ContentContainer;
					if (flag2)
					{
						this.m_Content.RemoveFromHierarchy();
					}
					this.DestroyHeaderContent();
					this.m_Content = null;
				}
				this.m_Content = value;
				bool flag3 = this.m_Content != null;
				if (flag3)
				{
					this.m_Content.AddToClassList(MultiColumnHeaderColumn.contentUssClassName);
					this.m_ContentContainer.Add(this.m_Content);
				}
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000A2865 File Offset: 0x000A0A65
		// (set) Token: 0x060026B4 RID: 9908 RVA: 0x000A288C File Offset: 0x000A0A8C
		private bool isContentBound
		{
			get
			{
				return this.m_Content != null && (bool)this.m_Content.GetProperty(MultiColumnHeaderColumn.s_BoundVEPropertyName);
			}
			set
			{
				VisualElement content = this.m_Content;
				if (content != null)
				{
					content.SetProperty(MultiColumnHeaderColumn.s_BoundVEPropertyName, value);
				}
			}
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000A28B0 File Offset: 0x000A0AB0
		public MultiColumnHeaderColumn()
			: this(new Column())
		{
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000A28C0 File Offset: 0x000A0AC0
		public MultiColumnHeaderColumn(Column column)
		{
			this.column = column;
			this.column.changed += this.OnColumnChanged;
			this.column.resized += this.OnColumnResized;
			base.AddToClassList(MultiColumnHeaderColumn.ussClassName);
			base.style.marginLeft = 0f;
			base.style.marginTop = 0f;
			base.style.marginRight = 0f;
			base.style.marginBottom = 0f;
			base.style.paddingLeft = 0f;
			base.style.paddingTop = 0f;
			base.style.paddingRight = 0f;
			base.style.paddingBottom = 0f;
			base.Add(this.m_SortIndicatorContainer = new MultiColumnHeaderColumnSortIndicator());
			this.m_ContentContainer = new VisualElement();
			this.m_ContentContainer.style.flexGrow = 1f;
			this.m_ContentContainer.style.flexShrink = 1f;
			this.m_ContentContainer.AddToClassList(MultiColumnHeaderColumn.contentContainerUssClassName);
			base.Add(this.m_ContentContainer);
			this.UpdateHeaderTemplate();
			this.UpdateGeometryFromColumn();
			this.InitManipulators();
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000A2A54 File Offset: 0x000A0C54
		private void OnColumnChanged(Column c, ColumnDataType role)
		{
			bool flag = this.column != c;
			if (!flag)
			{
				bool flag2 = role == ColumnDataType.HeaderTemplate;
				if (flag2)
				{
					IVisualElementScheduledItem scheduledHeaderTemplateUpdate = this.m_ScheduledHeaderTemplateUpdate;
					if (scheduledHeaderTemplateUpdate != null)
					{
						scheduledHeaderTemplateUpdate.Pause();
					}
					this.m_ScheduledHeaderTemplateUpdate = base.schedule.Execute(new Action(this.UpdateHeaderTemplate));
				}
				else
				{
					this.UpdateDataFromColumn();
				}
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000A2AB9 File Offset: 0x000A0CB9
		private void OnColumnResized(Column c)
		{
			this.UpdateGeometryFromColumn();
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000A2AC4 File Offset: 0x000A0CC4
		private void InitManipulators()
		{
			this.AddManipulator(this.mover = new ColumnMover());
			this.mover.movingChanged += this.OnMoverChanged;
			this.AddManipulator(this.clickable = new Clickable(null));
			this.clickable.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = EventModifiers.Shift
			});
			EventModifiers eventModifiers = EventModifiers.Control;
			RuntimePlatform platform = Application.platform;
			bool flag = platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer;
			if (flag)
			{
				eventModifiers = EventModifiers.Command;
			}
			this.clickable.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = eventModifiers
			});
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000A2B94 File Offset: 0x000A0D94
		private void OnMoverChanged(ColumnMover mv)
		{
			bool moving = this.mover.moving;
			if (moving)
			{
				base.AddToClassList(MultiColumnHeaderColumn.movingUssClassName);
			}
			else
			{
				base.RemoveFromClassList(MultiColumnHeaderColumn.movingUssClassName);
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000A2BCC File Offset: 0x000A0DCC
		private void UpdateDataFromColumn()
		{
			bool flag = this.column == null;
			if (!flag)
			{
				base.name = this.column.name;
				this.UnbindHeaderContent();
				this.BindHeaderContent();
			}
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000A2C0C File Offset: 0x000A0E0C
		private void BindHeaderContent()
		{
			bool flag = !this.isContentBound;
			if (flag)
			{
				Action<VisualElement> action = this.content.GetProperty(MultiColumnHeaderColumn.s_BindingCallbackVEPropertyName) as Action<VisualElement>;
				if (action != null)
				{
					action(this.content);
				}
				this.isContentBound = true;
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000A2C60 File Offset: 0x000A0E60
		private void UnbindHeaderContent()
		{
			bool isContentBound = this.isContentBound;
			if (isContentBound)
			{
				Action<VisualElement> action = this.content.GetProperty(MultiColumnHeaderColumn.s_UnbindingCallbackVEPropertyName) as Action<VisualElement>;
				if (action != null)
				{
					action(this.content);
				}
				this.isContentBound = false;
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000A2CB0 File Offset: 0x000A0EB0
		private void DestroyHeaderContent()
		{
			this.UnbindHeaderContent();
			Action<VisualElement> action = this.content.GetProperty(MultiColumnHeaderColumn.s_DestroyCallbackVEPropertyName) as Action<VisualElement>;
			this.content.SetProperty(MultiColumnHeaderColumn.s_BindingCallbackVEPropertyName, null);
			this.content.SetProperty(MultiColumnHeaderColumn.s_UnbindingCallbackVEPropertyName, null);
			this.content.SetProperty(MultiColumnHeaderColumn.s_DestroyCallbackVEPropertyName, null);
			this.content.SetProperty(MultiColumnHeaderColumn.s_BoundVEPropertyName, null);
			if (action != null)
			{
				action(this.content);
			}
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000A2D50 File Offset: 0x000A0F50
		private VisualElement CreateDefaultHeaderContent()
		{
			VisualElement visualElement = new VisualElement
			{
				pickingMode = PickingMode.Ignore
			};
			visualElement.AddToClassList(MultiColumnHeaderColumn.defaultContentUssClassName);
			MultiColumnHeaderColumnIcon multiColumnHeaderColumnIcon = new MultiColumnHeaderColumnIcon
			{
				name = MultiColumnHeaderColumn.iconElementName,
				pickingMode = PickingMode.Ignore
			};
			Label label = new Label
			{
				name = MultiColumnHeaderColumn.titleElementName,
				pickingMode = PickingMode.Ignore
			};
			label.AddToClassList(MultiColumnHeaderColumn.titleUssClassName);
			visualElement.Add(multiColumnHeaderColumnIcon);
			visualElement.Add(label);
			return visualElement;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000A2DD0 File Offset: 0x000A0FD0
		private void DefaultBindHeaderContent(VisualElement ve)
		{
			Label label = ve.Q<Label>(MultiColumnHeaderColumn.titleElementName, null);
			MultiColumnHeaderColumnIcon multiColumnHeaderColumnIcon = ve.Q<MultiColumnHeaderColumnIcon>(null, null);
			ve.RemoveFromClassList(MultiColumnHeaderColumn.hasTitleUssClassName);
			bool flag = label != null;
			if (flag)
			{
				label.text = this.column.title;
			}
			bool flag2 = !string.IsNullOrEmpty(this.column.title);
			if (flag2)
			{
				ve.AddToClassList(MultiColumnHeaderColumn.hasTitleUssClassName);
			}
			bool flag3 = multiColumnHeaderColumnIcon != null;
			if (flag3)
			{
				bool flag4 = this.column.icon.texture != null || this.column.icon.sprite != null || this.column.icon.vectorImage != null;
				if (flag4)
				{
					multiColumnHeaderColumnIcon.isImageInline = true;
					multiColumnHeaderColumnIcon.image = this.column.icon.texture;
					multiColumnHeaderColumnIcon.sprite = this.column.icon.sprite;
					multiColumnHeaderColumnIcon.vectorImage = this.column.icon.vectorImage;
				}
				else
				{
					bool isImageInline = multiColumnHeaderColumnIcon.isImageInline;
					if (isImageInline)
					{
						multiColumnHeaderColumnIcon.image = null;
						multiColumnHeaderColumnIcon.sprite = null;
						multiColumnHeaderColumnIcon.vectorImage = null;
					}
				}
				multiColumnHeaderColumnIcon.UpdateClassList();
			}
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000A2F34 File Offset: 0x000A1134
		private void UpdateHeaderTemplate()
		{
			bool flag = this.column == null;
			if (!flag)
			{
				Func<VisualElement> func = this.column.makeHeader;
				Action<VisualElement> action = this.column.bindHeader;
				Action<VisualElement> action2 = this.column.unbindHeader;
				Action<VisualElement> action3 = this.column.destroyHeader;
				bool flag2 = func == null;
				if (flag2)
				{
					func = new Func<VisualElement>(this.CreateDefaultHeaderContent);
					action = new Action<VisualElement>(this.DefaultBindHeaderContent);
					action2 = null;
					action3 = null;
				}
				this.content = func();
				this.content.SetProperty(MultiColumnHeaderColumn.s_BindingCallbackVEPropertyName, action);
				this.content.SetProperty(MultiColumnHeaderColumn.s_UnbindingCallbackVEPropertyName, action2);
				this.content.SetProperty(MultiColumnHeaderColumn.s_DestroyCallbackVEPropertyName, action3);
				this.isContentBound = false;
				this.m_ScheduledHeaderTemplateUpdate = null;
				this.UpdateDataFromColumn();
			}
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000A3018 File Offset: 0x000A1218
		private void UpdateGeometryFromColumn()
		{
			bool flag = float.IsNaN(this.column.desiredWidth);
			if (!flag)
			{
				base.style.width = this.column.desiredWidth;
			}
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000A3058 File Offset: 0x000A1258
		public void Dispose()
		{
			this.mover.movingChanged -= this.OnMoverChanged;
			this.column.changed -= this.OnColumnChanged;
			this.column.resized -= this.OnColumnResized;
			this.RemoveManipulator(this.mover);
			this.RemoveManipulator(this.clickable);
			this.mover = null;
			this.column = null;
			this.content = null;
		}

		// Token: 0x0400129B RID: 4763
		public static readonly string ussClassName = MultiColumnCollectionHeader.ussClassName + "__column";

		// Token: 0x0400129C RID: 4764
		public static readonly string sortableUssClassName = MultiColumnHeaderColumn.ussClassName + "--sortable";

		// Token: 0x0400129D RID: 4765
		public static readonly string sortedAscendingUssClassName = MultiColumnHeaderColumn.ussClassName + "--sorted-ascending";

		// Token: 0x0400129E RID: 4766
		public static readonly string sortedDescendingUssClassName = MultiColumnHeaderColumn.ussClassName + "--sorted-descending";

		// Token: 0x0400129F RID: 4767
		public static readonly string movingUssClassName = MultiColumnHeaderColumn.ussClassName + "--moving";

		// Token: 0x040012A0 RID: 4768
		public static readonly string contentContainerUssClassName = MultiColumnHeaderColumn.ussClassName + "__content-container";

		// Token: 0x040012A1 RID: 4769
		public static readonly string contentUssClassName = MultiColumnHeaderColumn.ussClassName + "__content";

		// Token: 0x040012A2 RID: 4770
		public static readonly string defaultContentUssClassName = MultiColumnHeaderColumn.ussClassName + "__default-content";

		// Token: 0x040012A3 RID: 4771
		public static readonly string hasIconUssClassName = MultiColumnHeaderColumn.contentUssClassName + "--has-icon";

		// Token: 0x040012A4 RID: 4772
		public static readonly string hasTitleUssClassName = MultiColumnHeaderColumn.contentUssClassName + "--has-title";

		// Token: 0x040012A5 RID: 4773
		public static readonly string titleUssClassName = MultiColumnHeaderColumn.ussClassName + "__title";

		// Token: 0x040012A6 RID: 4774
		public static readonly string iconElementName = "unity-multi-column-header-column-icon";

		// Token: 0x040012A7 RID: 4775
		public static readonly string titleElementName = "unity-multi-column-header-column-title";

		// Token: 0x040012A8 RID: 4776
		private static readonly string s_BoundVEPropertyName = "__bound";

		// Token: 0x040012A9 RID: 4777
		private static readonly string s_BindingCallbackVEPropertyName = "__binding-callback";

		// Token: 0x040012AA RID: 4778
		private static readonly string s_UnbindingCallbackVEPropertyName = "__unbinding-callback";

		// Token: 0x040012AB RID: 4779
		private static readonly string s_DestroyCallbackVEPropertyName = "__destroy-callback";

		// Token: 0x040012AC RID: 4780
		private VisualElement m_ContentContainer;

		// Token: 0x040012AD RID: 4781
		private VisualElement m_Content;

		// Token: 0x040012AE RID: 4782
		private MultiColumnHeaderColumnSortIndicator m_SortIndicatorContainer;

		// Token: 0x040012AF RID: 4783
		private IVisualElementScheduledItem m_ScheduledHeaderTemplateUpdate;
	}
}
