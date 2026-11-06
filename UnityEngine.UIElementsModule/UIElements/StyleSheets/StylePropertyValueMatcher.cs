using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x020004A3 RID: 1187
	internal class StylePropertyValueMatcher : BaseStyleMatcher
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x0009C2F8 File Offset: 0x0009A4F8
		private StylePropertyValue current
		{
			get
			{
				return base.hasCurrent ? this.m_Values[base.currentIndex] : default(StylePropertyValue);
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x0009C329 File Offset: 0x0009A529
		public override int valueCount
		{
			get
			{
				return this.m_Values.Count;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x0000960A File Offset: 0x0000780A
		public override bool isCurrentVariable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x0009C338 File Offset: 0x0009A538
		public override bool isCurrentComma
		{
			get
			{
				return base.hasCurrent && this.m_Values[base.currentIndex].handle.valueType == StyleValueType.CommaSeparator;
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0009C374 File Offset: 0x0009A574
		public MatchResult Match(Expression exp, List<StylePropertyValue> values)
		{
			MatchResult matchResult = new MatchResult
			{
				errorCode = MatchResultErrorCode.None
			};
			bool flag = values == null || values.Count == 0;
			MatchResult matchResult2;
			if (flag)
			{
				matchResult.errorCode = MatchResultErrorCode.EmptyValue;
				matchResult2 = matchResult;
			}
			else
			{
				base.Initialize();
				this.m_Values = values;
				StyleValueHandle handle = this.m_Values[0].handle;
				bool flag2 = handle.valueType == StyleValueType.Keyword && handle.valueIndex == 1;
				bool flag3;
				if (flag2)
				{
					base.MoveNext();
					flag3 = true;
				}
				else
				{
					flag3 = base.Match(exp);
				}
				bool flag4 = !flag3;
				if (flag4)
				{
					StyleSheet sheet = this.current.sheet;
					matchResult.errorCode = MatchResultErrorCode.Syntax;
					matchResult.errorValue = sheet.ReadAsString(this.current.handle);
				}
				else
				{
					bool hasCurrent = base.hasCurrent;
					if (hasCurrent)
					{
						StyleSheet sheet2 = this.current.sheet;
						matchResult.errorCode = MatchResultErrorCode.ExpectedEndOfValue;
						matchResult.errorValue = sheet2.ReadAsString(this.current.handle);
					}
				}
				matchResult2 = matchResult;
			}
			return matchResult2;
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x0009C490 File Offset: 0x0009A690
		protected override bool MatchKeyword(string keyword)
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Keyword;
			bool flag2;
			if (flag)
			{
				StyleValueKeyword valueIndex = (StyleValueKeyword)current.handle.valueIndex;
				flag2 = valueIndex.ToUssString() == keyword.ToLowerInvariant();
			}
			else
			{
				bool flag3 = current.handle.valueType == StyleValueType.Enum;
				if (flag3)
				{
					string text = current.sheet.ReadEnum(current.handle);
					flag2 = text == keyword.ToLowerInvariant();
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x0009C518 File Offset: 0x0009A718
		protected override bool MatchNumber()
		{
			return this.current.handle.valueType == StyleValueType.Float;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x0009C540 File Offset: 0x0009A740
		protected override bool MatchInteger()
		{
			return this.current.handle.valueType == StyleValueType.Float;
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x0009C568 File Offset: 0x0009A768
		protected override bool MatchLength()
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Dimension;
			bool flag2;
			if (flag)
			{
				Dimension dimension = current.sheet.ReadDimension(current.handle);
				flag2 = dimension.unit == Dimension.Unit.Pixel;
			}
			else
			{
				bool flag3 = current.handle.valueType == StyleValueType.Float;
				if (flag3)
				{
					float num = current.sheet.ReadFloat(current.handle);
					flag2 = Mathf.Approximately(0f, num);
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x0009C5EC File Offset: 0x0009A7EC
		protected override bool MatchPercentage()
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Dimension;
			bool flag2;
			if (flag)
			{
				Dimension dimension = current.sheet.ReadDimension(current.handle);
				flag2 = dimension.unit == Dimension.Unit.Percent;
			}
			else
			{
				bool flag3 = current.handle.valueType == StyleValueType.Float;
				if (flag3)
				{
					float num = current.sheet.ReadFloat(current.handle);
					flag2 = Mathf.Approximately(0f, num);
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x0009C670 File Offset: 0x0009A870
		protected override bool MatchColor()
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Color;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				bool flag3 = current.handle.valueType == StyleValueType.Enum;
				if (flag3)
				{
					Color clear = Color.clear;
					string text = current.sheet.ReadAsString(current.handle);
					bool flag4 = StyleSheetColor.TryGetColor(text.ToLowerInvariant(), out clear);
					if (flag4)
					{
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x0009C6E8 File Offset: 0x0009A8E8
		protected override bool MatchResource()
		{
			return this.current.handle.valueType == StyleValueType.ResourcePath;
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x0009C710 File Offset: 0x0009A910
		protected override bool MatchUrl()
		{
			StyleValueType valueType = this.current.handle.valueType;
			return valueType == StyleValueType.AssetReference || valueType == StyleValueType.ScalableImage;
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x0009C748 File Offset: 0x0009A948
		protected override bool MatchTime()
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Dimension;
			bool flag2;
			if (flag)
			{
				Dimension dimension = current.sheet.ReadDimension(current.handle);
				flag2 = dimension.unit == Dimension.Unit.Second || dimension.unit == Dimension.Unit.Millisecond;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x0009C7A0 File Offset: 0x0009A9A0
		protected override bool MatchCustomIdent()
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Enum;
			bool flag2;
			if (flag)
			{
				string text = current.sheet.ReadAsString(current.handle);
				Match match = BaseStyleMatcher.s_CustomIdentRegex.Match(text);
				flag2 = match.Success && match.Length == text.Length;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x0009C80C File Offset: 0x0009AA0C
		protected override bool MatchAngle()
		{
			StylePropertyValue current = this.current;
			bool flag = current.handle.valueType == StyleValueType.Dimension;
			if (flag)
			{
				Dimension dimension = current.sheet.ReadDimension(current.handle);
				Dimension.Unit unit = dimension.unit;
				Dimension.Unit unit2 = unit;
				if (unit2 - Dimension.Unit.Degree <= 3)
				{
					return true;
				}
			}
			bool flag2 = current.handle.valueType == StyleValueType.Float;
			bool flag3;
			if (flag2)
			{
				float num = current.sheet.ReadFloat(current.handle);
				flag3 = Mathf.Approximately(0f, num);
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x040011D9 RID: 4569
		private List<StylePropertyValue> m_Values;
	}
}
