using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000303 RID: 771
	public struct StyleFont : IStyleValue<Font>, IEquatable<StyleFont>
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x00069330 File Offset: 0x00067530
		// (set) Token: 0x06001A6B RID: 6763 RVA: 0x00069353 File Offset: 0x00067553
		public Font value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : null;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x00069364 File Offset: 0x00067564
		// (set) Token: 0x06001A6D RID: 6765 RVA: 0x0006937C File Offset: 0x0006757C
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

		// Token: 0x06001A6E RID: 6766 RVA: 0x00069386 File Offset: 0x00067586
		public StyleFont(Font v)
		{
			this = new StyleFont(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00069392 File Offset: 0x00067592
		public StyleFont(StyleKeyword keyword)
		{
			this = new StyleFont(null, keyword);
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0006939E File Offset: 0x0006759E
		internal StyleFont(Font v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000693B0 File Offset: 0x000675B0
		public static bool operator ==(StyleFont lhs, StyleFont rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000693E4 File Offset: 0x000675E4
		public static bool operator !=(StyleFont lhs, StyleFont rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00069400 File Offset: 0x00067600
		public static implicit operator StyleFont(StyleKeyword keyword)
		{
			return new StyleFont(keyword);
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00069418 File Offset: 0x00067618
		public static implicit operator StyleFont(Font v)
		{
			return new StyleFont(v);
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00069430 File Offset: 0x00067630
		public bool Equals(StyleFont other)
		{
			return other == this;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00069450 File Offset: 0x00067650
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleFont)
			{
				StyleFont styleFont = (StyleFont)obj;
				flag = this.Equals(styleFont);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0006947C File Offset: 0x0006767C
		public override int GetHashCode()
		{
			return (((this.m_Value != null) ? this.m_Value.GetHashCode() : 0) * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000694B8 File Offset: 0x000676B8
		public override string ToString()
		{
			return this.DebugString<Font>();
		}

		// Token: 0x04000AEC RID: 2796
		private Font m_Value;

		// Token: 0x04000AED RID: 2797
		private StyleKeyword m_Keyword;
	}
}
