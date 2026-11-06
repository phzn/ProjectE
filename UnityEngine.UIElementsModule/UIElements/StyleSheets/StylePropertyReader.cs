using System;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x0200048F RID: 1167
	internal class StylePropertyReader
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x0009766B File Offset: 0x0009586B
		// (set) Token: 0x0600246B RID: 9323 RVA: 0x00097673 File Offset: 0x00095873
		public StyleProperty property { get; private set; }

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x0009767C File Offset: 0x0009587C
		// (set) Token: 0x0600246D RID: 9325 RVA: 0x00097684 File Offset: 0x00095884
		public StylePropertyId propertyId { get; private set; }

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0009768D File Offset: 0x0009588D
		// (set) Token: 0x0600246F RID: 9327 RVA: 0x00097695 File Offset: 0x00095895
		public int valueCount { get; private set; }

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x0009769E File Offset: 0x0009589E
		// (set) Token: 0x06002471 RID: 9329 RVA: 0x000976A6 File Offset: 0x000958A6
		public float dpiScaling { get; private set; }

		// Token: 0x06002472 RID: 9330 RVA: 0x000976B0 File Offset: 0x000958B0
		public void SetContext(StyleSheet sheet, StyleComplexSelector selector, StyleVariableContext varContext, float dpiScaling = 1f)
		{
			this.m_Sheet = sheet;
			this.m_Properties = selector.rule.properties;
			this.m_PropertyIds = StyleSheetCache.GetPropertyIds(sheet, selector.ruleIndex);
			this.m_Resolver.variableContext = varContext;
			this.dpiScaling = dpiScaling;
			this.LoadProperties();
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00097705 File Offset: 0x00095905
		public void SetInlineContext(StyleSheet sheet, StyleProperty[] properties, StylePropertyId[] propertyIds, float dpiScaling = 1f)
		{
			this.m_Sheet = sheet;
			this.m_Properties = properties;
			this.m_PropertyIds = propertyIds;
			this.dpiScaling = dpiScaling;
			this.LoadProperties();
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x00097730 File Offset: 0x00095930
		public StylePropertyId MoveNextProperty()
		{
			this.m_CurrentPropertyIndex++;
			this.m_CurrentValueIndex += this.valueCount;
			this.SetCurrentProperty();
			return this.propertyId;
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x00097770 File Offset: 0x00095970
		public StylePropertyValue GetValue(int index)
		{
			return this.m_Values[this.m_CurrentValueIndex + index];
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x00097798 File Offset: 0x00095998
		public StyleValueType GetValueType(int index)
		{
			return this.m_Values[this.m_CurrentValueIndex + index].handle.valueType;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000977CC File Offset: 0x000959CC
		public bool IsValueType(int index, StyleValueType type)
		{
			return this.m_Values[this.m_CurrentValueIndex + index].handle.valueType == type;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00097804 File Offset: 0x00095A04
		public bool IsKeyword(int index, StyleValueKeyword keyword)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			return stylePropertyValue.handle.valueType == StyleValueType.Keyword && stylePropertyValue.handle.valueIndex == (int)keyword;
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0009784C File Offset: 0x00095A4C
		public string ReadAsString(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			return stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00097884 File Offset: 0x00095A84
		public Length ReadLength(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			bool flag = stylePropertyValue.handle.valueType == StyleValueType.Keyword;
			Length length;
			if (flag)
			{
				StyleValueKeyword valueIndex = (StyleValueKeyword)stylePropertyValue.handle.valueIndex;
				StyleValueKeyword styleValueKeyword = valueIndex;
				StyleValueKeyword styleValueKeyword2 = styleValueKeyword;
				if (styleValueKeyword2 != StyleValueKeyword.Auto)
				{
					if (styleValueKeyword2 != StyleValueKeyword.None)
					{
						length = default(Length);
					}
					else
					{
						length = Length.None();
					}
				}
				else
				{
					length = Length.Auto();
				}
			}
			else
			{
				length = stylePropertyValue.sheet.ReadDimension(stylePropertyValue.handle).ToLength();
			}
			return length;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0009791C File Offset: 0x00095B1C
		public TimeValue ReadTimeValue(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			return stylePropertyValue.sheet.ReadDimension(stylePropertyValue.handle).ToTime();
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0009795C File Offset: 0x00095B5C
		public Translate ReadTranslate(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			StylePropertyValue stylePropertyValue3 = ((this.valueCount > 2) ? this.m_Values[this.m_CurrentValueIndex + index + 2] : default(StylePropertyValue));
			return StylePropertyReader.ReadTranslate(this.valueCount, stylePropertyValue, stylePropertyValue2, stylePropertyValue3);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000979E8 File Offset: 0x00095BE8
		public TransformOrigin ReadTransformOrigin(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			StylePropertyValue stylePropertyValue3 = ((this.valueCount > 2) ? this.m_Values[this.m_CurrentValueIndex + index + 2] : default(StylePropertyValue));
			return StylePropertyReader.ReadTransformOrigin(this.valueCount, stylePropertyValue, stylePropertyValue2, stylePropertyValue3);
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x00097A74 File Offset: 0x00095C74
		public Rotate ReadRotate(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			StylePropertyValue stylePropertyValue3 = ((this.valueCount > 2) ? this.m_Values[this.m_CurrentValueIndex + index + 2] : default(StylePropertyValue));
			StylePropertyValue stylePropertyValue4 = ((this.valueCount > 3) ? this.m_Values[this.m_CurrentValueIndex + index + 3] : default(StylePropertyValue));
			return StylePropertyReader.ReadRotate(this.valueCount, stylePropertyValue, stylePropertyValue2, stylePropertyValue3, stylePropertyValue4);
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x00097B2C File Offset: 0x00095D2C
		public Scale ReadScale(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			StylePropertyValue stylePropertyValue3 = ((this.valueCount > 2) ? this.m_Values[this.m_CurrentValueIndex + index + 2] : default(StylePropertyValue));
			return StylePropertyReader.ReadScale(this.valueCount, stylePropertyValue, stylePropertyValue2, stylePropertyValue3);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00097BB8 File Offset: 0x00095DB8
		public float ReadFloat(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			return stylePropertyValue.sheet.ReadFloat(stylePropertyValue.handle);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00097BF0 File Offset: 0x00095DF0
		public int ReadInt(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			return (int)stylePropertyValue.sheet.ReadFloat(stylePropertyValue.handle);
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00097C28 File Offset: 0x00095E28
		public Color ReadColor(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			Color color = Color.clear;
			bool flag = stylePropertyValue.handle.valueType == StyleValueType.Enum;
			if (flag)
			{
				string text = stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
				StyleSheetColor.TryGetColor(text.ToLowerInvariant(), out color);
			}
			else
			{
				color = stylePropertyValue.sheet.ReadColor(stylePropertyValue.handle);
			}
			return color;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x00097CA4 File Offset: 0x00095EA4
		public int ReadEnum(StyleEnumType enumType, int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StyleValueHandle handle = stylePropertyValue.handle;
			bool flag = handle.valueType == StyleValueType.Keyword;
			string text;
			if (flag)
			{
				StyleValueKeyword styleValueKeyword = stylePropertyValue.sheet.ReadKeyword(handle);
				text = styleValueKeyword.ToUssString();
			}
			else
			{
				text = stylePropertyValue.sheet.ReadEnum(handle);
			}
			int num;
			StylePropertyUtil.TryGetEnumIntValue(enumType, text, out num);
			return num;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00097D1C File Offset: 0x00095F1C
		public FontDefinition ReadFontDefinition(int index)
		{
			FontAsset fontAsset = null;
			Font font = null;
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StyleValueType valueType = stylePropertyValue.handle.valueType;
			StyleValueType styleValueType = valueType;
			if (styleValueType != StyleValueType.Keyword)
			{
				if (styleValueType != StyleValueType.ResourcePath)
				{
					if (styleValueType != StyleValueType.AssetReference)
					{
						Debug.LogWarning("Invalid value for font " + stylePropertyValue.handle.valueType.ToString());
					}
					else
					{
						font = stylePropertyValue.sheet.ReadAssetReference(stylePropertyValue.handle) as Font;
						bool flag = font == null;
						if (flag)
						{
							fontAsset = stylePropertyValue.sheet.ReadAssetReference(stylePropertyValue.handle) as FontAsset;
						}
					}
				}
				else
				{
					string text = stylePropertyValue.sheet.ReadResourcePath(stylePropertyValue.handle);
					bool flag2 = !string.IsNullOrEmpty(text);
					if (flag2)
					{
						font = Panel.LoadResource(text, typeof(Font), this.dpiScaling) as Font;
						bool flag3 = font == null;
						if (flag3)
						{
							fontAsset = Panel.LoadResource(text, typeof(FontAsset), this.dpiScaling) as FontAsset;
						}
					}
					bool flag4 = fontAsset == null && font == null;
					if (flag4)
					{
						Debug.LogWarning(string.Format("Font not found for path: {0}", text));
					}
				}
			}
			else
			{
				bool flag5 = stylePropertyValue.handle.valueIndex != 6;
				if (flag5)
				{
					string text2 = "Invalid keyword for font ";
					StyleValueKeyword valueIndex = (StyleValueKeyword)stylePropertyValue.handle.valueIndex;
					Debug.LogWarning(text2 + valueIndex.ToString());
				}
			}
			bool flag6 = font != null;
			FontDefinition fontDefinition;
			if (flag6)
			{
				fontDefinition = FontDefinition.FromFont(font);
			}
			else
			{
				bool flag7 = fontAsset != null;
				if (flag7)
				{
					fontDefinition = FontDefinition.FromSDFFont(fontAsset);
				}
				else
				{
					fontDefinition = default(FontDefinition);
				}
			}
			return fontDefinition;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00097EFC File Offset: 0x000960FC
		public Font ReadFont(int index)
		{
			Font font = null;
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StyleValueType valueType = stylePropertyValue.handle.valueType;
			StyleValueType styleValueType = valueType;
			if (styleValueType != StyleValueType.Keyword)
			{
				if (styleValueType != StyleValueType.ResourcePath)
				{
					if (styleValueType != StyleValueType.AssetReference)
					{
						Debug.LogWarning("Invalid value for font " + stylePropertyValue.handle.valueType.ToString());
					}
					else
					{
						font = stylePropertyValue.sheet.ReadAssetReference(stylePropertyValue.handle) as Font;
					}
				}
				else
				{
					string text = stylePropertyValue.sheet.ReadResourcePath(stylePropertyValue.handle);
					bool flag = !string.IsNullOrEmpty(text);
					if (flag)
					{
						font = Panel.LoadResource(text, typeof(Font), this.dpiScaling) as Font;
					}
					bool flag2 = font == null;
					if (flag2)
					{
						Debug.LogWarning(string.Format("Font not found for path: {0}", text));
					}
				}
			}
			else
			{
				bool flag3 = stylePropertyValue.handle.valueIndex != 6;
				if (flag3)
				{
					string text2 = "Invalid keyword for font ";
					StyleValueKeyword valueIndex = (StyleValueKeyword)stylePropertyValue.handle.valueIndex;
					Debug.LogWarning(text2 + valueIndex.ToString());
				}
			}
			return font;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x00098040 File Offset: 0x00096240
		public Background ReadBackground(int index)
		{
			ImageSource imageSource = default(ImageSource);
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			bool flag = stylePropertyValue.handle.valueType == StyleValueType.Keyword;
			if (flag)
			{
				bool flag2 = stylePropertyValue.handle.valueIndex != 6;
				if (flag2)
				{
					string text = "Invalid keyword for image source ";
					StyleValueKeyword valueIndex = (StyleValueKeyword)stylePropertyValue.handle.valueIndex;
					Debug.LogWarning(text + valueIndex.ToString());
				}
			}
			else
			{
				bool flag3 = !StylePropertyReader.TryGetImageSourceFromValue(stylePropertyValue, this.dpiScaling, out imageSource);
				if (flag3)
				{
				}
			}
			bool flag4 = imageSource.texture != null;
			Background background;
			if (flag4)
			{
				background = Background.FromTexture2D(imageSource.texture);
			}
			else
			{
				bool flag5 = imageSource.sprite != null;
				if (flag5)
				{
					background = Background.FromSprite(imageSource.sprite);
				}
				else
				{
					bool flag6 = imageSource.vectorImage != null;
					if (flag6)
					{
						background = Background.FromVectorImage(imageSource.vectorImage);
					}
					else
					{
						bool flag7 = imageSource.renderTexture != null;
						if (flag7)
						{
							background = Background.FromRenderTexture(imageSource.renderTexture);
						}
						else
						{
							background = default(Background);
						}
					}
				}
			}
			return background;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x00098170 File Offset: 0x00096370
		public Cursor ReadCursor(int index)
		{
			float num = 0f;
			float num2 = 0f;
			int num3 = 0;
			Texture2D texture2D = null;
			StyleValueType valueType = this.GetValueType(index);
			bool flag = valueType == StyleValueType.ResourcePath || valueType == StyleValueType.AssetReference || valueType == StyleValueType.ScalableImage || valueType == StyleValueType.MissingAssetReference;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = this.valueCount < 1;
				if (flag3)
				{
					Debug.LogWarning(string.Format("USS 'cursor' has invalid value at {0}.", index));
				}
				else
				{
					ImageSource imageSource = default(ImageSource);
					StylePropertyValue value = this.GetValue(index);
					bool flag4 = StylePropertyReader.TryGetImageSourceFromValue(value, this.dpiScaling, out imageSource);
					if (flag4)
					{
						texture2D = imageSource.texture;
						bool flag5 = this.valueCount >= 3;
						if (flag5)
						{
							StylePropertyValue value2 = this.GetValue(index + 1);
							StylePropertyValue value3 = this.GetValue(index + 2);
							bool flag6 = value2.handle.valueType != StyleValueType.Float || value3.handle.valueType != StyleValueType.Float;
							if (flag6)
							{
								Debug.LogWarning("USS 'cursor' property requires two integers for the hot spot value.");
							}
							else
							{
								num = value2.sheet.ReadFloat(value2.handle);
								num2 = value3.sheet.ReadFloat(value3.handle);
							}
						}
					}
				}
			}
			else
			{
				bool flag7 = StylePropertyReader.getCursorIdFunc != null;
				if (flag7)
				{
					StylePropertyValue value4 = this.GetValue(index);
					num3 = StylePropertyReader.getCursorIdFunc(value4.sheet, value4.handle);
				}
			}
			return new Cursor
			{
				texture = texture2D,
				hotspot = new Vector2(num, num2),
				defaultCursorId = num3
			};
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x00098314 File Offset: 0x00096514
		public TextShadow ReadTextShadow(int index)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			Color color = Color.clear;
			bool flag = this.valueCount >= 2;
			if (flag)
			{
				int num4 = index;
				StyleValueType styleValueType = this.GetValueType(num4);
				bool flag2 = false;
				bool flag3 = styleValueType == StyleValueType.Color || styleValueType == StyleValueType.Enum;
				if (flag3)
				{
					color = this.ReadColor(num4++);
					flag2 = true;
				}
				bool flag4 = num4 + 1 < this.valueCount;
				if (flag4)
				{
					styleValueType = this.GetValueType(num4);
					StyleValueType valueType = this.GetValueType(num4 + 1);
					bool flag5 = (styleValueType == StyleValueType.Dimension || styleValueType == StyleValueType.Float) && (valueType == StyleValueType.Dimension || valueType == StyleValueType.Float);
					if (flag5)
					{
						StylePropertyValue value = this.GetValue(num4++);
						StylePropertyValue value2 = this.GetValue(num4++);
						num = value.sheet.ReadDimension(value.handle).value;
						num2 = value2.sheet.ReadDimension(value2.handle).value;
					}
				}
				bool flag6 = num4 < this.valueCount;
				if (flag6)
				{
					styleValueType = this.GetValueType(num4);
					bool flag7 = styleValueType == StyleValueType.Dimension || styleValueType == StyleValueType.Float;
					if (flag7)
					{
						StylePropertyValue value3 = this.GetValue(num4++);
						num3 = value3.sheet.ReadDimension(value3.handle).value;
					}
					else
					{
						bool flag8 = styleValueType == StyleValueType.Color || styleValueType == StyleValueType.Enum;
						if (flag8)
						{
							bool flag9 = !flag2;
							if (flag9)
							{
								color = this.ReadColor(num4);
							}
						}
					}
				}
				bool flag10 = num4 < this.valueCount;
				if (flag10)
				{
					styleValueType = this.GetValueType(num4);
					bool flag11 = styleValueType == StyleValueType.Color || styleValueType == StyleValueType.Enum;
					if (flag11)
					{
						bool flag12 = !flag2;
						if (flag12)
						{
							color = this.ReadColor(num4);
						}
					}
				}
			}
			return new TextShadow
			{
				offset = new Vector2(num, num2),
				blurRadius = num3,
				color = color
			};
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x00098524 File Offset: 0x00096724
		public BackgroundPosition ReadBackgroundPositionX(int index)
		{
			return this.ReadBackgroundPosition(index, BackgroundPositionKeyword.Left);
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x00098540 File Offset: 0x00096740
		public BackgroundPosition ReadBackgroundPositionY(int index)
		{
			return this.ReadBackgroundPosition(index, BackgroundPositionKeyword.Top);
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0009855C File Offset: 0x0009675C
		private BackgroundPosition ReadBackgroundPosition(int index, BackgroundPositionKeyword keyword)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			return StylePropertyReader.ReadBackgroundPosition(this.valueCount, stylePropertyValue, stylePropertyValue2, keyword);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000985BC File Offset: 0x000967BC
		public BackgroundRepeat ReadBackgroundRepeat(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			return StylePropertyReader.ReadBackgroundRepeat(this.valueCount, stylePropertyValue, stylePropertyValue2);
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0009861C File Offset: 0x0009681C
		public BackgroundSize ReadBackgroundSize(int index)
		{
			StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
			StylePropertyValue stylePropertyValue2 = ((this.valueCount > 1) ? this.m_Values[this.m_CurrentValueIndex + index + 1] : default(StylePropertyValue));
			return StylePropertyReader.ReadBackgroundSize(this.valueCount, stylePropertyValue, stylePropertyValue2);
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0009867C File Offset: 0x0009687C
		public void ReadListEasingFunction(List<EasingFunction> list, int index)
		{
			list.Clear();
			do
			{
				StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
				StyleValueHandle handle = stylePropertyValue.handle;
				bool flag = handle.valueType == StyleValueType.Enum;
				if (flag)
				{
					string text = stylePropertyValue.sheet.ReadEnum(handle);
					int num;
					StylePropertyUtil.TryGetEnumIntValue(StyleEnumType.EasingMode, text, out num);
					list.Add(new EasingFunction((EasingMode)num));
					index++;
				}
				bool flag2 = index < this.valueCount;
				if (flag2)
				{
					bool flag3 = this.m_Values[this.m_CurrentValueIndex + index].handle.valueType == StyleValueType.CommaSeparator;
					if (flag3)
					{
						index++;
					}
				}
			}
			while (index < this.valueCount);
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0009873C File Offset: 0x0009693C
		public void ReadListTimeValue(List<TimeValue> list, int index)
		{
			list.Clear();
			do
			{
				StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
				TimeValue timeValue = stylePropertyValue.sheet.ReadDimension(stylePropertyValue.handle).ToTime();
				list.Add(timeValue);
				index++;
				bool flag = index < this.valueCount;
				if (flag)
				{
					bool flag2 = this.m_Values[this.m_CurrentValueIndex + index].handle.valueType == StyleValueType.CommaSeparator;
					if (flag2)
					{
						index++;
					}
				}
			}
			while (index < this.valueCount);
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000987E0 File Offset: 0x000969E0
		public void ReadListStylePropertyName(List<StylePropertyName> list, int index)
		{
			list.Clear();
			do
			{
				StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
				string text = stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
				list.Add(new StylePropertyName(text));
				index++;
				bool flag = index < this.valueCount;
				if (flag)
				{
					bool flag2 = this.m_Values[this.m_CurrentValueIndex + index].handle.valueType == StyleValueType.CommaSeparator;
					if (flag2)
					{
						index++;
					}
				}
			}
			while (index < this.valueCount);
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x00098880 File Offset: 0x00096A80
		public void ReadListString(List<string> list, int index)
		{
			list.Clear();
			do
			{
				StylePropertyValue stylePropertyValue = this.m_Values[this.m_CurrentValueIndex + index];
				string text = stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
				list.Add(text);
				index++;
				bool flag = index < this.valueCount;
				if (flag)
				{
					bool flag2 = this.m_Values[this.m_CurrentValueIndex + index].handle.valueType == StyleValueType.CommaSeparator;
					if (flag2)
					{
						index++;
					}
				}
			}
			while (index < this.valueCount);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0009891C File Offset: 0x00096B1C
		private void LoadProperties()
		{
			this.m_CurrentPropertyIndex = 0;
			this.m_CurrentValueIndex = 0;
			this.m_Values.Clear();
			this.m_ValueCount.Clear();
			foreach (StyleProperty styleProperty in this.m_Properties)
			{
				int num = 0;
				bool flag = true;
				bool requireVariableResolve = styleProperty.requireVariableResolve;
				if (requireVariableResolve)
				{
					this.m_Resolver.Init(styleProperty, this.m_Sheet, styleProperty.values);
					int num2 = 0;
					while (num2 < styleProperty.values.Length && flag)
					{
						StyleValueHandle styleValueHandle = styleProperty.values[num2];
						bool flag2 = styleValueHandle.IsVarFunction();
						if (flag2)
						{
							flag = this.m_Resolver.ResolveVarFunction(ref num2);
						}
						else
						{
							this.m_Resolver.AddValue(styleValueHandle);
						}
						num2++;
					}
					bool flag3 = flag && this.m_Resolver.ValidateResolvedValues();
					if (flag3)
					{
						this.m_Values.AddRange(this.m_Resolver.resolvedValues);
						num += this.m_Resolver.resolvedValues.Count;
					}
					else
					{
						StyleValueHandle styleValueHandle2 = new StyleValueHandle
						{
							valueType = StyleValueType.Keyword,
							valueIndex = 3
						};
						this.m_Values.Add(new StylePropertyValue
						{
							sheet = this.m_Sheet,
							handle = styleValueHandle2
						});
						num++;
					}
				}
				else
				{
					num = styleProperty.values.Length;
					for (int j = 0; j < num; j++)
					{
						this.m_Values.Add(new StylePropertyValue
						{
							sheet = this.m_Sheet,
							handle = styleProperty.values[j]
						});
					}
				}
				this.m_ValueCount.Add(num);
			}
			this.SetCurrentProperty();
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x00098B08 File Offset: 0x00096D08
		private void SetCurrentProperty()
		{
			bool flag = this.m_CurrentPropertyIndex < this.m_PropertyIds.Length;
			if (flag)
			{
				this.property = this.m_Properties[this.m_CurrentPropertyIndex];
				this.propertyId = this.m_PropertyIds[this.m_CurrentPropertyIndex];
				this.valueCount = this.m_ValueCount[this.m_CurrentPropertyIndex];
			}
			else
			{
				this.property = null;
				this.propertyId = StylePropertyId.Unknown;
				this.valueCount = 0;
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00098B88 File Offset: 0x00096D88
		public static TransformOrigin ReadTransformOrigin(int valCount, StylePropertyValue val1, StylePropertyValue val2, StylePropertyValue zVvalue)
		{
			Length length = Length.Percent(50f);
			Length length2 = Length.Percent(50f);
			float num = 0f;
			switch (valCount)
			{
			case 1:
			{
				bool flag;
				bool flag2;
				Length length3 = StylePropertyReader.ReadTransformOriginEnum(val1, out flag, out flag2);
				bool flag3 = flag2;
				if (flag3)
				{
					length = length3;
				}
				else
				{
					length2 = length3;
				}
				goto IL_00F3;
			}
			case 2:
				break;
			case 3:
			{
				bool flag4 = zVvalue.handle.valueType == StyleValueType.Dimension || zVvalue.handle.valueType == StyleValueType.Float;
				if (flag4)
				{
					Dimension dimension = zVvalue.sheet.ReadDimension(zVvalue.handle);
					num = dimension.value;
				}
				break;
			}
			default:
				goto IL_00F3;
			}
			bool flag5;
			bool flag6;
			Length length4 = StylePropertyReader.ReadTransformOriginEnum(val1, out flag5, out flag6);
			bool flag7;
			bool flag8;
			Length length5 = StylePropertyReader.ReadTransformOriginEnum(val2, out flag7, out flag8);
			bool flag9 = !flag6 || !flag7;
			if (flag9)
			{
				bool flag10 = flag8 && flag5;
				if (flag10)
				{
					length = length5;
					length2 = length4;
				}
			}
			else
			{
				length = length4;
				length2 = length5;
			}
			IL_00F3:
			return new TransformOrigin(length, length2, num);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x00098C98 File Offset: 0x00096E98
		private static Length ReadTransformOriginEnum(StylePropertyValue value, out bool isVertical, out bool isHorizontal)
		{
			bool flag = value.handle.valueType == StyleValueType.Enum;
			if (flag)
			{
				switch (StylePropertyReader.ReadEnum(StyleEnumType.TransformOriginOffset, value))
				{
				case 1:
					isVertical = false;
					isHorizontal = true;
					return Length.Percent(0f);
				case 2:
					isVertical = false;
					isHorizontal = true;
					return Length.Percent(100f);
				case 3:
					isVertical = true;
					isHorizontal = false;
					return Length.Percent(0f);
				case 4:
					isVertical = true;
					isHorizontal = false;
					return Length.Percent(100f);
				case 5:
					isVertical = true;
					isHorizontal = true;
					return Length.Percent(50f);
				}
			}
			else
			{
				bool flag2 = value.handle.valueType == StyleValueType.Dimension || value.handle.valueType == StyleValueType.Float;
				if (flag2)
				{
					isVertical = true;
					isHorizontal = true;
					return value.sheet.ReadDimension(value.handle).ToLength();
				}
			}
			isVertical = false;
			isHorizontal = false;
			return Length.Percent(50f);
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00098DBC File Offset: 0x00096FBC
		public static Translate ReadTranslate(int valCount, StylePropertyValue val1, StylePropertyValue val2, StylePropertyValue val3)
		{
			bool flag = val1.handle.valueType == StyleValueType.Keyword && val1.handle.valueIndex == 6;
			Translate translate;
			if (flag)
			{
				translate = Translate.None();
			}
			else
			{
				Length length = 0f;
				Length length2 = 0f;
				float num = 0f;
				switch (valCount)
				{
				case 1:
				{
					bool flag2 = val1.handle.valueType == StyleValueType.Dimension || val1.handle.valueType == StyleValueType.Float;
					if (flag2)
					{
						length = val1.sheet.ReadDimension(val1.handle).ToLength();
						length2 = val1.sheet.ReadDimension(val1.handle).ToLength();
					}
					goto IL_01C3;
				}
				case 2:
					break;
				case 3:
				{
					bool flag3 = val3.handle.valueType == StyleValueType.Dimension || val3.handle.valueType == StyleValueType.Float;
					if (flag3)
					{
						Dimension dimension = val3.sheet.ReadDimension(val3.handle);
						bool flag4 = dimension.unit != Dimension.Unit.Pixel && dimension.unit > Dimension.Unit.Unitless;
						if (flag4)
						{
							num = dimension.value;
						}
					}
					break;
				}
				default:
					goto IL_01C3;
				}
				bool flag5 = val1.handle.valueType == StyleValueType.Dimension || val1.handle.valueType == StyleValueType.Float;
				if (flag5)
				{
					length = val1.sheet.ReadDimension(val1.handle).ToLength();
				}
				bool flag6 = val2.handle.valueType == StyleValueType.Dimension || val2.handle.valueType == StyleValueType.Float;
				if (flag6)
				{
					length2 = val2.sheet.ReadDimension(val2.handle).ToLength();
				}
				IL_01C3:
				translate = new Translate(length, length2, num);
			}
			return translate;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x00098F9C File Offset: 0x0009719C
		public static Scale ReadScale(int valCount, StylePropertyValue val1, StylePropertyValue val2, StylePropertyValue val3)
		{
			bool flag = val1.handle.valueType == StyleValueType.Keyword && val1.handle.valueIndex == 6;
			Scale scale;
			if (flag)
			{
				scale = Scale.None();
			}
			else
			{
				Vector3 one = Vector3.one;
				switch (valCount)
				{
				case 1:
				{
					bool flag2 = val1.handle.valueType == StyleValueType.Dimension || val1.handle.valueType == StyleValueType.Float;
					if (flag2)
					{
						one.x = val1.sheet.ReadFloat(val1.handle);
						one.y = one.x;
					}
					goto IL_0173;
				}
				case 2:
					break;
				case 3:
				{
					bool flag3 = val3.handle.valueType == StyleValueType.Dimension || val3.handle.valueType == StyleValueType.Float;
					if (flag3)
					{
						one.z = val3.sheet.ReadFloat(val3.handle);
					}
					break;
				}
				default:
					goto IL_0173;
				}
				bool flag4 = val1.handle.valueType == StyleValueType.Dimension || val1.handle.valueType == StyleValueType.Float;
				if (flag4)
				{
					one.x = val1.sheet.ReadFloat(val1.handle);
				}
				bool flag5 = val2.handle.valueType == StyleValueType.Dimension || val2.handle.valueType == StyleValueType.Float;
				if (flag5)
				{
					one.y = val2.sheet.ReadFloat(val2.handle);
				}
				IL_0173:
				scale = new Scale(one);
			}
			return scale;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00099128 File Offset: 0x00097328
		public static Rotate ReadRotate(int valCount, StylePropertyValue val1, StylePropertyValue val2, StylePropertyValue val3, StylePropertyValue val4)
		{
			bool flag = val1.handle.valueType == StyleValueType.Keyword && val1.handle.valueIndex == 6;
			Rotate rotate;
			if (flag)
			{
				rotate = Rotate.None();
			}
			else
			{
				Rotate rotate2 = Rotate.Initial();
				if (valCount == 1)
				{
					bool flag2 = val1.handle.valueType == StyleValueType.Dimension;
					if (flag2)
					{
						rotate2.angle = StylePropertyReader.ReadAngle(val1);
					}
				}
				rotate = rotate2;
			}
			return rotate;
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000991A4 File Offset: 0x000973A4
		private static bool TryReadEnum(StyleEnumType enumType, StylePropertyValue value, out int intValue)
		{
			StyleValueHandle handle = value.handle;
			bool flag = handle.valueType == StyleValueType.Keyword;
			string text;
			if (flag)
			{
				StyleValueKeyword styleValueKeyword = value.sheet.ReadKeyword(handle);
				text = styleValueKeyword.ToUssString();
			}
			else
			{
				text = value.sheet.ReadEnum(handle);
			}
			return StylePropertyUtil.TryGetEnumIntValue(enumType, text, out intValue);
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00099200 File Offset: 0x00097400
		private static int ReadEnum(StyleEnumType enumType, StylePropertyValue value)
		{
			StyleValueHandle handle = value.handle;
			bool flag = handle.valueType == StyleValueType.Keyword;
			string text;
			if (flag)
			{
				StyleValueKeyword styleValueKeyword = value.sheet.ReadKeyword(handle);
				text = styleValueKeyword.ToUssString();
			}
			else
			{
				text = value.sheet.ReadEnum(handle);
			}
			int num;
			StylePropertyUtil.TryGetEnumIntValue(enumType, text, out num);
			return num;
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00099260 File Offset: 0x00097460
		public static Angle ReadAngle(StylePropertyValue value)
		{
			bool flag = value.handle.valueType == StyleValueType.Keyword;
			Angle angle;
			if (flag)
			{
				StyleValueKeyword valueIndex = (StyleValueKeyword)value.handle.valueIndex;
				StyleValueKeyword styleValueKeyword = valueIndex;
				StyleValueKeyword styleValueKeyword2 = styleValueKeyword;
				if (styleValueKeyword2 != StyleValueKeyword.None)
				{
					angle = default(Angle);
				}
				else
				{
					angle = Angle.None();
				}
			}
			else
			{
				angle = value.sheet.ReadDimension(value.handle).ToAngle();
			}
			return angle;
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000992D0 File Offset: 0x000974D0
		public static BackgroundPosition ReadBackgroundPosition(int valCount, StylePropertyValue val1, StylePropertyValue val2, BackgroundPositionKeyword keyword)
		{
			bool flag = valCount == 1;
			if (flag)
			{
				bool flag2 = val1.handle.valueType == StyleValueType.Enum;
				if (flag2)
				{
					return new BackgroundPosition((BackgroundPositionKeyword)StylePropertyReader.ReadEnum(StyleEnumType.BackgroundPositionKeyword, val1));
				}
				bool flag3 = val1.handle.valueType == StyleValueType.Dimension || val1.handle.valueType == StyleValueType.Float;
				if (flag3)
				{
					return new BackgroundPosition(keyword, val1.sheet.ReadDimension(val1.handle).ToLength());
				}
			}
			else
			{
				bool flag4 = valCount == 2;
				if (flag4)
				{
					bool flag5 = val1.handle.valueType == StyleValueType.Enum && (val2.handle.valueType == StyleValueType.Dimension || val2.handle.valueType == StyleValueType.Float);
					if (flag5)
					{
						return new BackgroundPosition((BackgroundPositionKeyword)StylePropertyReader.ReadEnum(StyleEnumType.BackgroundPositionKeyword, val1), val1.sheet.ReadDimension(val2.handle).ToLength());
					}
				}
			}
			return default(BackgroundPosition);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000993D8 File Offset: 0x000975D8
		public static BackgroundRepeat ReadBackgroundRepeat(int valCount, StylePropertyValue val1, StylePropertyValue val2)
		{
			BackgroundRepeat backgroundRepeat = default(BackgroundRepeat);
			bool flag = valCount == 1;
			if (flag)
			{
				int num;
				bool flag2 = StylePropertyReader.TryReadEnum(StyleEnumType.RepeatXY, val1, out num);
				if (flag2)
				{
					bool flag3 = num == 0;
					if (flag3)
					{
						backgroundRepeat.x = Repeat.Repeat;
						backgroundRepeat.y = Repeat.NoRepeat;
					}
					else
					{
						bool flag4 = num == 1;
						if (flag4)
						{
							backgroundRepeat.x = Repeat.NoRepeat;
							backgroundRepeat.y = Repeat.Repeat;
						}
					}
				}
				else
				{
					backgroundRepeat.x = (Repeat)StylePropertyReader.ReadEnum(StyleEnumType.Repeat, val1);
					backgroundRepeat.y = backgroundRepeat.x;
				}
			}
			else
			{
				backgroundRepeat.x = (Repeat)StylePropertyReader.ReadEnum(StyleEnumType.Repeat, val1);
				backgroundRepeat.y = (Repeat)StylePropertyReader.ReadEnum(StyleEnumType.Repeat, val2);
			}
			return backgroundRepeat;
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0009948C File Offset: 0x0009768C
		public static BackgroundSize ReadBackgroundSize(int valCount, StylePropertyValue val1, StylePropertyValue val2)
		{
			BackgroundSize backgroundSize = default(BackgroundSize);
			bool flag = valCount == 1;
			if (flag)
			{
				bool flag2 = val1.handle.valueType == StyleValueType.Keyword;
				if (flag2)
				{
					bool flag3 = val1.handle.valueIndex == 2;
					if (flag3)
					{
						backgroundSize.x = Length.Auto();
						backgroundSize.y = Length.Auto();
					}
				}
				else
				{
					bool flag4 = val1.handle.valueType == StyleValueType.Enum;
					if (flag4)
					{
						backgroundSize.sizeType = (BackgroundSizeType)StylePropertyReader.ReadEnum(StyleEnumType.BackgroundSizeType, val1);
					}
					else
					{
						bool flag5 = val1.handle.valueType == StyleValueType.Dimension;
						if (flag5)
						{
							backgroundSize.x = val1.sheet.ReadDimension(val1.handle).ToLength();
							backgroundSize.y = Length.Auto();
						}
					}
				}
			}
			else
			{
				bool flag6 = valCount == 2;
				if (flag6)
				{
					bool flag7 = val1.handle.valueType == StyleValueType.Keyword;
					if (flag7)
					{
						bool flag8 = val1.handle.valueIndex == 2;
						if (flag8)
						{
							backgroundSize.x = Length.Auto();
						}
					}
					else
					{
						bool flag9 = val1.handle.valueType == StyleValueType.Dimension;
						if (flag9)
						{
							backgroundSize.x = val1.sheet.ReadDimension(val1.handle).ToLength();
						}
					}
					bool flag10 = val2.handle.valueType == StyleValueType.Keyword;
					if (flag10)
					{
						bool flag11 = val2.handle.valueIndex == 2;
						if (flag11)
						{
							backgroundSize.y = Length.Auto();
						}
					}
					else
					{
						bool flag12 = val2.handle.valueType == StyleValueType.Dimension;
						if (flag12)
						{
							backgroundSize.y = val2.sheet.ReadDimension(val2.handle).ToLength();
						}
					}
				}
			}
			return backgroundSize;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00099668 File Offset: 0x00097868
		internal static bool TryGetImageSourceFromValue(StylePropertyValue propertyValue, float dpiScaling, out ImageSource source)
		{
			source = default(ImageSource);
			StyleValueType valueType = propertyValue.handle.valueType;
			StyleValueType styleValueType = valueType;
			if (styleValueType <= StyleValueType.AssetReference)
			{
				if (styleValueType != StyleValueType.ResourcePath)
				{
					if (styleValueType == StyleValueType.AssetReference)
					{
						Object @object = propertyValue.sheet.ReadAssetReference(propertyValue.handle);
						source.texture = @object as Texture2D;
						source.sprite = @object as Sprite;
						source.vectorImage = @object as VectorImage;
						source.renderTexture = @object as RenderTexture;
						bool flag = source.IsNull();
						if (flag)
						{
							Debug.LogWarning("Invalid image specified");
							return false;
						}
						goto IL_0254;
					}
				}
				else
				{
					string text = propertyValue.sheet.ReadResourcePath(propertyValue.handle);
					bool flag2 = !string.IsNullOrEmpty(text);
					if (flag2)
					{
						source.sprite = Panel.LoadResource(text, typeof(Sprite), dpiScaling) as Sprite;
						bool flag3 = source.IsNull();
						if (flag3)
						{
							source.texture = Panel.LoadResource(text, typeof(Texture2D), dpiScaling) as Texture2D;
						}
						bool flag4 = source.IsNull();
						if (flag4)
						{
							source.vectorImage = Panel.LoadResource(text, typeof(VectorImage), dpiScaling) as VectorImage;
						}
						bool flag5 = source.IsNull();
						if (flag5)
						{
							source.renderTexture = Panel.LoadResource(text, typeof(RenderTexture), dpiScaling) as RenderTexture;
						}
					}
					bool flag6 = source.IsNull();
					if (flag6)
					{
						Debug.LogWarning(string.Format("Image not found for path: {0}", text));
						return false;
					}
					goto IL_0254;
				}
			}
			else if (styleValueType != StyleValueType.ScalableImage)
			{
				if (styleValueType == StyleValueType.MissingAssetReference)
				{
					return false;
				}
			}
			else
			{
				ScalableImage scalableImage = propertyValue.sheet.ReadScalableImage(propertyValue.handle);
				bool flag7 = scalableImage.normalImage == null && scalableImage.highResolutionImage == null;
				if (flag7)
				{
					Debug.LogWarning("Invalid scalable image specified");
					return false;
				}
				source.texture = scalableImage.normalImage;
				bool flag8 = !Mathf.Approximately(dpiScaling % 1f, 0f);
				if (flag8)
				{
					source.texture.filterMode = FilterMode.Bilinear;
				}
				goto IL_0254;
			}
			Debug.LogWarning("Invalid value for image texture " + propertyValue.handle.valueType.ToString());
			return false;
			IL_0254:
			return true;
		}

		// Token: 0x04001185 RID: 4485
		internal static StylePropertyReader.GetCursorIdFunction getCursorIdFunc;

		// Token: 0x04001186 RID: 4486
		private List<StylePropertyValue> m_Values = new List<StylePropertyValue>();

		// Token: 0x04001187 RID: 4487
		private List<int> m_ValueCount = new List<int>();

		// Token: 0x04001188 RID: 4488
		private StyleVariableResolver m_Resolver = new StyleVariableResolver();

		// Token: 0x04001189 RID: 4489
		private StyleSheet m_Sheet;

		// Token: 0x0400118A RID: 4490
		private StyleProperty[] m_Properties;

		// Token: 0x0400118B RID: 4491
		private StylePropertyId[] m_PropertyIds;

		// Token: 0x0400118C RID: 4492
		private int m_CurrentValueIndex;

		// Token: 0x0400118D RID: 4493
		private int m_CurrentPropertyIndex;

		// Token: 0x02000490 RID: 1168
		// (Invoke) Token: 0x060024A2 RID: 9378
		internal delegate int GetCursorIdFunction(StyleSheet sheet, StyleValueHandle handle);
	}
}
