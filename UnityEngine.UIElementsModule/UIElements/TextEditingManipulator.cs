using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200036A RID: 874
	internal class TextEditingManipulator
	{
		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x000707F8 File Offset: 0x0006E9F8
		private bool touchScreenTextFieldChanged
		{
			get
			{
				bool touchScreenTextFieldInitialized = this.m_TouchScreenTextFieldInitialized;
				TextEditingUtilities textEditingUtilities = this.editingUtilities;
				bool? flag = ((textEditingUtilities != null) ? new bool?(textEditingUtilities.TouchScreenKeyboardShouldBeUsed()) : null);
				return !((touchScreenTextFieldInitialized == flag.GetValueOrDefault()) & (flag != null));
			}
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x00070840 File Offset: 0x0006EA40
		public TextEditingManipulator(TextElement textElement)
		{
			this.m_TextElement = textElement;
			this.editingUtilities = new TextEditingUtilities(textElement.selectingManipulator.m_SelectingUtilities, textElement.uitkTextHandle, textElement.text);
			this.InitTextEditorEventHandler();
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x0007088C File Offset: 0x0006EA8C
		private void InitTextEditorEventHandler()
		{
			TextEditingUtilities textEditingUtilities = this.editingUtilities;
			this.m_TouchScreenTextFieldInitialized = textEditingUtilities != null && textEditingUtilities.TouchScreenKeyboardShouldBeUsed();
			bool touchScreenTextFieldInitialized = this.m_TouchScreenTextFieldInitialized;
			if (touchScreenTextFieldInitialized)
			{
				this.editingEventHandler = new TouchScreenTextEditorEventHandler(this.m_TextElement, this.editingUtilities);
			}
			else
			{
				this.editingEventHandler = new KeyboardTextEditorEventHandler(this.m_TextElement, this.editingUtilities);
			}
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000708F0 File Offset: 0x0006EAF0
		internal void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			bool isReadOnly = this.m_TextElement.edition.isReadOnly;
			if (!isReadOnly)
			{
				FocusInEvent focusInEvent = evt as FocusInEvent;
				if (focusInEvent == null)
				{
					FocusOutEvent focusOutEvent = evt as FocusOutEvent;
					if (focusOutEvent != null)
					{
						this.OnFocusOutEvent(focusOutEvent);
					}
				}
				else
				{
					this.OnFocusInEvent(focusInEvent);
				}
				TextEditorEventHandler textEditorEventHandler = this.editingEventHandler;
				if (textEditorEventHandler != null)
				{
					textEditorEventHandler.ExecuteDefaultActionAtTarget(evt);
				}
			}
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0007095C File Offset: 0x0006EB5C
		private void OnFocusInEvent(FocusInEvent _)
		{
			this.m_TextElement.edition.SaveValueAndText();
			this.m_TextElement.focusController.selectedTextElement = this.m_TextElement;
			bool touchScreenTextFieldChanged = this.touchScreenTextFieldChanged;
			if (touchScreenTextFieldChanged)
			{
				this.InitTextEditorEventHandler();
			}
			bool flag = this.m_HardwareKeyboardPoller == null;
			if (flag)
			{
				this.m_HardwareKeyboardPoller = this.m_TextElement.schedule.Execute(delegate
				{
					bool touchScreenTextFieldChanged2 = this.touchScreenTextFieldChanged;
					if (touchScreenTextFieldChanged2)
					{
						this.InitTextEditorEventHandler();
						this.m_TextElement.Blur();
					}
				}).Every(250L);
			}
			else
			{
				this.m_HardwareKeyboardPoller.Resume();
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000709EF File Offset: 0x0006EBEF
		private void OnFocusOutEvent(FocusOutEvent _)
		{
			IVisualElementScheduledItem hardwareKeyboardPoller = this.m_HardwareKeyboardPoller;
			if (hardwareKeyboardPoller != null)
			{
				hardwareKeyboardPoller.Pause();
			}
			this.editingUtilities.OnBlur();
		}

		// Token: 0x04000C3B RID: 3131
		private TextElement m_TextElement;

		// Token: 0x04000C3C RID: 3132
		internal TextEditorEventHandler editingEventHandler;

		// Token: 0x04000C3D RID: 3133
		internal TextEditingUtilities editingUtilities;

		// Token: 0x04000C3E RID: 3134
		private bool m_TouchScreenTextFieldInitialized;

		// Token: 0x04000C3F RID: 3135
		private IVisualElementScheduledItem m_HardwareKeyboardPoller = null;
	}
}
