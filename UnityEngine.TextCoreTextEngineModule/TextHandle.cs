using System;
using System.Text;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200003B RID: 59
	internal class TextHandle
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00020906 File Offset: 0x0001EB06
		public TextHandle()
		{
			this.textGenerationSettings = new TextGenerationSettings();
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0002091C File Offset: 0x0001EB1C
		internal TextInfo textInfo
		{
			get
			{
				bool flag = this.m_TextInfo == null;
				if (flag)
				{
					this.m_TextInfo = new TextInfo();
				}
				return this.m_TextInfo;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00020950 File Offset: 0x0001EB50
		internal bool IsTextInfoAllocated()
		{
			return this.m_TextInfo != null;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0002096C File Offset: 0x0001EB6C
		internal static TextInfo layoutTextInfo
		{
			get
			{
				bool flag = TextHandle.m_LayoutTextInfo == null;
				if (flag)
				{
					TextHandle.m_LayoutTextInfo = new TextInfo();
				}
				return TextHandle.m_LayoutTextInfo;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0002099B File Offset: 0x0001EB9B
		public void SetDirty()
		{
			this.isDirty = true;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000209A8 File Offset: 0x0001EBA8
		public bool IsDirty()
		{
			int hashCode = this.textGenerationSettings.GetHashCode();
			bool flag = this.m_PreviousGenerationSettingsHash == hashCode && !this.isDirty;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				this.m_PreviousGenerationSettingsHash = hashCode;
				this.isDirty = false;
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000209F4 File Offset: 0x0001EBF4
		public Vector2 GetCursorPositionFromStringIndexUsingCharacterHeight(int index, bool inverseYAxis = true)
		{
			bool flag = this.textGenerationSettings == null;
			Vector2 vector;
			if (flag)
			{
				vector = Vector2.zero;
			}
			else
			{
				Rect screenRect = this.textGenerationSettings.screenRect;
				Vector2 vector2 = screenRect.position;
				bool flag2 = this.textInfo.characterCount == 0;
				if (flag2)
				{
					vector = vector2;
				}
				else
				{
					int num = ((index >= this.textInfo.characterCount) ? (this.textInfo.characterCount - 1) : index);
					TextElementInfo textElementInfo = this.textInfo.textElementInfo[num];
					float descender = textElementInfo.descender;
					float num2 = ((index >= this.textInfo.characterCount) ? textElementInfo.xAdvance : textElementInfo.origin);
					vector2 += (inverseYAxis ? new Vector2(num2, screenRect.height - descender) : new Vector2(num2, descender));
					vector = vector2;
				}
			}
			return vector;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00020AD4 File Offset: 0x0001ECD4
		public Vector2 GetCursorPositionFromStringIndexUsingLineHeight(int index, bool useXAdvance = false, bool inverseYAxis = true)
		{
			bool flag = this.textGenerationSettings == null;
			Vector2 vector;
			if (flag)
			{
				vector = Vector2.zero;
			}
			else
			{
				Rect screenRect = this.textGenerationSettings.screenRect;
				Vector2 vector2 = screenRect.position;
				bool flag2 = this.textInfo.characterCount == 0;
				if (flag2)
				{
					vector = vector2;
				}
				else
				{
					bool flag3 = index >= this.textInfo.characterCount;
					if (flag3)
					{
						index = this.textInfo.characterCount - 1;
					}
					TextElementInfo textElementInfo = this.textInfo.textElementInfo[index];
					LineInfo lineInfo = this.textInfo.lineInfo[textElementInfo.lineNumber];
					bool flag4 = index >= this.textInfo.characterCount - 1 || useXAdvance;
					if (flag4)
					{
						vector2 += (inverseYAxis ? new Vector2(textElementInfo.xAdvance, screenRect.height - lineInfo.descender) : new Vector2(textElementInfo.xAdvance, lineInfo.descender));
						vector = vector2;
					}
					else
					{
						vector2 += (inverseYAxis ? new Vector2(textElementInfo.origin, screenRect.height - lineInfo.descender) : new Vector2(textElementInfo.origin, lineInfo.descender));
						vector = vector2;
					}
				}
			}
			return vector;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00020C14 File Offset: 0x0001EE14
		public int GetCursorIndexFromPosition(Vector2 position, bool inverseYAxis = true)
		{
			bool flag = this.textGenerationSettings == null;
			int num;
			if (flag)
			{
				num = 0;
			}
			else
			{
				if (inverseYAxis)
				{
					position.y = this.textGenerationSettings.screenRect.height - position.y;
				}
				int num2 = 0;
				bool flag2 = this.textInfo.lineCount > 1;
				if (flag2)
				{
					num2 = this.FindNearestLine(position);
				}
				int num3 = this.FindNearestCharacterOnLine(position, num2, false);
				TextElementInfo textElementInfo = this.textInfo.textElementInfo[num3];
				Vector3 bottomLeft = textElementInfo.bottomLeft;
				Vector3 topRight = textElementInfo.topRight;
				float num4 = (position.x - bottomLeft.x) / (topRight.x - bottomLeft.x);
				num = ((num4 < 0.5f || textElementInfo.character == '\n') ? num3 : (num3 + 1));
			}
			return num;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00020CE8 File Offset: 0x0001EEE8
		public int LineDownCharacterPosition(int originalPos)
		{
			bool flag = originalPos >= this.textInfo.characterCount;
			int num;
			if (flag)
			{
				num = this.textInfo.characterCount - 1;
			}
			else
			{
				TextElementInfo textElementInfo = this.textInfo.textElementInfo[originalPos];
				int lineNumber = textElementInfo.lineNumber;
				bool flag2 = lineNumber + 1 >= this.textInfo.lineCount;
				if (flag2)
				{
					num = this.textInfo.characterCount - 1;
				}
				else
				{
					int lastCharacterIndex = this.textInfo.lineInfo[lineNumber + 1].lastCharacterIndex;
					int num2 = -1;
					float num3 = float.PositiveInfinity;
					float num4 = 0f;
					int i = this.textInfo.lineInfo[lineNumber + 1].firstCharacterIndex;
					while (i < lastCharacterIndex)
					{
						TextElementInfo textElementInfo2 = this.textInfo.textElementInfo[i];
						float num5 = textElementInfo.origin - textElementInfo2.origin;
						float num6 = num5 / (textElementInfo2.xAdvance - textElementInfo2.origin);
						bool flag3 = num6 >= 0f && num6 <= 1f;
						if (flag3)
						{
							bool flag4 = num6 < 0.5f;
							if (flag4)
							{
								return i;
							}
							return i + 1;
						}
						else
						{
							num5 = Mathf.Abs(num5);
							bool flag5 = num5 < num3;
							if (flag5)
							{
								num2 = i;
								num3 = num5;
								num4 = num6;
							}
							i++;
						}
					}
					bool flag6 = num2 == -1;
					if (flag6)
					{
						num = lastCharacterIndex;
					}
					else
					{
						bool flag7 = num4 < 0.5f;
						if (flag7)
						{
							num = num2;
						}
						else
						{
							num = num2 + 1;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00020E88 File Offset: 0x0001F088
		public int LineUpCharacterPosition(int originalPos)
		{
			bool flag = originalPos >= this.textInfo.characterCount;
			if (flag)
			{
				originalPos--;
			}
			TextElementInfo textElementInfo = this.textInfo.textElementInfo[originalPos];
			int lineNumber = textElementInfo.lineNumber;
			bool flag2 = lineNumber - 1 < 0;
			int num;
			if (flag2)
			{
				num = 0;
			}
			else
			{
				int num2 = this.textInfo.lineInfo[lineNumber].firstCharacterIndex - 1;
				int num3 = -1;
				float num4 = float.PositiveInfinity;
				float num5 = 0f;
				int i = this.textInfo.lineInfo[lineNumber - 1].firstCharacterIndex;
				while (i < num2)
				{
					TextElementInfo textElementInfo2 = this.textInfo.textElementInfo[i];
					float num6 = textElementInfo.origin - textElementInfo2.origin;
					float num7 = num6 / (textElementInfo2.xAdvance - textElementInfo2.origin);
					bool flag3 = num7 >= 0f && num7 <= 1f;
					if (flag3)
					{
						bool flag4 = num7 < 0.5f;
						if (flag4)
						{
							return i;
						}
						return i + 1;
					}
					else
					{
						num6 = Mathf.Abs(num6);
						bool flag5 = num6 < num4;
						if (flag5)
						{
							num3 = i;
							num4 = num6;
							num5 = num7;
						}
						i++;
					}
				}
				bool flag6 = num3 == -1;
				if (flag6)
				{
					num = num2;
				}
				else
				{
					bool flag7 = num5 < 0.5f;
					if (flag7)
					{
						num = num3;
					}
					else
					{
						num = num3 + 1;
					}
				}
			}
			return num;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00021000 File Offset: 0x0001F200
		public int FindWordIndex(int cursorIndex)
		{
			for (int i = 0; i < this.textInfo.wordCount; i++)
			{
				WordInfo wordInfo = this.textInfo.wordInfo[i];
				bool flag = wordInfo.firstCharacterIndex <= cursorIndex && wordInfo.lastCharacterIndex >= cursorIndex;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00021064 File Offset: 0x0001F264
		public int FindNearestLine(Vector2 position)
		{
			float num = float.PositiveInfinity;
			int num2 = -1;
			for (int i = 0; i < this.textInfo.lineCount; i++)
			{
				LineInfo lineInfo = this.textInfo.lineInfo[i];
				float ascender = lineInfo.ascender;
				float descender = lineInfo.descender;
				bool flag = ascender > position.y && descender < position.y;
				if (flag)
				{
					return i;
				}
				float num3 = Mathf.Abs(ascender - position.y);
				float num4 = Mathf.Abs(descender - position.y);
				float num5 = Mathf.Min(num3, num4);
				bool flag2 = num5 < num;
				if (flag2)
				{
					num = num5;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00021128 File Offset: 0x0001F328
		public int FindNearestCharacterOnLine(Vector2 position, int line, bool visibleOnly)
		{
			int firstCharacterIndex = this.textInfo.lineInfo[line].firstCharacterIndex;
			int lastCharacterIndex = this.textInfo.lineInfo[line].lastCharacterIndex;
			float num = float.PositiveInfinity;
			int num2 = lastCharacterIndex;
			for (int i = firstCharacterIndex; i <= lastCharacterIndex; i++)
			{
				TextElementInfo textElementInfo = this.textInfo.textElementInfo[i];
				bool flag = visibleOnly && !textElementInfo.isVisible;
				if (!flag)
				{
					bool flag2 = textElementInfo.character == '\r' || textElementInfo.character == '\n';
					if (!flag2)
					{
						Vector3 bottomLeft = textElementInfo.bottomLeft;
						Vector3 vector = new Vector3(textElementInfo.bottomLeft.x, textElementInfo.topRight.y, 0f);
						Vector3 topRight = textElementInfo.topRight;
						Vector3 vector2 = new Vector3(textElementInfo.topRight.x, textElementInfo.bottomLeft.y, 0f);
						bool flag3 = TextHandle.PointIntersectRectangle(position, bottomLeft, vector, topRight, vector2);
						if (flag3)
						{
							num2 = i;
							break;
						}
						float num3 = TextHandle.DistanceToLine(bottomLeft, vector, position);
						float num4 = TextHandle.DistanceToLine(vector, topRight, position);
						float num5 = TextHandle.DistanceToLine(topRight, vector2, position);
						float num6 = TextHandle.DistanceToLine(vector2, bottomLeft, position);
						float num7 = ((num3 < num4) ? num3 : num4);
						num7 = ((num7 < num5) ? num7 : num5);
						num7 = ((num7 < num6) ? num7 : num6);
						bool flag4 = num > num7;
						if (flag4)
						{
							num = num7;
							num2 = i;
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000212E4 File Offset: 0x0001F4E4
		public int FindIntersectingLink(Vector3 position, bool inverseYAxis = true)
		{
			if (inverseYAxis)
			{
				position.y = this.textGenerationSettings.screenRect.height - position.y;
			}
			for (int i = 0; i < this.textInfo.linkCount; i++)
			{
				LinkInfo linkInfo = this.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				Vector3 zero3 = Vector3.zero;
				Vector3 zero4 = Vector3.zero;
				for (int j = 0; j < linkInfo.linkTextLength; j++)
				{
					int num = linkInfo.linkTextfirstCharacterIndex + j;
					TextElementInfo textElementInfo = this.textInfo.textElementInfo[num];
					int lineNumber = textElementInfo.lineNumber;
					bool flag2 = !flag;
					if (flag2)
					{
						flag = true;
						zero = new Vector3(textElementInfo.bottomLeft.x, textElementInfo.descender, 0f);
						zero2 = new Vector3(textElementInfo.bottomLeft.x, textElementInfo.ascender, 0f);
						bool flag3 = linkInfo.linkTextLength == 1;
						if (flag3)
						{
							flag = false;
							zero3 = new Vector3(textElementInfo.topRight.x, textElementInfo.descender, 0f);
							zero4 = new Vector3(textElementInfo.topRight.x, textElementInfo.ascender, 0f);
							bool flag4 = TextHandle.PointIntersectRectangle(position, zero, zero2, zero4, zero3);
							if (flag4)
							{
								return i;
							}
						}
					}
					bool flag5 = flag && j == linkInfo.linkTextLength - 1;
					if (flag5)
					{
						flag = false;
						zero3 = new Vector3(textElementInfo.topRight.x, textElementInfo.descender, 0f);
						zero4 = new Vector3(textElementInfo.topRight.x, textElementInfo.ascender, 0f);
						bool flag6 = TextHandle.PointIntersectRectangle(position, zero, zero2, zero4, zero3);
						if (flag6)
						{
							return i;
						}
					}
					else
					{
						bool flag7 = flag && lineNumber != this.textInfo.textElementInfo[num + 1].lineNumber;
						if (flag7)
						{
							flag = false;
							zero3 = new Vector3(textElementInfo.topRight.x, textElementInfo.descender, 0f);
							zero4 = new Vector3(textElementInfo.topRight.x, textElementInfo.ascender, 0f);
							bool flag8 = TextHandle.PointIntersectRectangle(position, zero, zero2, zero4, zero3);
							if (flag8)
							{
								return i;
							}
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0002157C File Offset: 0x0001F77C
		private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = m - a;
			Vector3 vector3 = c - b;
			Vector3 vector4 = m - b;
			float num = Vector3.Dot(vector, vector2);
			float num2 = Vector3.Dot(vector3, vector4);
			return 0f <= num && num <= Vector3.Dot(vector, vector) && 0f <= num2 && num2 <= Vector3.Dot(vector3, vector3);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000215F0 File Offset: 0x0001F7F0
		private static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = a - point;
			float num = Vector3.Dot(vector, vector2);
			bool flag = num > 0f;
			float num2;
			if (flag)
			{
				num2 = Vector3.Dot(vector2, vector2);
			}
			else
			{
				Vector3 vector3 = point - b;
				bool flag2 = Vector3.Dot(vector, vector3) > 0f;
				if (flag2)
				{
					num2 = Vector3.Dot(vector3, vector3);
				}
				else
				{
					Vector3 vector4 = vector2 - vector * (num / Vector3.Dot(vector, vector));
					num2 = Vector3.Dot(vector4, vector4);
				}
			}
			return num2;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0002167C File Offset: 0x0001F87C
		public int GetLineNumber(int index)
		{
			bool flag = index <= 0;
			if (flag)
			{
				index = 0;
			}
			else
			{
				bool flag2 = index >= this.textInfo.characterCount;
				if (flag2)
				{
					index = Mathf.Max(0, this.textInfo.characterCount - 1);
				}
			}
			return this.textInfo.textElementInfo[index].lineNumber;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000216E0 File Offset: 0x0001F8E0
		public float GetLineHeight(int lineNumber)
		{
			bool flag = lineNumber <= 0;
			if (flag)
			{
				lineNumber = 0;
			}
			else
			{
				bool flag2 = lineNumber >= this.textInfo.lineCount;
				if (flag2)
				{
					lineNumber = Mathf.Max(0, this.textInfo.lineCount - 1);
				}
			}
			return this.textInfo.lineInfo[lineNumber].lineHeight;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00021744 File Offset: 0x0001F944
		public float GetLineHeightFromCharacterIndex(int index)
		{
			bool flag = index <= 0;
			if (flag)
			{
				index = 0;
			}
			else
			{
				bool flag2 = index >= this.textInfo.characterCount;
				if (flag2)
				{
					index = Mathf.Max(0, this.textInfo.characterCount - 1);
				}
			}
			return this.GetLineHeight(this.textInfo.textElementInfo[index].lineNumber);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000217AC File Offset: 0x0001F9AC
		public float GetCharacterHeightFromIndex(int index)
		{
			bool flag = index <= 0;
			if (flag)
			{
				index = 0;
			}
			else
			{
				bool flag2 = index >= this.textInfo.characterCount;
				if (flag2)
				{
					index = Mathf.Max(0, this.textInfo.characterCount - 1);
				}
			}
			TextElementInfo textElementInfo = this.textInfo.textElementInfo[index];
			return textElementInfo.ascender - textElementInfo.descender;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00021818 File Offset: 0x0001FA18
		public bool IsElided()
		{
			bool flag = this.textInfo == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.textInfo.characterCount == 0;
				flag2 = flag3 || TextGenerator.isTextTruncated;
			}
			return flag2;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00021858 File Offset: 0x0001FA58
		public string Substring(int startIndex, int length)
		{
			bool flag = startIndex < 0 || startIndex + length > this.textInfo.characterCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException();
			}
			StringBuilder stringBuilder = new StringBuilder(length);
			for (int i = startIndex; i < startIndex + length; i++)
			{
				stringBuilder.Append(this.textInfo.textElementInfo[i].character);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000218CC File Offset: 0x0001FACC
		public int IndexOf(char value, int startIndex)
		{
			bool flag = startIndex < 0 || startIndex >= this.textInfo.characterCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = startIndex; i < this.textInfo.characterCount; i++)
			{
				bool flag2 = this.textInfo.textElementInfo[i].character == value;
				if (flag2)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00021940 File Offset: 0x0001FB40
		public int LastIndexOf(char value, int startIndex)
		{
			bool flag = startIndex < 0 || startIndex >= this.textInfo.characterCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException();
			}
			for (int i = startIndex; i >= 0; i--)
			{
				bool flag2 = this.textInfo.textElementInfo[i].character == value;
				if (flag2)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000219B0 File Offset: 0x0001FBB0
		protected float ComputeTextWidth(TextGenerationSettings tgs)
		{
			this.UpdatePreferredValues(tgs);
			return this.m_PreferredSize.x;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000219D8 File Offset: 0x0001FBD8
		protected float ComputeTextHeight(TextGenerationSettings tgs)
		{
			this.UpdatePreferredValues(tgs);
			return this.m_PreferredSize.y;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000219FD File Offset: 0x0001FBFD
		protected void UpdatePreferredValues(TextGenerationSettings tgs)
		{
			this.m_PreferredSize = TextGenerator.GetPreferredValues(tgs, TextHandle.layoutTextInfo);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00021A14 File Offset: 0x0001FC14
		internal TextInfo Update(string newText)
		{
			this.textGenerationSettings.text = newText;
			return this.Update(this.textGenerationSettings);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00021A40 File Offset: 0x0001FC40
		protected TextInfo Update(TextGenerationSettings tgs)
		{
			bool flag = !this.IsDirty();
			TextInfo textInfo;
			if (flag)
			{
				textInfo = this.textInfo;
			}
			else
			{
				this.textInfo.isDirty = true;
				TextGenerator.GenerateText(tgs, this.textInfo);
				this.textGenerationSettings = tgs;
				textInfo = this.textInfo;
			}
			return textInfo;
		}

		// Token: 0x040002B2 RID: 690
		private Vector2 m_PreferredSize;

		// Token: 0x040002B3 RID: 691
		private TextInfo m_TextInfo;

		// Token: 0x040002B4 RID: 692
		private static TextInfo m_LayoutTextInfo;

		// Token: 0x040002B5 RID: 693
		private int m_PreviousGenerationSettingsHash;

		// Token: 0x040002B6 RID: 694
		protected TextGenerationSettings textGenerationSettings;

		// Token: 0x040002B7 RID: 695
		protected static TextGenerationSettings s_LayoutSettings = new TextGenerationSettings();

		// Token: 0x040002B8 RID: 696
		private bool isDirty;
	}
}
