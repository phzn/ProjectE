using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F3 RID: 755
	internal class StyleValueCollection
	{
		// Token: 0x06001997 RID: 6551 RVA: 0x0006774C File Offset: 0x0006594C
		public StyleLength GetStyleLength(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleLength styleLength;
			if (flag)
			{
				styleLength = new StyleLength(styleValue.length, styleValue.keyword);
			}
			else
			{
				styleLength = StyleKeyword.Null;
			}
			return styleLength;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00067790 File Offset: 0x00065990
		public StyleFloat GetStyleFloat(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleFloat styleFloat;
			if (flag)
			{
				styleFloat = new StyleFloat(styleValue.number, styleValue.keyword);
			}
			else
			{
				styleFloat = StyleKeyword.Null;
			}
			return styleFloat;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x000677D4 File Offset: 0x000659D4
		public StyleInt GetStyleInt(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleInt styleInt;
			if (flag)
			{
				styleInt = new StyleInt((int)styleValue.number, styleValue.keyword);
			}
			else
			{
				styleInt = StyleKeyword.Null;
			}
			return styleInt;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00067818 File Offset: 0x00065A18
		public StyleColor GetStyleColor(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleColor styleColor;
			if (flag)
			{
				styleColor = new StyleColor(styleValue.color, styleValue.keyword);
			}
			else
			{
				styleColor = StyleKeyword.Null;
			}
			return styleColor;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0006785C File Offset: 0x00065A5C
		public StyleBackground GetStyleBackground(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				Texture2D texture2D = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Texture2D) : null);
				bool flag2 = texture2D != null;
				if (flag2)
				{
					return new StyleBackground(texture2D, styleValue.keyword);
				}
				Sprite sprite = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Sprite) : null);
				bool flag3 = sprite != null;
				if (flag3)
				{
					return new StyleBackground(sprite, styleValue.keyword);
				}
				VectorImage vectorImage = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as VectorImage) : null);
				bool flag4 = vectorImage != null;
				if (flag4)
				{
					return new StyleBackground(vectorImage, styleValue.keyword);
				}
			}
			return StyleKeyword.Null;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00067958 File Offset: 0x00065B58
		public StyleBackgroundPosition GetStyleBackgroundPosition(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleBackgroundPosition styleBackgroundPosition;
			if (flag)
			{
				styleBackgroundPosition = new StyleBackgroundPosition(styleValue.position);
			}
			else
			{
				styleBackgroundPosition = StyleKeyword.Null;
			}
			return styleBackgroundPosition;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00067994 File Offset: 0x00065B94
		public StyleBackgroundRepeat GetStyleBackgroundRepeat(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleBackgroundRepeat styleBackgroundRepeat;
			if (flag)
			{
				styleBackgroundRepeat = new StyleBackgroundRepeat(styleValue.repeat);
			}
			else
			{
				styleBackgroundRepeat = StyleKeyword.Null;
			}
			return styleBackgroundRepeat;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x000679D0 File Offset: 0x00065BD0
		public StyleFont GetStyleFont(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleFont styleFont;
			if (flag)
			{
				Font font = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Font) : null);
				styleFont = new StyleFont(font, styleValue.keyword);
			}
			else
			{
				styleFont = StyleKeyword.Null;
			}
			return styleFont;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00067A30 File Offset: 0x00065C30
		public StyleFontDefinition GetStyleFontDefinition(StylePropertyId id)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = this.TryGetStyleValue(id, ref styleValue);
			StyleFontDefinition styleFontDefinition;
			if (flag)
			{
				object obj = (styleValue.resource.IsAllocated ? styleValue.resource.Target : null);
				styleFontDefinition = new StyleFontDefinition(obj, styleValue.keyword);
			}
			else
			{
				styleFontDefinition = StyleKeyword.Null;
			}
			return styleFontDefinition;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00067A8C File Offset: 0x00065C8C
		public bool TryGetStyleValue(StylePropertyId id, ref StyleValue value)
		{
			value.id = StylePropertyId.Unknown;
			foreach (StyleValue styleValue in this.m_Values)
			{
				bool flag = styleValue.id == id;
				if (flag)
				{
					value = styleValue;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00067B04 File Offset: 0x00065D04
		public void SetStyleValue(StyleValue value)
		{
			for (int i = 0; i < this.m_Values.Count; i++)
			{
				bool flag = this.m_Values[i].id == value.id;
				if (flag)
				{
					bool flag2 = value.keyword == StyleKeyword.Null;
					if (flag2)
					{
						this.m_Values.RemoveAt(i);
					}
					else
					{
						this.m_Values[i] = value;
					}
					return;
				}
			}
			this.m_Values.Add(value);
		}

		// Token: 0x04000AC6 RID: 2758
		internal List<StyleValue> m_Values = new List<StyleValue>();
	}
}
