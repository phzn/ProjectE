using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002FD RID: 765
	public struct StyleColor : IStyleValue<Color>, IEquatable<StyleColor>
	{
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x00068934 File Offset: 0x00066B34
		// (set) Token: 0x06001A18 RID: 6680 RVA: 0x0006895B File Offset: 0x00066B5B
		public Color value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : Color.clear;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0006896C File Offset: 0x00066B6C
		// (set) Token: 0x06001A1A RID: 6682 RVA: 0x00068984 File Offset: 0x00066B84
		public StyleKeyword keyword
		{
			get
			{
				return this.m_Keyword;
			}
			set
			{
				this.m_Keyword = value;
			}
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0006898E File Offset: 0x00066B8E
		public StyleColor(Color v)
		{
			this = new StyleColor(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0006899A File Offset: 0x00066B9A
		public StyleColor(StyleKeyword keyword)
		{
			this = new StyleColor(Color.clear, keyword);
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000689AA File Offset: 0x00066BAA
		internal StyleColor(Color v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000689BC File Offset: 0x00066BBC
		public static bool operator ==(StyleColor lhs, StyleColor rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000689F0 File Offset: 0x00066BF0
		public static bool operator !=(StyleColor lhs, StyleColor rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00068A0C File Offset: 0x00066C0C
		public static bool operator ==(StyleColor lhs, Color rhs)
		{
			StyleColor styleColor = new StyleColor(rhs);
			return lhs == styleColor;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00068A30 File Offset: 0x00066C30
		public static bool operator !=(StyleColor lhs, Color rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00068A4C File Offset: 0x00066C4C
		public static implicit operator StyleColor(StyleKeyword keyword)
		{
			return new StyleColor(keyword);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00068A64 File Offset: 0x00066C64
		public static implicit operator StyleColor(Color v)
		{
			return new StyleColor(v);
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00068A7C File Offset: 0x00066C7C
		public bool Equals(StyleColor other)
		{
			return other == this;
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x00068A9C File Offset: 0x00066C9C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleColor)
			{
				StyleColor styleColor = (StyleColor)obj;
				flag = this.Equals(styleColor);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00068AC8 File Offset: 0x00066CC8
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00068AFC File Offset: 0x00066CFC
		public override string ToString()
		{
			return this.DebugString<Color>();
		}

		// Token: 0x04000ADF RID: 2783
		private Color m_Value;

		// Token: 0x04000AE0 RID: 2784
		private StyleKeyword m_Keyword;
	}
}
