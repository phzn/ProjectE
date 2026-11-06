using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000B1 RID: 177
	public class Foldout : BindableElement, INotifyValueChanged<bool>
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000174FA File Offset: 0x000156FA
		internal Toggle toggle
		{
			get
			{
				return this.m_Toggle;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00017502 File Offset: 0x00015702
		public override VisualElement contentContainer
		{
			get
			{
				return this.m_Container;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001750A File Offset: 0x0001570A
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00017517 File Offset: 0x00015717
		public string text
		{
			get
			{
				return this.m_Toggle.text;
			}
			set
			{
				this.m_Toggle.text = value;
				VisualElement visualElement = this.m_Toggle.visualInput.Q(null, Toggle.textUssClassName);
				if (visualElement != null)
				{
					visualElement.AddToClassList(Foldout.textUssClassName);
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001754E File Offset: 0x0001574E
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00017558 File Offset: 0x00015758
		public bool value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				bool flag = this.m_Value == value;
				if (!flag)
				{
					using (ChangeEvent<bool> pooled = ChangeEvent<bool>.GetPooled(this.m_Value, value))
					{
						pooled.target = this;
						this.SetValueWithoutNotify(value);
						this.SendEvent(pooled);
						base.SaveViewData();
					}
				}
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000175C0 File Offset: 0x000157C0
		public void SetValueWithoutNotify(bool newValue)
		{
			this.m_Value = newValue;
			this.m_Toggle.SetValueWithoutNotify(this.m_Value);
			this.contentContainer.style.display = (newValue ? DisplayStyle.Flex : DisplayStyle.None);
			bool value = this.m_Value;
			if (value)
			{
				base.pseudoStates |= PseudoStates.Checked;
			}
			else
			{
				base.pseudoStates &= ~PseudoStates.Checked;
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00017634 File Offset: 0x00015834
		internal override void OnViewDataReady()
		{
			base.OnViewDataReady();
			string fullHierarchicalViewDataKey = base.GetFullHierarchicalViewDataKey();
			base.OverwriteFromViewData(this, fullHierarchicalViewDataKey);
			this.SetValueWithoutNotify(this.m_Value);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00017668 File Offset: 0x00015868
		private void Apply(KeyboardNavigationOperation op, EventBase sourceEvent)
		{
			bool flag = this.Apply(op);
			if (flag)
			{
				sourceEvent.StopPropagation();
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001768C File Offset: 0x0001588C
		private bool Apply(KeyboardNavigationOperation op)
		{
			bool flag;
			switch (op)
			{
			case KeyboardNavigationOperation.SelectAll:
			case KeyboardNavigationOperation.Cancel:
			case KeyboardNavigationOperation.Submit:
			case KeyboardNavigationOperation.Previous:
			case KeyboardNavigationOperation.Next:
			case KeyboardNavigationOperation.PageUp:
			case KeyboardNavigationOperation.PageDown:
			case KeyboardNavigationOperation.Begin:
			case KeyboardNavigationOperation.End:
				flag = false;
				break;
			case KeyboardNavigationOperation.MoveRight:
				this.SetValueWithoutNotify(true);
				flag = true;
				break;
			case KeyboardNavigationOperation.MoveLeft:
				this.SetValueWithoutNotify(false);
				flag = true;
				break;
			default:
				throw new ArgumentOutOfRangeException("op", op, null);
			}
			return flag;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00017708 File Offset: 0x00015908
		public Foldout()
		{
			base.AddToClassList(Foldout.ussClassName);
			base.delegatesFocus = true;
			this.m_Container = new VisualElement
			{
				name = "unity-content"
			};
			this.m_Toggle.RegisterValueChangedCallback<bool>(delegate(ChangeEvent<bool> evt)
			{
				this.value = this.m_Toggle.value;
				evt.StopPropagation();
			});
			this.m_Toggle.AddToClassList(Foldout.toggleUssClassName);
			this.m_Toggle.visualInput.AddToClassList(Foldout.inputUssClassName);
			this.m_Toggle.visualInput.Q(null, Toggle.checkmarkUssClassName).AddToClassList(Foldout.checkmarkUssClassName);
			this.m_Toggle.AddManipulator(this.m_NavigationManipulator = new KeyboardNavigationManipulator(new Action<KeyboardNavigationOperation, EventBase>(this.Apply)));
			base.hierarchy.Add(this.m_Toggle);
			this.m_Container.AddToClassList(Foldout.contentUssClassName);
			base.hierarchy.Add(this.m_Container);
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnAttachToPanel), TrickleDown.NoTrickleDown);
			this.SetValueWithoutNotify(true);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00017830 File Offset: 0x00015A30
		private void OnAttachToPanel(AttachToPanelEvent evt)
		{
			for (int i = 0; i <= Foldout.ussFoldoutMaxDepth; i++)
			{
				base.RemoveFromClassList(Foldout.ussFoldoutDepthClassName + i.ToString());
			}
			base.RemoveFromClassList(Foldout.ussFoldoutDepthClassName + "max");
			this.m_Toggle.AssignInspectorStyleIfNecessary(Foldout.toggleInspectorUssClassName);
			int foldoutDepth = this.GetFoldoutDepth();
			bool flag = foldoutDepth > Foldout.ussFoldoutMaxDepth;
			if (flag)
			{
				base.AddToClassList(Foldout.ussFoldoutDepthClassName + "max");
			}
			else
			{
				base.AddToClassList(Foldout.ussFoldoutDepthClassName + foldoutDepth.ToString());
			}
		}

		// Token: 0x040002A5 RID: 677
		private Toggle m_Toggle = new Toggle();

		// Token: 0x040002A6 RID: 678
		private VisualElement m_Container;

		// Token: 0x040002A7 RID: 679
		[SerializeField]
		private bool m_Value;

		// Token: 0x040002A8 RID: 680
		public static readonly string ussClassName = "unity-foldout";

		// Token: 0x040002A9 RID: 681
		public static readonly string toggleUssClassName = Foldout.ussClassName + "__toggle";

		// Token: 0x040002AA RID: 682
		public static readonly string contentUssClassName = Foldout.ussClassName + "__content";

		// Token: 0x040002AB RID: 683
		public static readonly string inputUssClassName = Foldout.ussClassName + "__input";

		// Token: 0x040002AC RID: 684
		public static readonly string checkmarkUssClassName = Foldout.ussClassName + "__checkmark";

		// Token: 0x040002AD RID: 685
		public static readonly string textUssClassName = Foldout.ussClassName + "__text";

		// Token: 0x040002AE RID: 686
		internal static readonly string toggleInspectorUssClassName = Foldout.toggleUssClassName + "--inspector";

		// Token: 0x040002AF RID: 687
		internal static readonly string ussFoldoutDepthClassName = Foldout.ussClassName + "--depth-";

		// Token: 0x040002B0 RID: 688
		internal static readonly int ussFoldoutMaxDepth = 4;

		// Token: 0x040002B1 RID: 689
		private KeyboardNavigationManipulator m_NavigationManipulator;

		// Token: 0x020000B2 RID: 178
		public new class UxmlFactory : UxmlFactory<Foldout, Foldout.UxmlTraits>
		{
		}

		// Token: 0x020000B3 RID: 179
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x0600063F RID: 1599 RVA: 0x000179B0 File Offset: 0x00015BB0
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				Foldout foldout = ve as Foldout;
				bool flag = foldout != null;
				if (flag)
				{
					foldout.text = this.m_Text.GetValueFromBag(bag, cc);
					foldout.SetValueWithoutNotify(this.m_Value.GetValueFromBag(bag, cc));
				}
			}

			// Token: 0x040002B2 RID: 690
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};

			// Token: 0x040002B3 RID: 691
			private UxmlBoolAttributeDescription m_Value = new UxmlBoolAttributeDescription
			{
				name = "value",
				defaultValue = true
			};
		}
	}
}
