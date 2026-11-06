using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002FC RID: 764
	public struct StyleBackgroundSize : IStyleValue<BackgroundSize>, IEquatable<StyleBackgroundSize>
	{
		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00068774 File Offset: 0x00066974
		// (set) Token: 0x06001A09 RID: 6665 RVA: 0x0006879F File Offset: 0x0006699F
		public BackgroundSize value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(BackgroundSize);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001A0A RID: 6666 RVA: 0x000687B0 File Offset: 0x000669B0
		// (set) Token: 0x06001A0B RID: 6667 RVA: 0x000687C8 File Offset: 0x000669C8
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

		// Token: 0x06001A0C RID: 6668 RVA: 0x000687D2 File Offset: 0x000669D2
		public StyleBackgroundSize(BackgroundSize v)
		{
			this = new StyleBackgroundSize(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000687E0 File Offset: 0x000669E0
		public StyleBackgroundSize(StyleKeyword keyword)
		{
			this = new StyleBackgroundSize(default(BackgroundSize), keyword);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000687FF File Offset: 0x000669FF
		internal StyleBackgroundSize(BackgroundSize v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00068810 File Offset: 0x00066A10
		public static bool operator ==(StyleBackgroundSize lhs, StyleBackgroundSize rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00068844 File Offset: 0x00066A44
		public static bool operator !=(StyleBackgroundSize lhs, StyleBackgroundSize rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00068860 File Offset: 0x00066A60
		public static implicit operator StyleBackgroundSize(StyleKeyword keyword)
		{
			return new StyleBackgroundSize(keyword);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00068878 File Offset: 0x00066A78
		public static implicit operator StyleBackgroundSize(BackgroundSize v)
		{
			return new StyleBackgroundSize(v);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x00068890 File Offset: 0x00066A90
		public bool Equals(StyleBackgroundSize other)
		{
			return other == this;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000688B0 File Offset: 0x00066AB0
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleBackgroundSize)
			{
				StyleBackgroundSize styleBackgroundSize = (StyleBackgroundSize)obj;
				flag = this.Equals(styleBackgroundSize);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000688DC File Offset: 0x00066ADC
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00068910 File Offset: 0x00066B10
		public override string ToString()
		{
			return this.DebugString<BackgroundSize>();
		}

		// Token: 0x04000ADD RID: 2781
		private BackgroundSize m_Value;

		// Token: 0x04000ADE RID: 2782
		private StyleKeyword m_Keyword;
	}
}
