using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000071 RID: 113
	public static class TMP_TextUtilities
	{
		// Token: 0x0600059C RID: 1436 RVA: 0x0003647C File Offset: 0x0003467C
		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera)
		{
			int num = TMP_TextUtilities.FindNearestCharacter(textComponent, position, camera, false);
			RectTransform rectTransform = textComponent.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tmp_CharacterInfo = textComponent.textInfo.characterInfo[num];
			Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
			if ((position.x - vector.x) / (vector2.x - vector.x) < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x000364FC File Offset: 0x000346FC
		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera, out CaretPosition cursor)
		{
			int num = TMP_TextUtilities.FindNearestLine(textComponent, position, camera);
			int num2 = TMP_TextUtilities.FindNearestCharacterOnLine(textComponent, position, num, camera, false);
			if (textComponent.textInfo.lineInfo[num].characterCount == 1)
			{
				cursor = CaretPosition.Left;
				return num2;
			}
			RectTransform rectTransform = textComponent.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			TMP_CharacterInfo tmp_CharacterInfo = textComponent.textInfo.characterInfo[num2];
			Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
			Vector3 vector2 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
			if ((position.x - vector.x) / (vector2.x - vector.x) < 0.5f)
			{
				cursor = CaretPosition.Left;
				return num2;
			}
			cursor = CaretPosition.Right;
			return num2;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000365AC File Offset: 0x000347AC
		public static int FindNearestLine(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int num2 = -1;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tmp_LineInfo = text.textInfo.lineInfo[i];
				float y = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.ascender, 0f)).y;
				float y2 = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.descender, 0f)).y;
				if (y > position.y && y2 < position.y)
				{
					return i;
				}
				float num3 = Mathf.Abs(y - position.y);
				float num4 = Mathf.Abs(y2 - position.y);
				float num5 = Mathf.Min(num3, num4);
				if (num5 < num)
				{
					num = num5;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00036698 File Offset: 0x00034898
		public static int FindNearestCharacterOnLine(TMP_Text text, Vector3 position, int line, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			int firstCharacterIndex = text.textInfo.lineInfo[line].firstCharacterIndex;
			int lastCharacterIndex = text.textInfo.lineInfo[line].lastCharacterIndex;
			float num = float.PositiveInfinity;
			int num2 = lastCharacterIndex;
			for (int i = firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						num2 = i;
						break;
					}
					float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
					float num5 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
					float num6 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
					float num7 = ((num3 < num4) ? num3 : num4);
					num7 = ((num7 < num5) ? num7 : num5);
					num7 = ((num7 < num6) ? num7 : num6);
					if (num > num7)
					{
						num = num7;
						num2 = i;
					}
				}
			}
			return num2;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00036818 File Offset: 0x00034A18
		public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
		{
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			rectTransform.GetWorldCorners(TMP_TextUtilities.m_rectWorldCorners);
			return TMP_TextUtilities.PointIntersectRectangle(position, TMP_TextUtilities.m_rectWorldCorners[0], TMP_TextUtilities.m_rectWorldCorners[1], TMP_TextUtilities.m_rectWorldCorners[2], TMP_TextUtilities.m_rectWorldCorners[3]);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00036878 File Offset: 0x00034A78
		public static int FindIntersectingCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00036954 File Offset: 0x00034B54
		public static int FindNearestCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int num2 = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tmp_CharacterInfo.isVisible)
				{
					Vector3 vector = rectTransform.TransformPoint(tmp_CharacterInfo.bottomLeft);
					Vector3 vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.topRight.y, 0f));
					Vector3 vector3 = rectTransform.TransformPoint(tmp_CharacterInfo.topRight);
					Vector3 vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.bottomLeft.y, 0f));
					if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector3, vector4))
					{
						return i;
					}
					float num3 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
					float num4 = TMP_TextUtilities.DistanceToLine(vector2, vector3, position);
					float num5 = TMP_TextUtilities.DistanceToLine(vector3, vector4, position);
					float num6 = TMP_TextUtilities.DistanceToLine(vector4, vector, position);
					float num7 = ((num3 < num4) ? num3 : num4);
					num7 = ((num7 < num5) ? num7 : num5);
					num7 = ((num7 < num6) ? num7 : num6);
					if (num > num7)
					{
						num = num7;
						num2 = i;
					}
				}
			}
			return num2;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00036AA8 File Offset: 0x00034CA8
		public static int FindIntersectingWord(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				float num = float.NegativeInfinity;
				float num2 = float.PositiveInfinity;
				for (int j = 0; j < tmp_WordInfo.characterCount; j++)
				{
					int num3 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = tmp_CharacterInfo.lineNumber;
					bool isVisible = tmp_CharacterInfo.isVisible;
					num = Mathf.Max(num, tmp_CharacterInfo.ascender);
					num2 = Mathf.Min(num2, tmp_CharacterInfo.descender);
					if (!flag && isVisible)
					{
						flag = true;
						vector = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f);
						vector2 = new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f);
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							vector3 = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
							vector4 = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
							vector = rectTransform.TransformPoint(new Vector3(vector.x, num2, 0f));
							vector2 = rectTransform.TransformPoint(new Vector3(vector2.x, num, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(vector4.x, num, 0f));
							vector3 = rectTransform.TransformPoint(new Vector3(vector3.x, num2, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
					}
					if (flag && j == tmp_WordInfo.characterCount - 1)
					{
						flag = false;
						vector3 = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
						vector4 = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
						vector = rectTransform.TransformPoint(new Vector3(vector.x, num2, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(vector2.x, num, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(vector4.x, num, 0f));
						vector3 = rectTransform.TransformPoint(new Vector3(vector3.x, num2, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
					else if (flag && lineNumber != text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						flag = false;
						vector3 = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f);
						vector4 = new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f);
						vector = rectTransform.TransformPoint(new Vector3(vector.x, num2, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(vector2.x, num, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(vector4.x, num, 0f));
						vector3 = rectTransform.TransformPoint(new Vector3(vector3.x, num2, 0f));
						num = float.NegativeInfinity;
						num2 = float.PositiveInfinity;
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00036E74 File Offset: 0x00035074
		public static int FindNearestWord(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int num2 = 0;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tmp_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_WordInfo.characterCount; j++)
				{
					int num3 = tmp_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = tmp_CharacterInfo.lineNumber;
					bool isVisible = tmp_CharacterInfo.isVisible;
					if (!flag && isVisible)
					{
						flag = true;
						vector = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
						vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
						if (tmp_WordInfo.characterCount == 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
							float num4 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
							float num5 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
							float num6 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
							float num7 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
							float num8 = ((num4 < num5) ? num4 : num5);
							num8 = ((num8 < num6) ? num8 : num6);
							num8 = ((num8 < num7) ? num8 : num7);
							if (num > num8)
							{
								num = num8;
								num2 = i;
							}
						}
					}
					if (flag && j == tmp_WordInfo.characterCount - 1)
					{
						flag = false;
						vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
						float num9 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
						float num10 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
						float num11 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
						float num12 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
						float num13 = ((num9 < num10) ? num9 : num10);
						num13 = ((num13 < num11) ? num13 : num11);
						num13 = ((num13 < num12) ? num13 : num12);
						if (num > num13)
						{
							num = num13;
							num2 = i;
						}
					}
					else if (flag && lineNumber != text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						flag = false;
						vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
						vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
						if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
						{
							return i;
						}
						float num14 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
						float num15 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
						float num16 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
						float num17 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
						float num18 = ((num14 < num15) ? num14 : num15);
						num18 = ((num18 < num16) ? num18 : num16);
						num18 = ((num18 < num17) ? num18 : num17);
						if (num > num18)
						{
							num = num18;
							num2 = i;
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00037234 File Offset: 0x00035434
		public static int FindIntersectingLine(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			int num = -1;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tmp_LineInfo = text.textInfo.lineInfo[i];
				float y = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.ascender, 0f)).y;
				float y2 = rectTransform.TransformPoint(new Vector3(0f, tmp_LineInfo.descender, 0f)).y;
				if (y > position.y && y2 < position.y)
				{
					return i;
				}
			}
			return num;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000372E0 File Offset: 0x000354E0
		public static int FindIntersectingLink(TMP_Text text, Vector3 position, Camera camera)
		{
			Transform transform = text.transform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(transform, position, camera, out position);
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_LinkInfo.linkTextLength; j++)
				{
					int num = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num];
					int lineNumber = tmp_CharacterInfo.lineNumber;
					if (text.overflowMode != TextOverflowModes.Page || tmp_CharacterInfo.pageNumber + 1 == text.pageToDisplay)
					{
						if (!flag)
						{
							flag = true;
							vector = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
							vector2 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
							if (tmp_LinkInfo.linkTextLength == 1)
							{
								flag = false;
								vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
								vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
								if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
								{
									return i;
								}
							}
						}
						if (flag && j == tmp_LinkInfo.linkTextLength - 1)
						{
							flag = false;
							vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
						else if (flag && lineNumber != text.textInfo.characterInfo[num + 1].lineNumber)
						{
							flag = false;
							vector3 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = transform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00037558 File Offset: 0x00035758
		public static int FindNearestLink(TMP_Text text, Vector3 position, Camera camera)
		{
			RectTransform rectTransform = text.rectTransform;
			TMP_TextUtilities.ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
			float num = float.PositiveInfinity;
			int num2 = 0;
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tmp_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				Vector3 vector3 = Vector3.zero;
				Vector3 vector4 = Vector3.zero;
				for (int j = 0; j < tmp_LinkInfo.linkTextLength; j++)
				{
					int num3 = tmp_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tmp_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = tmp_CharacterInfo.lineNumber;
					if (text.overflowMode != TextOverflowModes.Page || tmp_CharacterInfo.pageNumber + 1 == text.pageToDisplay)
					{
						if (!flag)
						{
							flag = true;
							vector = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.descender, 0f));
							vector2 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.bottomLeft.x, tmp_CharacterInfo.ascender, 0f));
							if (tmp_LinkInfo.linkTextLength == 1)
							{
								flag = false;
								vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
								vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
								if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
								{
									return i;
								}
								float num4 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
								float num5 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
								float num6 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
								float num7 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
								float num8 = ((num4 < num5) ? num4 : num5);
								num8 = ((num8 < num6) ? num8 : num6);
								num8 = ((num8 < num7) ? num8 : num7);
								if (num > num8)
								{
									num = num8;
									num2 = i;
								}
							}
						}
						if (flag && j == tmp_LinkInfo.linkTextLength - 1)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
							float num9 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
							float num10 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
							float num11 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
							float num12 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
							float num13 = ((num9 < num10) ? num9 : num10);
							num13 = ((num13 < num11) ? num13 : num11);
							num13 = ((num13 < num12) ? num13 : num12);
							if (num > num13)
							{
								num = num13;
								num2 = i;
							}
						}
						else if (flag && lineNumber != text.textInfo.characterInfo[num3 + 1].lineNumber)
						{
							flag = false;
							vector3 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.descender, 0f));
							vector4 = rectTransform.TransformPoint(new Vector3(tmp_CharacterInfo.topRight.x, tmp_CharacterInfo.ascender, 0f));
							if (TMP_TextUtilities.PointIntersectRectangle(position, vector, vector2, vector4, vector3))
							{
								return i;
							}
							float num14 = TMP_TextUtilities.DistanceToLine(vector, vector2, position);
							float num15 = TMP_TextUtilities.DistanceToLine(vector2, vector4, position);
							float num16 = TMP_TextUtilities.DistanceToLine(vector4, vector3, position);
							float num17 = TMP_TextUtilities.DistanceToLine(vector3, vector, position);
							float num18 = ((num14 < num15) ? num14 : num15);
							num18 = ((num18 < num16) ? num18 : num16);
							num18 = ((num18 < num17) ? num18 : num17);
							if (num > num18)
							{
								num = num18;
								num2 = i;
							}
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00037928 File Offset: 0x00035B28
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

		// Token: 0x060005A9 RID: 1449 RVA: 0x00037994 File Offset: 0x00035B94
		public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
		{
			worldPoint = Vector2.zero;
			Ray ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint);
			float num;
			if (!new Plane(transform.rotation * Vector3.back, transform.position).Raycast(ray, out num))
			{
				return false;
			}
			worldPoint = ray.GetPoint(num);
			return true;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000379F4 File Offset: 0x00035BF4
		private static bool IntersectLinePlane(TMP_TextUtilities.LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
		{
			intersectingPoint = Vector3.zero;
			Vector3 vector = line.Point2 - line.Point1;
			Vector3 vector2 = line.Point1 - point;
			float num = Vector3.Dot(normal, vector);
			float num2 = -Vector3.Dot(normal, vector2);
			if (Mathf.Abs(num) < Mathf.Epsilon)
			{
				return num2 == 0f;
			}
			float num3 = num2 / num;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			intersectingPoint = line.Point1 + num3 * vector;
			return true;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00037A88 File Offset: 0x00035C88
		public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
		{
			Vector3 vector = b - a;
			Vector3 vector2 = a - point;
			float num = Vector3.Dot(vector, vector2);
			if (num > 0f)
			{
				return Vector3.Dot(vector2, vector2);
			}
			Vector3 vector3 = point - b;
			if (Vector3.Dot(vector, vector3) > 0f)
			{
				return Vector3.Dot(vector3, vector3);
			}
			Vector3 vector4 = vector2 - vector * (num / Vector3.Dot(vector, vector));
			return Vector3.Dot(vector4, vector4);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00037AF6 File Offset: 0x00035CF6
		public static char ToLowerFast(char c)
		{
			if ((int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1)
			{
				return c;
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[(int)c];
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00037B14 File Offset: 0x00035D14
		public static char ToUpperFast(char c)
		{
			if ((int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)
			{
				return c;
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00037B32 File Offset: 0x00035D32
		internal static uint ToUpperASCIIFast(uint c)
		{
			if ((ulong)c > (ulong)((long)("-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)))
			{
				return c;
			}
			return (uint)"-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00037B54 File Offset: 0x00035D54
		public static int GetHashCode(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ (int)TMP_TextUtilities.ToUpperFast(s[i]);
			}
			return num;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00037B88 File Offset: 0x00035D88
		public static int GetSimpleHashCode(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ (int)s[i];
			}
			return num;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00037BB8 File Offset: 0x00035DB8
		public static uint GetSimpleHashCodeLowercase(string s)
		{
			uint num = 5381U;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ (uint)TMP_TextUtilities.ToLowerFast(s[i]);
			}
			return num;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00037BF0 File Offset: 0x00035DF0
		public static int HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case ':':
			case ';':
			case '<':
			case '=':
			case '>':
			case '?':
			case '@':
				break;
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			default:
				switch (hex)
				{
				case 'a':
					return 10;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				}
				break;
			}
			return 15;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00037CC0 File Offset: 0x00035EC0
		public static int StringHexToInt(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num += TMP_TextUtilities.HexToInt(s[i]) * (int)Mathf.Pow(16f, (float)(s.Length - 1 - i));
			}
			return num;
		}

		// Token: 0x04000557 RID: 1367
		private static Vector3[] m_rectWorldCorners = new Vector3[4];

		// Token: 0x04000558 RID: 1368
		private const string k_lookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

		// Token: 0x04000559 RID: 1369
		private const string k_lookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

		// Token: 0x020000A9 RID: 169
		private struct LineSegment
		{
			// Token: 0x06000651 RID: 1617 RVA: 0x0003922F File Offset: 0x0003742F
			public LineSegment(Vector3 p1, Vector3 p2)
			{
				this.Point1 = p1;
				this.Point2 = p2;
			}

			// Token: 0x04000611 RID: 1553
			public Vector3 Point1;

			// Token: 0x04000612 RID: 1554
			public Vector3 Point2;
		}
	}
}
