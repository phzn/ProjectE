using System;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200003A RID: 58
	internal static class TextGeneratorUtilities
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0001DBE0 File Offset: 0x0001BDE0
		public static bool Approximately(float a, float b)
		{
			return b - 0.0001f < a && a < b + 0.0001f;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0001DC0C File Offset: 0x0001BE0C
		public static Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			bool flag = tagCount == 4;
			Color32 color;
			if (flag)
			{
				byte b = (byte)(TextGeneratorUtilities.HexToInt(hexChars[1]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[1]));
				byte b2 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[2]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[2]));
				byte b3 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[3]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[3]));
				color = new Color32(b, b2, b3, byte.MaxValue);
			}
			else
			{
				bool flag2 = tagCount == 5;
				if (flag2)
				{
					byte b4 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[1]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[1]));
					byte b5 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[2]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[2]));
					byte b6 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[3]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[3]));
					byte b7 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[4]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[4]));
					color = new Color32(b4, b5, b6, b7);
				}
				else
				{
					bool flag3 = tagCount == 7;
					if (flag3)
					{
						byte b8 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[1]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[2]));
						byte b9 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[3]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[4]));
						byte b10 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[5]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[6]));
						color = new Color32(b8, b9, b10, byte.MaxValue);
					}
					else
					{
						bool flag4 = tagCount == 9;
						if (flag4)
						{
							byte b11 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[1]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[2]));
							byte b12 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[3]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[4]));
							byte b13 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[5]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[6]));
							byte b14 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[7]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[8]));
							color = new Color32(b11, b12, b13, b14);
						}
						else
						{
							bool flag5 = tagCount == 10;
							if (flag5)
							{
								byte b15 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[7]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[7]));
								byte b16 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[8]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[8]));
								byte b17 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[9]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[9]));
								color = new Color32(b15, b16, b17, byte.MaxValue);
							}
							else
							{
								bool flag6 = tagCount == 11;
								if (flag6)
								{
									byte b18 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[7]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[7]));
									byte b19 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[8]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[8]));
									byte b20 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[9]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[9]));
									byte b21 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[10]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[10]));
									color = new Color32(b18, b19, b20, b21);
								}
								else
								{
									bool flag7 = tagCount == 13;
									if (flag7)
									{
										byte b22 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[7]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[8]));
										byte b23 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[9]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[10]));
										byte b24 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[11]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[12]));
										color = new Color32(b22, b23, b24, byte.MaxValue);
									}
									else
									{
										bool flag8 = tagCount == 15;
										if (flag8)
										{
											byte b25 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[7]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[8]));
											byte b26 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[9]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[10]));
											byte b27 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[11]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[12]));
											byte b28 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[13]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[14]));
											color = new Color32(b25, b26, b27, b28);
										}
										else
										{
											color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
										}
									}
								}
							}
						}
					}
				}
			}
			return color;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0001DFCC File Offset: 0x0001C1CC
		public static Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			bool flag = length == 7;
			Color32 color;
			if (flag)
			{
				byte b = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 1]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 2]));
				byte b2 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 3]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 4]));
				byte b3 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 5]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 6]));
				color = new Color32(b, b2, b3, byte.MaxValue);
			}
			else
			{
				bool flag2 = length == 9;
				if (flag2)
				{
					byte b4 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 1]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 2]));
					byte b5 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 3]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 4]));
					byte b6 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 5]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 6]));
					byte b7 = (byte)(TextGeneratorUtilities.HexToInt(hexChars[startIndex + 7]) * 16U + TextGeneratorUtilities.HexToInt(hexChars[startIndex + 8]));
					color = new Color32(b4, b5, b6, b7);
				}
				else
				{
					color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
			}
			return color;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0001E0F0 File Offset: 0x0001C2F0
		public static uint HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0U;
			case '1':
				return 1U;
			case '2':
				return 2U;
			case '3':
				return 3U;
			case '4':
				return 4U;
			case '5':
				return 5U;
			case '6':
				return 6U;
			case '7':
				return 7U;
			case '8':
				return 8U;
			case '9':
				return 9U;
			case ':':
			case ';':
			case '<':
			case '=':
			case '>':
			case '?':
			case '@':
				break;
			case 'A':
				return 10U;
			case 'B':
				return 11U;
			case 'C':
				return 12U;
			case 'D':
				return 13U;
			case 'E':
				return 14U;
			case 'F':
				return 15U;
			default:
				switch (hex)
				{
				case 'a':
					return 10U;
				case 'b':
					return 11U;
				case 'c':
					return 12U;
				case 'd':
					return 13U;
				case 'e':
					return 14U;
				case 'f':
					return 15U;
				}
				break;
			}
			return 15U;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001E1F8 File Offset: 0x0001C3F8
		public static float ConvertToFloat(char[] chars, int startIndex, int length)
		{
			int num;
			return TextGeneratorUtilities.ConvertToFloat(chars, startIndex, length, out num);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0001E214 File Offset: 0x0001C414
		public static float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
		{
			bool flag = startIndex == 0;
			float num;
			if (flag)
			{
				lastIndex = 0;
				num = -32767f;
			}
			else
			{
				int num2 = startIndex + length;
				bool flag2 = true;
				float num3 = 0f;
				int num4 = 1;
				bool flag3 = chars[startIndex] == '+';
				if (flag3)
				{
					num4 = 1;
					startIndex++;
				}
				else
				{
					bool flag4 = chars[startIndex] == '-';
					if (flag4)
					{
						num4 = -1;
						startIndex++;
					}
				}
				float num5 = 0f;
				int i = startIndex;
				while (i < num2)
				{
					uint num6 = (uint)chars[i];
					bool flag5 = (num6 >= 48U && num6 <= 57U) || num6 == 46U;
					if (flag5)
					{
						bool flag6 = num6 == 46U;
						if (flag6)
						{
							flag2 = false;
							num3 = 0.1f;
						}
						else
						{
							bool flag7 = flag2;
							if (flag7)
							{
								num5 = num5 * 10f + (float)((ulong)(num6 - 48U) * (ulong)((long)num4));
							}
							else
							{
								num5 += (num6 - 48U) * num3 * (float)num4;
								num3 *= 0.1f;
							}
						}
					}
					else
					{
						bool flag8 = num6 == 44U;
						if (flag8)
						{
							bool flag9 = i + 1 < num2 && chars[i + 1] == ' ';
							if (flag9)
							{
								lastIndex = i + 1;
							}
							else
							{
								lastIndex = i;
							}
							return num5;
						}
					}
					IL_0116:
					i++;
					continue;
					goto IL_0116;
				}
				lastIndex = num2;
				num = num5;
			}
			return num;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0001E358 File Offset: 0x0001C558
		public static Vector2 PackUV(float x, float y, float scale)
		{
			Vector2 vector;
			vector.x = (float)((int)(x * 511f));
			vector.y = (float)((int)(y * 511f));
			vector.x = vector.x * 4096f + vector.y;
			vector.y = scale;
			return vector;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0001E3B0 File Offset: 0x0001C5B0
		public static void ResizeInternalArray<T>(ref T[] array)
		{
			int num = Mathf.NextPowerOfTwo(array.Length + 1);
			Array.Resize<T>(ref array, num);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0001E3D2 File Offset: 0x0001C5D2
		public static void ResizeInternalArray<T>(ref T[] array, int size)
		{
			size = Mathf.NextPowerOfTwo(size + 1);
			Array.Resize<T>(ref array, size);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0001E3E8 File Offset: 0x0001C5E8
		private static bool IsTagName(ref string text, string tag, int index)
		{
			bool flag = text.Length < index + tag.Length;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				for (int i = 0; i < tag.Length; i++)
				{
					bool flag3 = TextUtilities.ToUpperFast(text[index + i]) != tag[i];
					if (flag3)
					{
						return false;
					}
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0001E450 File Offset: 0x0001C650
		private static bool IsTagName(ref int[] text, string tag, int index)
		{
			bool flag = text.Length < index + tag.Length;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				for (int i = 0; i < tag.Length; i++)
				{
					bool flag3 = TextUtilities.ToUpperFast((char)text[index + i]) != tag[i];
					if (flag3)
					{
						return false;
					}
				}
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0001E4B4 File Offset: 0x0001C6B4
		internal static void InsertOpeningTextStyle(TextStyle style, ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			bool flag = style == null;
			if (!flag)
			{
				textStyleStackDepth++;
				textStyleStacks[textStyleStackDepth].Push(style.hashCode);
				uint[] styleOpeningTagArray = style.styleOpeningTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleOpeningTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth--;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0001E504 File Offset: 0x0001C704
		internal static void InsertClosingTextStyle(TextStyle style, ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			bool flag = style == null;
			if (!flag)
			{
				textStyleStackDepth++;
				textStyleStacks[textStyleStackDepth].Push(style.hashCode);
				uint[] styleClosingTagArray = style.styleClosingTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleClosingTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth--;
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0001E554 File Offset: 0x0001C754
		public static bool ReplaceOpeningStyleTag(ref TextBackingContainer sourceText, int srcIndex, out int srcOffset, ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			int styleHashCode = TextGeneratorUtilities.GetStyleHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TextStyle style = TextGeneratorUtilities.GetStyle(generationSettings, styleHashCode);
			bool flag = style == null || srcOffset == 0;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				textStyleStackDepth++;
				textStyleStacks[textStyleStackDepth].Push(style.hashCode);
				uint[] styleOpeningTagArray = style.styleOpeningTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleOpeningTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth--;
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0001E5D0 File Offset: 0x0001C7D0
		public static void ReplaceOpeningStyleTag(ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			int num = textStyleStacks[textStyleStackDepth + 1].Pop();
			TextStyle style = TextGeneratorUtilities.GetStyle(generationSettings, num);
			bool flag = style == null;
			if (!flag)
			{
				textStyleStackDepth++;
				uint[] styleOpeningTagArray = style.styleOpeningTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleOpeningTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth--;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0001E624 File Offset: 0x0001C824
		private static bool ReplaceOpeningStyleTag(ref uint[] sourceText, int srcIndex, out int srcOffset, ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			int styleHashCode = TextGeneratorUtilities.GetStyleHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TextStyle style = TextGeneratorUtilities.GetStyle(generationSettings, styleHashCode);
			bool flag = style == null || srcOffset == 0;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				textStyleStackDepth++;
				textStyleStacks[textStyleStackDepth].Push(style.hashCode);
				uint[] styleOpeningTagArray = style.styleOpeningTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleOpeningTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth--;
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		public static void ReplaceClosingStyleTag(ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			int num = textStyleStacks[textStyleStackDepth + 1].Pop();
			TextStyle style = TextGeneratorUtilities.GetStyle(generationSettings, num);
			bool flag = style == null;
			if (!flag)
			{
				textStyleStackDepth++;
				uint[] styleClosingTagArray = style.styleClosingTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleClosingTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth--;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0001E6F4 File Offset: 0x0001C8F4
		internal static void InsertOpeningStyleTag(TextStyle style, ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			bool flag = style == null;
			if (!flag)
			{
				textStyleStacks[0].Push(style.hashCode);
				uint[] styleOpeningTagArray = style.styleOpeningTagArray;
				TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleOpeningTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
				textStyleStackDepth = 0;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0001E73C File Offset: 0x0001C93C
		internal static void InsertClosingStyleTag(ref TextProcessingElement[] charBuffer, ref int writeIndex, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			int num = textStyleStacks[0].Pop();
			TextStyle style = TextGeneratorUtilities.GetStyle(generationSettings, num);
			uint[] styleClosingTagArray = style.styleClosingTagArray;
			TextGeneratorUtilities.InsertTextStyleInTextProcessingArray(ref charBuffer, ref writeIndex, styleClosingTagArray, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
			textStyleStackDepth = 0;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0001E77C File Offset: 0x0001C97C
		private static void InsertTextStyleInTextProcessingArray(ref TextProcessingElement[] charBuffer, ref int writeIndex, uint[] styleDefinition, ref int textStyleStackDepth, ref TextProcessingStack<int>[] textStyleStacks, ref TextGenerationSettings generationSettings)
		{
			bool flag = generationSettings.tagNoParsing;
			int num = styleDefinition.Length;
			bool flag2 = writeIndex + num >= charBuffer.Length;
			if (flag2)
			{
				TextGeneratorUtilities.ResizeInternalArray<TextProcessingElement>(ref charBuffer, writeIndex + num);
			}
			int i = 0;
			while (i < num)
			{
				uint num2 = styleDefinition[i];
				bool flag3 = num2 == 92U && i + 1 < num;
				if (flag3)
				{
					uint num3 = styleDefinition[i + 1];
					uint num4 = num3;
					if (num4 <= 92U)
					{
						if (num4 != 85U)
						{
							if (num4 == 92U)
							{
								i++;
							}
						}
						else
						{
							bool flag4 = i + 9 < num;
							if (flag4)
							{
								num2 = TextGeneratorUtilities.GetUTF32(styleDefinition, i + 2);
								i += 9;
							}
						}
					}
					else if (num4 != 110U)
					{
						switch (num4)
						{
						case 117U:
						{
							bool flag5 = i + 5 < num;
							if (flag5)
							{
								num2 = TextGeneratorUtilities.GetUTF16(styleDefinition, i + 2);
								i += 5;
							}
							break;
						}
						}
					}
					else
					{
						num2 = 10U;
						i++;
					}
				}
				bool flag6 = num2 == 60U;
				if (flag6)
				{
					int markupTagHashCode = TextGeneratorUtilities.GetMarkupTagHashCode(styleDefinition, i + 1);
					MarkupTag markupTag = (MarkupTag)markupTagHashCode;
					MarkupTag markupTag2 = markupTag;
					if (markupTag2 <= MarkupTag.SHY)
					{
						if (markupTag2 <= MarkupTag.SLASH_NO_PARSE)
						{
							if (markupTag2 != MarkupTag.NO_PARSE)
							{
								if (markupTag2 == MarkupTag.SLASH_NO_PARSE)
								{
									flag = false;
								}
							}
							else
							{
								flag = true;
							}
						}
						else if (markupTag2 != MarkupTag.BR)
						{
							if (markupTag2 != MarkupTag.CR)
							{
								if (markupTag2 == MarkupTag.SHY)
								{
									bool flag7 = flag;
									if (!flag7)
									{
										charBuffer[writeIndex].unicode = 173U;
										writeIndex++;
										i += 4;
										goto IL_0332;
									}
								}
							}
							else
							{
								bool flag8 = flag;
								if (!flag8)
								{
									charBuffer[writeIndex].unicode = 13U;
									writeIndex++;
									i += 3;
									goto IL_0332;
								}
							}
						}
						else
						{
							bool flag9 = flag;
							if (!flag9)
							{
								charBuffer[writeIndex].unicode = 10U;
								writeIndex++;
								i += 3;
								goto IL_0332;
							}
						}
					}
					else if (markupTag2 <= MarkupTag.NBSP)
					{
						if (markupTag2 != MarkupTag.ZWJ)
						{
							if (markupTag2 == MarkupTag.NBSP)
							{
								bool flag10 = flag;
								if (!flag10)
								{
									charBuffer[writeIndex].unicode = 160U;
									writeIndex++;
									i += 5;
									goto IL_0332;
								}
							}
						}
						else
						{
							bool flag11 = flag;
							if (!flag11)
							{
								charBuffer[writeIndex].unicode = 8205U;
								writeIndex++;
								i += 4;
								goto IL_0332;
							}
						}
					}
					else if (markupTag2 != MarkupTag.ZWSP)
					{
						if (markupTag2 != MarkupTag.STYLE)
						{
							if (markupTag2 == MarkupTag.SLASH_STYLE)
							{
								bool flag12 = flag;
								if (!flag12)
								{
									TextGeneratorUtilities.ReplaceClosingStyleTag(ref charBuffer, ref writeIndex, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
									i += 7;
									goto IL_0332;
								}
							}
						}
						else
						{
							bool flag13 = flag;
							if (!flag13)
							{
								int num5;
								bool flag14 = TextGeneratorUtilities.ReplaceOpeningStyleTag(ref styleDefinition, i, out num5, ref charBuffer, ref writeIndex, ref textStyleStackDepth, ref textStyleStacks, ref generationSettings);
								if (flag14)
								{
									i = num5;
									goto IL_0332;
								}
							}
						}
					}
					else
					{
						bool flag15 = flag;
						if (!flag15)
						{
							charBuffer[writeIndex].unicode = 8203U;
							writeIndex++;
							i += 5;
							goto IL_0332;
						}
					}
					goto IL_031B;
				}
				goto IL_031B;
				IL_0332:
				i++;
				continue;
				IL_031B:
				charBuffer[writeIndex].unicode = num2;
				writeIndex++;
				goto IL_0332;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0001EACC File Offset: 0x0001CCCC
		public static TextStyle GetStyle(TextGenerationSettings generationSetting, int hashCode)
		{
			TextStyle textStyle = null;
			TextStyleSheet textStyleSheet = generationSetting.styleSheet;
			bool flag = textStyleSheet != null;
			if (flag)
			{
				textStyle = textStyleSheet.GetStyle(hashCode);
				bool flag2 = textStyle != null;
				if (flag2)
				{
					return textStyle;
				}
			}
			textStyleSheet = generationSetting.textSettings.defaultStyleSheet;
			bool flag3 = textStyleSheet != null;
			if (flag3)
			{
				textStyle = textStyleSheet.GetStyle(hashCode);
			}
			return textStyle;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0001EB30 File Offset: 0x0001CD30
		public static int GetStyleHashCode(ref uint[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				bool flag = text[i] == 34U;
				if (!flag)
				{
					bool flag2 = text[i] == 62U;
					if (flag2)
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ (int)TextGeneratorUtilities.ToUpperASCIIFast((char)text[i]);
				}
			}
			return num;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0001EB94 File Offset: 0x0001CD94
		public static int GetStyleHashCode(ref TextBackingContainer text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Capacity; i++)
			{
				bool flag = text[i] == 34U;
				if (!flag)
				{
					bool flag2 = text[i] == 62U;
					if (flag2)
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ (int)TextGeneratorUtilities.ToUpperASCIIFast((char)text[i]);
				}
			}
			return num;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0001EC04 File Offset: 0x0001CE04
		public static uint GetUTF16(uint[] text, int i)
		{
			uint num = 0U;
			num += TextGeneratorUtilities.HexToInt((char)text[i]) << 12;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 1]) << 8;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 2]) << 4;
			return num + TextGeneratorUtilities.HexToInt((char)text[i + 3]);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0001EC58 File Offset: 0x0001CE58
		public static uint GetUTF16(TextBackingContainer text, int i)
		{
			uint num = 0U;
			num += TextGeneratorUtilities.HexToInt((char)text[i]) << 12;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 1]) << 8;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 2]) << 4;
			return num + TextGeneratorUtilities.HexToInt((char)text[i + 3]);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0001ECC0 File Offset: 0x0001CEC0
		public static uint GetUTF32(uint[] text, int i)
		{
			uint num = 0U;
			num += TextGeneratorUtilities.HexToInt((char)text[i]) << 28;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 1]) << 24;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 2]) << 20;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 3]) << 16;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 4]) << 12;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 5]) << 8;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 6]) << 4;
			return num + TextGeneratorUtilities.HexToInt((char)text[i + 7]);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0001ED58 File Offset: 0x0001CF58
		public static uint GetUTF32(TextBackingContainer text, int i)
		{
			uint num = 0U;
			num += TextGeneratorUtilities.HexToInt((char)text[i]) << 28;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 1]) << 24;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 2]) << 20;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 3]) << 16;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 4]) << 12;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 5]) << 8;
			num += TextGeneratorUtilities.HexToInt((char)text[i + 6]) << 4;
			return num + TextGeneratorUtilities.HexToInt((char)text[i + 7]);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0001EE18 File Offset: 0x0001D018
		private static int GetTagHashCode(ref int[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				bool flag = text[i] == 34;
				if (!flag)
				{
					bool flag2 = text[i] == 62;
					if (flag2)
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ (int)TextUtilities.ToUpperASCIIFast((uint)((ushort)text[i]));
				}
			}
			return num;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0001EE7C File Offset: 0x0001D07C
		private static int GetTagHashCode(ref string text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				bool flag = text[i] == '"';
				if (!flag)
				{
					bool flag2 = text[i] == '>';
					if (flag2)
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num) ^ (int)TextUtilities.ToUpperASCIIFast((uint)text[i]);
				}
			}
			return num;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0001EEEC File Offset: 0x0001D0EC
		public static void FillCharacterVertexBuffers(int i, bool convertToLinearSpace, TextGenerationSettings generationSettings, TextInfo textInfo)
		{
			int materialReferenceIndex = textInfo.textElementInfo[i].materialReferenceIndex;
			int vertexCount = textInfo.meshInfo[materialReferenceIndex].vertexCount;
			bool flag = vertexCount >= textInfo.meshInfo[materialReferenceIndex].vertices.Length;
			if (flag)
			{
				textInfo.meshInfo[materialReferenceIndex].ResizeMeshInfo(Mathf.NextPowerOfTwo((vertexCount + 4) / 4));
			}
			TextElementInfo[] textElementInfo = textInfo.textElementInfo;
			textInfo.textElementInfo[i].vertexIndex = vertexCount;
			bool inverseYAxis = generationSettings.inverseYAxis;
			if (inverseYAxis)
			{
				Vector3 vector;
				vector.x = 0f;
				vector.y = generationSettings.screenRect.y + generationSettings.screenRect.height;
				vector.z = 0f;
				Vector3 vector2 = textElementInfo[i].vertexBottomLeft.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = vector2 + vector;
				vector2 = textElementInfo[i].vertexTopLeft.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = vector2 + vector;
				vector2 = textElementInfo[i].vertexTopRight.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = vector2 + vector;
				vector2 = textElementInfo[i].vertexBottomRight.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = vector2 + vector;
			}
			else
			{
				textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = textElementInfo[i].vertexBottomLeft.position;
				textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = textElementInfo[i].vertexTopLeft.position;
				textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = textElementInfo[i].vertexTopRight.position;
				textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = textElementInfo[i].vertexBottomRight.position;
			}
			textInfo.meshInfo[materialReferenceIndex].uvs0[vertexCount] = textElementInfo[i].vertexBottomLeft.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs0[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs0[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs0[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs2[vertexCount] = textElementInfo[i].vertexBottomLeft.uv2;
			textInfo.meshInfo[materialReferenceIndex].uvs2[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv2;
			textInfo.meshInfo[materialReferenceIndex].uvs2[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv2;
			textInfo.meshInfo[materialReferenceIndex].uvs2[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv2;
			textInfo.meshInfo[materialReferenceIndex].colors32[vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexBottomLeft.color) : textElementInfo[i].vertexBottomLeft.color);
			textInfo.meshInfo[materialReferenceIndex].colors32[1 + vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexTopLeft.color) : textElementInfo[i].vertexTopLeft.color);
			textInfo.meshInfo[materialReferenceIndex].colors32[2 + vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexTopRight.color) : textElementInfo[i].vertexTopRight.color);
			textInfo.meshInfo[materialReferenceIndex].colors32[3 + vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexBottomRight.color) : textElementInfo[i].vertexBottomRight.color);
			textInfo.meshInfo[materialReferenceIndex].vertexCount = vertexCount + 4;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0001F3E8 File Offset: 0x0001D5E8
		public static void FillSpriteVertexBuffers(int i, bool convertToLinearSpace, TextGenerationSettings generationSettings, TextInfo textInfo)
		{
			int materialReferenceIndex = textInfo.textElementInfo[i].materialReferenceIndex;
			int vertexCount = textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TextElementInfo[] textElementInfo = textInfo.textElementInfo;
			textInfo.textElementInfo[i].vertexIndex = vertexCount;
			bool inverseYAxis = generationSettings.inverseYAxis;
			if (inverseYAxis)
			{
				Vector3 vector;
				vector.x = 0f;
				vector.y = generationSettings.screenRect.y + generationSettings.screenRect.height;
				vector.z = 0f;
				Vector3 vector2 = textElementInfo[i].vertexBottomLeft.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = vector2 + vector;
				vector2 = textElementInfo[i].vertexTopLeft.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = vector2 + vector;
				vector2 = textElementInfo[i].vertexTopRight.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = vector2 + vector;
				vector2 = textElementInfo[i].vertexBottomRight.position;
				vector2.y *= -1f;
				textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = vector2 + vector;
			}
			else
			{
				textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = textElementInfo[i].vertexBottomLeft.position;
				textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = textElementInfo[i].vertexTopLeft.position;
				textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = textElementInfo[i].vertexTopRight.position;
				textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = textElementInfo[i].vertexBottomRight.position;
			}
			textInfo.meshInfo[materialReferenceIndex].uvs0[vertexCount] = textElementInfo[i].vertexBottomLeft.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs0[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs0[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs0[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv;
			textInfo.meshInfo[materialReferenceIndex].uvs2[vertexCount] = textElementInfo[i].vertexBottomLeft.uv2;
			textInfo.meshInfo[materialReferenceIndex].uvs2[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv2;
			textInfo.meshInfo[materialReferenceIndex].uvs2[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv2;
			textInfo.meshInfo[materialReferenceIndex].uvs2[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv2;
			textInfo.meshInfo[materialReferenceIndex].colors32[vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexBottomLeft.color) : textElementInfo[i].vertexBottomLeft.color);
			textInfo.meshInfo[materialReferenceIndex].colors32[1 + vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexTopLeft.color) : textElementInfo[i].vertexTopLeft.color);
			textInfo.meshInfo[materialReferenceIndex].colors32[2 + vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexTopRight.color) : textElementInfo[i].vertexTopRight.color);
			textInfo.meshInfo[materialReferenceIndex].colors32[3 + vertexCount] = (convertToLinearSpace ? TextGeneratorUtilities.GammaToLinear(textElementInfo[i].vertexBottomRight.color) : textElementInfo[i].vertexBottomRight.color);
			textInfo.meshInfo[materialReferenceIndex].vertexCount = vertexCount + 4;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0001F8AC File Offset: 0x0001DAAC
		public static void AdjustLineOffset(int startIndex, int endIndex, float offset, TextInfo textInfo)
		{
			Vector3 vector = new Vector3(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				TextElementInfo[] textElementInfo = textInfo.textElementInfo;
				int num = i;
				textElementInfo[num].bottomLeft = textElementInfo[num].bottomLeft - vector;
				TextElementInfo[] textElementInfo2 = textInfo.textElementInfo;
				int num2 = i;
				textElementInfo2[num2].topLeft = textElementInfo2[num2].topLeft - vector;
				TextElementInfo[] textElementInfo3 = textInfo.textElementInfo;
				int num3 = i;
				textElementInfo3[num3].topRight = textElementInfo3[num3].topRight - vector;
				TextElementInfo[] textElementInfo4 = textInfo.textElementInfo;
				int num4 = i;
				textElementInfo4[num4].bottomRight = textElementInfo4[num4].bottomRight - vector;
				TextElementInfo[] textElementInfo5 = textInfo.textElementInfo;
				int num5 = i;
				textElementInfo5[num5].ascender = textElementInfo5[num5].ascender - vector.y;
				TextElementInfo[] textElementInfo6 = textInfo.textElementInfo;
				int num6 = i;
				textElementInfo6[num6].baseLine = textElementInfo6[num6].baseLine - vector.y;
				TextElementInfo[] textElementInfo7 = textInfo.textElementInfo;
				int num7 = i;
				textElementInfo7[num7].descender = textElementInfo7[num7].descender - vector.y;
				bool isVisible = textInfo.textElementInfo[i].isVisible;
				if (isVisible)
				{
					TextElementInfo[] textElementInfo8 = textInfo.textElementInfo;
					int num8 = i;
					textElementInfo8[num8].vertexBottomLeft.position = textElementInfo8[num8].vertexBottomLeft.position - vector;
					TextElementInfo[] textElementInfo9 = textInfo.textElementInfo;
					int num9 = i;
					textElementInfo9[num9].vertexTopLeft.position = textElementInfo9[num9].vertexTopLeft.position - vector;
					TextElementInfo[] textElementInfo10 = textInfo.textElementInfo;
					int num10 = i;
					textElementInfo10[num10].vertexTopRight.position = textElementInfo10[num10].vertexTopRight.position - vector;
					TextElementInfo[] textElementInfo11 = textInfo.textElementInfo;
					int num11 = i;
					textElementInfo11[num11].vertexBottomRight.position = textElementInfo11[num11].vertexBottomRight.position - vector;
				}
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0001FA78 File Offset: 0x0001DC78
		public static void ResizeLineExtents(int size, TextInfo textInfo)
		{
			size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size + 1));
			LineInfo[] array = new LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				bool flag = i < textInfo.lineInfo.Length;
				if (flag)
				{
					array[i] = textInfo.lineInfo[i];
				}
				else
				{
					array[i].lineExtents.min = TextGeneratorUtilities.largePositiveVector2;
					array[i].lineExtents.max = TextGeneratorUtilities.largeNegativeVector2;
					array[i].ascender = -32767f;
					array[i].descender = 32767f;
				}
			}
			textInfo.lineInfo = array;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0001FB38 File Offset: 0x0001DD38
		public static FontStyles LegacyStyleToNewStyle(FontStyle fontStyle)
		{
			FontStyles fontStyles;
			switch (fontStyle)
			{
			case FontStyle.Bold:
				fontStyles = FontStyles.Bold;
				break;
			case FontStyle.Italic:
				fontStyles = FontStyles.Italic;
				break;
			case FontStyle.BoldAndItalic:
				fontStyles = FontStyles.Bold | FontStyles.Italic;
				break;
			default:
				fontStyles = FontStyles.Normal;
				break;
			}
			return fontStyles;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0001FB74 File Offset: 0x0001DD74
		public static TextAlignment LegacyAlignmentToNewAlignment(TextAnchor anchor)
		{
			TextAlignment textAlignment;
			switch (anchor)
			{
			case TextAnchor.UpperLeft:
				textAlignment = TextAlignment.TopLeft;
				break;
			case TextAnchor.UpperCenter:
				textAlignment = TextAlignment.TopCenter;
				break;
			case TextAnchor.UpperRight:
				textAlignment = TextAlignment.TopRight;
				break;
			case TextAnchor.MiddleLeft:
				textAlignment = TextAlignment.MiddleLeft;
				break;
			case TextAnchor.MiddleCenter:
				textAlignment = TextAlignment.MiddleCenter;
				break;
			case TextAnchor.MiddleRight:
				textAlignment = TextAlignment.MiddleRight;
				break;
			case TextAnchor.LowerLeft:
				textAlignment = TextAlignment.BottomLeft;
				break;
			case TextAnchor.LowerCenter:
				textAlignment = TextAlignment.BottomCenter;
				break;
			case TextAnchor.LowerRight:
				textAlignment = TextAlignment.BottomRight;
				break;
			default:
				textAlignment = TextAlignment.TopLeft;
				break;
			}
			return textAlignment;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0001FC04 File Offset: 0x0001DE04
		public static uint ConvertToUTF32(uint highSurrogate, uint lowSurrogate)
		{
			return (highSurrogate - 55296U) * 1024U + (lowSurrogate - 56320U + 65536U);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0001FC34 File Offset: 0x0001DE34
		public static int GetMarkupTagHashCode(TextBackingContainer styleDefinition, int readIndex)
		{
			int num = 0;
			int num2 = readIndex + 16;
			int capacity = styleDefinition.Capacity;
			while (readIndex < num2 && readIndex < capacity)
			{
				uint num3 = styleDefinition[readIndex];
				bool flag = num3 == 62U || num3 == 61U || num3 == 32U;
				if (flag)
				{
					return num;
				}
				num = ((num << 5) + num) ^ (int)TextGeneratorUtilities.ToUpperASCIIFast(num3);
				readIndex++;
			}
			return num;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
		public static int GetMarkupTagHashCode(uint[] styleDefinition, int readIndex)
		{
			int num = 0;
			int num2 = readIndex + 16;
			int num3 = styleDefinition.Length;
			while (readIndex < num2 && readIndex < num3)
			{
				uint num4 = styleDefinition[readIndex];
				bool flag = num4 == 62U || num4 == 61U || num4 == 32U;
				if (flag)
				{
					return num;
				}
				num = ((num << 5) + num) ^ (int)TextGeneratorUtilities.ToUpperASCIIFast(num4);
				readIndex++;
			}
			return num;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0001FD10 File Offset: 0x0001DF10
		public static char ToUpperASCIIFast(char c)
		{
			bool flag = (int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1;
			char c2;
			if (flag)
			{
				c2 = c;
			}
			else
			{
				c2 = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
			}
			return c2;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0001FD44 File Offset: 0x0001DF44
		public static uint ToUpperASCIIFast(uint c)
		{
			bool flag = (ulong)c > (ulong)((long)("-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1));
			uint num;
			if (flag)
			{
				num = c;
			}
			else
			{
				num = (uint)"-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
			}
			return num;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0001FD7C File Offset: 0x0001DF7C
		public static char ToUpperFast(char c)
		{
			bool flag = (int)c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1;
			char c2;
			if (flag)
			{
				c2 = c;
			}
			else
			{
				c2 = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[(int)c];
			}
			return c2;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0001FDB0 File Offset: 0x0001DFB0
		public static int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
		{
			int i = startIndex;
			int num = 0;
			while (i < startIndex + length)
			{
				parameters[num] = TextGeneratorUtilities.ConvertToFloat(chars, startIndex, length, out i);
				length -= i - startIndex + 1;
				startIndex = i + 1;
				num++;
			}
			return num;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0001FDF8 File Offset: 0x0001DFF8
		public static bool IsBitmapRendering(GlyphRenderMode glyphRenderMode)
		{
			return glyphRenderMode == GlyphRenderMode.RASTER || glyphRenderMode == GlyphRenderMode.RASTER_HINTED || glyphRenderMode == GlyphRenderMode.SMOOTH || glyphRenderMode == GlyphRenderMode.SMOOTH_HINTED;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0001FE30 File Offset: 0x0001E030
		public static bool IsBaseGlyph(uint c)
		{
			return (c < 768U || c > 879U) && (c < 6832U || c > 6911U) && (c < 7616U || c > 7679U) && (c < 8400U || c > 8447U) && (c < 65056U || c > 65071U) && c != 3633U && (c < 3636U || c > 3642U) && (c < 3655U || c > 3662U) && (c < 1425U || c > 1469U) && c != 1471U && (c < 1473U || c > 1474U) && (c < 1476U || c > 1477U) && c != 1479U && (c < 1552U || c > 1562U) && (c < 1611U || c > 1631U) && c != 1648U && (c < 1750U || c > 1756U) && (c < 1759U || c > 1764U) && (c < 1767U || c > 1768U) && (c < 1770U || c > 1773U) && (c < 2259U || c > 2273U) && (c < 2275U || c > 2303U) && (c < 64434U || c > 64449U);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0001FFC4 File Offset: 0x0001E1C4
		public static Color MinAlpha(this Color c1, Color c2)
		{
			float num = ((c1.a < c2.a) ? c1.a : c2.a);
			return new Color(c1.r, c1.g, c1.b, num);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0002000C File Offset: 0x0001E20C
		internal static Color32 GammaToLinear(Color32 c)
		{
			return new Color32(TextGeneratorUtilities.GammaToLinear(c.r), TextGeneratorUtilities.GammaToLinear(c.g), TextGeneratorUtilities.GammaToLinear(c.b), c.a);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0002004C File Offset: 0x0001E24C
		private static byte GammaToLinear(byte value)
		{
			float num = (float)value / 255f;
			bool flag = num <= 0.04045f;
			byte b;
			if (flag)
			{
				b = (byte)(num / 12.92f * 255f);
			}
			else
			{
				bool flag2 = num < 1f;
				if (flag2)
				{
					b = (byte)(Mathf.Pow((num + 0.055f) / 1.055f, 2.4f) * 255f);
				}
				else
				{
					bool flag3 = num == 1f;
					if (flag3)
					{
						b = byte.MaxValue;
					}
					else
					{
						b = (byte)(Mathf.Pow(num, 2.2f) * 255f);
					}
				}
			}
			return b;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000200DC File Offset: 0x0001E2DC
		public static bool IsValidUTF16(TextBackingContainer text, int index)
		{
			for (int i = 0; i < 4; i++)
			{
				uint num = text[index + i];
				bool flag = (num < 48U || num > 57U) && (num < 97U || num > 102U) && (num < 65U || num > 70U);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0002013C File Offset: 0x0001E33C
		public static bool IsValidUTF32(TextBackingContainer text, int index)
		{
			for (int i = 0; i < 8; i++)
			{
				uint num = text[index + i];
				bool flag = (num < 48U || num > 57U) && (num < 97U || num > 102U) && (num < 65U || num > 70U);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0002019C File Offset: 0x0001E39C
		internal static bool IsEmoji(uint c)
		{
			return c == 8205U || c == 8252U || c == 8265U || c == 8419U || c == 8482U || c == 8505U || (c >= 8596U && c <= 8601U) || (c >= 8617U && c <= 8618U) || (c >= 8986U && c <= 8987U) || c == 9000U || c == 9096U || c == 9167U || (c >= 9193U && c <= 9203U) || (c >= 9208U && c <= 9210U) || c == 9410U || (c >= 9642U && c <= 9643U) || c == 9654U || c == 9664U || (c >= 9723U && c <= 9726U) || (c >= 9728U && c <= 9733U) || (c >= 9735U && c <= 9746U) || (c >= 9748U && c <= 9861U) || (c >= 9872U && c <= 9989U) || (c >= 9992U && c <= 10002U) || c == 10004U || c == 10006U || c == 10013U || c == 10017U || c == 10024U || (c >= 10035U && c <= 10036U) || c == 10052U || c == 10055U || c == 10060U || c == 10062U || (c >= 10067U && c <= 10069U) || c == 10071U || (c >= 10083U && c <= 10087U) || (c >= 10133U && c <= 10135U) || c == 10145U || c == 10160U || c == 10175U || (c >= 10548U && c <= 10549U) || (c >= 11013U && c <= 11015U) || (c >= 11035U && c <= 11036U) || c == 11088U || c == 11093U || c == 12336U || c == 12349U || c == 12951U || c == 12953U || c == 65039U || (c >= 126976U && c <= 127231U) || (c >= 127245U && c <= 127247U) || c == 127279U || (c >= 127340U && c <= 127345U) || (c >= 127358U && c <= 127359U) || c == 127374U || (c >= 127377U && c <= 127386U) || (c >= 127405U && c <= 127487U) || (c >= 127489U && c <= 127503U) || c == 127514U || c == 127535U || (c >= 127538U && c <= 127546U) || (c >= 127548U && c <= 127551U) || (c >= 127561U && c <= 128317U) || (c >= 128326U && c <= 128591U) || (c >= 128640U && c <= 128767U) || (c >= 128884U && c <= 128895U) || (c >= 128981U && c <= 129023U) || (c >= 129036U && c <= 129039U) || (c >= 129096U && c <= 129103U) || (c >= 129114U && c <= 129119U) || (c >= 129160U && c <= 129167U) || (c >= 129198U && c <= 129279U) || (c >= 129292U && c <= 129338U) || (c >= 129340U && c <= 129349U) || (c >= 129351U && c <= 129791U) || (c >= 130048U && c <= 131069U) || (c >= 917536U && c <= 917631U);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0002065C File Offset: 0x0001E85C
		internal static bool IsHangul(uint c)
		{
			return (c >= 4352U && c <= 4607U) || (c >= 43360U && c <= 43391U) || (c >= 55216U && c <= 55295U) || (c >= 12592U && c <= 12687U) || (c >= 65440U && c <= 65500U) || (c >= 44032U && c <= 55215U);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000206D8 File Offset: 0x0001E8D8
		internal static bool IsCJK(uint c)
		{
			return (c >= 12288U && c <= 12351U) || (c >= 94176U && c <= 5887U) || (c >= 12544U && c <= 12591U) || (c >= 12704U && c <= 12735U) || (c >= 19968U && c <= 40959U) || (c >= 13312U && c <= 19903U) || (c >= 131072U && c <= 173791U) || (c >= 173824U && c <= 177983U) || (c >= 177984U && c <= 178207U) || (c >= 178208U && c <= 183983U) || (c >= 183984U && c <= 191456U) || (c >= 196608U && c <= 201546U) || (c >= 63744U && c <= 64255U) || (c >= 194560U && c <= 195103U) || (c >= 12032U && c <= 12255U) || (c >= 11904U && c <= 12031U) || (c >= 12736U && c <= 12783U) || (c >= 12272U && c <= 12287U) || (c >= 12352U && c <= 12447U) || (c >= 110848U && c <= 110895U) || (c >= 110576U && c <= 110591U) || (c >= 110592U && c <= 110847U) || (c >= 110896U && c <= 110959U) || (c >= 12688U && c <= 12703U) || (c >= 12448U && c <= 12543U) || (c >= 12784U && c <= 12799U) || (c >= 65381U && c <= 65439U);
		}

		// Token: 0x040002AA RID: 682
		public static readonly Vector2 largePositiveVector2 = new Vector2(2.1474836E+09f, 2.1474836E+09f);

		// Token: 0x040002AB RID: 683
		public static readonly Vector2 largeNegativeVector2 = new Vector2(-214748370f, -214748370f);

		// Token: 0x040002AC RID: 684
		public const float largePositiveFloat = 32767f;

		// Token: 0x040002AD RID: 685
		public const float largeNegativeFloat = -32767f;

		// Token: 0x040002AE RID: 686
		private const int k_DoubleQuotes = 34;

		// Token: 0x040002AF RID: 687
		private const int k_GreaterThan = 62;

		// Token: 0x040002B0 RID: 688
		private const int k_ZeroWidthSpace = 8203;

		// Token: 0x040002B1 RID: 689
		private const string k_LookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";
	}
}
