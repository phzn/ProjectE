using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000081 RID: 129
	internal class ButtonStripField : BaseField<int>
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x0001528C File Offset: 0x0001348C
		public void AddButton(string text, string name = "")
		{
			Button button = this.CreateButton(name);
			button.text = text;
			base.Add(button);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000152B4 File Offset: 0x000134B4
		public void AddButton(Background icon, string name = "")
		{
			Button button = this.CreateButton(name);
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("unity-button-strip-field__button-icon");
			visualElement.style.backgroundImage = icon;
			button.Add(visualElement);
			base.Add(button);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00015300 File Offset: 0x00013500
		private Button CreateButton(string name)
		{
			Button button = new Button
			{
				name = name
			};
			button.AddToClassList("unity-button-strip-field__button");
			button.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnButtonDetachFromPanel), TrickleDown.NoTrickleDown);
			button.clicked += delegate
			{
				this.value = this.m_Buttons.IndexOf(button);
			};
			this.m_Buttons.Add(button);
			base.Add(button);
			this.RefreshButtonsStyling();
			return button;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000153A0 File Offset: 0x000135A0
		private void OnButtonDetachFromPanel(DetachFromPanelEvent evt)
		{
			VisualElement visualElement = evt.currentTarget as VisualElement;
			ButtonStripField buttonStripField;
			bool flag;
			if (visualElement != null)
			{
				buttonStripField = visualElement.parent as ButtonStripField;
				flag = buttonStripField != null;
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				buttonStripField.RefreshButtonsStyling();
				buttonStripField.EnsureValueIsValid();
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000153E4 File Offset: 0x000135E4
		private void RefreshButtonsStyling()
		{
			for (int i = 0; i < this.m_Buttons.Count; i++)
			{
				Button button = this.m_Buttons[i];
				bool flag = this.m_Buttons.Count == 1;
				bool flag2 = i == 0;
				bool flag3 = i == this.m_Buttons.Count - 1;
				button.EnableInClassList("unity-button-strip-field__button--alone", flag);
				button.EnableInClassList("unity-button-strip-field__button--left", !flag && flag2);
				button.EnableInClassList("unity-button-strip-field__button--right", !flag && flag3);
				button.EnableInClassList("unity-button-strip-field__button--middle", !flag && !flag2 && !flag3);
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00015495 File Offset: 0x00013695
		public ButtonStripField()
			: base(null)
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000154AB File Offset: 0x000136AB
		public ButtonStripField(string label)
			: base(label)
		{
			base.AddToClassList("unity-button-strip-field");
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000154CD File Offset: 0x000136CD
		public override void SetValueWithoutNotify(int newValue)
		{
			newValue = Mathf.Clamp(newValue, 0, this.m_Buttons.Count - 1);
			base.SetValueWithoutNotify(newValue);
			this.RefreshButtonsState();
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000154F5 File Offset: 0x000136F5
		private void EnsureValueIsValid()
		{
			this.SetValueWithoutNotify(Mathf.Clamp(this.value, 0, this.m_Buttons.Count - 1));
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00015518 File Offset: 0x00013718
		private void RefreshButtonsState()
		{
			for (int i = 0; i < this.m_Buttons.Count; i++)
			{
				bool flag = i == this.value;
				if (flag)
				{
					this.m_Buttons[i].pseudoStates |= PseudoStates.Checked;
				}
				else
				{
					this.m_Buttons[i].pseudoStates &= ~PseudoStates.Checked;
				}
			}
		}

		// Token: 0x04000222 RID: 546
		public const string className = "unity-button-strip-field";

		// Token: 0x04000223 RID: 547
		private const string k_ButtonClass = "unity-button-strip-field__button";

		// Token: 0x04000224 RID: 548
		private const string k_IconClass = "unity-button-strip-field__button-icon";

		// Token: 0x04000225 RID: 549
		private const string k_ButtonLeftClass = "unity-button-strip-field__button--left";

		// Token: 0x04000226 RID: 550
		private const string k_ButtonMiddleClass = "unity-button-strip-field__button--middle";

		// Token: 0x04000227 RID: 551
		private const string k_ButtonRightClass = "unity-button-strip-field__button--right";

		// Token: 0x04000228 RID: 552
		private const string k_ButtonAloneClass = "unity-button-strip-field__button--alone";

		// Token: 0x04000229 RID: 553
		private List<Button> m_Buttons = new List<Button>();

		// Token: 0x02000082 RID: 130
		public new class UxmlFactory : UxmlFactory<ButtonStripField, ButtonStripField.UxmlTraits>
		{
		}

		// Token: 0x02000083 RID: 131
		public new class UxmlTraits : BaseField<int>.UxmlTraits
		{
		}
	}
}
