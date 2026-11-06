using System;
using System.Collections.Generic;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements
{
	// Token: 0x0200030F RID: 783
	internal static class StyleValueExtensions
	{
		// Token: 0x06001B0F RID: 6927 RVA: 0x0006A7E0 File Offset: 0x000689E0
		internal static string DebugString<T>(this IStyleValue<T> styleValue)
		{
			return (styleValue.keyword != StyleKeyword.Undefined) ? string.Format("{0}", styleValue.keyword) : string.Format("{0}", styleValue.value);
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0006A828 File Offset: 0x00068A28
		internal static YogaValue ToYogaValue(this Length length)
		{
			bool flag = length.IsAuto();
			YogaValue yogaValue;
			if (flag)
			{
				yogaValue = YogaValue.Auto();
			}
			else
			{
				bool flag2 = length.IsNone();
				if (flag2)
				{
					yogaValue = float.NaN;
				}
				else
				{
					LengthUnit unit = length.unit;
					LengthUnit lengthUnit = unit;
					if (lengthUnit != LengthUnit.Pixel)
					{
						if (lengthUnit != LengthUnit.Percent)
						{
							Debug.LogAssertion(string.Format("Unexpected unit '{0}'", length.unit));
							yogaValue = float.NaN;
						}
						else
						{
							yogaValue = YogaValue.Percent(length.value);
						}
					}
					else
					{
						yogaValue = YogaValue.Point(length.value);
					}
				}
			}
			return yogaValue;
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0006A8C0 File Offset: 0x00068AC0
		internal static Length ToLength(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Length length;
			if (styleKeyword2 != StyleKeyword.Auto)
			{
				if (styleKeyword2 != StyleKeyword.None)
				{
					Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
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
			return length;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0006A920 File Offset: 0x00068B20
		internal static Rotate ToRotate(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Rotate rotate;
			if (styleKeyword2 != StyleKeyword.None)
			{
				Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
				rotate = default(Rotate);
			}
			else
			{
				rotate = Rotate.None();
			}
			return rotate;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x0006A970 File Offset: 0x00068B70
		internal static Scale ToScale(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Scale scale;
			if (styleKeyword2 != StyleKeyword.None)
			{
				Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
				scale = default(Scale);
			}
			else
			{
				scale = Scale.None();
			}
			return scale;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0006A9C0 File Offset: 0x00068BC0
		internal static Translate ToTranslate(this StyleKeyword keyword)
		{
			StyleKeyword styleKeyword = keyword;
			StyleKeyword styleKeyword2 = styleKeyword;
			Translate translate;
			if (styleKeyword2 != StyleKeyword.None)
			{
				Debug.LogAssertion("Unexpected StyleKeyword '" + keyword.ToString() + "'");
				translate = default(Translate);
			}
			else
			{
				translate = Translate.None();
			}
			return translate;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0006AA10 File Offset: 0x00068C10
		internal static Length ToLength(this StyleLength styleLength)
		{
			StyleKeyword keyword = styleLength.keyword;
			StyleKeyword styleKeyword = keyword;
			Length length;
			if (styleKeyword - StyleKeyword.Auto > 1)
			{
				length = styleLength.value;
			}
			else
			{
				length = styleLength.keyword.ToLength();
			}
			return length;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0006AA4A File Offset: 0x00068C4A
		internal static void CopyFrom<T>(this List<T> list, List<T> other)
		{
			list.Clear();
			list.AddRange(other);
		}
	}
}
