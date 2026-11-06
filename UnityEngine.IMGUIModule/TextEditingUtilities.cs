using System;
using System.Collections.Generic;
using UnityEngine.Bindings;
using UnityEngine.TextCore.Text;

namespace UnityEngine
{
	// Token: 0x0200003E RID: 62
	internal class TextEditingUtilities
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x000106A3 File Offset: 0x0000E8A3
		private bool hasSelection
		{
			get
			{
				return this.m_TextSelectingUtility.hasSelection;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x000106B0 File Offset: 0x0000E8B0
		private string SelectedText
		{
			get
			{
				return this.m_TextSelectingUtility.selectedText;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x000106BD File Offset: 0x0000E8BD
		private int m_iAltCursorPos
		{
			get
			{
				return this.m_TextSelectingUtility.iAltCursorPos;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000106CC File Offset: 0x0000E8CC
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x000106E9 File Offset: 0x0000E8E9
		internal bool revealCursor
		{
			get
			{
				return this.m_TextSelectingUtility.revealCursor;
			}
			set
			{
				this.m_TextSelectingUtility.revealCursor = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000106FC File Offset: 0x0000E8FC
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00010719 File Offset: 0x0000E919
		private int cursorIndex
		{
			get
			{
				return this.m_TextSelectingUtility.cursorIndex;
			}
			set
			{
				this.m_TextSelectingUtility.cursorIndex = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0001072C File Offset: 0x0000E92C
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x00010749 File Offset: 0x0000E949
		private int selectIndex
		{
			get
			{
				return this.m_TextSelectingUtility.selectIndex;
			}
			set
			{
				this.m_TextSelectingUtility.selectIndex = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001075C File Offset: 0x0000E95C
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00010774 File Offset: 0x0000E974
		public string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				bool flag = value == this.m_Text;
				if (!flag)
				{
					this.m_Text = value ?? string.Empty;
				}
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000107A4 File Offset: 0x0000E9A4
		public TextEditingUtilities(TextSelectingUtilities selectingUtilities, TextHandle textHandle, string text)
		{
			this.m_TextSelectingUtility = selectingUtilities;
			this.m_TextHandle = textHandle;
			this.m_Text = text;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000107D4 File Offset: 0x0000E9D4
		public bool UpdateImeState()
		{
			bool flag = GUIUtility.compositionString.Length > 0;
			if (flag)
			{
				bool flag2 = !this.isCompositionActive;
				if (flag2)
				{
					this.m_UpdateImeWindowPosition = true;
					this.ReplaceSelection(string.Empty);
				}
				this.isCompositionActive = true;
			}
			else
			{
				this.isCompositionActive = false;
			}
			return this.isCompositionActive;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00010834 File Offset: 0x0000EA34
		public bool ShouldUpdateImeWindowPosition()
		{
			return this.m_UpdateImeWindowPosition;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001084C File Offset: 0x0000EA4C
		public void SetImeWindowPosition(Vector2 worldPosition)
		{
			Vector2 cursorPositionFromStringIndexUsingCharacterHeight = this.m_TextHandle.GetCursorPositionFromStringIndexUsingCharacterHeight(this.cursorIndex, true);
			GUIUtility.compositionCursorPos = worldPosition + cursorPositionFromStringIndexUsingCharacterHeight;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001087C File Offset: 0x0000EA7C
		public string GeneratePreviewString(bool richText)
		{
			this.RestoreCursorState();
			string compositionString = GUIUtility.compositionString;
			bool flag = this.isCompositionActive;
			string text;
			if (flag)
			{
				text = (richText ? this.text.Insert(this.cursorIndex, "<u>" + compositionString + "</u>") : this.text.Insert(this.cursorIndex, compositionString));
			}
			else
			{
				text = this.text;
			}
			return text;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000108E8 File Offset: 0x0000EAE8
		public void EnableCursorPreviewState()
		{
			bool flag = this.m_CursorIndexSavedState != -1;
			if (!flag)
			{
				this.m_CursorIndexSavedState = this.m_TextSelectingUtility.cursorIndex;
				this.cursorIndex = (this.selectIndex = this.m_CursorIndexSavedState + GUIUtility.compositionString.Length);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001093C File Offset: 0x0000EB3C
		public void RestoreCursorState()
		{
			bool flag = this.m_CursorIndexSavedState == -1;
			if (!flag)
			{
				this.cursorIndex = (this.selectIndex = this.m_CursorIndexSavedState);
				this.m_CursorIndexSavedState = -1;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00010978 File Offset: 0x0000EB78
		[VisibleToOtherModules]
		internal bool HandleKeyEvent(Event e)
		{
			this.RestoreCursorState();
			this.InitKeyActions();
			EventModifiers modifiers = e.modifiers;
			e.modifiers &= ~EventModifiers.CapsLock;
			bool flag = TextEditingUtilities.s_KeyEditOps.ContainsKey(e);
			bool flag2;
			if (flag)
			{
				TextEditOp textEditOp = TextEditingUtilities.s_KeyEditOps[e];
				this.PerformOperation(textEditOp);
				e.modifiers = modifiers;
				flag2 = true;
			}
			else
			{
				e.modifiers = modifiers;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000109E8 File Offset: 0x0000EBE8
		private void PerformOperation(TextEditOp operation)
		{
			this.revealCursor = true;
			switch (operation)
			{
			case TextEditOp.MoveLeft:
				this.m_TextSelectingUtility.MoveLeft();
				return;
			case TextEditOp.MoveRight:
				this.m_TextSelectingUtility.MoveRight();
				return;
			case TextEditOp.MoveUp:
				this.m_TextSelectingUtility.MoveUp();
				return;
			case TextEditOp.MoveDown:
				this.m_TextSelectingUtility.MoveDown();
				return;
			case TextEditOp.MoveLineStart:
				this.m_TextSelectingUtility.MoveLineStart();
				return;
			case TextEditOp.MoveLineEnd:
				this.m_TextSelectingUtility.MoveLineEnd();
				return;
			case TextEditOp.MoveTextStart:
				this.m_TextSelectingUtility.MoveTextStart();
				return;
			case TextEditOp.MoveTextEnd:
				this.m_TextSelectingUtility.MoveTextEnd();
				return;
			case TextEditOp.MoveGraphicalLineStart:
				this.m_TextSelectingUtility.MoveGraphicalLineStart();
				return;
			case TextEditOp.MoveGraphicalLineEnd:
				this.m_TextSelectingUtility.MoveGraphicalLineEnd();
				return;
			case TextEditOp.MoveWordLeft:
				this.m_TextSelectingUtility.MoveWordLeft();
				return;
			case TextEditOp.MoveWordRight:
				this.m_TextSelectingUtility.MoveWordRight();
				return;
			case TextEditOp.MoveParagraphForward:
				this.m_TextSelectingUtility.MoveParagraphForward();
				return;
			case TextEditOp.MoveParagraphBackward:
				this.m_TextSelectingUtility.MoveParagraphBackward();
				return;
			case TextEditOp.MoveToStartOfNextWord:
				this.m_TextSelectingUtility.MoveToStartOfNextWord();
				return;
			case TextEditOp.MoveToEndOfPreviousWord:
				this.m_TextSelectingUtility.MoveToEndOfPreviousWord();
				return;
			case TextEditOp.Delete:
				this.Delete();
				return;
			case TextEditOp.Backspace:
				this.Backspace();
				return;
			case TextEditOp.DeleteWordBack:
				this.DeleteWordBack();
				return;
			case TextEditOp.DeleteWordForward:
				this.DeleteWordForward();
				return;
			case TextEditOp.DeleteLineBack:
				this.DeleteLineBack();
				return;
			case TextEditOp.Cut:
				this.Cut();
				return;
			case TextEditOp.Paste:
				this.Paste();
				return;
			}
			Debug.Log("Unimplemented: " + operation.ToString());
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00010BD6 File Offset: 0x0000EDD6
		private static void MapKey(string key, TextEditOp action)
		{
			TextEditingUtilities.s_KeyEditOps[Event.KeyboardEvent(key)] = action;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00010BEC File Offset: 0x0000EDEC
		private void InitKeyActions()
		{
			bool flag = TextEditingUtilities.s_KeyEditOps != null;
			if (!flag)
			{
				TextEditingUtilities.s_KeyEditOps = new Dictionary<Event, TextEditOp>();
				TextEditingUtilities.MapKey("left", TextEditOp.MoveLeft);
				TextEditingUtilities.MapKey("right", TextEditOp.MoveRight);
				TextEditingUtilities.MapKey("up", TextEditOp.MoveUp);
				TextEditingUtilities.MapKey("down", TextEditOp.MoveDown);
				TextEditingUtilities.MapKey("delete", TextEditOp.Delete);
				TextEditingUtilities.MapKey("backspace", TextEditOp.Backspace);
				TextEditingUtilities.MapKey("#backspace", TextEditOp.Backspace);
				bool flag2 = SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX;
				if (flag2)
				{
					TextEditingUtilities.MapKey("^left", TextEditOp.MoveGraphicalLineStart);
					TextEditingUtilities.MapKey("^right", TextEditOp.MoveGraphicalLineEnd);
					TextEditingUtilities.MapKey("&left", TextEditOp.MoveWordLeft);
					TextEditingUtilities.MapKey("&right", TextEditOp.MoveWordRight);
					TextEditingUtilities.MapKey("&up", TextEditOp.MoveParagraphBackward);
					TextEditingUtilities.MapKey("&down", TextEditOp.MoveParagraphForward);
					TextEditingUtilities.MapKey("%left", TextEditOp.MoveGraphicalLineStart);
					TextEditingUtilities.MapKey("%right", TextEditOp.MoveGraphicalLineEnd);
					TextEditingUtilities.MapKey("%up", TextEditOp.MoveTextStart);
					TextEditingUtilities.MapKey("%down", TextEditOp.MoveTextEnd);
					TextEditingUtilities.MapKey("%x", TextEditOp.Cut);
					TextEditingUtilities.MapKey("%v", TextEditOp.Paste);
					TextEditingUtilities.MapKey("^d", TextEditOp.Delete);
					TextEditingUtilities.MapKey("^h", TextEditOp.Backspace);
					TextEditingUtilities.MapKey("^b", TextEditOp.MoveLeft);
					TextEditingUtilities.MapKey("^f", TextEditOp.MoveRight);
					TextEditingUtilities.MapKey("^a", TextEditOp.MoveLineStart);
					TextEditingUtilities.MapKey("^e", TextEditOp.MoveLineEnd);
					TextEditingUtilities.MapKey("&delete", TextEditOp.DeleteWordForward);
					TextEditingUtilities.MapKey("&backspace", TextEditOp.DeleteWordBack);
					TextEditingUtilities.MapKey("%backspace", TextEditOp.DeleteLineBack);
				}
				else
				{
					TextEditingUtilities.MapKey("home", TextEditOp.MoveGraphicalLineStart);
					TextEditingUtilities.MapKey("end", TextEditOp.MoveGraphicalLineEnd);
					TextEditingUtilities.MapKey("%left", TextEditOp.MoveWordLeft);
					TextEditingUtilities.MapKey("%right", TextEditOp.MoveWordRight);
					TextEditingUtilities.MapKey("%up", TextEditOp.MoveParagraphBackward);
					TextEditingUtilities.MapKey("%down", TextEditOp.MoveParagraphForward);
					TextEditingUtilities.MapKey("^left", TextEditOp.MoveToEndOfPreviousWord);
					TextEditingUtilities.MapKey("^right", TextEditOp.MoveToStartOfNextWord);
					TextEditingUtilities.MapKey("^up", TextEditOp.MoveParagraphBackward);
					TextEditingUtilities.MapKey("^down", TextEditOp.MoveParagraphForward);
					TextEditingUtilities.MapKey("^delete", TextEditOp.DeleteWordForward);
					TextEditingUtilities.MapKey("^backspace", TextEditOp.DeleteWordBack);
					TextEditingUtilities.MapKey("%backspace", TextEditOp.DeleteLineBack);
					TextEditingUtilities.MapKey("^x", TextEditOp.Cut);
					TextEditingUtilities.MapKey("^v", TextEditOp.Paste);
					TextEditingUtilities.MapKey("#delete", TextEditOp.Cut);
					TextEditingUtilities.MapKey("#insert", TextEditOp.Paste);
				}
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00010E6C File Offset: 0x0000F06C
		public bool DeleteLineBack()
		{
			this.RestoreCursorState();
			bool hasSelection = this.hasSelection;
			bool flag;
			if (hasSelection)
			{
				this.DeleteSelection();
				flag = true;
			}
			else
			{
				int num = this.cursorIndex;
				int num2 = num;
				while (num2-- != 0)
				{
					bool flag2 = this.text[num2] == '\n';
					if (flag2)
					{
						num = num2 + 1;
						break;
					}
				}
				bool flag3 = num2 == -1;
				if (flag3)
				{
					num = 0;
				}
				bool flag4 = this.cursorIndex != num;
				if (flag4)
				{
					this.text = this.text.Remove(num, this.cursorIndex - num);
					this.m_TextSelectingUtility.selectIndex = (this.cursorIndex = num);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00010F2C File Offset: 0x0000F12C
		public bool DeleteWordBack()
		{
			this.RestoreCursorState();
			bool hasSelection = this.hasSelection;
			bool flag;
			if (hasSelection)
			{
				this.DeleteSelection();
				flag = true;
			}
			else
			{
				int num = this.m_TextSelectingUtility.FindEndOfPreviousWord(this.cursorIndex);
				bool flag2 = this.cursorIndex != num;
				if (flag2)
				{
					this.text = this.text.Remove(num, this.cursorIndex - num);
					this.selectIndex = (this.cursorIndex = num);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00010FB4 File Offset: 0x0000F1B4
		public bool DeleteWordForward()
		{
			this.RestoreCursorState();
			bool hasSelection = this.hasSelection;
			bool flag;
			if (hasSelection)
			{
				this.DeleteSelection();
				flag = true;
			}
			else
			{
				int num = this.m_TextSelectingUtility.FindStartOfNextWord(this.cursorIndex);
				bool flag2 = this.cursorIndex < this.text.Length;
				if (flag2)
				{
					this.text = this.text.Remove(this.cursorIndex, num - this.cursorIndex);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00011034 File Offset: 0x0000F234
		public bool Delete()
		{
			this.RestoreCursorState();
			bool hasSelection = this.hasSelection;
			bool flag;
			if (hasSelection)
			{
				this.DeleteSelection();
				flag = true;
			}
			else
			{
				bool flag2 = this.cursorIndex < this.text.Length;
				if (flag2)
				{
					this.text = this.text.Remove(this.cursorIndex, this.m_TextSelectingUtility.NextCodePointIndex(this.cursorIndex) - this.cursorIndex);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000110B0 File Offset: 0x0000F2B0
		public bool Backspace()
		{
			this.RestoreCursorState();
			bool hasSelection = this.hasSelection;
			bool flag;
			if (hasSelection)
			{
				this.DeleteSelection();
				flag = true;
			}
			else
			{
				bool flag2 = this.cursorIndex > 0;
				if (flag2)
				{
					int num = this.m_TextSelectingUtility.PreviousCodePointIndex(this.cursorIndex);
					this.text = this.text.Remove(num, this.cursorIndex - num);
					this.m_TextSelectingUtility.SetCursorIndexWithoutNotify(num);
					this.m_TextSelectingUtility.SetSelectIndexWithoutNotify(num);
					this.m_TextSelectingUtility.ClearCursorPos();
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011148 File Offset: 0x0000F348
		public bool DeleteSelection()
		{
			bool flag = this.cursorIndex == this.selectIndex;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.cursorIndex < this.selectIndex;
				if (flag3)
				{
					this.text = this.text.Substring(0, this.cursorIndex) + this.text.Substring(this.selectIndex, this.text.Length - this.selectIndex);
					this.m_TextSelectingUtility.SetSelectIndexWithoutNotify(this.cursorIndex);
				}
				else
				{
					this.text = this.text.Substring(0, this.selectIndex) + this.text.Substring(this.cursorIndex, this.text.Length - this.cursorIndex);
					this.m_TextSelectingUtility.SetCursorIndexWithoutNotify(this.selectIndex);
				}
				this.m_TextSelectingUtility.ClearCursorPos();
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011240 File Offset: 0x0000F440
		public void ReplaceSelection(string replace)
		{
			this.RestoreCursorState();
			this.DeleteSelection();
			this.text = this.text.Insert(this.cursorIndex, replace);
			int num = this.cursorIndex + replace.Length;
			this.m_TextSelectingUtility.SetCursorIndexWithoutNotify(num);
			this.m_TextSelectingUtility.SetSelectIndexWithoutNotify(num);
			this.m_TextSelectingUtility.ClearCursorPos();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000112A9 File Offset: 0x0000F4A9
		public void Insert(char c)
		{
			this.ReplaceSelection(c.ToString());
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000112BC File Offset: 0x0000F4BC
		public void MoveSelectionToAltCursor()
		{
			this.RestoreCursorState();
			bool flag = this.m_iAltCursorPos == -1;
			if (!flag)
			{
				int iAltCursorPos = this.m_iAltCursorPos;
				string selectedText = this.SelectedText;
				this.text = this.text.Insert(iAltCursorPos, selectedText);
				bool flag2 = iAltCursorPos < this.cursorIndex;
				if (flag2)
				{
					this.cursorIndex += selectedText.Length;
					this.selectIndex += selectedText.Length;
				}
				this.DeleteSelection();
				this.selectIndex = (this.cursorIndex = iAltCursorPos);
				this.m_TextSelectingUtility.ClearCursorPos();
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011364 File Offset: 0x0000F564
		public bool CanPaste()
		{
			return GUIUtility.systemCopyBuffer.Length != 0;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00011384 File Offset: 0x0000F584
		public bool Cut()
		{
			this.m_TextSelectingUtility.Copy();
			return this.DeleteSelection();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000113A8 File Offset: 0x0000F5A8
		public bool Paste()
		{
			this.RestoreCursorState();
			string text = GUIUtility.systemCopyBuffer;
			bool flag = text != "";
			bool flag3;
			if (flag)
			{
				bool flag2 = !this.multiline;
				if (flag2)
				{
					text = TextEditingUtilities.ReplaceNewlinesWithSpaces(text);
				}
				this.ReplaceSelection(text);
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000113F8 File Offset: 0x0000F5F8
		private static string ReplaceNewlinesWithSpaces(string value)
		{
			value = value.Replace("\r\n", " ");
			value = value.Replace('\n', ' ');
			value = value.Replace('\r', ' ');
			return value;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011435 File Offset: 0x0000F635
		internal void OnBlur()
		{
			this.revealCursor = false;
			this.m_TextSelectingUtility.SelectNone();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001144C File Offset: 0x0000F64C
		internal bool TouchScreenKeyboardShouldBeUsed()
		{
			RuntimePlatform platform = Application.platform;
			RuntimePlatform runtimePlatform = platform;
			RuntimePlatform runtimePlatform2 = runtimePlatform;
			bool flag;
			if (runtimePlatform2 != RuntimePlatform.Android && runtimePlatform2 - RuntimePlatform.WebGLPlayer > 3)
			{
				flag = TouchScreenKeyboard.isSupported;
			}
			else
			{
				flag = !TouchScreenKeyboard.isInPlaceEditingAllowed;
			}
			return flag;
		}

		// Token: 0x04000174 RID: 372
		private TextSelectingUtilities m_TextSelectingUtility;

		// Token: 0x04000175 RID: 373
		private TextHandle m_TextHandle;

		// Token: 0x04000176 RID: 374
		private int m_CursorIndexSavedState = -1;

		// Token: 0x04000177 RID: 375
		internal bool isCompositionActive;

		// Token: 0x04000178 RID: 376
		private bool m_UpdateImeWindowPosition;

		// Token: 0x04000179 RID: 377
		public bool multiline = false;

		// Token: 0x0400017A RID: 378
		private string m_Text;

		// Token: 0x0400017B RID: 379
		private static Dictionary<Event, TextEditOp> s_KeyEditOps;
	}
}
