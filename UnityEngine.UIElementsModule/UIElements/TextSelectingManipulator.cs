using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200036B RID: 875
	internal class TextSelectingManipulator
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x00070A3D File Offset: 0x0006EC3D
		// (set) Token: 0x06001D06 RID: 7430 RVA: 0x00070A48 File Offset: 0x0006EC48
		internal bool isClicking
		{
			get
			{
				return this.m_IsClicking;
			}
			set
			{
				bool flag = this.m_IsClicking == value;
				if (!flag)
				{
					this.m_IsClicking = value;
				}
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x00070A6C File Offset: 0x0006EC6C
		public TextSelectingManipulator(TextElement textElement)
		{
			this.m_TextElement = textElement;
			this.m_SelectingUtilities = new TextSelectingUtilities(this.m_TextElement.uitkTextHandle);
			TextSelectingUtilities selectingUtilities = this.m_SelectingUtilities;
			selectingUtilities.OnCursorIndexChange = (Action)Delegate.Combine(selectingUtilities.OnCursorIndexChange, new Action(this.OnCursorIndexChange));
			TextSelectingUtilities selectingUtilities2 = this.m_SelectingUtilities;
			selectingUtilities2.OnSelectIndexChange = (Action)Delegate.Combine(selectingUtilities2.OnSelectIndexChange, new Action(this.OnSelectIndexChange));
			TextSelectingUtilities selectingUtilities3 = this.m_SelectingUtilities;
			selectingUtilities3.OnRevealCursorChange = (Action)Delegate.Combine(selectingUtilities3.OnRevealCursorChange, new Action(this.OnRevealCursor));
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x00070B26 File Offset: 0x0006ED26
		// (set) Token: 0x06001D09 RID: 7433 RVA: 0x00070B3A File Offset: 0x0006ED3A
		internal int cursorIndex
		{
			get
			{
				TextSelectingUtilities selectingUtilities = this.m_SelectingUtilities;
				return (selectingUtilities != null) ? selectingUtilities.cursorIndex : (-1);
			}
			set
			{
				this.m_SelectingUtilities.cursorIndex = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x00070B49 File Offset: 0x0006ED49
		// (set) Token: 0x06001D0B RID: 7435 RVA: 0x00070B5D File Offset: 0x0006ED5D
		internal int selectIndex
		{
			get
			{
				TextSelectingUtilities selectingUtilities = this.m_SelectingUtilities;
				return (selectingUtilities != null) ? selectingUtilities.selectIndex : (-1);
			}
			set
			{
				this.m_SelectingUtilities.selectIndex = value;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00070B6C File Offset: 0x0006ED6C
		private void OnRevealCursor()
		{
			this.m_TextElement.IncrementVersion(VersionChangeType.Repaint);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00070B80 File Offset: 0x0006ED80
		private void OnSelectIndexChange()
		{
			this.m_TextElement.IncrementVersion(VersionChangeType.Repaint);
			bool flag = this.HasSelection() && this.m_TextElement.focusController != null;
			if (flag)
			{
				this.m_TextElement.focusController.selectedTextElement = this.m_TextElement;
			}
			bool revealCursor = this.m_SelectingUtilities.revealCursor;
			if (revealCursor)
			{
				Action<bool> updateScrollOffset = this.m_TextElement.edition.UpdateScrollOffset;
				if (updateScrollOffset != null)
				{
					updateScrollOffset(false);
				}
			}
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x00070C00 File Offset: 0x0006EE00
		private void OnCursorIndexChange()
		{
			this.m_TextElement.IncrementVersion(VersionChangeType.Repaint);
			bool flag = this.HasSelection() && this.m_TextElement.focusController != null;
			if (flag)
			{
				this.m_TextElement.focusController.selectedTextElement = this.m_TextElement;
			}
			bool revealCursor = this.m_SelectingUtilities.revealCursor;
			if (revealCursor)
			{
				Action<bool> updateScrollOffset = this.m_TextElement.edition.UpdateScrollOffset;
				if (updateScrollOffset != null)
				{
					updateScrollOffset(false);
				}
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00070C80 File Offset: 0x0006EE80
		internal bool RevealCursor()
		{
			return this.m_SelectingUtilities.revealCursor;
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00070CA0 File Offset: 0x0006EEA0
		internal bool HasSelection()
		{
			return this.m_SelectingUtilities.hasSelection;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x00070CC0 File Offset: 0x0006EEC0
		internal bool HasFocus()
		{
			return this.m_TextElement.hasFocus;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x00070CE0 File Offset: 0x0006EEE0
		internal void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			FocusEvent focusEvent = evt as FocusEvent;
			if (focusEvent == null)
			{
				BlurEvent blurEvent = evt as BlurEvent;
				if (blurEvent == null)
				{
					PointerDownEvent pointerDownEvent = evt as PointerDownEvent;
					if (pointerDownEvent == null)
					{
						KeyDownEvent keyDownEvent = evt as KeyDownEvent;
						if (keyDownEvent == null)
						{
							PointerMoveEvent pointerMoveEvent = evt as PointerMoveEvent;
							if (pointerMoveEvent == null)
							{
								PointerUpEvent pointerUpEvent = evt as PointerUpEvent;
								if (pointerUpEvent == null)
								{
									ValidateCommandEvent validateCommandEvent = evt as ValidateCommandEvent;
									if (validateCommandEvent == null)
									{
										ExecuteCommandEvent executeCommandEvent = evt as ExecuteCommandEvent;
										if (executeCommandEvent != null)
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
									this.OnPointerUpEvent(pointerUpEvent);
								}
							}
							else
							{
								this.OnPointerMoveEvent(pointerMoveEvent);
							}
						}
						else
						{
							this.OnKeyDown(keyDownEvent);
						}
					}
					else
					{
						this.OnPointerDownEvent(pointerDownEvent);
					}
				}
				else
				{
					this.OnBlurEvent(blurEvent);
				}
			}
			else
			{
				this.OnFocusEvent(focusEvent);
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x00070DBC File Offset: 0x0006EFBC
		private void OnFocusEvent(FocusEvent evt)
		{
			this.selectAllOnMouseUp = false;
			bool flag = PointerDeviceState.GetPressedButtons(PointerId.mousePointerId) != 0 || (this.m_TextElement.panel.contextType == ContextType.Editor && Event.current == null);
			if (flag)
			{
				this.selectAllOnMouseUp = this.m_TextElement.selection.selectAllOnMouseUp;
			}
			this.m_SelectingUtilities.OnFocus(this.m_TextElement.selection.selectAllOnFocus);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x00070E34 File Offset: 0x0006F034
		private void OnBlurEvent(BlurEvent evt)
		{
			this.selectAllOnMouseUp = this.m_TextElement.selection.selectAllOnMouseUp;
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x00070E50 File Offset: 0x0006F050
		private void OnKeyDown(KeyDownEvent evt)
		{
			bool flag = !this.m_TextElement.hasFocus;
			if (!flag)
			{
				evt.GetEquivalentImguiEvent(this.m_ImguiEvent);
				bool flag2 = this.m_SelectingUtilities.HandleKeyEvent(this.m_ImguiEvent);
				if (flag2)
				{
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00070E9C File Offset: 0x0006F09C
		private void OnPointerDownEvent(PointerDownEvent evt)
		{
			Vector3 vector = evt.localPosition - this.m_TextElement.contentRect.min;
			bool flag = evt.button == 0;
			if (flag)
			{
				bool flag2 = evt.timestamp - this.m_LastMouseDownTimeStamp < (long)Event.GetDoubleClickTime();
				if (flag2)
				{
					this.m_ConsecutiveMouseDownCount++;
				}
				else
				{
					this.m_ConsecutiveMouseDownCount = 1;
				}
				bool flag3 = this.m_ConsecutiveMouseDownCount == 2 && this.m_TextElement.selection.doubleClickSelectsWord;
				if (flag3)
				{
					bool flag4 = this.cursorIndex == 0 && this.cursorIndex != this.selectIndex;
					if (flag4)
					{
						this.m_SelectingUtilities.MoveCursorToPosition_Internal(vector, evt.shiftKey);
					}
					this.m_SelectingUtilities.SelectCurrentWord();
					this.m_SelectingUtilities.MouseDragSelectsWholeWords(true);
					this.m_SelectingUtilities.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
				}
				else
				{
					bool flag5 = this.m_ConsecutiveMouseDownCount == 3 && this.m_TextElement.selection.tripleClickSelectsLine;
					if (flag5)
					{
						this.m_SelectingUtilities.SelectCurrentParagraph();
						this.m_SelectingUtilities.MouseDragSelectsWholeWords(true);
						this.m_SelectingUtilities.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
					}
					else
					{
						this.m_SelectingUtilities.MoveCursorToPosition_Internal(vector, evt.shiftKey);
						Action<bool> updateScrollOffset = this.m_TextElement.edition.UpdateScrollOffset;
						if (updateScrollOffset != null)
						{
							updateScrollOffset(false);
						}
						this.m_SelectingUtilities.MouseDragSelectsWholeWords(false);
						this.m_SelectingUtilities.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
					}
				}
				this.m_LastMouseDownTimeStamp = evt.timestamp;
				this.isClicking = true;
				this.m_TextElement.CapturePointer(evt.pointerId);
				this.m_ClickStartPosition = vector;
			}
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x00071068 File Offset: 0x0006F268
		private void OnPointerMoveEvent(PointerMoveEvent evt)
		{
			bool flag = !this.isClicking;
			if (!flag)
			{
				Vector3 vector = evt.localPosition - this.m_TextElement.contentRect.min;
				this.m_Dragged = this.m_Dragged || this.MoveDistanceQualifiesForDrag(this.m_ClickStartPosition, vector);
				bool dragged = this.m_Dragged;
				if (dragged)
				{
					this.m_SelectingUtilities.SelectToPosition(vector);
					Action<bool> updateScrollOffset = this.m_TextElement.edition.UpdateScrollOffset;
					if (updateScrollOffset != null)
					{
						updateScrollOffset(false);
					}
					this.selectAllOnMouseUp = this.m_TextElement.selection.selectAllOnMouseUp && !this.m_SelectingUtilities.hasSelection;
				}
				evt.StopPropagation();
			}
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x0007113C File Offset: 0x0006F33C
		private void OnPointerUpEvent(PointerUpEvent evt)
		{
			bool flag = evt.button != 0 || !this.isClicking;
			if (!flag)
			{
				bool flag2 = this.selectAllOnMouseUp;
				if (flag2)
				{
					this.m_SelectingUtilities.SelectAll();
				}
				this.selectAllOnMouseUp = false;
				this.m_Dragged = false;
				this.isClicking = false;
				this.m_TextElement.ReleasePointer(evt.pointerId);
				evt.StopPropagation();
			}
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000711AC File Offset: 0x0006F3AC
		private void OnValidateCommandEvent(ValidateCommandEvent evt)
		{
			bool flag = !this.m_TextElement.hasFocus;
			if (!flag)
			{
				string commandName = evt.commandName;
				string text = commandName;
				if (!(text == "Cut") && !(text == "Paste") && !(text == "Delete") && !(text == "UndoRedoPerformed"))
				{
					if (!(text == "Copy"))
					{
						if (!(text == "SelectAll"))
						{
						}
					}
					else
					{
						bool flag2 = !this.m_SelectingUtilities.hasSelection;
						if (flag2)
						{
							return;
						}
					}
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00071248 File Offset: 0x0006F448
		private void OnExecuteCommandEvent(ExecuteCommandEvent evt)
		{
			bool flag = !this.m_TextElement.hasFocus;
			if (!flag)
			{
				string commandName = evt.commandName;
				string text = commandName;
				if (!(text == "OnLostFocus"))
				{
					if (!(text == "Copy"))
					{
						if (text == "SelectAll")
						{
							this.m_SelectingUtilities.SelectAll();
							evt.StopPropagation();
						}
					}
					else
					{
						this.m_SelectingUtilities.Copy();
						evt.StopPropagation();
					}
				}
				else
				{
					evt.StopPropagation();
				}
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000712D0 File Offset: 0x0006F4D0
		private bool MoveDistanceQualifiesForDrag(Vector2 start, Vector2 current)
		{
			return (start - current).sqrMagnitude >= 16f;
		}

		// Token: 0x04000C40 RID: 3136
		internal TextSelectingUtilities m_SelectingUtilities;

		// Token: 0x04000C41 RID: 3137
		private bool selectAllOnMouseUp;

		// Token: 0x04000C42 RID: 3138
		private TextElement m_TextElement;

		// Token: 0x04000C43 RID: 3139
		private Vector2 m_ClickStartPosition;

		// Token: 0x04000C44 RID: 3140
		private bool m_Dragged;

		// Token: 0x04000C45 RID: 3141
		private bool m_IsClicking;

		// Token: 0x04000C46 RID: 3142
		private const int k_DragThresholdSqr = 16;

		// Token: 0x04000C47 RID: 3143
		private int m_ConsecutiveMouseDownCount;

		// Token: 0x04000C48 RID: 3144
		private long m_LastMouseDownTimeStamp = 0L;

		// Token: 0x04000C49 RID: 3145
		private readonly Event m_ImguiEvent = new Event();
	}
}
