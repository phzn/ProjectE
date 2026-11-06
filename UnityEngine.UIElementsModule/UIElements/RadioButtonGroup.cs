using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000116 RID: 278
	public class RadioButtonGroup : BaseField<int>, IGroupBox
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00024324 File Offset: 0x00022524
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x00024344 File Offset: 0x00022544
		public IEnumerable<string> choices
		{
			get
			{
				foreach (RadioButton radioButton in this.m_RadioButtons)
				{
					yield return radioButton.text;
					radioButton = null;
				}
				List<RadioButton>.Enumerator enumerator = default(List<RadioButton>.Enumerator);
				yield break;
				yield break;
			}
			set
			{
				bool flag = !value.HasValues();
				if (flag)
				{
					this.m_RadioButtonContainer.Clear();
					bool flag2 = base.panel != null;
					if (!flag2)
					{
						foreach (RadioButton radioButton in this.m_RadioButtons)
						{
							radioButton.UnregisterValueChangedCallback<bool>(this.m_RadioButtonValueChangedCallback);
						}
						this.m_RadioButtons.Clear();
					}
				}
				else
				{
					int num = 0;
					foreach (string text in value)
					{
						bool flag3 = num < this.m_RadioButtons.Count;
						if (flag3)
						{
							this.m_RadioButtons[num].text = text;
							this.m_RadioButtonContainer.Insert(num, this.m_RadioButtons[num]);
						}
						else
						{
							RadioButton radioButton2 = new RadioButton
							{
								text = text
							};
							radioButton2.RegisterValueChangedCallback<bool>(this.m_RadioButtonValueChangedCallback);
							this.m_RadioButtons.Add(radioButton2);
							this.m_RadioButtonContainer.Add(radioButton2);
						}
						num++;
					}
					int num2 = this.m_RadioButtons.Count - 1;
					for (int i = num2; i >= num; i--)
					{
						this.m_RadioButtons[i].RemoveFromHierarchy();
					}
					this.UpdateRadioButtons();
				}
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x000244F0 File Offset: 0x000226F0
		public override VisualElement contentContainer
		{
			get
			{
				return this.m_RadioButtonContainer ?? this;
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000244FD File Offset: 0x000226FD
		public RadioButtonGroup()
			: this(null, null)
		{
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002450C File Offset: 0x0002270C
		public RadioButtonGroup(string label, List<string> radioButtonChoices = null)
			: base(label, null)
		{
			base.AddToClassList(RadioButtonGroup.ussClassName);
			VisualElement visualInput = base.visualInput;
			VisualElement visualElement = new VisualElement();
			visualElement.name = RadioButtonGroup.containerUssClassName;
			VisualElement visualElement2 = visualElement;
			this.m_RadioButtonContainer = visualElement;
			visualInput.Add(visualElement2);
			this.m_RadioButtonContainer.AddToClassList(RadioButtonGroup.containerUssClassName);
			this.m_RadioButtonValueChangedCallback = new EventCallback<ChangeEvent<bool>>(this.RadioButtonValueChangedCallback);
			this.choices = radioButtonChoices;
			this.value = -1;
			base.visualInput.focusable = false;
			base.delegatesFocus = true;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000245A8 File Offset: 0x000227A8
		private void RadioButtonValueChangedCallback(ChangeEvent<bool> evt)
		{
			bool newValue = evt.newValue;
			if (newValue)
			{
				this.value = this.m_RadioButtons.IndexOf(evt.target as RadioButton);
				evt.StopPropagation();
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000245E6 File Offset: 0x000227E6
		public override void SetValueWithoutNotify(int newValue)
		{
			base.SetValueWithoutNotify(newValue);
			this.UpdateRadioButtons();
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000245F8 File Offset: 0x000227F8
		private void UpdateRadioButtons()
		{
			bool flag = this.value >= 0 && this.value < this.m_RadioButtons.Count;
			if (flag)
			{
				this.m_RadioButtons[this.value].value = true;
			}
			else
			{
				foreach (RadioButton radioButton in this.m_RadioButtons)
				{
					radioButton.value = false;
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00024694 File Offset: 0x00022894
		void IGroupBox.OnOptionAdded(IGroupBoxOption option)
		{
			RadioButton radioButton = option as RadioButton;
			bool flag = radioButton == null;
			if (flag)
			{
				throw new ArgumentException("[UI Toolkit] Internal group box error. Expected a radio button element. Please report this using Help -> Report a bug...");
			}
			bool flag2 = this.m_RadioButtons.Contains(radioButton);
			if (!flag2)
			{
				radioButton.RegisterValueChangedCallback<bool>(this.m_RadioButtonValueChangedCallback);
				int num = this.m_RadioButtonContainer.IndexOf(radioButton);
				bool flag3 = num < 0 || num > this.m_RadioButtons.Count;
				if (flag3)
				{
					this.m_RadioButtons.Add(radioButton);
					this.m_RadioButtonContainer.Add(radioButton);
				}
				else
				{
					this.m_RadioButtons.Insert(num, radioButton);
				}
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00024734 File Offset: 0x00022934
		void IGroupBox.OnOptionRemoved(IGroupBoxOption option)
		{
			RadioButton radioButton = option as RadioButton;
			bool flag = radioButton == null;
			if (flag)
			{
				throw new ArgumentException("[UI Toolkit] Internal group box error. Expected a radio button element. Please report this using Help -> Report a bug...");
			}
			int num = this.m_RadioButtons.IndexOf(radioButton);
			radioButton.UnregisterValueChangedCallback<bool>(this.m_RadioButtonValueChangedCallback);
			this.m_RadioButtons.Remove(radioButton);
			bool flag2 = this.value == num;
			if (flag2)
			{
				this.value = -1;
			}
		}

		// Token: 0x0400043C RID: 1084
		public new static readonly string ussClassName = "unity-radio-button-group";

		// Token: 0x0400043D RID: 1085
		public static readonly string containerUssClassName = RadioButtonGroup.ussClassName + "__container";

		// Token: 0x0400043E RID: 1086
		private List<RadioButton> m_RadioButtons = new List<RadioButton>();

		// Token: 0x0400043F RID: 1087
		private EventCallback<ChangeEvent<bool>> m_RadioButtonValueChangedCallback;

		// Token: 0x04000440 RID: 1088
		private VisualElement m_RadioButtonContainer;

		// Token: 0x02000117 RID: 279
		public new class UxmlFactory : UxmlFactory<RadioButtonGroup, RadioButtonGroup.UxmlTraits>
		{
		}

		// Token: 0x02000118 RID: 280
		public new class UxmlTraits : BaseFieldTraits<int, UxmlIntAttributeDescription>
		{
			// Token: 0x06000968 RID: 2408 RVA: 0x000247C4 File Offset: 0x000229C4
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				RadioButtonGroup radioButtonGroup = (RadioButtonGroup)ve;
				radioButtonGroup.choices = BaseField<int>.UxmlTraits.ParseChoiceList(this.m_Choices.GetValueFromBag(bag, cc));
			}

			// Token: 0x04000441 RID: 1089
			private UxmlStringAttributeDescription m_Choices = new UxmlStringAttributeDescription
			{
				name = "choices"
			};
		}
	}
}
