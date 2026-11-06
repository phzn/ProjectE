using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000D0 RID: 208
	internal class KeyboardTextEditorEventHandler : TextEditorEventHandler
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x0001A97D File Offset: 0x00018B7D
		public KeyboardTextEditorEventHandler(TextElement textElement, TextEditingUtilities editingUtilities)
			: base(textElement, editingUtilities)
		{
			editingUtilities.multiline = textElement.edition.multiline;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001A9A8 File Offset: 0x00018BA8
		public override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			FocusEvent focusEvent = evt as FocusEvent;
			if (focusEvent == null)
			{
				BlurEvent blurEvent = evt as BlurEvent;
				if (blurEvent == null)
				{
					KeyDownEvent keyDownEvent = evt as KeyDownEvent;
					if (keyDownEvent == null)
					{
						ValidateCommandEvent validateCommandEvent = evt as ValidateCommandEvent;
						if (validateCommandEvent == null)
						{
							ExecuteCommandEvent executeCommandEvent = evt as ExecuteCommandEvent;
							if (executeCommandEvent == null)
							{
								NavigationMoveEvent navigationMoveEvent = evt as NavigationMoveEvent;
								if (navigationMoveEvent == null)
								{
									NavigationSubmitEvent navigationSubmitEvent = evt as NavigationSubmitEvent;
									if (navigationSubmitEvent == null)
									{
										NavigationCancelEvent navigationCancelEvent = evt as NavigationCancelEvent;
										if (navigationCancelEvent != null)
										{
											this.OnNavigationEvent<NavigationCancelEvent>(navigationCancelEvent);
										}
									}
									else
									{
										this.OnNavigationEvent<NavigationSubmitEvent>(navigationSubmitEvent);
									}
								}
								else
								{
									this.OnNavigationEvent<NavigationMoveEvent>(navigationMoveEvent);
								}
							}
							else
							{
								this.OnExecuteCommandEvent(executeCommandEvent);
							}
						}
						else
						{
							this.OnValidateCommandEvent(validateCommandEvent);
						}
					}
					else
					{
						this.OnKeyDown(keyDownEvent);
					}
				}
				else
				{
					this.OnBlur(blurEvent);
				}
			}
			else
			{
				this.OnFocus(focusEvent);
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001AA8B File Offset: 0x00018C8B
		private void OnFocus(FocusEvent _)
		{
			GUIUtility.imeCompositionMode = IMECompositionMode.On;
			this.textElement.edition.SaveValueAndText();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001AAA6 File Offset: 0x00018CA6
		private void OnBlur(BlurEvent _)
		{
			GUIUtility.imeCompositionMode = IMECompositionMode.Auto;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001AAB0 File Offset: 0x00018CB0
		private void OnKeyDown(KeyDownEvent evt)
		{
			bool flag = !this.textElement.hasFocus;
			if (!flag)
			{
				this.m_Changed = false;
				evt.GetEquivalentImguiEvent(this.m_ImguiEvent);
				bool flag2 = this.editingUtilities.HandleKeyEvent(this.m_ImguiEvent);
				if (flag2)
				{
					bool flag3 = this.textElement.text != this.editingUtilities.text;
					if (flag3)
					{
						this.m_Changed = true;
					}
					evt.StopPropagation();
				}
				else
				{
					char c = evt.character;
					bool flag4 = evt.actionKey && (!evt.altKey || c == '\0');
					if (flag4)
					{
						return;
					}
					bool flag5 = c == '\t' && evt.keyCode == KeyCode.None && evt.modifiers == EventModifiers.None;
					if (flag5)
					{
						return;
					}
					bool flag6 = evt.keyCode == KeyCode.Tab || (evt.keyCode == KeyCode.Tab && evt.character == '\t' && evt.modifiers == EventModifiers.Shift);
					if (flag6)
					{
						bool flag7 = !this.textElement.edition.multiline || evt.shiftKey;
						if (flag7)
						{
							bool flag8 = evt.ShouldSendNavigationMoveEvent();
							if (flag8)
							{
								this.textElement.focusController.FocusNextInDirection(evt.shiftKey ? VisualElementFocusChangeDirection.left : VisualElementFocusChangeDirection.right);
								evt.StopPropagation();
							}
							return;
						}
						bool flag9 = !evt.ShouldSendNavigationMoveEvent();
						if (flag9)
						{
							return;
						}
					}
					bool flag10 = !this.textElement.edition.multiline && (evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return);
					if (flag10)
					{
						Action updateValueFromText = this.textElement.edition.UpdateValueFromText;
						if (updateValueFromText != null)
						{
							updateValueFromText();
						}
					}
					evt.StopPropagation();
					bool flag11 = (this.textElement.edition.multiline ? (c == '\n' && evt.shiftKey) : ((c == '\n' || c == '\r' || c == '\n') && !evt.altKey));
					if (flag11)
					{
						Action moveFocusToCompositeRoot = this.textElement.edition.MoveFocusToCompositeRoot;
						if (moveFocusToCompositeRoot != null)
						{
							moveFocusToCompositeRoot();
						}
						return;
					}
					bool flag12 = evt.keyCode == KeyCode.Escape;
					if (flag12)
					{
						this.textElement.edition.RestoreValueAndText();
						Action updateValueFromText2 = this.textElement.edition.UpdateValueFromText;
						if (updateValueFromText2 != null)
						{
							updateValueFromText2();
						}
						Action moveFocusToCompositeRoot2 = this.textElement.edition.MoveFocusToCompositeRoot;
						if (moveFocusToCompositeRoot2 != null)
						{
							moveFocusToCompositeRoot2();
						}
					}
					bool flag13 = evt.keyCode == KeyCode.Tab;
					if (flag13)
					{
						c = '\t';
					}
					bool flag14 = !this.textElement.edition.AcceptCharacter(c);
					if (flag14)
					{
						return;
					}
					bool flag15 = c >= ' ' || evt.keyCode == KeyCode.Tab || (this.textElement.edition.multiline && !evt.altKey && (c == '\n' || c == '\r' || c == '\n'));
					if (flag15)
					{
						this.editingUtilities.Insert(c);
						this.m_Changed = true;
					}
					else
					{
						bool isCompositionActive = this.editingUtilities.isCompositionActive;
						bool flag16 = this.editingUtilities.UpdateImeState() || isCompositionActive != this.editingUtilities.isCompositionActive;
						if (flag16)
						{
							this.m_Changed = true;
						}
					}
				}
				bool changed = this.m_Changed;
				if (changed)
				{
					this.UpdateLabel();
				}
				Action<bool> updateScrollOffset = this.textElement.edition.UpdateScrollOffset;
				if (updateScrollOffset != null)
				{
					updateScrollOffset(evt.keyCode == KeyCode.Backspace);
				}
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001AE54 File Offset: 0x00019054
		private void UpdateLabel()
		{
			string text = this.editingUtilities.text;
			bool flag = this.editingUtilities.UpdateImeState();
			bool flag2 = flag && this.editingUtilities.ShouldUpdateImeWindowPosition();
			if (flag2)
			{
				this.editingUtilities.SetImeWindowPosition(new Vector2(this.textElement.worldBound.x, this.textElement.worldBound.y));
			}
			string text2 = this.editingUtilities.GeneratePreviewString(this.textElement.enableRichText);
			this.textElement.edition.UpdateText(text2);
			bool flag3 = !this.textElement.edition.isDelayed;
			if (flag3)
			{
				Action updateValueFromText = this.textElement.edition.UpdateValueFromText;
				if (updateValueFromText != null)
				{
					updateValueFromText();
				}
			}
			bool flag4 = flag;
			if (flag4)
			{
				this.editingUtilities.text = text;
				this.editingUtilities.EnableCursorPreviewState();
			}
			this.textElement.uitkTextHandle.Update();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001AF5C File Offset: 0x0001915C
		private void OnValidateCommandEvent(ValidateCommandEvent evt)
		{
			bool flag = !this.textElement.hasFocus;
			if (!flag)
			{
				string commandName = evt.commandName;
				string text = commandName;
				if (!(text == "Copy") && !(text == "SelectAll"))
				{
					if (!(text == "Cut"))
					{
						if (!(text == "Paste"))
						{
							if (!(text == "Delete"))
							{
								if (!(text == "UndoRedoPerformed"))
								{
								}
							}
						}
						else
						{
							bool flag2 = !this.editingUtilities.CanPaste();
							if (flag2)
							{
								return;
							}
						}
					}
					else
					{
						bool flag3 = !this.textElement.selection.HasSelection();
						if (flag3)
						{
							return;
						}
					}
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001B01C File Offset: 0x0001921C
		private void OnExecuteCommandEvent(ExecuteCommandEvent evt)
		{
			bool flag = !this.textElement.hasFocus;
			if (!flag)
			{
				this.m_Changed = false;
				bool flag2 = false;
				string text = this.editingUtilities.text;
				string commandName = evt.commandName;
				string text2 = commandName;
				if (!(text2 == "OnLostFocus"))
				{
					if (!(text2 == "Cut"))
					{
						if (!(text2 == "Paste"))
						{
							if (text2 == "Delete")
							{
								this.editingUtilities.Cut();
								flag2 = true;
								evt.StopPropagation();
							}
						}
						else
						{
							this.editingUtilities.Paste();
							flag2 = true;
							evt.StopPropagation();
						}
					}
					else
					{
						this.editingUtilities.Cut();
						flag2 = true;
						evt.StopPropagation();
					}
					bool flag3 = flag2;
					if (flag3)
					{
						bool flag4 = text != this.editingUtilities.text;
						if (flag4)
						{
							this.m_Changed = true;
						}
						evt.StopPropagation();
					}
					bool changed = this.m_Changed;
					if (changed)
					{
						this.UpdateLabel();
					}
					Action<bool> updateScrollOffset = this.textElement.edition.UpdateScrollOffset;
					if (updateScrollOffset != null)
					{
						updateScrollOffset(false);
					}
				}
				else
				{
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001B148 File Offset: 0x00019348
		private void OnNavigationEvent<TEvent>(NavigationEventBase<TEvent> evt) where TEvent : NavigationEventBase<TEvent>, new()
		{
			bool flag = evt.deviceType == NavigationDeviceType.Keyboard || evt.deviceType == NavigationDeviceType.Unknown;
			if (flag)
			{
				evt.StopPropagation();
				evt.PreventDefault();
			}
		}

		// Token: 0x04000318 RID: 792
		private readonly Event m_ImguiEvent = new Event();

		// Token: 0x04000319 RID: 793
		internal bool m_Changed;

		// Token: 0x0400031A RID: 794
		private const int k_LineFeed = 10;

		// Token: 0x0400031B RID: 795
		private const int k_Space = 32;
	}
}
