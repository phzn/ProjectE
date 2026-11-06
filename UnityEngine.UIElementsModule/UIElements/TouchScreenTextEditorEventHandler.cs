using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000D9 RID: 217
	internal class TouchScreenTextEditorEventHandler : TextEditorEventHandler
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x0001CDBB File Offset: 0x0001AFBB
		public TouchScreenTextEditorEventHandler(TextElement textElement, TextEditingUtilities editingUtilities)
			: base(textElement, editingUtilities)
		{
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001CDDC File Offset: 0x0001AFDC
		private void PollTouchScreenKeyboard()
		{
			this.m_TouchKeyboardAllowsInPlaceEditing = TouchScreenKeyboard.isInPlaceEditingAllowed;
			bool flag = TouchScreenKeyboard.isSupported && !this.m_TouchKeyboardAllowsInPlaceEditing;
			if (flag)
			{
				bool flag2 = this.m_TouchKeyboardPoller == null;
				if (flag2)
				{
					TextElement textElement = this.textElement;
					this.m_TouchKeyboardPoller = ((textElement != null) ? textElement.schedule.Execute(new Action(this.DoPollTouchScreenKeyboard)).Every(100L) : null);
				}
				else
				{
					this.m_TouchKeyboardPoller.Resume();
				}
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001CE5C File Offset: 0x0001B05C
		private void DoPollTouchScreenKeyboard()
		{
			bool flag = this.editingUtilities.TouchScreenKeyboardShouldBeUsed();
			if (flag)
			{
				bool flag2 = this.textElement.m_TouchScreenKeyboard == null;
				if (!flag2)
				{
					ITextEdition edition = this.textElement.edition;
					TouchScreenKeyboard touchScreenKeyboard = this.textElement.m_TouchScreenKeyboard;
					string text = touchScreenKeyboard.text;
					bool flag3 = touchScreenKeyboard.status > TouchScreenKeyboard.Status.Visible;
					if (flag3)
					{
						bool flag4 = touchScreenKeyboard.status == TouchScreenKeyboard.Status.Canceled;
						if (flag4)
						{
							edition.RestoreValueAndText();
						}
						else
						{
							text = touchScreenKeyboard.text;
							bool flag5 = this.editingUtilities.text != text;
							if (flag5)
							{
								edition.UpdateText(text);
								this.textElement.uitkTextHandle.Update();
							}
						}
						this.CloseTouchScreenKeyboard();
						bool flag6 = !edition.isDelayed;
						if (flag6)
						{
							Action updateValueFromText = edition.UpdateValueFromText;
							if (updateValueFromText != null)
							{
								updateValueFromText();
							}
						}
						Action updateTextFromValue = edition.UpdateTextFromValue;
						if (updateTextFromValue != null)
						{
							updateTextFromValue();
						}
						this.textElement.Blur();
					}
					else
					{
						bool flag7 = this.editingUtilities.text == text;
						if (!flag7)
						{
							bool hideMobileInput = edition.hideMobileInput;
							if (hideMobileInput)
							{
								bool flag8 = this.editingUtilities.text != text;
								if (flag8)
								{
									bool flag9 = false;
									this.editingUtilities.text = "";
									foreach (char c in text)
									{
										bool flag10 = !edition.AcceptCharacter(c);
										if (flag10)
										{
											return;
										}
										bool flag11 = c > '\0';
										if (flag11)
										{
											TextEditingUtilities editingUtilities = this.editingUtilities;
											editingUtilities.text += c.ToString();
											flag9 = true;
										}
									}
									bool flag12 = flag9;
									if (flag12)
									{
										this.UpdateStringPositionFromKeyboard();
									}
									edition.UpdateText(this.editingUtilities.text);
									this.textElement.uitkTextHandle.Update();
								}
								else
								{
									bool flag13 = !this.m_IsClicking && touchScreenKeyboard != null && touchScreenKeyboard.canGetSelection;
									if (flag13)
									{
										this.UpdateStringPositionFromKeyboard();
									}
								}
							}
							else
							{
								edition.UpdateText(text);
								this.textElement.uitkTextHandle.Update();
							}
							bool flag14 = !edition.isDelayed;
							if (flag14)
							{
								Action updateValueFromText2 = edition.UpdateValueFromText;
								if (updateValueFromText2 != null)
								{
									updateValueFromText2();
								}
							}
							Action updateTextFromValue2 = edition.UpdateTextFromValue;
							if (updateTextFromValue2 != null)
							{
								updateTextFromValue2();
							}
							Action<bool> updateScrollOffset = this.textElement.edition.UpdateScrollOffset;
							if (updateScrollOffset != null)
							{
								updateScrollOffset(false);
							}
						}
					}
				}
			}
			else
			{
				this.CloseTouchScreenKeyboard();
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001D108 File Offset: 0x0001B308
		private void UpdateStringPositionFromKeyboard()
		{
			bool flag = this.textElement.m_TouchScreenKeyboard == null;
			if (!flag)
			{
				RangeInt selection = this.textElement.m_TouchScreenKeyboard.selection;
				int start = selection.start;
				int end = selection.end;
				bool flag2 = this.textElement.selection.selectIndex != start;
				if (flag2)
				{
					this.textElement.selection.selectIndex = start;
				}
				bool flag3 = this.textElement.selection.cursorIndex != end;
				if (flag3)
				{
					this.textElement.selection.cursorIndex = end;
				}
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		private void CloseTouchScreenKeyboard()
		{
			bool flag = this.textElement.m_TouchScreenKeyboard != null;
			if (flag)
			{
				this.textElement.m_TouchScreenKeyboard.active = false;
				this.textElement.m_TouchScreenKeyboard = null;
				IVisualElementScheduledItem touchKeyboardPoller = this.m_TouchKeyboardPoller;
				if (touchKeyboardPoller != null)
				{
					touchKeyboardPoller.Pause();
				}
				TouchScreenKeyboard.hideInput = true;
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001D204 File Offset: 0x0001B404
		private void OpenTouchScreenKeyboard()
		{
			ITextEdition edition = this.textElement.edition;
			TouchScreenKeyboard.hideInput = edition.hideMobileInput;
			this.textElement.m_TouchScreenKeyboard = TouchScreenKeyboard.Open(this.textElement.text, edition.keyboardType, !edition.isPassword && edition.autoCorrection, edition.multiline, edition.isPassword);
			bool hideMobileInput = edition.hideMobileInput;
			if (hideMobileInput)
			{
				int selectIndex = this.textElement.selection.selectIndex;
				int cursorIndex = this.textElement.selection.cursorIndex;
				int num = ((selectIndex < cursorIndex) ? (cursorIndex - selectIndex) : (selectIndex - cursorIndex));
				int num2 = ((selectIndex < cursorIndex) ? selectIndex : cursorIndex);
				this.textElement.m_TouchScreenKeyboard.selection = new RangeInt(num2, num);
			}
			else
			{
				TouchScreenKeyboard touchScreenKeyboard = this.textElement.m_TouchScreenKeyboard;
				string text = this.textElement.m_TouchScreenKeyboard.text;
				touchScreenKeyboard.selection = new RangeInt((text != null) ? text.Length : 0, 0);
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001D300 File Offset: 0x0001B500
		public override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			bool flag = !this.editingUtilities.TouchScreenKeyboardShouldBeUsed() || this.textElement.edition.isReadOnly;
			if (!flag)
			{
				if (!(evt is PointerDownEvent))
				{
					PointerUpEvent pointerUpEvent = evt as PointerUpEvent;
					if (pointerUpEvent == null)
					{
						if (!(evt is FocusInEvent))
						{
							FocusOutEvent focusOutEvent = evt as FocusOutEvent;
							if (focusOutEvent != null)
							{
								this.OnFocusOutEvent(focusOutEvent);
							}
						}
						else
						{
							this.OnFocusInEvent();
						}
					}
					else
					{
						this.OnPointerUpEvent(pointerUpEvent);
					}
				}
				else
				{
					this.OnPointerDownEvent();
				}
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001D394 File Offset: 0x0001B594
		private void OnPointerDownEvent()
		{
			this.m_IsClicking = true;
			bool flag = this.textElement.m_TouchScreenKeyboard != null && this.textElement.edition.hideMobileInput;
			if (flag)
			{
				int num = this.textElement.selection.cursorIndex;
				string text = this.textElement.m_TouchScreenKeyboard.text;
				int num2 = ((text != null) ? text.Length : 0);
				bool flag2 = num < 0;
				if (flag2)
				{
					num = 0;
				}
				bool flag3 = num > num2;
				if (flag3)
				{
					num = num2;
				}
				this.textElement.m_TouchScreenKeyboard.selection = new RangeInt(num, 0);
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001D42B File Offset: 0x0001B62B
		private void OnPointerUpEvent(PointerUpEvent evt)
		{
			this.m_IsClicking = false;
			evt.StopPropagation();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001D43C File Offset: 0x0001B63C
		private void OnFocusInEvent()
		{
			bool flag = this.textElement.m_TouchScreenKeyboard != null;
			if (!flag)
			{
				this.OpenTouchScreenKeyboard();
				bool flag2 = this.textElement.m_TouchScreenKeyboard != null;
				if (flag2)
				{
					this.PollTouchScreenKeyboard();
				}
				this.textElement.edition.SaveValueAndText();
				Action<bool> updateScrollOffset = this.textElement.edition.UpdateScrollOffset;
				if (updateScrollOffset != null)
				{
					updateScrollOffset(false);
				}
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001D4AC File Offset: 0x0001B6AC
		private void OnFocusOutEvent(FocusOutEvent evt)
		{
			TextElement textElement = (TextElement)evt.target;
			TextElement textElement2 = textElement.focusController.m_LastPendingFocusedElement as TextElement;
			bool flag = textElement2 == textElement || textElement2 == null || textElement2.edition.keyboardType != textElement.edition.keyboardType || textElement2.edition.multiline != textElement.edition.multiline || textElement2.edition.hideMobileInput != textElement.edition.hideMobileInput;
			if (flag)
			{
				this.CloseTouchScreenKeyboard();
			}
			else
			{
				this.textElement.m_TouchScreenKeyboard = null;
				IVisualElementScheduledItem touchKeyboardPoller = this.m_TouchKeyboardPoller;
				if (touchKeyboardPoller != null)
				{
					touchKeyboardPoller.Pause();
				}
			}
		}

		// Token: 0x0400034C RID: 844
		private IVisualElementScheduledItem m_TouchKeyboardPoller = null;

		// Token: 0x0400034D RID: 845
		private bool m_TouchKeyboardAllowsInPlaceEditing = false;

		// Token: 0x0400034E RID: 846
		private bool m_IsClicking = false;
	}
}
